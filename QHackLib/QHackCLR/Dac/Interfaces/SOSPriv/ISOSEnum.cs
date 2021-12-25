using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("286CA186-E763-4F61-9760-487D43AE4341")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSEnum
	{
		HRESULT Skip([In] uint count);
		HRESULT Reset();
		HRESULT GetCount([Out] out uint pCount);
	}
}
