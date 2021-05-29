using Keystone;
using Microsoft.Diagnostics.Runtime;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace QTRHacker.Functions.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id))
			{

			}

		}
	}
}
