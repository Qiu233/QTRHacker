using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataStackWalkRequest : uint
	{
		CLRDATA_STACK_WALK_REQUEST_SET_FIRST_FRAME = 0xe1000000,
	}
}
