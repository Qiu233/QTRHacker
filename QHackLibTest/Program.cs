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
		public static Context context;
		static void Main(string[] args)
		{
			/*Process[] localByName = Process.GetProcessesByName("Terraria");
			int pid = localByName[0].Id;
			context = Context.Create(pid);

			FunctionAddressHelper.Initialize((UInt32)pid, "Terraria.exe");
			UInt32 func = FunctionAddressHelper.GetFunctionAddress("Terraria.Item::SetDefaults");




			int faddr = AobscanHelper.AobscanASM(context, "sub [edx+0x00000340],eax");
			InlineHook l = new InlineHook(context);
			AssemblySnippet ass = AssemblySnippet.FromDotNetCall(func, null, 0x3833D68, 3063, 0);
			int addr = l.Inject(ass, faddr);
			Console.WriteLine("{0:X8}", addr);
			
			Console.WriteLine("{0:X8}", faddr);
			
			


			context.Close();*/
		}
	}
}
