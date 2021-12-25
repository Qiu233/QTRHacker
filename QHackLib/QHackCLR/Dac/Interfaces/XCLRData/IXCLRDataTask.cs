using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("A5B0BEEA-EC62-4618-8012-A24FFC23934C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataTask
	{
		HRESULT GetProcess([Out] out IXCLRDataProcess process);
		HRESULT GetCurrentAppDomain([Out] out IXCLRDataAppDomain appDomain);
		HRESULT GetUniqueID([Out] out ulong id);
		HRESULT GetFlags([Out] out uint flags);
		HRESULT IsSameObject([In] IXCLRDataTask task);
		HRESULT GetManagedObject([Out] out IXCLRDataValue value);
		HRESULT GetDesiredExecutionState([Out] out uint state);
		HRESULT SetDesiredExecutionState([In] uint state);
		HRESULT CreateStackWalk([In] uint flags, [Out] out IXCLRDataStackWalk stackWalk);
		HRESULT GetOSThreadID([Out] out uint id);
		HRESULT GetContext([In] uint contextFlags, [In] uint contextBufSize, [Out] out uint contextSize, [Out/*, */] byte* contextBuf);
		HRESULT SetContext([In] uint contextSize, [In/*, */] byte* context);
		HRESULT GetCurrentExceptionState([Out] out IXCLRDataExceptionState exception);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] byte* outBuffer);
		HRESULT GetName([In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetLastExceptionState([Out] out IXCLRDataExceptionState exception);
	}
}
