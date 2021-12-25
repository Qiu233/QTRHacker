using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CLRDATA_MODULE_EXTENT
	{
		public CLRDATA_ADDRESS Base;
		public uint Length;
		public CLRDataModuleExtentType Type;
	}
}
