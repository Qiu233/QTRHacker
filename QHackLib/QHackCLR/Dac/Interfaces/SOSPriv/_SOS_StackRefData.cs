using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct _SOS_StackRefData
	{
		public int HasRegisterInformation;
		public int Register;
		public int Offset;
		public CLRDATA_ADDRESS Address;
		public CLRDATA_ADDRESS Object;
		public uint Flags;
		public SOSStackSourceType SourceType;
		public CLRDATA_ADDRESS Source;
		public CLRDATA_ADDRESS StackPointer;
	}
}
