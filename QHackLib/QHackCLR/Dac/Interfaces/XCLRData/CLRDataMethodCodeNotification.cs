using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataMethodCodeNotification : uint
	{
		CLRDATA_METHNOTIFY_NONE = 0x00000000,
		CLRDATA_METHNOTIFY_GENERATED = 0x00000001,
		CLRDATA_METHNOTIFY_DISCARDED = 0x00000002,
	}
}
