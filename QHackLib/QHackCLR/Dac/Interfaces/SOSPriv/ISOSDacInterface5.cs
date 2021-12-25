using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("127d6abe-6c86-4e48-8e7b-220781c58101")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface5
	{
		HRESULT GetTieredVersions(CLRDATA_ADDRESS methodDesc, int rejitId, DacpTieredVersionData* nativeCodeAddrs, int cNativeCodeAddrs, int* pcNativeCodeAddrs);
	}
}
