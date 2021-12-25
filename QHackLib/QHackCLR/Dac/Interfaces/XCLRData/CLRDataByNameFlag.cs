using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataByNameFlag : uint
	{
		CLRDATA_BYNAME_CASE_SENSITIVE = 0x00000000,
		CLRDATA_BYNAME_CASE_INSENSITIVE = 0x00000001,
	}
}
