using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public class FunctionAddressHelper
	{
		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern void InitCL(UInt32 pid, bool dotnet, string moduleName);

		[DllImport("CheatLibrary.dll", CharSet = CharSet.Unicode)]
		private static extern unsafe bool SearchFunctionByName(string fullName, void** addr, UInt32 times);

		private FunctionAddressHelper() { }

		public static void Initialize(UInt32 pid, string moduleName)
		{
			InitCL(pid, true, moduleName);
		}

		public static unsafe UInt32 GetFunctionAddress(string fullName, UInt32 times = 1)
		{
			void* a;
			SearchFunctionByName(fullName, &a, times);
			return (UInt32)a;
		}

	}
}
