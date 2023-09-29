using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DAC.Defs;

[ComImport, Guid("5c552ab6-fc09-4cb3-8e36-22fa03c798b7"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal unsafe interface IXCLRDataProcess
{
	HRESULT Flush();

	HRESULT StartEnumTasks(
	   /* [out] */ byte* handle);

	HRESULT EnumTask(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** task);

	HRESULT EndEnumTasks(
	   /* [in] */ ulong handle);

	HRESULT GetTaskByOSThreadID(
	   /* [in] */ uint osThreadID,
	   /* [out] */ byte** task);

	HRESULT GetTaskByUniqueID(
	   /* [in] */ ulong taskID,
	   /* [out] */ byte** task);

	HRESULT GetFlags(
	   /* [out] */ byte* flags);

	HRESULT IsSameObject(
	   /* [in] */ byte* process);

	HRESULT GetManagedObject(
	   /* [out] */ byte** value);

	HRESULT GetDesiredExecutionState(
	   /* [out] */ byte* state);

	HRESULT SetDesiredExecutionState(
	   /* [in] */ uint state);

	HRESULT GetAddressType(
	   /* [in] */ CLRDATA_ADDRESS address,
	   /* [out] */ byte* type);

	HRESULT GetRuntimeNameByAddress(
	   /* [in] */ CLRDATA_ADDRESS address,
	   /* [in] */ uint flags,
	   /* [in] */ uint bufLen,
	   /* [out] */ byte* nameLen,
	   /* [size_is][out] */ char* nameBuf,
	   /* [out] */ byte* displacement);

	HRESULT StartEnumAppDomains(
	   /* [out] */ byte* handle);

	HRESULT EnumAppDomain(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** appDomain);

	HRESULT EndEnumAppDomains(
	   /* [in] */ ulong handle);

	HRESULT GetAppDomainByUniqueID(
	   /* [in] */ ulong id,
	   /* [out] */ out IXCLRDataAppDomain appDomain);

	HRESULT StartEnumAssemblies(
	   /* [out] */ byte* handle);

	HRESULT EnumAssembly(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** assembly);

	HRESULT EndEnumAssemblies(
	   /* [in] */ ulong handle);

	HRESULT StartEnumModules(
	   /* [out] */ byte* handle);

	HRESULT EnumModule(
	   /* [out][in] */ byte* handle,
	   /* [out] */ byte** mod);

	HRESULT EndEnumModules(
	   /* [in] */ ulong handle);

	HRESULT GetModuleByAddress(
	   /* [in] */ CLRDATA_ADDRESS address,
	   /* [out] */ byte** mod);

	HRESULT StartEnumMethodInstancesByAddress(
	   /* [in] */ CLRDATA_ADDRESS address,
	   /* [in] */ byte* appDomain,
	   /* [out] */ byte* handle);

	HRESULT EnumMethodInstanceByAddress(
	   /* [in] */ byte* handle,
	   /* [out] */ byte** method);

	HRESULT EndEnumMethodInstancesByAddress(
	   /* [in] */ ulong handle);

	HRESULT GetDataByAddress(
	   /* [in] */ CLRDATA_ADDRESS address,
	   /* [in] */ uint flags,
	   /* [in] */ byte* appDomain,
	   /* [in] */ byte* tlsTask,
	   /* [in] */ uint bufLen,
	   /* [out] */ byte* nameLen,
	   /* [size_is][out] */ char* nameBuf,
	   /* [out] */ byte** value,
	   /* [out] */ byte* displacement);

	HRESULT GetExceptionStateByExceptionRecord(
	   /* [in] */ byte* record,
	   /* [out] */ byte** exState);

	HRESULT TranslateExceptionRecordToNotification(
	   /* [in] */ byte* record,
	   /* [in] */ byte* notify);

	HRESULT Request(
	   /* [in] */ uint reqCode,
	   /* [in] */ uint inBufferSize,
	   /* [size_is][in] */ byte* inBuffer,
	   /* [in] */ uint outBufferSize,
	   /* [size_is][out] */ byte* outBuffer);

	HRESULT CreateMemoryValue(
	   /* [in] */ byte* appDomain,
	   /* [in] */ byte* tlsTask,
	   /* [in] */ byte* type,
	   /* [in] */ CLRDATA_ADDRESS addr,
	   /* [out] */ byte** value);

	HRESULT SetAllTypeNotifications(
	   byte* mod,
	   uint flags);

	HRESULT SetAllCodeNotifications(
	   byte* mod,
	   uint flags);

	HRESULT GetTypeNotifications(
	   /* [in] */ uint numTokens,
	   /* [size_is][in] */ byte** mods,
	   /* [in] */ byte* singleMod,
	   /* [size_is][in] */ uint* tokens,
	   /* [size_is][out] */ uint* flags);

	HRESULT SetTypeNotifications(
	   /* [in] */ uint numTokens,
	   /* [size_is][in] */ byte** mods,
	   /* [in] */ byte* singleMod,
	   /* [size_is][in] */ uint* tokens,
	   /* [size_is][in] */ uint* flags,
	   /* [in] */ uint singleFlags);

	HRESULT GetCodeNotifications(
	   /* [in] */ uint numTokens,
	   /* [size_is][in] */ byte** mods,
	   /* [in] */ byte* singleMod,
	   /* [size_is][in] */ uint* tokens,
	   /* [size_is][out] */ uint* flags);

	HRESULT SetCodeNotifications(
	   /* [in] */ uint numTokens,
	   /* [size_is][in] */ byte** mods,
	   /* [in] */ byte* singleMod,
	   /* [size_is][in] */ uint* tokens,
	   /* [size_is][in] */ uint* flags,
	   /* [in] */ uint singleFlags);

	HRESULT GetOtherNotificationFlags(
	   /* [out] */ byte* flags);

	HRESULT SetOtherNotificationFlags(
	   /* [in] */ uint flags);

	HRESULT StartEnumMethodDefinitionsByAddress(
	   /* [in] */ CLRDATA_ADDRESS address,
	   /* [out] */ byte* handle);

	HRESULT EnumMethodDefinitionByAddress(
	   /* [in] */ byte* handle,
	   /* [out] */ byte** method);

	HRESULT EndEnumMethodDefinitionsByAddress(
	   /* [in] */ ulong handle);

	HRESULT FollowStub(
	   /* [in] */ uint inFlags,
	   /* [in] */ CLRDATA_ADDRESS inAddr,
	   /* [in] */ byte* inBuffer,
	   /* [out] */ byte* outAddr,
	   /* [out] */ byte* outBuffer,
	   /* [out] */ byte* outFlags);

	HRESULT FollowStub2(
	   /* [in] */ byte* task,
	   /* [in] */ uint inFlags,
	   /* [in] */ CLRDATA_ADDRESS inAddr,
	   /* [in] */ byte* inBuffer,
	   /* [out] */ byte* outAddr,
	   /* [out] */ byte* outBuffer,
	   /* [out] */ byte* outFlags);

	HRESULT DumpNativeImage(
	   /* [in] */ CLRDATA_ADDRESS loadedBase,
	   /* [in] */ string name,
	   /* [in] */ byte* display,
	   /* [in] */ byte* libSupport,
	   /* [in] */ byte* dis);

}
