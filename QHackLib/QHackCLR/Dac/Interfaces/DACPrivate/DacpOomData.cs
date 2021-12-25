using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpOomData
	{
		public int Reason;
		public ulong Alloc_size;
		public ulong Available_pagefile_mb;
		public ulong Gc_index;
		public int Fgm;
		public ulong Size;
		public int Loh_p;
	}
}
