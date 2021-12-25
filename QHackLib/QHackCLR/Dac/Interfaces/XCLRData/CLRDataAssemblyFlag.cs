using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDataAssemblyFlag : uint
	{
		CLRDATA_ASSEMBLY_DEFAULT = 0x00000000,
	}
}
