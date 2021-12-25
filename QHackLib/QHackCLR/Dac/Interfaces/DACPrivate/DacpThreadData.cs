using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpThreadData
	{
		public uint CorThreadId;
		public uint OsThreadId;
		public int State;
		public uint PreemptiveGCDisabled;
		public CLRDATA_ADDRESS AllocContextPtr;
		public CLRDATA_ADDRESS AllocContextLimit;
		public CLRDATA_ADDRESS Context;
		public CLRDATA_ADDRESS Domain;
		public CLRDATA_ADDRESS PFrame;
		public uint LockCount;
		public CLRDATA_ADDRESS FirstNestedException;
		public CLRDATA_ADDRESS Teb;
		public CLRDATA_ADDRESS FiberData;
		public CLRDATA_ADDRESS LastThrownObjectHandle;
		public CLRDATA_ADDRESS NextThread;
	}
}
