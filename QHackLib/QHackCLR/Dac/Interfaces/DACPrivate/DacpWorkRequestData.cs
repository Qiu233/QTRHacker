using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpWorkRequestData
	{
		public CLRDATA_ADDRESS Function;
		public CLRDATA_ADDRESS Context;
		public CLRDATA_ADDRESS NextWorkRequest;
	}
}
