using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct _SOS_StackRefError
	{
		public SOSStackSourceType SourceType;
		public CLRDATA_ADDRESS Source;
		public CLRDATA_ADDRESS StackPointer;
	}
}
