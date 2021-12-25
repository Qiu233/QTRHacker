using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataProcessFlag : uint
	{
		CLRDATA_PROCESS_DEFAULT = 0x00000000,
		CLRDATA_PROCESS_IN_GC = 0x00000001,
	}
}
