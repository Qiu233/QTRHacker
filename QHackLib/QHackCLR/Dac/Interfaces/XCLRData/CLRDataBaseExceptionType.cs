using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataBaseExceptionType : uint
	{
		CLRDATA_EXBASE_EXCEPTION,
		CLRDATA_EXBASE_OUT_OF_MEMORY,
		CLRDATA_EXBASE_INVALID_ARGUMENT,
	}
}
