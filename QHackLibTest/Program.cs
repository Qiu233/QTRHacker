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
			Process[] localByName = Process.GetProcessesByName("Terraria");
			string pname = localByName[0].ProcessName;
			int pid = localByName[0].Id;
			context = Context.Create(pname, (UInt32)pid);

			FunctionAddressHelper.Initialize((UInt32)pid, "Terraria.exe");

			int faddr = AobscanHelper.AobscanASM(context, "sub [edx+00000340],eax");
			InlineHook l = new InlineHook(context);
			AssemblySnippet ass = AssemblySnippet.FromASMCode("mov eax,50");
			l.Inject(ass, (UInt32)faddr);

			Console.WriteLine("{0:X8}", faddr);
			context.Close();
		}
	}
}
