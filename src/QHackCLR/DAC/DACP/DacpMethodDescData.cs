using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.DACP;

public enum Flags
{
	kUnknown,
	kRequested,
	kActive,
	kReverted,
}

[StructLayout(LayoutKind.Sequential)]
public struct DacpReJitData
{
	public CLRDATA_ADDRESS rejitID = 0;
	public Flags flags = Flags.kUnknown;
	public CLRDATA_ADDRESS NativeCodeAddr = 0;
	public DacpReJitData() { }
};

[StructLayout(LayoutKind.Sequential)]
public struct DacpMethodDescData
{
	public uint bHasNativeCode = 0;
	public uint bIsDynamic = 0;
	public ushort wSlotNumber = 0;
	public CLRDATA_ADDRESS NativeCodeAddr = 0;

	public CLRDATA_ADDRESS AddressOfNativeCodeSlot = 0;

	public CLRDATA_ADDRESS MethodDescPtr = 0;
	public CLRDATA_ADDRESS MethodTablePtr = 0;
	public CLRDATA_ADDRESS ModulePtr = 0;

	public uint MDToken = 0;
	public CLRDATA_ADDRESS GCInfo = 0;
	public CLRDATA_ADDRESS GCStressCodeCopy = 0;


	public CLRDATA_ADDRESS managedDynamicMethodObject = 0;

	public CLRDATA_ADDRESS requestedIP = 0;


	public DacpReJitData rejitDataCurrent = new();


	public DacpReJitData rejitDataRequested = new();

	public uint cJittedRejitVersions = 0;
	public DacpMethodDescData() { }
}
