using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpCCWData
	{
		public CLRDATA_ADDRESS OuterIUnknown;
		public CLRDATA_ADDRESS ManagedObject;
		public CLRDATA_ADDRESS Handle;
		public CLRDATA_ADDRESS CcwAddress;
		public int RefCount;
		public int InterfaceCount;
		public int IsNeutered;
		public int JupiterRefCount;
		public int IsPegged;
		public int IsGlobalPegged;
		public int HasStrongRef;
		public int IsExtendsCOMObject;
		public int IsAggregated;
	}
}
