using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("31201a94-4337-49b7-aef7-0c7550540920")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataExceptionNotification3
	{
		HRESULT OnGcEvent([In] GcEvtArgs gcEvtArgs);
	}
}
