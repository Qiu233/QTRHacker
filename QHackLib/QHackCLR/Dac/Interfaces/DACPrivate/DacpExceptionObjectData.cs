using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpExceptionObjectData
	{
		public CLRDATA_ADDRESS Message;
		public CLRDATA_ADDRESS InnerException;
		public CLRDATA_ADDRESS StackTrace;
		public CLRDATA_ADDRESS WatsonBuckets;
		public CLRDATA_ADDRESS StackTraceString;
		public CLRDATA_ADDRESS RemoteStackTraceString;
		public int HResult;
		public int XCode;
	}
}
