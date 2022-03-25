using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
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
			var pos = ctx.MyPlayer.Position;
			Projectile.NewProjectile(ctx, null, pos.X, pos.Y, 0, 0, 502, 0, 0, 255, 0, 0.5f);
		}
	}
}
