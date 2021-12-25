using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CLRDATA_METHDEF_EXTENT
	{
		public CLRDATA_ADDRESS StartAddress;
		public CLRDATA_ADDRESS EndAddress;
		public uint EnCVersion;
		public CLRDataMethodDefinitionExtentType Type;
	}
}
