using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpJitManagerInfo
	{
		public CLRDATA_ADDRESS ManagerAddr;
		public uint CodeType;
		public CLRDATA_ADDRESS PtrHeapList;
	}
}
