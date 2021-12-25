using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("11206399-4B66-4EDB-98EA-85654E59AD45")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface6
	{
		HRESULT GetMethodTableCollectibleData(CLRDATA_ADDRESS mt, DacpMethodTableCollectibleData* data);
	}
}
