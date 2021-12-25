using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("90B8FCC3-7251-4B0A-AE3D-5C13A67EC9AA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface10
	{
		HRESULT GetObjectComWrappersData(CLRDATA_ADDRESS objAddr, CLRDATA_ADDRESS* rcw, uint count, CLRDATA_ADDRESS* mowList, uint* pNeeded);
		HRESULT IsComWrappersCCW(CLRDATA_ADDRESS ccw, int* isComWrappersCCW);
		HRESULT GetComWrappersCCWData(CLRDATA_ADDRESS ccw, CLRDATA_ADDRESS* managedObject, int* refCount);
		HRESULT IsComWrappersRCW(CLRDATA_ADDRESS rcw, int* isComWrappersRCW);
		HRESULT GetComWrappersRCWData(CLRDATA_ADDRESS rcw, CLRDATA_ADDRESS* identity);
	}
}
