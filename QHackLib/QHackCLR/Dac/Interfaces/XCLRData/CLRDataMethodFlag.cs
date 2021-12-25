using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataMethodFlag : uint
	{
		CLRDATA_METHOD_DEFAULT = 0x00000000,
		CLRDATA_METHOD_HAS_THIS = 0x00000001,
	}
}
