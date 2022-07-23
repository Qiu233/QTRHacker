using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker
{
	public static class Helpers
	{
		public static string ToHexAddr(this nint v) => v.ToString(IntPtr.Size == 8 ? "X16" : "X8");

		public static string ToHexAddr(this nuint v) => v.ToString(UIntPtr.Size == 8 ? "X16" : "X8");
	}
}
