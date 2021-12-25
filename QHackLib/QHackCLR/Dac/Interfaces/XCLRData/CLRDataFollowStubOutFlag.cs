using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataFollowStubOutFlag : uint
	{
		CLRDATA_FOLLOW_STUB_INTERMEDIATE = 0x00000000,
		CLRDATA_FOLLOW_STUB_EXIT = 0x00000001,
	}
}
