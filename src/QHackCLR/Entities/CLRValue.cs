using QHackCLR.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public class CLRValue : AddressableTypedEntity
{
	internal CLRValue(CLRType type, nuint address) : base(type, address)
	{
	}

	public override nuint OffsetBase => Address;

	public unsafe T GetValue<T>() where T : unmanaged
	{
		if (Type.DataSize < sizeof(T))
			throw new InvalidOperationException("Size exceeded.");
		return ReadABS<T>(0);
	}
	public byte[] ReadBytes(int size)
	{
		byte[] data = new byte[size];
		DataAccess.Read(Address, (Span<byte>)data);
		return data;
	}
	public byte[] ReadBytes() => ReadBytes((int)Type.DataSize);
}
