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
			using (GameContext gc = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0].Id, e => { Console.WriteLine("You're probably not playing the version 1.3.5.3"); }))
			{
				//Console.WriteLine("{0:X8}", BitConverter.ToInt32(BitConverter.GetBytes(4f), 0));
				GetNear(gc, "Terraria.Player", "Update", 0x2CDD);
				GetAddress(gc, "Terraria.Player", "Update", 0x2CDD);

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
