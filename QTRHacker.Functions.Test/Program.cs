using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id);
			Console.WriteLine(gc.MyPlayer.Inventory[0].Type);
			gc.Close();
		}
	}
}
