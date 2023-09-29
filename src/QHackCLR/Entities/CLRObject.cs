using QHackCLR.Builders;
using QHackCLR.DAC.DACP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QHackCLR.Entities;

public unsafe class CLRObject : AddressableTypedEntity
{
	internal readonly DacpObjectData Data;
	internal CLRObject(CLRType type, nuint address) : base(type, address)
	{
		Data = new DacpObjectData();
		fixed (DacpObjectData* ptr = &Data)
			ObjectHelper.SOSDac.GetObjectData(address, ptr);
	}

	public override nuint OffsetBase => Address + (uint)sizeof(nuint);

	public bool IsArray => Type.IsArray;
	public bool IsValueType => Type.IsValueType;
	public bool IsBoxed => IsValueType;

	public AddressableTypedEntity GetArrayElement(int[] indices)
	{
		if (!IsArray)
			throw new InvalidOperationException("Not an array");
		uint offset = GetArrayElementOffset(indices);
		CLRType componentType = Type.ComponentType!;
		if (componentType.IsValueType)
			return new CLRValue(componentType, Address + offset);
		return new CLRObject(componentType, ReadABS<nuint>(offset));
	}

	public T ReadArrayElement<T>(int[] indices) where T : unmanaged => ReadABS<T>(GetArrayElementOffset(indices));
	public AddressableTypedEntity GetArrayElement(int index) => GetArrayElement(new int[] { index });
	public nuint GetArrayElementAddress(int[] indices) => Type.GetArrayElementAddress(Address, indices);

	public uint GetArrayElementOffset(int[] indices) => Type.GetArrayElementOffset(Address, indices);
	public int GetLength() => Type.GetLength(Address);
	public int GetLength(int dimension) => Type.GetLength(Address, dimension);
	public int GetLowerBound(int dimension) => Type.GetLowerBound(Address, dimension);
	public nuint GetElementsBase() => Type.GetElementsBase(Address);

}
