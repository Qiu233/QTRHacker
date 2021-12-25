using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpReJitData2
	{
		public uint RejitID;
		public DacpReJitDataFlags Flags;
		public CLRDATA_ADDRESS Il;
		public CLRDATA_ADDRESS IlCodeVersionNodePtr;
	}
}
