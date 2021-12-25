using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("774F4E1B-FB7B-491B-976D-A8130FE355E9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSStackRefErrorEnum
	{
		HRESULT Next([In] uint count, [Out/*, *//*, */] _SOS_StackRefError* _ref, [Out] out uint pFetched);
	}
}
