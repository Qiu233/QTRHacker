using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLibTest
{
	class Program
	{
		static void Main(string[] args)
		{
			Process[] localByName = Process.GetProcessesByName("Terraria");
			int pid = localByName[0].Id;
			using (Context context = Context.Create(pid))
			{
				var r = AobscanHelper.Aobscan(context, "D8 0D 38 0F ED 50", false, 0x50EB6FE9);
				NativeFunctions.MEMORY_BASIC_INFORMATION mbi;
				var flag = NativeFunctions.VirtualQueryEx(context.Handle, 0x50EB6FE9, out mbi, 28);
				Console.WriteLine(mbi.BaseAddress.ToString("X8"));
				Console.WriteLine(r.ToString("X8"));
			}
		}
	}
}
