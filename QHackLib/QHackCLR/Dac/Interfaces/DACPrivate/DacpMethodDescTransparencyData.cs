using System;
using System.Runtime.InteropServices;
using QHackCLR.Dac.COM;
using QHackCLR.Dac.Utils;
namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct DacpMethodDescTransparencyData
	{
		public int BHasCriticalTransparentInfo;
		public int BIsCritical;
		public int BIsTreatAsSafe;
	}
}
