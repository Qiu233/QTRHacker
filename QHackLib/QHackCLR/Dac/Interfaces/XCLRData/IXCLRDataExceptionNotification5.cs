using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("e77a39ea-3548-44d9-b171-8569ed1a9423")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataExceptionNotification5
	{
		HRESULT OnCodeGenerated2([In] IXCLRDataMethodInstance method, [In] CLRDATA_ADDRESS nativeCodeLocation);
	}
}
