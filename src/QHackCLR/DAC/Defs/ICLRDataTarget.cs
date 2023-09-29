using QHackCLR.Common;
using System.Runtime.InteropServices;

namespace QHackCLR.DAC.Defs;

[ComImport, Guid("3E11CCEE-D08B-43e5-AF01-32717A64DA03"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public unsafe interface ICLRDataTarget
{
	HRESULT GetMachineType(
		out uint machineType);

	HRESULT GetPointerSize(
		uint* pointerSize);

	HRESULT GetImageBase(
	   /* [string][in] */ string imagePath,
		CLRDATA_ADDRESS* baseAddress);

	HRESULT ReadVirtual(
		CLRDATA_ADDRESS address,
	   /* [length_is][size_is][out] */ byte* buffer,
		uint bytesRequested,
		uint* bytesRead);

	HRESULT WriteVirtual(
		CLRDATA_ADDRESS address,
	   /* [size_is][in] */ byte* buffer,
		uint bytesRequested,
		uint* bytesWritten);

	HRESULT GetTLSValue(
		uint threadID,
		uint index,
		CLRDATA_ADDRESS* value);

	HRESULT SetTLSValue(
		uint threadID,
		uint index,
		CLRDATA_ADDRESS value);

	HRESULT GetCurrentThreadID(
		uint* threadID);

	HRESULT GetThreadContext(
		uint threadID,
		uint contextFlags,
		uint contextSize,
	   /* [size_is][out] */ byte* context);

	HRESULT SetThreadContext(
		uint threadID,
		uint contextSize,
	   /* [size_is][in] */ byte* context);

	HRESULT Request(
		uint reqCode,
		uint inBufferSize,
	   /* [size_is][in] */ byte* inBuffer,
		uint outBufferSize,
	   /* [size_is][out] */ byte* outBuffer);
}
