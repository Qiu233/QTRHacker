using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGcHeapData
	{
		public int BServerMode;
		public int BGcStructuresValid;
		public uint HeapCount;
		public uint G_max_generation;
	}
}
