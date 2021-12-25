using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataAppDomainFlag : uint
	{
		CLRDATA_DOMAIN_DEFAULT = 0x00000000,
	}
}
