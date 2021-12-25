using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("4eca42d8-7e7b-4c8a-a116-7bfbf6929267")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface9
	{
		HRESULT GetBreakingChangeVersion(int* pVersion);
	}
}
