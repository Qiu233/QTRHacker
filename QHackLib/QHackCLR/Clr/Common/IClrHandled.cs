using System;

namespace QHackCLR.Clr
{
	public interface IClrHandled : IEquatable<IClrHandled>
	{
		nuint ClrHandle { get; }
	}
}
