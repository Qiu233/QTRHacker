using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpModuleData
	{
		public CLRDATA_ADDRESS Address;
		public CLRDATA_ADDRESS PEAssembly;
		public CLRDATA_ADDRESS IlBase;
		public CLRDATA_ADDRESS MetadataStart;
		public ulong MetadataSize;
		public CLRDATA_ADDRESS Assembly;
		public int BIsReflection;
		public int BIsPEFile;
		public ulong DwBaseClassIndex;
		public ulong DwModuleID;
		public uint DwTransientFlags;
		public CLRDATA_ADDRESS TypeDefToMethodTableMap;
		public CLRDATA_ADDRESS TypeRefToMethodTableMap;
		public CLRDATA_ADDRESS MethodDefToDescMap;
		public CLRDATA_ADDRESS FieldDefToDescMap;
		public CLRDATA_ADDRESS MemberRefToDescMap;
		public CLRDATA_ADDRESS FileReferencesMap;
		public CLRDATA_ADDRESS ManifestModuleReferencesMap;
		public CLRDATA_ADDRESS PLookupTableHeap;
		public CLRDATA_ADDRESS PThunkHeap;
		public ulong DwModuleIndex;
	}
}
