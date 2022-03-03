using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Functions.GameObjects.Terraria;
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
		}
	}
}
