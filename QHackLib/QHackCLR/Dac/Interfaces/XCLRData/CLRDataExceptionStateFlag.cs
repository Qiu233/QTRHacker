using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataExceptionStateFlag : uint
	{
		CLRDATA_EXCEPTION_DEFAULT = 0x00000000,
		CLRDATA_EXCEPTION_NESTED = 0x00000001,
		CLRDATA_EXCEPTION_PARTIAL = 0x00000002,
	}
}
