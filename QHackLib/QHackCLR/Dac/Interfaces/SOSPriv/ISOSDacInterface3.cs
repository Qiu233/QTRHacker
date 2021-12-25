using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("B08C5CDC-FD8A-49C5-AB38-5FEEF35235B4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface3
	{
		HRESULT GetGCInterestingInfoData(CLRDATA_ADDRESS interestingInfoAddr, DacpGCInterestingInfoData* data);
		HRESULT GetGCInterestingInfoStaticData(DacpGCInterestingInfoData* data);
		HRESULT GetGCGlobalMechanisms(nuint* globalMechanisms);
	}
}
