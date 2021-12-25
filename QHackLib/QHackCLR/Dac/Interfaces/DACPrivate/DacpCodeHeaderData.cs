using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpCodeHeaderData
	{
		public CLRDATA_ADDRESS GCInfo;
		public JITTypes JITType;
		public CLRDATA_ADDRESS MethodDescPtr;
		public CLRDATA_ADDRESS MethodStart;
		public uint MethodSize;
		public CLRDATA_ADDRESS ColdRegionStart;
		public uint ColdRegionSize;
		public uint HotRegionSize;
	}
}
