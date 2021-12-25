using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("8FA642BD-9F10-4799-9AA3-512AE78C77EE")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSStackRefEnum
	{
		HRESULT Next([In] uint count, [Out/*, *//*, */] _SOS_StackRefData* _ref, [Out] out uint pFetched);
		HRESULT EnumerateErrors([Out] out ISOSStackRefErrorEnum ppEnum);
	}
}
