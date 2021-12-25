using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("E59D8D22-ADA7-49a2-89B5-A415AFCFC95F")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataStackWalk
	{
		HRESULT GetContext([In] uint contextFlags, [In] uint contextBufSize, [Out] out uint contextSize, [Out/*, */] out byte contextBuf);
		HRESULT SetContext([In] uint contextSize, [In/*, */] in byte context);
		HRESULT Next();
		HRESULT GetStackSizeSkipped([Out] out ulong stackSizeSkipped);
		HRESULT GetFrameType([Out] out CLRDataSimpleFrameType simpleType, [Out] out CLRDataDetailedFrameType detailedType);
		HRESULT GetFrame([Out] out IXCLRDataFrame frame);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT SetContext2([In] uint flags, [In] uint contextSize, [In/*, */] in byte context);
	}
}
