using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	[StructLayout(LayoutKind.Sequential)]
	public readonly struct HookParameters
	{
		public readonly nuint TargetAddress;
		public readonly uint Size;
		public readonly bool IsOnce;
		public readonly bool Preserve;

		public HookParameters(nuint targetAddress, uint size, bool isOnce = false, bool preserve = true)
		{
			TargetAddress = targetAddress;
			Size = size;
			IsOnce = isOnce;
			Preserve = preserve;
		}
	}
}
