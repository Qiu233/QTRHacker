using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("1C4D9A4B-702D-4CF6-B290-1DB6F43050D0")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataFrame2
	{
		HRESULT GetExactGenericArgsToken([Out] out IXCLRDataValue genericToken);
	}
}
