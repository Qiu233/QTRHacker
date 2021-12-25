using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.DataTargets
{
	public enum ClrFlavor
	{
		/// <summary>
		/// This is the full version of CLR included with windows.
		/// </summary>
		Desktop = 0,

		/// <summary>
		/// For .NET Core
		/// </summary>
		Core = 3
	}
}
