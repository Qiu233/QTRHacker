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

			byte[] bytes = Assembler.AssembleInstructionBlock("add esp,4", 0);

			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(0x200, 0x3000, 5, 6, 7);
			byte[] s = snippet.GetByteCode(0x500);
			foreach (var ss in s)
			{
				Console.WriteLine("{0:X2}", ss);
			}

			InlineHook l = new InlineHook(context);

			AssemblySnippet ass = AssemblySnippet.FromASMCode("mov eax,200");
			l.Inject(ass, (UInt32)faddr, false, 0);


			for (int i = 0; i < bytes.Length; i++)
			{
				Console.Write("{0:X2}  ", bytes[i]);
			}
			Console.WriteLine();


			Console.WriteLine("{0:X8}", faddr);
			context.Close();
		}
	}
}
