using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpMethodTableData
	{
		public int BIsFree;
		public CLRDATA_ADDRESS Module;
		public CLRDATA_ADDRESS Class;
		public CLRDATA_ADDRESS ParentMethodTable;
		public ushort WNumInterfaces;
		public ushort WNumMethods;
		public ushort WNumVtableSlots;
		public ushort WNumVirtuals;
		public uint BaseSize;
		public uint ComponentSize;
		public int Token;
		public uint DwAttrClass;
		public int BIsShared;
		public int BIsDynamic;
		public int BContainsPointers;
	}
}
