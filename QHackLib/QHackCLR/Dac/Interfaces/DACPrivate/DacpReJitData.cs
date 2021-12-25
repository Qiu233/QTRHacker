using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpReJitData
	{
		public CLRDATA_ADDRESS RejitID;
		public DacpReJitDataFlags Flags;
		public CLRDATA_ADDRESS NativeCodeAddr;
	}
}
