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
				/*GetNear(gc, "Terraria.Projectile", "AI", 0x20F4E);
				GetAddress(gc, "Terraria.Projectile", "AI", 0x20F4E);
				*/
				/*AssemblySnippet asm = AssemblySnippet.FromEmpty();
				asm.Content.Add(Instruction.Create("push ecx"));
				asm.Content.Add(Instruction.Create("push edx"));
				asm.Content.Add(
					AssemblySnippet.Loop(
						AssemblySnippet.Loop(
							AssemblySnippet.FromClrCall(
								gc.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"), null, false,
								gc.Map.BaseAddress, "[esp+4]", "[esp]", 255),
							gc.MaxTilesY, false),
						gc.MaxTilesX, false));
				asm.Content.Add(Instruction.Create("pop edx"));
				asm.Content.Add(Instruction.Create("pop ecx"));
				Console.WriteLine(asm.GetCode());
				Console.WriteLine("Over");
				Console.Read();*/
				/*Console.WriteLine((gc.MyPlayer.BaseAddress + gc.HContext.MainAddressHelper.GetFieldOffset("Terraria.Player", "wingTimeMax")).ToString("X8"));
				Console.WriteLine(gc.HContext.MainAddressHelper.GetFieldOffset("Terraria.Item", "stack").ToString("X8"));
				Console.WriteLine(AobscanHelper.AobscanASM(gc.HContext.Handle, "dec dword ptr [ecx+0xA8]\nmov eax,[ebp+0xC]").ToString("X8"));
				Console.WriteLine(AobscanHelper.AobscanASM(gc.HContext.Handle, "dec dword ptr [eax+0xA8]\nmov eax,[ebp-0x20]").ToString("X8"));*/
				Console.WriteLine(gc.MyPlayer.Bank.Item.Length);
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
