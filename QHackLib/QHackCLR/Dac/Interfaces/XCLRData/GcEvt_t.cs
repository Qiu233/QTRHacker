using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum GcEvt_t : uint
	{
		GC_MARK_END = 1,
		GC_EVENT_TYPE_MAX,
	}
}
