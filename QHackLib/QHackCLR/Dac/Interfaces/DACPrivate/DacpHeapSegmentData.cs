using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpHeapSegmentData
	{
		public CLRDATA_ADDRESS SegmentAddr;
		public CLRDATA_ADDRESS Allocated;
		public CLRDATA_ADDRESS Committed;
		public CLRDATA_ADDRESS Reserved;
		public CLRDATA_ADDRESS Used;
		public CLRDATA_ADDRESS Mem;
		public CLRDATA_ADDRESS Next;
		public CLRDATA_ADDRESS Gc_heap;
		public CLRDATA_ADDRESS HighAllocMark;
		public nuint Flags;
		public CLRDATA_ADDRESS Background_allocated;
	}
}
