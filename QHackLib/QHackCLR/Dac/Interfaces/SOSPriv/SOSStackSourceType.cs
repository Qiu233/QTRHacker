using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum SOSStackSourceType : uint
	{
		SOS_StackSourceIP,
		SOS_StackSourceFrame,
	}
}
