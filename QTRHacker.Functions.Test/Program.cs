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
				Utils.InfiniteMana_E(gc);
			}
			Assembler.Assemble("sub [esi+0x344],edi\nmov eax,1\nlea esp,[ebp-08]", 0).ToList().ForEach(t=>Console.WriteLine(t.ToString("X2")));

		}
	}
}
