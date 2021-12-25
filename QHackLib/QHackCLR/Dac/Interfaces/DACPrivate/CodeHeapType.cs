using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[Flags]
	public enum CodeHeapType : uint
	{
		CODEHEAP_LOADER,
		CODEHEAP_HOST,
		CODEHEAP_UNKNOWN,
	}
}
