using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CLRDATA_IL_OFFSET_MARKER : int
	{
		CLRDATA_IL_OFFSET_NO_MAPPING = -1,
		CLRDATA_IL_OFFSET_PROLOG = -2,
		CLRDATA_IL_OFFSET_EPILOG = -3,
	}
}
