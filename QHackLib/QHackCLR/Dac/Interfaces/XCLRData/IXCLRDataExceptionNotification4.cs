using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("C25E926E-5F09-4AA2-BBAD-B7FC7F10CFD7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataExceptionNotification4
	{
		HRESULT ExceptionCatcherEnter([In] IXCLRDataMethodInstance catchingMethod, uint catcherNativeOffset);
	}
}
