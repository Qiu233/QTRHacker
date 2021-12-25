using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum SOSRefFlags : uint
	{
		SOSRefInterior = 1,
		SOSRefPinned = 2,
	}
}
