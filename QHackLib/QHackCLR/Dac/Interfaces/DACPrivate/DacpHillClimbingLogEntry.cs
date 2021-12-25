using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpHillClimbingLogEntry
	{
		public uint TickCount;
		public int Transition;
		public int NewControlSetting;
		public int LastHistoryCount;
		public double LastHistoryMean;
	}
}
