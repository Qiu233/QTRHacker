using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
				/*var ah = context.GetAddressHelper("Terraria.exe");
				var clrmethod = ah.GetClrMethod("Terraria.Main", "get_LocalPlayer");
				int offset = clrmethod.ILOffsetMap.Max(t => t.ILOffset);
				int rawAddress = (int)clrmethod.ILOffsetMap.First(t => t.ILOffset == offset).StartAddress;
				Console.WriteLine(rawAddress.ToString("X8"));*/
				context.Runtime.Modules.ToList().ForEach(t => Console.WriteLine(t.Name));
				Console.Read();
			}
		}
	}
}
