using QHackCLR.Clr.Builders.Helpers;
using QHackCLR.Dac.Interfaces;
using QHackCLR.Dac.Utils;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	/// <summary>
	/// Represents objects allocated on the heap.
	/// </summary>
	public unsafe sealed class ClrObject : AddressableTypedEntity
	{
		public override ClrType Type { get; }
		public readonly DacpObjectData Data;
		public ClrObject(IClrObjectHelper helper, nuint address) : base(helper, address)
		{
			helper.SOSDac.GetObjectData(address, out Data);
			Type = helper.TypeFactory.GetClrType(Data.MethodTable);
		}

		public bool IsArray => Type.IsArray;
		public bool IsBoxed => Type.IsValueType;
		public ulong Size => Data.Size;

		public unsafe ClrValue BoxedValue
		{
			get
			{
				if (!IsBoxed)
					throw new InvalidOperationException("Not an boxed value.");
				return new ClrValue(Type, Address + (uint)sizeof(nuint));
			}
		}

		public unsafe override AddressableTypedEntity GetFieldValue(string name)
		{
			ClrInstanceField field = Type.EnumerateInstanceFields().FirstOrDefault(t => t.Name == name) ??
				throw new ArgumentException("No such field", nameof(name));
			return field.GetValue(Address + (uint)sizeof(nuint));
		}

		public int GetLength() => Read<int>((uint)sizeof(nuint));

		public int GetLength(int dimension)
		{
			int rank = Type.Rank;
			if (dimension >= rank)
				throw new ArgumentOutOfRangeException(nameof(dimension));
			if (Type.CorElementType == CorElementType.SZArray)//SZArray
				return GetLength();
			return Read<int>((uint)(sizeof(nuint) * 2 + sizeof(int) * dimension));
		}

		public int GetLowerBound(int dimension)
		{
			int rank = Type.Rank;
			if (dimension >= rank)
				throw new ArgumentOutOfRangeException(nameof(dimension));
			if (rank == 1)
				return 0;
			return Read<int>((uint)(sizeof(nuint) * 2 + sizeof(int) * (rank + dimension)));
		}

		public nuint GetElementsBase()
		{
			if (!Type.IsArray)
				throw new InvalidOperationException("Not an array");
			if (Type.CorElementType == CorElementType.SZArray)
				return (uint)(sizeof(nuint) * 2);
			return (uint)(sizeof(nuint) * 2 + (8 * Type.Rank));
		}

		public uint GetArrayElementOffset(params int[] indices)
		{
			int rank = Type.Rank;
			if (indices.Length != rank)
				throw new ArgumentException("Rank does not match", nameof(indices));
			if (Type.CorElementType == CorElementType.SZArray)
				return (uint)(sizeof(nuint) * 2 + (indices[0] * Type.ComponentSize));
			int offset = 0;
			for (int i = 0; i < rank; i++)
			{
				int currentValueOffset = indices[i] - GetLowerBound(i);
				if ((uint)currentValueOffset >= GetLength(i))
					throw new ArgumentOutOfRangeException(nameof(indices));
				offset *= GetLength(i);
				offset += currentValueOffset;
			}
			return (uint)(sizeof(nuint) * 2 + (8 * rank) + (offset * Type.ComponentSize));
		}

		public nuint GetArrayElementAddress(params int[] indices) => Address + GetArrayElementOffset(indices);

		public AddressableTypedEntity GetArrayElement(params int[] indices)
		{
			if (!Type.IsArray)
				throw new InvalidOperationException("Not an array");
			uint offset = GetArrayElementOffset(indices);
			ClrType componentType = Type.ComponentType;
			if (componentType.IsValueType)
				return new ClrValue(componentType, Address + offset);
			return new ClrObject(ObjectHelper, Read<nuint>(offset));
		}

		public T ReadArrayElement<T>(params int[] indices) where T : unmanaged => Read<T>(GetArrayElementOffset(indices));
		public AddressableTypedEntity GetArrayElement(int index) => GetArrayElement(new int[] { index });
	}
}
// |<-----------Object-Layout------------->|
// | sync_block | method_table | fields... |
// |  pointer   |   pointer    |           |
// |____________|______________|___________|
//              ^ref           ^offset base

// Things become complicated for arrays.

// SZArray (string is special SZArray of char)
// |<------Fields-Layout------>|
// |   length   |  elements... |
// |   pointer  |              |
// |____________|______________|
// ^offset base

// Array
// |<-------------------Fields-Layout-------------------->|
// |  length  |  lengths   |  lowerbounds  |  elements... |
// |  pointer |  int[rank] |   int[rank]   |              |
// |__________|____________|_______________|______________|
// ^offset base                            ^where elements begin