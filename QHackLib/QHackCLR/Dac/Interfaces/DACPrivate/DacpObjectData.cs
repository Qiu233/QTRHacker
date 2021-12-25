using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpObjectData
	{
		public CLRDATA_ADDRESS MethodTable;
		public DacpObjectType ObjectType;
		public ulong Size;
		public CLRDATA_ADDRESS ElementTypeHandle;
		public CorElementType ElementType;
		public uint DwRank;
		public ulong DwNumComponents;
		public ulong DwComponentSize;
		public CLRDATA_ADDRESS ArrayDataPtr;
		public CLRDATA_ADDRESS ArrayBoundsPtr;
		public CLRDATA_ADDRESS ArrayLowerBoundsPtr;
		public CLRDATA_ADDRESS RCW;
		public CLRDATA_ADDRESS CCW;
	}
}
