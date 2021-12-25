using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpGenerationAllocData
	{
		public DacpAllocData AllocData_0;
		public DacpAllocData AllocData_1;
		public DacpAllocData AllocData_2;
		public DacpAllocData AllocData_3;
	}
}
