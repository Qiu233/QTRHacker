using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpThreadpoolData
	{
		public int CpuUtilization;
		public int NumIdleWorkerThreads;
		public int NumWorkingWorkerThreads;
		public int NumRetiredWorkerThreads;
		public int MinLimitTotalWorkerThreads;
		public int MaxLimitTotalWorkerThreads;
		public CLRDATA_ADDRESS FirstUnmanagedWorkRequest;
		public CLRDATA_ADDRESS HillClimbingLog;
		public int HillClimbingLogFirstIndex;
		public int HillClimbingLogSize;
		public uint NumTimers;
		public int NumCPThreads;
		public int NumFreeCPThreads;
		public int MaxFreeCPThreads;
		public int NumRetiredCPThreads;
		public int MaxLimitTotalCPThreads;
		public int CurrentLimitTotalCPThreads;
		public int MinLimitTotalCPThreads;
		public CLRDATA_ADDRESS AsyncTimerCallbackCompletionFPtr;
	}
}
