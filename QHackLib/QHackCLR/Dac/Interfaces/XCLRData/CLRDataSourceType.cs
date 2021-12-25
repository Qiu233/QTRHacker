using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataSourceType : uint
	{
		CLRDATA_SOURCE_TYPE_INVALID = 0x00,
	}
}
