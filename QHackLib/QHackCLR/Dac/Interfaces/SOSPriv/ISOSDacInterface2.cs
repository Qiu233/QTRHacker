using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("A16026EC-96F4-40BA-87FB-5575986FB7AF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface2
	{
		HRESULT GetObjectExceptionData(CLRDATA_ADDRESS objAddr, DacpExceptionObjectData* data);
		HRESULT IsRCWDCOMProxy(CLRDATA_ADDRESS rcwAddr, int* isDCOMProxy);
	}
}
