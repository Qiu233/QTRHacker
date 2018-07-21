using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.FunctionHelper.Assemble;
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


			byte[] bytes = Assembler.Assemble("mov eax,20");

			Ldasm.ldasm_data data = new Ldasm.ldasm_data();
			UInt32 len = Ldasm.ldasm(bytes, ref data, false);

			int addr = AobscanHelper.AobscanASM(context, "sub [edx+00000340H],eax");
			Console.WriteLine("{0:x8}", addr);

			for (int i = 0; i < bytes.Length; i++)
			{
				Console.Write("{0:X2}  ", bytes[i]);
			}
			Console.WriteLine();

			Console.WriteLine("{0:X8}", faddr);
			Console.WriteLine("指令长度：" + len);
			context.Close();
		}
	}
}
