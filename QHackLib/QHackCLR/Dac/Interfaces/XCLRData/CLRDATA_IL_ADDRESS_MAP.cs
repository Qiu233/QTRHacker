using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CLRDATA_IL_ADDRESS_MAP
	{
		public int ILOffset;
		public CLRDATA_ADDRESS StartAddress;
		public CLRDATA_ADDRESS EndAddress;
		public CLRDataSourceType Type;
	}
}
