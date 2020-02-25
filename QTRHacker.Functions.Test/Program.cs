using Keystone;
using Microsoft.Diagnostics.Runtime;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
				var myPlayer = gc.MyPlayer;

				var chars = CharactersLoader.LoadCharacters(File.ReadAllText("./RainbowFonts/ASCII/Numbers.rbfont"));
				RainbowTextDrawer rtd = new RainbowTextDrawer(chars);
				rtd.DrawString("453858025", new MPointF());
				rtd.Emit(gc, new MPointF(myPlayer.X, myPlayer.Y));
				Console.Read();
			}
		}
	}
}
