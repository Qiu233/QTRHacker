using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("3E269830-4A2B-4301-8EE2-D6805B29B2FA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSHandleEnum
	{
		HRESULT Next([In] uint count, [Out/*, *//*, */] _SOSHandleData* handles, [Out] out uint pNeeded);
	}
}
