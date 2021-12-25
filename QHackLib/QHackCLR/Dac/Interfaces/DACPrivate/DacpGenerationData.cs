using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGenerationData
	{
		public CLRDATA_ADDRESS Start_segment;
		public CLRDATA_ADDRESS Allocation_start;
		public CLRDATA_ADDRESS AllocContextPtr;
		public CLRDATA_ADDRESS AllocContextLimit;
	}
}
