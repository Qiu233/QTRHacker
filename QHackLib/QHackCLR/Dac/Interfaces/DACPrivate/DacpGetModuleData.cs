using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGetModuleData
	{
		public int IsDynamic;
		public int IsInMemory;
		public int IsFileLayout;
		public CLRDATA_ADDRESS PEAssembly;
		public CLRDATA_ADDRESS LoadedPEAddress;
		public ulong LoadedPESize;
		public CLRDATA_ADDRESS InMemoryPdbAddress;
		public ulong InMemoryPdbSize;
	}
}
