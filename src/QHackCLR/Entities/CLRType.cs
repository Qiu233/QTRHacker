using Microsoft.VisualBasic;
using QHackCLR.Builders;
using QHackCLR.Common;
using QHackCLR.DAC;
using QHackCLR.DAC.DACP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QHackCLR.Entities;

public sealed unsafe class CLRType : CLREntity
{
	private CLRType? m_BaseType, m_ComponentType;
	private CorElementType? m_ElementType;
	private IReadOnlyList<CLRField>? m_Fields;
	private IReadOnlyList<CLRMethod>? m_Methods;
	internal ITypeHelper TypeHelper { get; }
	internal IObjectHelper ObjectHelper => TypeHelper.ObjectHelper;
	public string Name { get; }
	internal readonly DacpMethodTableData Data;
	internal CLRType(ITypeHelper helper, nuint ClrHandle) : base(ClrHandle)
	{
		TypeHelper = helper;
		Name = helper.SOSDac.GetMethodTableName(NativeHandle) ?? throw new QHackCLRException($"Cannot get name of Type: {NativeHandle}");

		Data = new DacpMethodTableData();
		fixed (DacpMethodTableData* ptr = &Data)
			helper.SOSDac.GetMethodTableData(NativeHandle, ptr);
	}

	public override string ToString()
	{
		return Name + "@" + NativeHandle;
	}

	public CLRHeap Heap => TypeHelper.Heap;
	public CLRModule Module => TypeHelper.GetModule(Data.Module);
	public CLRType? BaseType => m_BaseType ??= TypeHelper.TypeFactory.GetCLRType(Data.ParentMethodTable);
	public CLRType? ComponentType
	{
		get
		{
			if (m_ComponentType is not null)
				return m_ComponentType;
			MethodTable table = TypeHelper.DataAccess.ReadValue<MethodTable>(ClrHandle);
			return m_ComponentType = TypeHelper.TypeFactory.GetCLRType(table.ElementTypeHnd);
		}
	}
	public TypeAttributes TypeAttributes => (TypeAttributes)Data.dwAttrClass;

	public uint ComponentSize => Data.ComponentSize;
	public uint BaseSize => Data.BaseSize;
	public uint DataSize => Data.BaseSize - (uint)(2 * sizeof(UIntPtr));
	public CorElementType ElementType => m_ElementType ??= GetCorElementType();
	public bool IsArray => ElementType == CorElementType.ELEMENT_TYPE_ARRAY || ElementType == CorElementType.ELEMENT_TYPE_SZARRAY;

	public int Rank
	{
		get
		{
			if (!IsArray)
				return 0;
			if (ElementType == CorElementType.ELEMENT_TYPE_SZARRAY)
				return 1;
			return (int)((BaseSize - sizeof(UIntPtr) * 3) / 8);
		}
	}

	public IReadOnlyList<CLRField> Fields => m_Fields ??= TypeHelper.EnumerateFields(this).ToList();
	public IReadOnlyList<CLRMethod> MethodsInVTable => m_Methods ??= TypeHelper.EnumerateVTableMethods(this).ToList();

	public int GetLength(nuint obj) => TypeHelper.DataAccess.ReadValue<int>(obj + (nuint)sizeof(nuint));
	public int GetLength(nuint obj, int dimension)
	{
		int rank = Rank;
		if (dimension >= rank)
			throw new ArgumentOutOfRangeException(nameof(dimension));
		if (ElementType == CorElementType.ELEMENT_TYPE_SZARRAY)//SZArray
			return GetLength(obj);
		return TypeHelper.DataAccess.ReadValue<int>(obj + (nuint)(sizeof(UIntPtr) * 2 + 4 * dimension));
	}

	private CorElementType GetCorElementType()
	{
		if (this == Heap.ObjectType)
			return CorElementType.ELEMENT_TYPE_OBJECT;
		if (this == Heap.StringType)
			return CorElementType.ELEMENT_TYPE_STRING;
		if (ComponentSize != 0)
			return (BaseSize > (uint)(3 * IntPtr.Size)) ? CorElementType.ELEMENT_TYPE_ARRAY : CorElementType.ELEMENT_TYPE_SZARRAY;
		CLRType? baseType = BaseType;
		if (baseType is null)
			return CorElementType.ELEMENT_TYPE_OBJECT;
		if (baseType == Heap.ObjectType)
			return CorElementType.ELEMENT_TYPE_CLASS;
		if (baseType.Name != "System.ValueType")
			return baseType.ElementType;
		return Utils.GetCorElementTypeFromName(Name);
	}


	public IEnumerable<CLRStaticField> EnumerateStaticFields() => Fields.OfType<CLRStaticField>();
	public IEnumerable<CLRInstanceField> EnumerateInstanceFields() => Fields.OfType<CLRInstanceField>();

	public bool IsValueType => ElementType.IsValueType();
	public bool IsPrimitive => ElementType.IsPrimitive();
	public bool IsObjectReference => ElementType.IsObjectReference();

	public bool IsShared => Data.bIsShared != 0;
	public bool IsDynamic => Data.bIsDynamic != 0;
	public uint MDToken => Data.cl;


	public int GetLowerBound(nuint objRef, int dimension)
	{
		int rank = Rank;
		if (dimension >= rank)
			throw new ArgumentOutOfRangeException(nameof(dimension));
		if (rank == 1)
			return 0;
		return TypeHelper.DataAccess.ReadValue<int>(objRef + (uint)(sizeof(nuint) * 2 + 4 * (rank + dimension)));
	}
	public nuint GetElementsBase(nuint objRef)
	{
		if (!IsArray)
			throw new InvalidOperationException("Not an array");
		if (ElementType == CorElementType.ELEMENT_TYPE_SZARRAY)
			return objRef + (uint)sizeof(nuint) * 2;
		return objRef + (uint)(sizeof(nuint) * 2 + (8 * Rank));
	}

	public uint GetArrayElementOffset(nuint objRef, int[] indices)
	{
		int rank = Rank;
		if (indices.Length != rank)
			throw new ArgumentException("Rank does not match");
		if (ElementType == CorElementType.ELEMENT_TYPE_SZARRAY)
			return (uint)(sizeof(nuint) * 2 + (indices[0] * ComponentSize));
		int offset = 0;
		for (int i = 0; i < rank; i++)
		{
			int currentValueOffset = indices[i] - GetLowerBound(objRef, i);
			if (currentValueOffset >= GetLength(objRef, i))
				throw new ArgumentOutOfRangeException(nameof(indices));
			offset *= GetLength(objRef, i);
			offset += currentValueOffset;
		}
		return (uint)(sizeof(nuint) * 2 + (8 * rank) + (offset * ComponentSize));
	}

	public nuint GetArrayElementAddress(nuint objRef, int[] indices) => objRef + GetArrayElementOffset(objRef, indices);

	public CLRInstanceField? GetInstanceFieldByName(string name) => EnumerateInstanceFields().FirstOrDefault(f => f.Name == name);
	public CLRStaticField? GetStaticFieldByName(string name) => EnumerateStaticFields().FirstOrDefault(f => f.Name == name);
}
