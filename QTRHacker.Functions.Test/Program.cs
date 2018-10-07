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
			ProjectileImage.ProjImage a = ProjectileImage.ProjImage.FromImage("./test.png");
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{
				a.Emit(gc, gc.MyPlayer.X, gc.MyPlayer.Y);
				//Projectile.NewProjectile(gc, gc.MyPlayer.X, gc.MyPlayer.Y, 0, 0, 27, 100, 0);
				//Console.WriteLine(gc.HContext.FunctionAddressHelper.FunctionsAddress["Terraria.Projectile::NewProjectile"].ToString("X8"));
			}
		}
	}
}
