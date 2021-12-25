using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpMethodDescData
	{
		public int BHasNativeCode;
		public int BIsDynamic;
		public ushort WSlotNumber;
		public CLRDATA_ADDRESS NativeCodeAddr;
		public CLRDATA_ADDRESS AddressOfNativeCodeSlot;
		public CLRDATA_ADDRESS MethodDescPtr;
		public CLRDATA_ADDRESS MethodTablePtr;
		public CLRDATA_ADDRESS ModulePtr;
		public int MDToken;
		public CLRDATA_ADDRESS GCInfo;
		public CLRDATA_ADDRESS GCStressCodeCopy;
		public CLRDATA_ADDRESS ManagedDynamicMethodObject;
		public CLRDATA_ADDRESS RequestedIP;
		public DacpReJitData RejitDataCurrent;
		public DacpReJitData RejitDataRequested;
		public uint CJittedRejitVersions;
	}
}
