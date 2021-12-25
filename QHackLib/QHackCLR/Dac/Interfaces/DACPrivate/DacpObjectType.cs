using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum DacpObjectType : uint
	{
		OBJ_STRING,
		OBJ_FREE,
		OBJ_OBJECT,
		OBJ_ARRAY,
		OBJ_OTHER,
	}
}
