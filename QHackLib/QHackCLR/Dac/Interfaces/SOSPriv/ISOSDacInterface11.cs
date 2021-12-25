using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("96BA1DB9-14CD-4492-8065-1CAAECF6E5CF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface11
	{
		HRESULT IsTrackedType(CLRDATA_ADDRESS objAddr, int* isTrackedType, int* hasTaggedMemory);
		HRESULT GetTaggedMemory(CLRDATA_ADDRESS objAddr, CLRDATA_ADDRESS* taggedMemory, nuint* taggedMemorySizeInBytes);
	}
}
