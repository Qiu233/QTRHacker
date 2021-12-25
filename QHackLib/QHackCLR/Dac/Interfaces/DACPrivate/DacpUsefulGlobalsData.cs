using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpUsefulGlobalsData
	{
		public CLRDATA_ADDRESS ArrayMethodTable;
		public CLRDATA_ADDRESS StringMethodTable;
		public CLRDATA_ADDRESS ObjectMethodTable;
		public CLRDATA_ADDRESS ExceptionMethodTable;
		public CLRDATA_ADDRESS FreeMethodTable;
	}
}
