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
				Console.WriteLine(context.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF938].StartAddress.ToString("X8"));
				Console.WriteLine(context.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF962].StartAddress.ToString("X8"));
			}
		}
	}
}
