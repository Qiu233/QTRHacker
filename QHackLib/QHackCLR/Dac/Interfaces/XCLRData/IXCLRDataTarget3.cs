using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("59d9b5e1-4a6f-4531-84c3-51d12da22fd4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataTarget3
	{
		HRESULT GetMetaData([In] char* imagePath, [In] uint imageTimestamp, [In] uint imageSize, [In] in Guid mvid, [In] uint mdRva, [In] uint flags, [In] uint bufferSize, [Out/*, *//*, */] out byte buffer, [Out] out uint dataSize);
	}
}
