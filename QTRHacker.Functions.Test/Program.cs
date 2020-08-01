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
				/*int f = gc.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Lighting", y => y.GetFullSignature() == "Terraria.Lighting.AddLight(Int32, Int32, Single, Single, Single)");
				int t = gc.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate");
				int map = gc.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "Map");
				for (int i = 100; i < 200; i++)
				{
					for (int j = 100; j < 200; j++)
					{
						InlineHook.InjectAndWait(gc.HContext,
						AssemblySnippet.FromClrCall(f, null, true, i, j, 0x3F800000, 0x3F800000, 0x3F800000)
						, t, true);
					}
				}*/
				/*GetNear(gc, "Terraria.Graphics.Light.TileLightScanner", "ApplyTileLight", 0x2eb6);
				GetAddress(gc, "Terraria.Graphics.Light.TileLightScanner", "ApplyTileLight", 0x2eb6);*/
				GetNear(gc, "Terraria.Player", "ResetEffects", 0x16);
				GetAddress(gc, "Terraria.Player", "ResetEffects", 0x16);

				Console.WriteLine("Over");
				Console.Read();
			}

		}
		static void GetAddress(GameContext ctx, string className, string functionName, int target)
		{
			var s = ctx.HContext.MainAddressHelper[className, functionName, target];
			Console.WriteLine(s.StartAddress.ToString("X8"));
			Console.WriteLine(s.EndAddress.ToString("X8"));
		}
		static void GetNear(GameContext ctx, string className, string functionName, int target)
		{
			var s = ctx.HContext.MainAddressHelper.Module.GetTypeByName(className).Methods.First(t => t.Name == functionName);
			int m1 = 0, m2 = int.MaxValue;
			foreach (var p in s.ILOffsetMap)
			{
				if (p.ILOffset <= target)
					m1 = Math.Max(p.ILOffset, m1);
				if (p.ILOffset >= target)
					m2 = Math.Min(p.ILOffset, m2);
			}
			Console.WriteLine(m1.ToString("X8"));
			Console.WriteLine(m2.ToString("X8"));
		}
	}
}
