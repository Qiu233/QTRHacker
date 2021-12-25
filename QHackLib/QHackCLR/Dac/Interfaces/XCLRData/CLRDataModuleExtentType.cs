using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataModuleExtentType : uint
	{
		CLRDATA_MODULE_PE_FILE,
		CLRDATA_MODULE_PREJIT_FILE,
		CLRDATA_MODULE_MEMORY_STREAM,
		CLRDATA_MODULE_OTHER,
	}
}
