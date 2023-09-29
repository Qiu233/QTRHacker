using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public abstract class CLREntity
{
	public nuint ClrHandle { get; }
	public CLRDATA_ADDRESS NativeHandle => new(ClrHandle);

	protected CLREntity(nuint handle) { ClrHandle = handle; }

	public override bool Equals(object? obj)
	{
		if (obj is CLREntity e)
			return ClrHandle == e.ClrHandle;
		return false;
	}
	public override int GetHashCode() => ClrHandle.GetHashCode();
	public static bool operator ==(CLREntity l, CLREntity r) => l.Equals(r);
	public static bool operator !=(CLREntity l, CLREntity r) => !(l == r);
}
