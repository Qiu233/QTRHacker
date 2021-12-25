using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpAppDomainStoreData
	{
		public CLRDATA_ADDRESS SharedDomain;
		public CLRDATA_ADDRESS SystemDomain;
		public int DomainCount;
	}
}
