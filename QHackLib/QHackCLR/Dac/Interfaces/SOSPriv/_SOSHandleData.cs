using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct _SOSHandleData
	{
		public CLRDATA_ADDRESS AppDomain;
		public CLRDATA_ADDRESS Handle;
		public CLRDATA_ADDRESS Secondary;
		public uint Type;
		public int StrongReference;
		public uint RefCount;
		public uint JupiterRefCount;
		public int IsPegged;
	}
}
