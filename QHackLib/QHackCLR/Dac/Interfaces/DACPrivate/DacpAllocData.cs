using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpAllocData
	{
		public CLRDATA_ADDRESS AllocBytes;
		public CLRDATA_ADDRESS AllocBytesLoh;
	}
}
