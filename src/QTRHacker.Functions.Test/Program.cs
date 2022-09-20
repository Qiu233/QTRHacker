using QHackCLR.Common;
using QHackCLR.DataTargets;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Core;
using QTRHacker.Core.GameObjects;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test
{
	enum FishingMode : byte
	{
		Disabled = 0, // do nothing
		ForceQuest, // force quest item
		ForceItem, // only the specified item will be caught
		ForceCrates, // only crates will be caught
		ForceEnemies, // only enemies will be caught
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct FishingData
	{
		public FishingMode Mode;
		public sbyte ForceLevel; // -1 means non-forceful
		public int ForceItemType;
	}
	class Program
	{
		public static unsafe int GetOffset(GameContext context, string module, string type, string field) => (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
		public static unsafe int GetOffset(GameContext context, string type, string field) => (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
		unsafe static void Main()
		{
			using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
			/*byte off = (byte)-((byte)(ctx.GameModuleHelper.GetClrType("Terraria.DataStructures.FishingAttempt").DataSize + 0xC));
			Console.WriteLine(off.ToString("X2"));
			nuint addr = AobscanHelper.AobscanMatch(ctx.HContext.Handle, $"8D55 {off:X2} 8BCB FF15 ******** 33C0").FirstOrDefault();
			if (addr != 0)
			{
				var hook = InlineHook.Hook(ctx.HContext, AssemblySnippet.FromCode(new AssemblyCode[] {
					(Instruction)"pushad",

					// if state.ForceLevel != -1:
					//   set common, uncommon, rare, veryrare, legendary
					// if Mode == ForceQuest:
					//   set rolledEnemySpawn = 0
					//   set rolledItemDrop = QuestItem
					//   jmp label_end
					// if Mode == ForceItem:
					//   set rolledEnemySpawn = 0
					//   set rolledItemDrop = state.ForceItemType
					//   jmp label_end
					// if Mode == ForceCrates && !crate:
					//   set rolledEnemySpawn = 0
					//   set crate = true
					//   jmp call_FishingCheck_RollItemDrop
					// if Mode == ForceEnemies && rolledEnemySpawn == 0:
					//   set rolledItemDrop = 0
					//   jmp call_FishingCheck_RollEnemySpawns

					(Instruction)"label_end:",
					(Instruction)"popad",
				}), new HookParameters(addr + 0xB, 4096, false, true));
				Console.WriteLine(hook.MemoryAllocation.AllocationBase.ToString("X8"));
				Console.Read();
				InlineHook.FreeHook(ctx.HContext, addr, true);
			}*//*
			ctx.MyPlayer.ControlUseItem = true;
			ctx.MyPlayer.ReleaseUseItem = true;
			ctx.RunByHookUpdate(AssemblySnippet.FromClrCall(
					ctx.GameModuleHelper.GetFunctionAddress("Terraria.Player", "ItemCheck"), true, ctx.MyPlayer.BaseAddress, null, null,
					new object[] { ctx.MyPlayerIndex }));*/
			foreach (var m in ctx.HContext.Runtime.AppDomain.Modules)
			{
			}
			Console.Read();
		}
	}
}