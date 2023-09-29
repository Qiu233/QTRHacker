using QHackCLR.Builders;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public abstract class AddressableTypedEntity
{
	public CLRType Type { get; }
	public nuint Address { get; }
	internal IObjectHelper ObjectHelper => Type.ObjectHelper;
	public DataAccess DataAccess => ObjectHelper.DataAccess;
	internal AddressableTypedEntity(CLRType type, nuint address)
	{
		Type = type;
		Address = address;
	}
	public override bool Equals(object? obj)
	{
		if (obj is AddressableTypedEntity e)
			return Address == e.Address;
		return false;
	}

	public override int GetHashCode() => Address.GetHashCode();
	public static bool operator ==(AddressableTypedEntity l, AddressableTypedEntity r) => l.Address == r.Address;
	public static bool operator !=(AddressableTypedEntity l, AddressableTypedEntity r) => !(l == r);

	public abstract nuint OffsetBase { get; }

	public T ReadABS<T>(uint offset) where T : unmanaged => ObjectHelper.DataAccess.ReadValue<T>(Address + offset);
	public void WriteABS<T>(uint offset, T value) where T : unmanaged => ObjectHelper.DataAccess.WriteValue<T>(Address + offset, value);
}
