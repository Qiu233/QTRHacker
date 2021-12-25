using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataGeneralRequest : uint
	{
		CLRDATA_REQUEST_REVISION = 0xe0000000,
	}
}
