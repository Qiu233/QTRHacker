using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("75DA9E4C-BD33-43C8-8F5C-96E8A5241F57")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataExceptionState
	{
		HRESULT GetFlags([Out] out uint flags);
		HRESULT GetPrevious([Out] out IXCLRDataExceptionState exState);
		HRESULT GetManagedObject([Out] out IXCLRDataValue value);
		HRESULT GetBaseType([Out] out CLRDataBaseExceptionType type);
		HRESULT GetCode([Out] out uint code);
		HRESULT GetString([In] uint bufLen, [Out] out uint strLen, [Out/*, */] char* str);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT IsSameState([In] in _EXCEPTION_RECORD64 exRecord, [In] uint contextSize, [In/*, */] in byte cxRecord);
		HRESULT IsSameState2([In] uint flags, [In] in _EXCEPTION_RECORD64 exRecord, [In] uint contextSize, [In/*, */] in byte cxRecord);
		HRESULT GetTask([Out] out IXCLRDataTask task);
	}
}
