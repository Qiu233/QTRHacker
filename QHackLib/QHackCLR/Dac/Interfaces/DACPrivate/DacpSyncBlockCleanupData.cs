using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpSyncBlockCleanupData
	{
		public CLRDATA_ADDRESS SyncBlockPointer;
		public CLRDATA_ADDRESS NextSyncBlock;
		public CLRDATA_ADDRESS BlockRCW;
		public CLRDATA_ADDRESS BlockClassFactory;
		public CLRDATA_ADDRESS BlockCCW;
	}
}
