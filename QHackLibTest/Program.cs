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



				int faddr = AobscanHelper.AobscanASM(context, "sub [edx+0x00000340],eax");
				AssemblySnippet ass = AssemblySnippet.FromDotNetCall(context.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::SetDefaults"), null, true, 0x3833D68, 3063, 0);
				InlineHook.InjectAndWait(context, ass, faddr, true);

				Console.WriteLine("{0:X8}", faddr);
			}
		}
	}
}
