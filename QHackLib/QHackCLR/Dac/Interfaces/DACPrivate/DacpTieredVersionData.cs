using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpTieredVersionData
	{
		public CLRDATA_ADDRESS NativeCodeAddr;
		public OptimizationTier OptimizationTier;
		public CLRDATA_ADDRESS NativeCodeVersionNodePtr;
	}
}
