using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpMethodTableFieldData
	{
		public ushort WNumInstanceFields;
		public ushort WNumStaticFields;
		public ushort WNumThreadStaticFields;
		public CLRDATA_ADDRESS FirstField;
		public ushort WContextStaticOffset;
		public ushort WContextStaticsSize;
	}
}
