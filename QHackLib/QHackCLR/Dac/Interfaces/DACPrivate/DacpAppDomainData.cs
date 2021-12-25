using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpAppDomainData
	{
		public CLRDATA_ADDRESS AppDomainPtr;
		public CLRDATA_ADDRESS AppSecDesc;
		public CLRDATA_ADDRESS PLowFrequencyHeap;
		public CLRDATA_ADDRESS PHighFrequencyHeap;
		public CLRDATA_ADDRESS PStubHeap;
		public CLRDATA_ADDRESS DomainLocalBlock;
		public CLRDATA_ADDRESS PDomainLocalModules;
		public uint DwId;
		public int AssemblyCount;
		public int FailedAssemblyCount;
		public DacpAppDomainDataStage AppDomainStage;
	}
}
