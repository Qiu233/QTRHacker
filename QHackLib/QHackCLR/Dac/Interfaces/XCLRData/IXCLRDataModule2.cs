using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[ComImport, Guid("34625881-7EB3-4524-817B-8DB9D064C760")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public unsafe interface IXCLRDataModule2
	{
		HRESULT SetJITCompilerFlags([In] uint dwFlags);
	}
}
