using QHackLib;
using QHackLib.FunctionHelper;
using QHackLib.FunctionHelper.InlineHook;
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
		[DllImport("kernel32.dll")]
		public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
		[DllImport("kernel32.dll")]
		public static extern int CloseHandle(int dwDesiredAccess);

		public const int PROCESS_ALL_ACCESS = 0x1F0FFF;

		private static int Handle;
		static void Main(string[] args)
		{
			Process[] localByName = Process.GetProcessesByName("Terraria");
			int pid = localByName[0].Id;
			Handle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);

			FunctionAddressHelper.Initialize((UInt32)pid, "Terraria.exe");
			uint f = FunctionAddressHelper.GetFunctionAddress("Terraria.Main::DrawInterface_Resources_Life");

			Ldasm.ldasm_data data = new Ldasm.ldasm_data();
			UInt32 len = Ldasm.ldasm(new byte[] { 0x8B, 0xEC, 1, 0, 1, 0, 1 }, ref data, false);

			Console.WriteLine("{0:x8}", f);
			Console.WriteLine("指令长度：" + len);
			CloseHandle(Handle);
		}
	}
}
