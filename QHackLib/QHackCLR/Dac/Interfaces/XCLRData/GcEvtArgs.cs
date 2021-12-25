using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Dac.Interfaces
{
	[StructLayout(LayoutKind.Sequential)]
	public struct GcEvtArgs
	{
		public GcEvt_t Typ;
		public int CondemnedGeneration;
	}
}
