using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpThreadStoreData
	{
		public int ThreadCount;
		public int UnstartedThreadCount;
		public int BackgroundThreadCount;
		public int PendingThreadCount;
		public int DeadThreadCount;
		public CLRDATA_ADDRESS FirstThread;
		public CLRDATA_ADDRESS FinalizerThread;
		public CLRDATA_ADDRESS GcThread;
		public uint FHostConfig;
	}
}
