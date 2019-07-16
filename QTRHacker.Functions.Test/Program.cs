using Keystone;
using Microsoft.Diagnostics.Runtime;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.ProjectileImage;
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
				var player = gc.MyPlayer;
				var itm = player.Inventory[0];
				itm.SetDefaults(3063);
				itm.Stack = 1;
				itm.Damage = 2000;
				Console.ReadKey();
			}
		}
	}
}
