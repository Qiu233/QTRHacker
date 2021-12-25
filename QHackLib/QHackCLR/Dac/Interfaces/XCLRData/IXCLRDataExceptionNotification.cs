using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("2D95A079-42A1-4837-818F-0B97D7048E0E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataExceptionNotification
	{
		HRESULT OnCodeGenerated([In] IXCLRDataMethodInstance method);
		HRESULT OnCodeDiscarded([In] IXCLRDataMethodInstance method);
		HRESULT OnProcessExecution([In] uint state);
		HRESULT OnTaskExecution([In] IXCLRDataTask task, [In] uint state);
		HRESULT OnModuleLoaded([In] IXCLRDataModule mod);
		HRESULT OnModuleUnloaded([In] IXCLRDataModule mod);
		HRESULT OnTypeLoaded([In] IXCLRDataTypeInstance typeInst);
		HRESULT OnTypeUnloaded([In] IXCLRDataTypeInstance typeInst);
	}
}
