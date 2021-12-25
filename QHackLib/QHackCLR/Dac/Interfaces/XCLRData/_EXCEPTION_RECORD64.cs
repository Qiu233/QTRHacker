using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct _EXCEPTION_RECORD64
	{
		public uint ExceptionCode;
		public uint ExceptionFlags;
		public ulong ExceptionRecord;
		public ulong ExceptionAddress;
		public uint NumberParameters;
		public uint __unusedAlignment;
		public fixed ulong ExceptionInformation[15];
	}
}
