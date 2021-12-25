using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataOtherNotifyFlag : uint
	{
		CLRDATA_NOTIFY_ON_MODULE_LOAD = 0x00000001,
		CLRDATA_NOTIFY_ON_MODULE_UNLOAD = 0x00000002,
		CLRDATA_NOTIFY_ON_EXCEPTION = 0x00000004,
		CLRDATA_NOTIFY_ON_EXCEPTION_CATCH_ENTER = 0x00000008,
	}
}
