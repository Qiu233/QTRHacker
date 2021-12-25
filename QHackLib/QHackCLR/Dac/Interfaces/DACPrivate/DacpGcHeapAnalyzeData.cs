using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGcHeapAnalyzeData
	{
		public CLRDATA_ADDRESS HeapAddr;
		public CLRDATA_ADDRESS Internal_root_array;
		public ulong Internal_root_array_index;
		public int Heap_analyze_success;
	}
}
