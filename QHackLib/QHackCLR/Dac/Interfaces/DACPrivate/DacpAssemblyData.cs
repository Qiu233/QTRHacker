using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpAssemblyData
	{
		public CLRDATA_ADDRESS AssemblyPtr;
		public CLRDATA_ADDRESS ClassLoader;
		public CLRDATA_ADDRESS ParentDomain;
		public CLRDATA_ADDRESS BaseDomainPtr;
		public CLRDATA_ADDRESS AssemblySecDesc;
		public int IsDynamic;
		public uint ModuleCount;
		public uint LoadContext;
		public int IsDomainNeutral;
		public uint DwLocationFlags;
	}
}
