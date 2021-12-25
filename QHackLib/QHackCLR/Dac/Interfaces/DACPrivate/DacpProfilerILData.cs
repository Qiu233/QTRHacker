using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpProfilerILData
	{
		public ModificationType Type;
		public CLRDATA_ADDRESS Il;
		public uint RejitID;
	}
}
