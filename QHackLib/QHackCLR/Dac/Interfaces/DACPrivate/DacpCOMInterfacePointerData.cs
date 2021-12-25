using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpCOMInterfacePointerData
	{
		public CLRDATA_ADDRESS MethodTable;
		public CLRDATA_ADDRESS InterfacePtr;
		public CLRDATA_ADDRESS ComContext;
	}
}
