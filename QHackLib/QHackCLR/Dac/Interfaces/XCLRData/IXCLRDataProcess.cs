using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("5c552ab6-fc09-4cb3-8e36-22fa03c798b7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataProcess
	{
		HRESULT Flush();
		HRESULT StartEnumTasks([Out] out nuint handle);
		HRESULT EnumTask([In, Out] ref nuint handle, [Out] out IXCLRDataTask task);
		HRESULT EndEnumTasks([In] nuint handle);
		HRESULT GetTaskByOSThreadID([In] uint osThreadID, [Out] out IXCLRDataTask task);
		HRESULT GetTaskByUniqueID([In] ulong taskID, [Out] out IXCLRDataTask task);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataProcess process);
		HRESULT GetManagedObject([Out] out IXCLRDataValue value);
		HRESULT GetDesiredExecutionState([Out] out uint state);
		HRESULT SetDesiredExecutionState([In] uint state);
		HRESULT GetAddressType([In] CLRDATA_ADDRESS address, [Out] out CLRDataAddressType type);
		HRESULT GetRuntimeNameByAddress([In] CLRDATA_ADDRESS address, [In] uint flags, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out CLRDATA_ADDRESS displacement);
		HRESULT StartEnumAppDomains([Out] out nuint handle);
		HRESULT EnumAppDomain([In, Out] ref nuint handle, [Out] out IXCLRDataAppDomain appDomain);
		HRESULT EndEnumAppDomains([In] nuint handle);
		HRESULT GetAppDomainByUniqueID([In] ulong id, [Out] out IXCLRDataAppDomain appDomain);
		HRESULT StartEnumAssemblies([Out] out nuint handle);
		HRESULT EnumAssembly([In, Out] ref nuint handle, [Out] out IXCLRDataAssembly assembly);
		HRESULT EndEnumAssemblies([In] nuint handle);
		HRESULT StartEnumModules([Out] out nuint handle);
		HRESULT EnumModule([In, Out] ref nuint handle, [Out] out IXCLRDataModule mod);
		HRESULT EndEnumModules([In] nuint handle);
		HRESULT GetModuleByAddress([In] CLRDATA_ADDRESS address, [Out] out IXCLRDataModule mod);
		HRESULT StartEnumMethodInstancesByAddress([In] CLRDATA_ADDRESS address, [In] IXCLRDataAppDomain appDomain, [Out] out nuint handle);
		HRESULT EnumMethodInstanceByAddress([In] in nuint handle, [Out] out IXCLRDataMethodInstance method);
		HRESULT EndEnumMethodInstancesByAddress([In] nuint handle);
		HRESULT GetDataByAddress([In] CLRDATA_ADDRESS address, [In] uint flags, [In] IXCLRDataAppDomain appDomain, [In] IXCLRDataTask tlsTask, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf, [Out] out IXCLRDataValue value, [Out] out CLRDATA_ADDRESS displacement);
		HRESULT GetExceptionStateByExceptionRecord([In] in _EXCEPTION_RECORD64 record, [Out] out IXCLRDataExceptionState exState);
		HRESULT TranslateExceptionRecordToNotification([In] in _EXCEPTION_RECORD64 record, [In] IXCLRDataExceptionNotification notify);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT CreateMemoryValue([In] IXCLRDataAppDomain appDomain, [In] IXCLRDataTask tlsTask, [In] IXCLRDataTypeInstance type, [In] CLRDATA_ADDRESS addr, [Out] out IXCLRDataValue value);
		HRESULT SetAllTypeNotifications(IXCLRDataModule mod, uint flags);
		HRESULT SetAllCodeNotifications(IXCLRDataModule mod, uint flags);
		HRESULT GetTypeNotifications([In] uint numTokens, [In/*, */] IXCLRDataModule[] mods, [In] IXCLRDataModule singleMod, [In/*, */] in int tokens, [Out/*, */] out uint flags);
		HRESULT SetTypeNotifications([In] uint numTokens, [In/*, */] IXCLRDataModule[] mods, [In] IXCLRDataModule singleMod, [In/*, */] in int tokens, [In/*, */] in uint flags, [In] uint singleFlags);
		HRESULT GetCodeNotifications([In] uint numTokens, [In/*, */] IXCLRDataModule[] mods, [In] IXCLRDataModule singleMod, [In/*, */] in int tokens, [Out/*, */] out uint flags);
		HRESULT SetCodeNotifications([In] uint numTokens, [In/*, */] IXCLRDataModule[] mods, [In] IXCLRDataModule singleMod, [In/*, */] in int tokens, [In/*, */] in uint flags, [In] uint singleFlags);
		HRESULT GetOtherNotificationFlags([Out] out uint flags);
		HRESULT SetOtherNotificationFlags([In] uint flags);
		HRESULT StartEnumMethodDefinitionsByAddress([In] CLRDATA_ADDRESS address, [Out] out nuint handle);
		HRESULT EnumMethodDefinitionByAddress([In] in nuint handle, [Out] out IXCLRDataMethodDefinition method);
		HRESULT EndEnumMethodDefinitionsByAddress([In] nuint handle);
		HRESULT FollowStub([In] uint inFlags, [In] CLRDATA_ADDRESS inAddr, [In] CLRDATA_FOLLOW_STUB_BUFFER* inBuffer, [Out] out CLRDATA_ADDRESS outAddr, [Out] out CLRDATA_FOLLOW_STUB_BUFFER outBuffer, [Out] out uint outFlags);
		HRESULT FollowStub2([In] IXCLRDataTask task, [In] uint inFlags, [In] CLRDATA_ADDRESS inAddr, [In] CLRDATA_FOLLOW_STUB_BUFFER* inBuffer, [Out] out CLRDATA_ADDRESS outAddr, [Out] out CLRDATA_FOLLOW_STUB_BUFFER outBuffer, [Out] out uint outFlags);
		HRESULT DumpNativeImage([In] CLRDATA_ADDRESS loadedBase, [In] char* name, [In] IXCLRDataDisplay display, [In] IXCLRLibrarySupport libSupport, [In] IXCLRDisassemblySupport dis);
	}
}
