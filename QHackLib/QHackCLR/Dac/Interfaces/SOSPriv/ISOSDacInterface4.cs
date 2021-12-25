using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("74B9D34C-A612-4B07-93DD-5462178FCE11")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface ISOSDacInterface4
	{
		HRESULT GetClrNotification(CLRDATA_ADDRESS* arguments, int count, int* pNeeded);
	}
}
