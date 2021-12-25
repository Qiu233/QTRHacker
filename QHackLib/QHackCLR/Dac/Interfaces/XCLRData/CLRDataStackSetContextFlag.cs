using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataStackSetContextFlag : uint
	{
		CLRDATA_STACK_SET_UNWIND_CONTEXT = 0x00000000,
		CLRDATA_STACK_SET_CURRENT_CONTEXT = 0x00000001,
	}
}
