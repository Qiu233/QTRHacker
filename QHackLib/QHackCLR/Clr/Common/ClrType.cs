using QHackCLR.Clr.Builders;
using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Clr.Structs;
using QHackCLR.Dac;
using QHackCLR.Dac.Helpers;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	/// <summary>
	/// Represents a type in CLR.<br/>
	/// </summary>
	public class ClrType : ClrEntity, IHasMetadata
	{
		public string Name { get; }
		internal IClrObjectHelper ClrObjectHelper => TypeHelper.ClrObjectHelper;
		public readonly DacpMethodTableData Data;
		protected ITypeHelper TypeHelper { get; }

		public ClrType(ITypeHelper helper, nuint handle) : base(handle)
		{
			TypeHelper = helper;

			Name = helper.SOSDac.GetMethodTableName(ClrHandle);
			helper.SOSDac.GetMethodTableData(ClrHandle, out Data);
		}

		public ClrInstanceField GetInstanceFieldByName(string name) => EnumerateInstanceFields().FirstOrDefault(t => t.Name == name);
		public ClrStaticField GetStaticFieldByName(string name) => EnumerateStaticFields().FirstOrDefault(t => t.Name == name);

		#region Lazy loads
		private ClrModule _Module;
		private ClrType _BaseType;
		private ClrType _ComponentType;
		private IReadOnlyList<ClrField> _Fields;
		private IReadOnlyList<ClrMethod> _Methods;
		private CorElementType _ClrElementType;
		#endregion

		public ClrHeap Heap => TypeHelper.Heap;
		public CorElementType CorElementType => _ClrElementType == default ? _ClrElementType = GetClrElementType() : _ClrElementType;


		// The following properties play roles in topology of the type system.
		// They are not statically set so it will take some time to perform actions at first time.
		// Especially when getting component type, there'll be a memory reading through process.

		/// <summary>
		/// Gets the module defining this type.
		/// </summary>
		public ClrModule Module => _Module ??= TypeHelper.GetModule((nuint)Data.Module);

		/// <summary>
		/// The Type inherited by this Type<br/>
		/// null for System.Object
		/// </summary>
		public ClrType BaseType => _BaseType ??= TypeHelper.TypeFactory.GetClrType(Data.ParentMethodTable);

		/// <summary>
		/// Only works for array, otherwise always null.<br/>
		/// Actually this property refers to the type of array's elements, so its name is supposed to be ElementType just as what clr does.<br/>
		/// I don't really know why it has a name of ComponentType. clrMD uses it, then so does this lib.<br/>
		/// Btw, <see cref="ComponentSize"/> refers to size of elements.
		/// </summary>
		public ClrType ComponentType => _ComponentType ??= TypeHelper.TypeFactory.GetClrType(
					TypeHelper.DataAccess.Read<MethodTable>(ClrHandle).ElementTypeHnd);


		/// <summary>
		/// Only works for array, otherwise always 0.
		/// </summary>
		public unsafe int Rank
		{
			get
			{
				if (!IsArray)
					return 0;
				if (CorElementType == CorElementType.SZArray)
					return 1;
				return unchecked((int)((BaseSize - sizeof(nuint) * 3) / sizeof(int) / 2));
			}
		}

		/// <summary>
		/// Indicates whether this Type has pointers as fields.<br/>
		/// Seems like flags.
		/// </summary>
		public int ContainsPointers => Data.BContainsPointers;

		/// <summary>
		/// Get all fields, both instance and static
		/// </summary>
		public IReadOnlyList<ClrField> Fields => _Fields ??= TypeHelper.EnumerateFields(this).ToList();
		public IReadOnlyList<ClrMethod> MethodsInVTable => _Methods ??= EnumerateVTableMethods().ToList();

		public IEnumerable<ClrStaticField> EnumerateStaticFields() => from field in Fields
																	  where field.IsStatic
																	  select field as ClrStaticField;

		public IEnumerable<ClrInstanceField> EnumerateInstanceFields() => from field in Fields
																		  where !field.IsStatic
																		  select field as ClrInstanceField;

		public IEnumerable<ClrMethod> EnumerateVTableMethods() => TypeHelper.EnumerateVTableMethods(this);

		private CorElementType GetClrElementType()
		{
			if (this == Heap.ObjectType)
				return CorElementType.Object;
			if (this == Heap.StringType)
				return CorElementType.String;
			if (ComponentSize > 0)
				return BaseSize > (uint)(3 * IntPtr.Size) ? CorElementType.Array : CorElementType.SZArray;
			ClrType baseType = BaseType;
			if (baseType is null)
				return CorElementType.Object;
			if (baseType == Heap.ObjectType)
				return CorElementType.Class;
			if (baseType.Name != "System.ValueType")
				return baseType.CorElementType;
			return Name switch
			{
				"System.Int32" => CorElementType.Int32,
				"System.Int16" => CorElementType.Int16,
				"System.Int64" => CorElementType.Int64,
				"System.IntPtr" => CorElementType.NativeInt,
				"System.UInt16" => CorElementType.UInt16,
				"System.UInt32" => CorElementType.UInt32,
				"System.UInt64" => CorElementType.UInt64,
				"System.UIntPtr" => CorElementType.NativeUInt,
				"System.Boolean" => CorElementType.Boolean,
				"System.Single" => CorElementType.Float,
				"System.Double" => CorElementType.Double,
				"System.Byte" => CorElementType.UInt8,
				"System.Char" => CorElementType.Char,
				"System.SByte" => CorElementType.Int8,
				"System.Enum" => CorElementType.Int32,
				_ => CorElementType.Struct,
			};
		}

		public bool IsArray => CorElementType == CorElementType.Array || CorElementType == CorElementType.SZArray;
		public bool IsValueType => CorElementType.IsValueType();
		public bool IsPrimitive => CorElementType.IsPrimitive();
		public bool IsObjectReference => CorElementType.IsObjectReference();

		public bool IsShared => Data.BIsShared != 0;
		public bool IsDynamic => Data.BIsDynamic != 0;

		/// <summary>
		/// ComponentSize is only available for constructed types including arrays and string.<br/>
		/// String's ComponentSize is always 2 which equals to sizeof(char).<br/>
		/// Array of class has a ComponentSize equal to pointer's size.<br/>
		/// Array of ValueType has a ComponentSize equal to the ValueType itself's size.
		/// </summary>
		public uint ComponentSize => Data.ComponentSize;

		/// <summary>
		/// <para>Base size means the least size this type would take when allocated on heap.<br/>
		/// For example, SZArray takes up at least 3 pointers, so in 32bit process its BaseSize should be 3*4=12.<br/>
		/// In this case, the SZArray has no Component and HEADER/METHODTABLE/LENGTH occupys 1 pointer size each.<br/>
		/// Note, 3 ptr is the least size of any object.</para>
		/// Some formula(ptr for pointer size):<br/>
		/// BaseSize(Array) = 3 * ptr + 8 * rank<br/>
		/// BaseSize(SZArray) = 3 * ptr<br/>
		/// BaseSize(string) = 3 * ptr<br/>
		/// BaseSize(EmptyType) = 3 * ptr
		/// </summary>
		public uint BaseSize => Data.BaseSize;

		/// <summary>
		/// Just the BaseSize with 2 pointer size substracted.<br/>
		/// For when getting size despite HEADER and METHODTABLE.
		/// </summary>
		public uint UserSize => BaseSize - 2 * (uint)UIntPtr.Size;

		public int MDToken => Data.Token;

		public TypeAttributes TypeAttributes => (TypeAttributes)Data.DwAttrClass;

		public bool IsNotPublic => TypeAttributes.HasFlag(TypeAttributes.NotPublic);
		public bool IsPublic => TypeAttributes.HasFlag(TypeAttributes.Public);
		public bool IsNestedPublic => TypeAttributes.HasFlag(TypeAttributes.NestedPublic);
		public bool IsNestedPrivate => TypeAttributes.HasFlag(TypeAttributes.NestedPrivate);
		public bool IsNestedFamily => TypeAttributes.HasFlag(TypeAttributes.NestedFamily);
		public bool IsNestedAssembly => TypeAttributes.HasFlag(TypeAttributes.NestedAssembly);
		public bool IsNestedFamANDAssem => TypeAttributes.HasFlag(TypeAttributes.NestedFamANDAssem);
		public bool IsNestedFamORAssem => TypeAttributes.HasFlag(TypeAttributes.NestedFamORAssem);

		public bool IsAbstract => TypeAttributes.HasFlag(TypeAttributes.Abstract);
		public bool IsSealed => TypeAttributes.HasFlag(TypeAttributes.Sealed);
		public bool IsSpecialName => TypeAttributes.HasFlag(TypeAttributes.SpecialName);

		public bool IsAutoLayout => TypeAttributes.HasFlag(TypeAttributes.AutoLayout);
		public bool IsSequentialLayout => TypeAttributes.HasFlag(TypeAttributes.SequentialLayout);
		public bool IsExplicitLayout => TypeAttributes.HasFlag(TypeAttributes.ExplicitLayout);

		public bool IsClass => TypeAttributes.HasFlag(TypeAttributes.Class);
		public bool IsInterface => TypeAttributes.HasFlag(TypeAttributes.Interface);

		public bool IsImport => TypeAttributes.HasFlag(TypeAttributes.Import);
		public bool IsSerializable => TypeAttributes.HasFlag(TypeAttributes.Serializable);
		public bool IsWindowsRuntime => TypeAttributes.HasFlag(TypeAttributes.WindowsRuntime);

		public bool IsAnsiClass => TypeAttributes.HasFlag(TypeAttributes.AnsiClass);
		public bool IsUnicodeClass => TypeAttributes.HasFlag(TypeAttributes.UnicodeClass);
		public bool IsAutoClass => TypeAttributes.HasFlag(TypeAttributes.AutoClass);

		public bool IsBeforeFieldInit => TypeAttributes.HasFlag(TypeAttributes.BeforeFieldInit);

		public bool IsRTSpecialName => TypeAttributes.HasFlag(TypeAttributes.RTSpecialName);
		public bool IsHasSecurity => TypeAttributes.HasFlag(TypeAttributes.HasSecurity);

		public bool IsNested => IsNestedPublic || IsNestedPrivate || IsNestedFamily || IsNestedAssembly || IsNestedFamANDAssem || IsNestedFamORAssem;
	}
}
