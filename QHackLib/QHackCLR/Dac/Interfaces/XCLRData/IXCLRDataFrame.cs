using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("271498C2-4085-4766-BC3A-7F8ED188A173")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataFrame
	{
		HRESULT GetFrameType([Out] out CLRDataSimpleFrameType simpleType, [Out] out CLRDataDetailedFrameType detailedType);
		HRESULT GetContext([In] uint contextFlags, [In] uint contextBufSize, [Out] out uint contextSize, [Out/*, */] out byte contextBuf);
		HRESULT GetAppDomain([Out] out IXCLRDataAppDomain appDomain);
		HRESULT GetNumArguments([Out] out uint numArgs);
		HRESULT GetArgumentByIndex([In] uint index, [Out] out IXCLRDataValue arg, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetNumLocalVariables([Out] out uint numLocals);
		HRESULT GetLocalVariableByIndex([In] uint index, [Out] out IXCLRDataValue localVariable, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* name);
		HRESULT GetCodeName([In] uint flags, [In] uint bufLen, [Out] out uint nameLen, [Out/*, */] char* nameBuf);
		HRESULT GetMethodInstance([Out] out IXCLRDataMethodInstance method);
		HRESULT Request([In] uint reqCode, [In] uint inBufferSize, [In/*, */] in byte inBuffer, [In] uint outBufferSize, [Out/*, */] out byte outBuffer);
		HRESULT GetNumTypeArguments([Out] out uint numTypeArgs);
		HRESULT GetTypeArgumentByIndex([In] uint index, [Out] out IXCLRDataTypeInstance typeArg);
	}
}
