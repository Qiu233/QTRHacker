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
			uint faddr = FunctionAddressHelper.GetFunctionAddress("Terraria.Main::DrawInterface_Resources_Life");


			byte[] bytes = AssemblySnippet.FromDotNetCall(0, 100, 0x2a2a2a2a, 0xFF, 0xEE, 0x99).GetByteCode();


			InlineHook l = new InlineHook();
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
