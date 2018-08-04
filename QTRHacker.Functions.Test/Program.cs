using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				Utils.LowGravity_E(gc);
			}
			//Assembler.Assemble("fldz\nfstp dword [esi+0x410]", 0).ToList().ForEach(t=>Console.WriteLine(t.ToString("X2")));

		}
	}
}
