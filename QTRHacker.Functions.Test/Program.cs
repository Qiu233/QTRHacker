using Keystone;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{
				Utils.HarpToTP_E(gc);
				Console.ReadKey();
				Utils.HookHarp_D(gc);
				
				/*MathFunctions c = new MathFunctions(gc.HContext);
				c.Functions.ToList().ForEach(t => Console.WriteLine(t.Key + ":\t" + t.Value.ToString("X8")));*/
			}
		}
	}
}
