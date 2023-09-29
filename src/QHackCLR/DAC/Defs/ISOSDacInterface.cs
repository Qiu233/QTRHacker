using QHackCLR.DAC.DACP;
using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Runtime.InteropServices;
using static QHackCLR.NativeMethods;
using QHackCLR.Common;

namespace QHackCLR.DAC.Defs;
enum VCSHeapType { IndcellHeap, LookupHeap, ResolveHeap, DispatchHeap, CacheEntryHeap }
enum ModuleMapType { TYPEDEFTOMETHODTABLE, TYPEREFTOMETHODTABLE }


[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.Bool)]
unsafe delegate bool DUMPEHINFO(
	uint clauseIndex,
	uint totalClauses,
	DACEHInfo* pEHInfo,
	void* token);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.Bool)]
unsafe delegate bool VISITRCWFORCLEANUP(
	CLRDATA_ADDRESS RCW,
	CLRDATA_ADDRESS Context,
	CLRDATA_ADDRESS Thread,
	uint bIsFreeThreaded,
	void* token);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
unsafe delegate void MODULEMAPTRAVERSE(
	uint index,
	CLRDATA_ADDRESS methodTable,
	void* token);
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
unsafe delegate void VISITHEAP(
	CLRDATA_ADDRESS blockData,
	nuint blockSize,
	uint blockIsCurrentBlock);

[ComImport, Guid("436f00f2-b42a-4b9f-870c-e73db66ae930"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal unsafe interface ISOSDacInterface
{
	HRESULT GetThreadStoreData(DacpThreadStoreData* data);

	HRESULT GetAppDomainStoreData(DacpAppDomainStoreData* data);

	HRESULT GetAppDomainList(
		uint count,
		CLRDATA_ADDRESS* values,
		uint* pNeeded);

	HRESULT GetAppDomainData(
		CLRDATA_ADDRESS addr,
		DacpAppDomainData* data);

	HRESULT GetAppDomainName(
		CLRDATA_ADDRESS addr,
		uint count,
		char* name,
		uint* pNeeded);

	HRESULT GetDomainFromContext(
		CLRDATA_ADDRESS context,
		CLRDATA_ADDRESS* domain);

	HRESULT GetAssemblyList(
		CLRDATA_ADDRESS appDomain,
		int count,
		CLRDATA_ADDRESS* values,
		int* pNeeded);

	HRESULT GetAssemblyData(
		CLRDATA_ADDRESS baseDomainPtr,
		CLRDATA_ADDRESS assembly,

		DacpAssemblyData* data);

	HRESULT GetAssemblyName(
		CLRDATA_ADDRESS assembly,
		uint count,
		char* name,
		uint* pNeeded);

	HRESULT GetModule(
		CLRDATA_ADDRESS addr,
		out IXCLRDataModule mod);
	//IXCLRDataModule** mod);

	HRESULT GetModuleData(
		CLRDATA_ADDRESS moduleAddr,

		DacpModuleData* data);

	HRESULT TraverseModuleMap(
		ModuleMapType mmt,
		CLRDATA_ADDRESS moduleAddr,
		[MarshalAs(UnmanagedType.FunctionPtr)] MODULEMAPTRAVERSE pCallback,
		void* token);

	HRESULT GetAssemblyModuleList(
		CLRDATA_ADDRESS assembly,
		uint count,
		CLRDATA_ADDRESS* modules,
		uint* pNeeded);

	HRESULT GetILForModule(
		CLRDATA_ADDRESS moduleAddr,
		uint rva,
		CLRDATA_ADDRESS* il);

	HRESULT GetThreadData(
		CLRDATA_ADDRESS thread,

		byte* data);

	HRESULT GetThreadFromThinlockID(
		uint thinLockId,
		CLRDATA_ADDRESS* pThread);

	HRESULT GetStackLimits(
		CLRDATA_ADDRESS threadPtr,
		CLRDATA_ADDRESS* lower,
		CLRDATA_ADDRESS* upper,
		CLRDATA_ADDRESS* fp);

	HRESULT GetMethodDescData(
		CLRDATA_ADDRESS methodDesc,
		CLRDATA_ADDRESS ip,

		DacpMethodDescData* data,
		uint cRevertedRejitVersions,

		byte* rgRevertedRejitData,
		uint* pcNeededRevertedRejitData);

	HRESULT GetMethodDescPtrFromIP(
		CLRDATA_ADDRESS ip,
		CLRDATA_ADDRESS* ppMD);

	HRESULT GetMethodDescName(
		CLRDATA_ADDRESS methodDesc,
		uint count,
		char* name,
		uint* pNeeded);

	HRESULT GetMethodDescPtrFromFrame(
		CLRDATA_ADDRESS frameAddr,
		CLRDATA_ADDRESS* ppMD);

	HRESULT GetMethodDescFromToken(
		CLRDATA_ADDRESS moduleAddr,
		uint token,
		CLRDATA_ADDRESS* methodDesc);

	HRESULT GetMethodDescTransparencyData(
		CLRDATA_ADDRESS methodDesc,

		byte* data);

	HRESULT GetCodeHeaderData(
		CLRDATA_ADDRESS ip,

		DacpCodeHeaderData* data);

	HRESULT GetJitManagerList(
		uint count,

		byte* managers,
		uint* pNeeded);

	HRESULT GetJitHelperFunctionName(
		CLRDATA_ADDRESS ip,
		uint count,
		byte* name,
		uint* pNeeded);

	HRESULT GetJumpThunkTarget(
		int* ctx,
		CLRDATA_ADDRESS* targetIP,
		CLRDATA_ADDRESS* targetMD);

	HRESULT GetThreadpoolData(

		byte* data);

	HRESULT GetWorkRequestData(
		CLRDATA_ADDRESS addrWorkRequest,

		DacpWorkRequestData* data);

	HRESULT GetHillClimbingLogEntry(
		CLRDATA_ADDRESS addr,

		byte* data);

	HRESULT GetObjectData(
		CLRDATA_ADDRESS objAddr,

		DacpObjectData* data);

	HRESULT GetObjectStringData(
		CLRDATA_ADDRESS obj,
		uint count,
		char* stringData,
		uint* pNeeded);

	HRESULT GetObjectClassName(
		CLRDATA_ADDRESS obj,
		uint count,
		char* className,
		uint* pNeeded);

	HRESULT GetMethodTableName(
		CLRDATA_ADDRESS mt,
		uint count,
		char* mtName,
		uint* pNeeded);

	HRESULT GetMethodTableData(
		CLRDATA_ADDRESS mt,

		DacpMethodTableData* data);

	HRESULT GetMethodTableSlot(
		CLRDATA_ADDRESS mt,
		uint slot,
		CLRDATA_ADDRESS* value);

	HRESULT GetMethodTableFieldData(
		CLRDATA_ADDRESS mt,

		DacpMethodTableFieldData* data);

	HRESULT GetMethodTableTransparencyData(
		CLRDATA_ADDRESS mt,

		byte* data);

	HRESULT GetMethodTableForEEClass(
		CLRDATA_ADDRESS eeClass,
		CLRDATA_ADDRESS* value);

	[PreserveSig]
	HRESULT GetFieldDescData(
		CLRDATA_ADDRESS fieldDesc,

		DacpFieldDescData* data);

	HRESULT GetFrameName(
		CLRDATA_ADDRESS vtable,
		uint count,
		char* frameName,
		uint* pNeeded);

	HRESULT GetPEFileBase(
		CLRDATA_ADDRESS addr,
		CLRDATA_ADDRESS* @base);

	HRESULT GetPEFileName(
		CLRDATA_ADDRESS addr,
		uint count,
		char* fileName,
		uint* pNeeded);

	HRESULT GetGCHeapData(

		byte* data);

	HRESULT GetGCHeapList(
		uint count,
		CLRDATA_ADDRESS* heaps,
		uint* pNeeded);

	HRESULT GetGCHeapDetails(
		CLRDATA_ADDRESS heap,

		byte* details);

	HRESULT GetGCHeapStaticData(

		byte* data);

	HRESULT GetHeapSegmentData(
		CLRDATA_ADDRESS seg,

		byte* data);

	HRESULT GetOOMData(
		CLRDATA_ADDRESS oomAddr,

		byte* data);

	HRESULT GetOOMStaticData(

		byte* data);

	HRESULT GetHeapAnalyzeData(
		CLRDATA_ADDRESS addr,

		byte* data);

	HRESULT GetHeapAnalyzeStaticData(

		byte* data);

	HRESULT GetDomainLocalModuleData(
		CLRDATA_ADDRESS addr,

		byte* data);

	HRESULT GetDomainLocalModuleDataFromAppDomain(
		CLRDATA_ADDRESS appDomainAddr,
		int moduleID,

		DacpDomainLocalModuleData* data);

	HRESULT GetDomainLocalModuleDataFromModule(
		CLRDATA_ADDRESS moduleAddr,

		DacpDomainLocalModuleData* data);

	HRESULT GetThreadLocalModuleData(
		CLRDATA_ADDRESS thread,
		uint index,

		byte* data);

	HRESULT GetSyncBlockData(
		uint number,

		byte* data);

	HRESULT GetSyncBlockCleanupData(
		CLRDATA_ADDRESS addr,

		byte* data);

	HRESULT GetHandleEnum(
		byte** ppHandleEnum);

	HRESULT GetHandleEnumForTypes(
		uint* types,
		uint count,
		byte** ppHandleEnum);

	HRESULT GetHandleEnumForGC(
		uint gen,
		byte** ppHandleEnum);

	HRESULT TraverseEHInfo(
		CLRDATA_ADDRESS ip,
		[MarshalAs(UnmanagedType.FunctionPtr)] DUMPEHINFO pCallback,
		void* token);

	HRESULT GetNestedExceptionData(
		CLRDATA_ADDRESS exception,
		CLRDATA_ADDRESS* exceptionObject,
		CLRDATA_ADDRESS* nextNestedException);

	HRESULT GetStressLogAddress(
		CLRDATA_ADDRESS* stressLog);

	HRESULT TraverseLoaderHeap(
		CLRDATA_ADDRESS loaderHeapAddr,
		[MarshalAs(UnmanagedType.FunctionPtr)] VISITHEAP pCallback);

	HRESULT GetCodeHeapList(
		CLRDATA_ADDRESS jitManager,
		uint count,

		byte* codeHeaps,
		uint* pNeeded);

	HRESULT TraverseVirtCallStubHeap(
		CLRDATA_ADDRESS pAppDomain,
		VCSHeapType heaptype,
		[MarshalAs(UnmanagedType.FunctionPtr)] VISITHEAP pCallback);

	HRESULT GetUsefulGlobals(

		DacpUsefulGlobalsData* data);

	HRESULT GetClrWatsonBuckets(
		CLRDATA_ADDRESS thread,
		void* pGenericModeBlock);

	HRESULT GetTLSIndex(
		uint* pIndex);

	HRESULT GetDacModuleHandle(
		nuint* phModule);

	HRESULT GetRCWData(
		CLRDATA_ADDRESS addr,

		byte* data);

	HRESULT GetRCWInterfaces(
		CLRDATA_ADDRESS rcw,
		uint count,

		byte* interfaces,
		uint* pNeeded);

	HRESULT GetCCWData(
		CLRDATA_ADDRESS ccw,

			byte* data);

	HRESULT GetCCWInterfaces(
		CLRDATA_ADDRESS ccw,
		uint count,

		byte* interfaces,
		uint* pNeeded);

	HRESULT TraverseRCWCleanupList(
		CLRDATA_ADDRESS cleanupListPtr,
		[MarshalAs(UnmanagedType.FunctionPtr)] VISITRCWFORCLEANUP pCallback,
		void* token);

	HRESULT GetStackReferences(
		/* [in] */ uint osThreadID,
		/* [out] */ byte** ppEnum);

	HRESULT GetRegisterName(
		/* [in] */ int regName,
		/* [in] */ uint count,
		/* [out] */ char* buffer,
		/* [out] */ uint* pNeeded);

	HRESULT GetThreadAllocData(
		CLRDATA_ADDRESS thread,

		byte* data);

	HRESULT GetHeapAllocData(
		uint count,

		byte* data,
		uint* pNeeded);

	HRESULT GetFailedAssemblyList(
		CLRDATA_ADDRESS appDomain,
		int count,
		CLRDATA_ADDRESS* values,
		uint* pNeeded);

	HRESULT GetPrivateBinPaths(
		CLRDATA_ADDRESS appDomain,
		int count,
		char* paths,
		uint* pNeeded);

	HRESULT GetAssemblyLocation(
		CLRDATA_ADDRESS assembly,
		int count,
		char* location,
		uint* pNeeded);

	HRESULT GetAppDomainConfigFile(
		CLRDATA_ADDRESS appDomain,
		int count,
		char* configFile,
		uint* pNeeded);

	HRESULT GetApplicationBase(
		CLRDATA_ADDRESS appDomain,
		int count,
		char* @base,
		uint* pNeeded);

	HRESULT GetFailedAssemblyData(
		CLRDATA_ADDRESS assembly,
		uint* pContext,
		HRESULT* pResult);

	HRESULT GetFailedAssemblyLocation(
		CLRDATA_ADDRESS assesmbly,
		uint count,
		char* location,
		uint* pNeeded);

	HRESULT GetFailedAssemblyDisplayName(
		CLRDATA_ADDRESS assembly,
		uint count,
		char* name,
		uint* pNeeded);
}
