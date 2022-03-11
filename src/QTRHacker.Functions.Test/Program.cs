using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		unsafe static void Main()
		{
			using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
			var tiles = ctx.Patches.WorldPainter_ClipBoard;
			int width = tiles.GetLength(0);
			int height = tiles.GetLength(1);
			Console.WriteLine(width);
			Console.WriteLine(height);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Console.WriteLine(tiles[i, j].Type);
				}
			}
		}
	}
}
