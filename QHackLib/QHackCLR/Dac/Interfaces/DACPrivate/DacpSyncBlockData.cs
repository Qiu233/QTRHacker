using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpSyncBlockData
	{
		public CLRDATA_ADDRESS Object;
		public int BFree;
		public CLRDATA_ADDRESS SyncBlockPointer;
		public uint COMFlags;
		public uint MonitorHeld;
		public uint Recursion;
		public CLRDATA_ADDRESS HoldingThread;
		public uint AdditionalThreadCount;
		public CLRDATA_ADDRESS AppDomainPtr;
	}
}
