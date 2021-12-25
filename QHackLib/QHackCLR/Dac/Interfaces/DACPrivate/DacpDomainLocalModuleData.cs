using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpDomainLocalModuleData
	{
		public CLRDATA_ADDRESS AppDomainAddr;
		public ulong ModuleID;
		public CLRDATA_ADDRESS PClassData;
		public CLRDATA_ADDRESS PDynamicClassTable;
		public CLRDATA_ADDRESS PGCStaticDataStart;
		public CLRDATA_ADDRESS PNonGCStaticDataStart;
	}
}
