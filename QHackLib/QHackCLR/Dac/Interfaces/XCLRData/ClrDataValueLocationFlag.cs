using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum ClrDataValueLocationFlag : uint
	{
		CLRDATA_VLOC_MEMORY = 0x00000000,
		CLRDATA_VLOC_REGISTER = 0x00000001,
	}
}
