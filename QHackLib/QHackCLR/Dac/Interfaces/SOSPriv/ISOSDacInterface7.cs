using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("c1020dde-fe98-4536-a53b-f35a74c327eb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface7
	{
		HRESULT GetPendingReJITID(CLRDATA_ADDRESS methodDesc, int* pRejitId);
		HRESULT GetReJITInformation(CLRDATA_ADDRESS methodDesc, int rejitId, DacpReJitData2* pRejitData);
		HRESULT GetProfilerModifiedILInformation(CLRDATA_ADDRESS methodDesc, DacpProfilerILData* pILData);
		HRESULT GetMethodsWithProfilerModifiedIL(CLRDATA_ADDRESS mod, CLRDATA_ADDRESS* methodDescs, int cMethodDescs, int* pcMethodDescs);
	}
}
