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
				context.Runtime.Modules.ToList().ForEach(t => Console.WriteLine(t));
				Console.Read();
			}
		}
	}
}
