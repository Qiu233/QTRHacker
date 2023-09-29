using QHackCLR.Common;
using System.Runtime.InteropServices;

namespace QHackCLR.DAC.DACP;

[StructLayout(LayoutKind.Sequential)]
public struct DacpModuleData
{
	public CLRDATA_ADDRESS Address = 0;
	public CLRDATA_ADDRESS File = 0; // A PEFile addr
	public CLRDATA_ADDRESS ilBase = 0;
	public CLRDATA_ADDRESS metadataStart = 0;
	public ulong metadataSize = 0;
	public CLRDATA_ADDRESS Assembly = 0; // Assembly pointer
	public uint bIsReflection = 0;
	public uint bIsPEFile = 0;
	public ulong dwBaseClassIndex = 0;
	public ulong dwModuleID = 0;

	public uint dwTransientFlags = 0;

	public CLRDATA_ADDRESS TypeDefToMethodTableMap = 0;
	public CLRDATA_ADDRESS TypeRefToMethodTableMap = 0;
	public CLRDATA_ADDRESS MethodDefToDescMap = 0;
	public CLRDATA_ADDRESS FieldDefToDescMap = 0;
	public CLRDATA_ADDRESS MemberRefToDescMap = 0;
	public CLRDATA_ADDRESS FileReferencesMap = 0;
	public CLRDATA_ADDRESS ManifestModuleReferencesMap = 0;

	public CLRDATA_ADDRESS pLookupTableHeap = 0;
	public CLRDATA_ADDRESS pThunkHeap = 0;

	public ulong dwModuleIndex = 0;
	public DacpModuleData() { }
}
