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
		public readonly int Size;
		public readonly bool IsOnce;
		public readonly bool RawCode;

		public HookParameters(nuint targetAddress, int size, bool isOnce, bool rawCode)
		{
			TargetAddress = targetAddress;
			Size = size;
			IsOnce = isOnce;
			RawCode = rawCode;
		}
	}
}
