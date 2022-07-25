using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib;

internal static class Utils
{
	public static bool Is32Bit => IntPtr.Size == 4;
}
