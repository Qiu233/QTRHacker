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

namespace QTRHacker.Functions.Test
{
	class Program
	{
		public static unsafe int GetOffset(GameContext context, string module, string type, string field) => (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
		public static unsafe int GetOffset(GameContext context, string type, string field) => (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
		unsafe static void Main()
		{
			using GameContext ctx = GameContext.OpenGame(Process.GetProcessesByName("Terraria")[0]);
			/*Console.WriteLine(ctx.MyPlayer.ControlCreativeMenu);
			ctx.MyPlayer.ControlCreativeMenu = true;
			Console.WriteLine(ctx.MyPlayer.ControlCreativeMenu);
			Console.WriteLine(ctx.GameModuleHelper
				.GetClrType("Terraria.Player")
				.GetInstanceFieldByName("controlCreativeMenu").Offset.ToString("X8"));*/
			/*var obj = ctx.GameModuleHelper.GetStaticHackObject("Terraria.Main", "CreativeMenu");
			var v = obj.InternalGetMember("<Enabled>k__BackingField");
			Console.WriteLine(v.OffsetBase.ToString("X8"));*/
			//Terraria.GameContent.Creative.CreativeUI::set_Enabled
			/*var hook = InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode("mov byte ptr [ecx+0x20], 1\nret"),
				new HookParameters(ctx.GameModuleHelper.GetFunctionAddress("Terraria.GameContent.Creative.CreativeUI", "set_Enabled"), 4096, false, false));*/
			/*var hook = InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode("mov byte ptr [ecx+0x20], 1"),
				new HookParameters(ctx.GameModuleHelper.GetFunctionAddress("Terraria.GameContent.Creative.CreativeUI", "set_Enabled"), 4096, false, true));*/
			//InlineHook.FreeHook(ctx.HContext, ctx.GameModuleHelper.GetFunctionAddress("Terraria.GameContent.Creative.CreativeUI", "set_Enabled"));

			HackObject c = ctx.MyPlayer.InternalObject.creativeTracker.ItemSacrifices;
			nuint addr = ctx.GameModuleHelper
				.GetFunctionAddress("Terraria.GameContent.Creative.ItemsSacrificedUnlocksTracker", 
				"RegisterItemSacrifice");
			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				AssemblySnippet.Loop(
					AssemblySnippet.FromCode(new AssemblyCode[] {
						(Instruction)$"mov ecx, {c.BaseAddress}",
						(Instruction)$"mov edx, [esp]",
						(Instruction)$"push 9999",
						(Instruction)$"call {addr}",
					}),
					GameConstants.MaxItemTypes, true)
			});
			ctx.RunOnManagedThread(code);
		}
	}
}
