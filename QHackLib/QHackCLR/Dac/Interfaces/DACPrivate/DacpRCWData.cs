using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpRCWData
	{
		public CLRDATA_ADDRESS IdentityPointer;
		public CLRDATA_ADDRESS UnknownPointer;
		public CLRDATA_ADDRESS ManagedObject;
		public CLRDATA_ADDRESS JupiterObject;
		public CLRDATA_ADDRESS VtablePtr;
		public CLRDATA_ADDRESS CreatorThread;
		public CLRDATA_ADDRESS CtxCookie;
		public int RefCount;
		public int InterfaceCount;
		public int IsJupiterObject;
		public int SupportsIInspectable;
		public int IsAggregated;
		public int IsContained;
		public int IsFreeThreaded;
		public int IsDisconnected;
	}
}
