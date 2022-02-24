using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Memory;
using QTRHacker.Functions.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public static class Utils
	{
		public unsafe static int GetOffset(GameContext ctx, string module, string type, string field)
		{
			return (int)ctx.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
		}
		public unsafe static int GetOffset(GameContext ctx, string type, string field)
		{
			return (int)ctx.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
		}
		public static void AobReplaceASM(GameContext ctx, string src, string target)
		{
			var addrs = AobscanHelper.Aobscan(ctx.HContext.Handle, Assembler.Assemble(src, 0));
			byte[] code = Assembler.Assemble(target, 0);
			foreach (var addr in addrs)
				ctx.HContext.DataAccess.WriteBytes(addr, code);
		}
		public static void AobReplace(GameContext ctx, string srcHex, string targetHex)
		{
			var addrs = AobscanHelper.Aobscan(ctx.HContext.Handle, AobscanHelper.GetHexCodeFromString(srcHex));
			byte[] code = AobscanHelper.GetHexCodeFromString(targetHex);
			foreach (var addr in addrs)
				ctx.HContext.DataAccess.WriteBytes(addr, code);
		}

		public static void InfiniteLife_E(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "statLife");
			AobReplaceASM(ctx, $"sub [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"add [edx+{off}],eax");
		}
		public static void InfiniteLife_D(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "statLife");
			AobReplaceASM(ctx, $"add [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"sub [edx+{off}],eax");
		}

		public static void InfiniteMana_E(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "statMana");
			AobReplaceASM(ctx, $"sub [esi+{off}],edi", $"add [esi+{off}],edi");
			AobReplaceASM(ctx, $"sub [esi+{off}],eax", $"add [esi+{off}],eax");
		}
		public static void InfiniteMana_D(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "statMana");
			AobReplaceASM(ctx, $"add [esi+{off}],edi", $"sub [esi+{off}],edi");
			AobReplaceASM(ctx, $"add [esi+{off}],eax", $"sub [esi+{off}],eax");
		}

		public static void InfiniteOxygen_E(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "breath");
			AobReplaceASM(ctx, $"dec dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"inc dword ptr [eax+{off}]");
		}
		public static void InfiniteOxygen_D(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "breath");
			AobReplaceASM(ctx, $"inc dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"dec dword ptr [eax+{off}]");
		}

		public static void InfiniteMinion_E(GameContext ctx)
		{
			int offA = GetOffset(ctx, "Terraria.Player", "maxMinions");
			int offB = GetOffset(ctx, "Terraria.Player", "maxTurrets");
			AobReplaceASM(ctx, $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1", $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999");
		}
		public static void InfiniteMinion_D(GameContext ctx)
		{
			int offA = GetOffset(ctx, "Terraria.Player", "maxMinions");
			int offB = GetOffset(ctx, "Terraria.Player", "maxTurrets");
			AobReplaceASM(ctx, $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999", $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1");
		}

		public static void InfiniteItemAmmo_E(GameContext ctx)
		{
			AobReplace(ctx, "FF 88 B0 00 00 00 8B 45 E0 83 B8", "90 90 90 90 90 90");//dec dword ptr [eax+0xB0]\nmov eax,[ebp-0x20]\ncmp
			AobReplace(ctx, "FF 89 B0 00 00 00 8B 45 0C 8B 55 F4", "90 90 90 90 90 90");//dec dword ptr [ecx+0xB0]\nmov eax,[ebp+0xC]\nmov edx[ebp-0xC]
		}
		public static void InfiniteItemAmmo_D(GameContext ctx)
		{
			AobReplace(ctx, "90 90 90 90 90 90 8B 45 E0 83 B8", "FF 88 B0 00 00 00");
			AobReplace(ctx, "90 90 90 90 90 90 8B 45 0C 8B 55 F4", "FF 89 B0 00 00 00");
		}

		public static void InfiniteFly_E(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "wingTime");
			AobReplace(ctx, $"D9 99 {AobscanHelper.GetMByteCode(off)} 80 B9 F7060000 00", "90 90 90 90 90 90");
		}
		public static void InfiniteFly_D(GameContext ctx)
		{
			int off = GetOffset(ctx, "Terraria.Player", "wingTime");
			AobReplace(ctx, "90 90 90 90 90 90 80 B9 F7060000 00", $"D9 99 {AobscanHelper.GetMByteCode(off)}");
		}

		/*public static void HighLight_E(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				@"C7 ** ** ******** D9 07 D9 45 F0 DF F1 DD D8 7A", true);
			if (a <= 0)
				return;
			InlineHook.Inject(ctx.HContext,
				AssemblySnippet.FromASMCode(
					@"mov dword ptr[ebp-0x10],0x3F800000
mov dword ptr[ebp-0x14],0x3F800000
mov dword ptr[ebp-0x18],0x3F800000"
),
					a + 7, false
				);
		}

		public static void HighLight_D(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(ctx.HContext.Handle, "C7 ** ** ******** E9 ** ** ** ** DF F1 DD D8 7A", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(ctx.HContext, a + 7);
		}*/

		public static void GhostMode_E(GameContext ctx)
		{
			ctx.MyPlayer.Ghost = true;
		}
		public static void GhostMode_D(GameContext ctx)
		{
			ctx.MyPlayer.Ghost = false;
		}

		public static void LowGravity_E(GameContext ctx)
		{
			int offA = GetOffset(ctx, "Terraria.Player", "slowFall");
			int offB = GetOffset(ctx, "Terraria.Player", "findTreasure");
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
			if (a == 0)
				return;
			InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offA}],1"),
				new HookParameters(a, 0x1000, false, false));
		}
		public static void LowGravity_D(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(GetOffset(ctx, "Terraria.Player", "findTreasure"))}").FirstOrDefault();
			if (a == 0)
				return;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void FastSpeed_E(GameContext ctx)
		{
			int offA = GetOffset(ctx, "Terraria.Player", "moveSpeed");
			int offB = GetOffset(ctx, "Terraria.Player", "boneArmor");
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"D9 E8 D9 9E {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
			if (a == 0)
				return;
			InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offA}],0x41A00000"),
				new HookParameters(a, 0x1000, false, false));
		}
		public static void FastSpeed_D(GameContext ctx)
		{
			int offB = GetOffset(ctx, "Terraria.Player", "boneArmor");
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"E9 ******** 90 90 90 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
			if (a == 0) return;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		/*[Obsolete("for the game of old version")]
		public static void ProjectileIgnoreTile_E(GameContext ctx)
		{
			int a = AobscanHelper.AobscanASM(
				ctx.HContext.Handle,
				"mov [ebp-0x20],eax\ncmp byte ptr [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, a, new byte[] { 0x8d }, 1, 0);
		}
		[Obsolete("for the game of old version")]
		public static void ProjectileIgnoreTile_D(GameContext ctx)
		{
			int a = AobscanHelper.AobscanASM(
				ctx.HContext.Handle,
				"mov [ebp-0x20],eax\ncmp byte ptr [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, a, new byte[] { 0x84 }, 1, 0);
		}*/

		public static void GrabItemFarAway_E(GameContext ctx)
		{
			nuint a = ctx.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
			if (ctx.HContext.DataAccess.Read<byte>(a) == 0xE9)
				return;
			InlineHook.Hook(ctx.HContext, AssemblySnippet.FromASMCode(
				"mov eax,1000\nret"),
				new HookParameters(a, 4096, false, false));
		}
		public static void GrabItemFarAway_D(GameContext ctx)
		{
			nuint a = ctx.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void BonusTwoSlots_E(GameContext ctx)
		{
			nuint a = ctx.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
			if (ctx.HContext.DataAccess.Read<byte>(a) == 0xE9)
				return;
			InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode(
				"mov eax,1\nret"),
				new HookParameters(a, 4096, false, false));
		}
		public static void BonusTwoSlots_D(GameContext ctx)
		{
			nuint a = ctx.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void GoldHoleDropsBag_E(GameContext ctx)
		{
			nuint a = AobscanHelper.AobscanASM(
				ctx.HContext.Handle,
				@"push 0
push 0
push 0x49
push 1
push 0
push 0
push 0
push 0").FirstOrDefault();
			if (a == 0)
				return;
			a += 2 * 5;
			InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode(
				"mov dword ptr [esp+8],3332"),
				new HookParameters(a, 4096, false, false));
		}
		public static void GoldHoleDropsBag_D(GameContext ctx)
		{
			nuint a = AobscanHelper.AobscanASM(
				   ctx.HContext.Handle,
				   @"push 0
push 0
push 0x49
push 1
push 0").FirstOrDefault();
			if (a == 0)
				return;
			a += 2 * 5;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void SlimeGunBurn_E(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"8b 85 b8 f3 ff ff 89 45 cc 8b 45 cc 40").FirstOrDefault();
			if (a == 0)
				return;
			a -= 0x1a;
			InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode(
				"mov dword ptr [esp+8],216000\nmov edx,0x99"),
				new HookParameters(a, 4096, false, false));
		}
		public static void SlimeGunBurn_D(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				   ctx.HContext.Handle,
				   "8b 85 b8 f3 ff ff 89 45 cc 8b 45 cc 40").FirstOrDefault();
			if (a == 0)
				return;
			a -= 0x1a;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void FishOnlyCrates_E(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"8B 45 0C C6 00 00 8B 45 08 C6 00 00 B9").FirstOrDefault();
			if (a == 0)
				return;
			a += 11;
			ctx.HContext.DataAccess.Write<byte>(a, 1);
		}
		public static void FishOnlyCrates_D(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"8B 45 0C C6 00 00 8B 45 08 C6 00 01 B9").FirstOrDefault();
			if (a == 0)
				return;
			a += 11;
			ctx.HContext.DataAccess.Write<byte>(a, 0);
		}

		public static void EnableAllRecipes_E(GameContext ctx)
		{
			var helper = ctx.GameModuleHelper;
			ctx.HContext.DataAccess.Write<byte>(
				helper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
				0xC3);
			helper.SetStaticFieldValue("Terraria.Main", "numAvailableRecipes", 3000);
			var array = new GameObjectArrayV<int>(ctx, helper.GetStaticHackObject("Terraria.Main", "availableRecipe"));
			int len = array.Length;
			for (int i = 0; i < len; i++)
				array[i] = i;
		}
		public static void EnableAllRecipes_D(GameContext ctx)
		{
			ctx.HContext.DataAccess.Write<byte>(
				ctx.GameModuleHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
				0x55);
		}

		public static void StrengthenVampireKnives_E(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"81 F9 21060000").FirstOrDefault();
			if (a == 0)
				return;
			a += 18;
			ctx.HContext.DataAccess.Write<int>(a, 100);
		}
		public static void StrengthenVampireKnives_D(GameContext ctx)
		{
			nuint a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"81 F9 21060000").FirstOrDefault();
			if (a == 0)
				return;
			a += 18;
			ctx.HContext.DataAccess.Write<int>(a, 4);
		}

		/*public static void SuperRange_E(GameContext ctx)
		{
			nuint a = Aobscan(
				ctx.HContext.Handle,
				"C7 05 ******** 05000000 C7 05 ******** 04000000 A1").FirstOrDefault();
			if (a == 0)
				return;
			nuint b = a + 6;
			nuint c = a + 16;
			int v = 0x1000;
			Write<int>(b, v);
			Write<int>(c, v);
		}
		public static void SuperRange_D(GameContext ctx)
		{
			nuint a = Aobscan(
				ctx.HContext.Handle,
				"C7 05 ******** 00100000 C7 05 ******** 00100000 A1").FirstOrDefault();
			if (a == 0)
				return;
			nuint b = a + 6;
			nuint c = a + 16;
			int v1 = 5;
			int v2 = 4;
			Write<int>(b, v1);
			Write<int>(c, v2);
		}

		public static void FastTileAndWallSpeed_E(GameContext ctx)
		{
			int offA = GetOffset(ctx, "Terraria.Player", "wallSpeed");
			int offB = GetOffset(ctx, "Terraria.Player", "tileSpeed");
			nuint a = Aobscan(
			ctx,
			$"D9 E8 D9 9E {AobscanHelper.GetMByteCode(offA)} D9 E8 D9 9E {AobscanHelper.GetMByteCode(offB)} 88 96").FirstOrDefault();
			if (a == 0) return;

			InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offA})],0x41200000"),
				new HookParameters(a, 4096, false, false));

			InlineHook.Hook(ctx.HContext,
				AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offB}],0x41200000"),
				new HookParameters(a+8, 4096, false, false));
		}
		public static void FastTileAndWallSpeed_D(GameContext ctx)
		{
			int a = Aobscan(
				   ctx,
				   "E9 ******** 90 90 90 E9 ******** 90 90 90 88 96", true);
			if (a <= 0) return;
			InlineHook.FreeHook(ctx.HContext, a);
			InlineHook.FreeHook(ctx.HContext, a + 8);
		}*/

		/*public static void MachinicalRulerEffect_E(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_RulerGrid)} C6 86 {AobscanHelper.GetMByteCode(Player.OFFSET_RulerLine)} 01");
			if (a <= 0)
				return;
			InlineHook.Inject(ctx.HContext, AssemblySnippet.FromASMCode(
				$"mov byte ptr [esi+{Player.OFFSET_RulerGrid}],0x1"),
				a, false, false);
		}
		public static void MachinicalRulerEffect_D(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				   ctx.HContext.Handle,
				   $"E9 ******** 90 C6 86 {AobscanHelper.GetMByteCode(Player.OFFSET_RulerLine)} 01", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void ShowCircuit_E(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_InfoAccMechShowWires)} 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_AccJarOfSouls)}");
			if (a <= 0)
				return;
			InlineHook.Inject(ctx.HContext, AssemblySnippet.FromASMCode(
				$"mov byte ptr [esi+{Player.OFFSET_InfoAccMechShowWires}],0x1"),
				a, false, false);
		}
		public static void ShowCircuit_D(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				   ctx.HContext.Handle,
				   $"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_AccJarOfSouls)}", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void ShadowDodge_E(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_ShadowDodge)} 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_PalladiumRegen)}");
			if (a <= 0)
				return;
			InlineHook.Inject(ctx.HContext, AssemblySnippet.FromASMCode(
				$"mov byte ptr [esi+{Player.OFFSET_ShadowDodge}],1"),
				a, false, false);
		}
		public static void ShadowDodge_D(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				   ctx.HContext.Handle,
				   $"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_PalladiumRegen)}", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(ctx.HContext, a);
		}*/

		public static void RevealMap(GameContext ctx)
		{
			AssemblySnippet asm = AssemblySnippet.FromEmpty();
			asm.Content.Add(Instruction.Create("push ecx"));
			asm.Content.Add(Instruction.Create("push edx"));
			asm.Content.Add(
				AssemblySnippet.Loop(
					AssemblySnippet.Loop(
						AssemblySnippet.FromCode(
							new AssemblyCode[] {
								(Instruction)"mov edx, [esp+4]",
								(Instruction)"push [esp]",
								(Instruction)"push 255",
								AssemblySnippet.FromClrCall(
									ctx.GameModuleHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"), false, ctx.Map.BaseAddress, null, null,
									Array.Empty<object>())
							}),
						ctx.MaxTilesY, false),
					ctx.MaxTilesX, false));
			asm.Content.Add(Instruction.Create("pop edx"));
			asm.Content.Add(Instruction.Create("pop ecx"));

			ctx.RunByHookOnUpdate(asm);
			ctx.RefreshMap = true;
		}


		/*
		public static void DropLavaOntoPlayers(GameContext ctx)
		{
			var players = ctx.Players;
			for (int i = 0; i < players.Length; i++)
			{
				var p = ctx.Players[i];
				if (p.Active)
				{
					int x = (int)Math.Round(p.Position.X / 16);
					int y = (int)Math.Round(p.Position.Y / 16);
					var tile = ctx.Tile[x, y];
					tile.LiquidType(1);
					tile.Liquid = 255;
					WorldGen.SquareTileFrame(ctx, x, y, true);
					if (ctx.NetMode == 1)
						NetMessage.SendWater(ctx, x, y);
				}
			}
		}

		public static void RightClickToTP(GameContext ctx)
		{
			byte s = 0;
			NativeFunctions.ReadProcessMemory(ctx.HContext.Handle,
				ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), ref s, 1, 0);
			if (s == 0xE9)//已经被修改，不能再hook
				return;
			var ass = AssemblySnippet.FromCode(
					new AssemblyCode[] {
						Instruction.Create("pushad"),
						Instruction.Create($"cmp byte ptr [{ctx.MapFullScreen_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{ctx.MouseRight_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{ctx.MouseRightRelease_Address}],0"),
						Instruction.Create("je _rwualfna"),
						AssemblySnippet.FromCode(
							new AssemblyCode[]{
								Instruction.Create($"mov byte ptr [{ctx.MapFullScreen_Address}],0"),
								Instruction.Create($"mov byte ptr [{ctx.MouseRightRelease_Address}],0"),
								AssemblySnippet.FromClrCall(
									ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main","get_LocalPlayer"), null, false),
								Instruction.Create("mov ebx,eax"),
								Instruction.Create("push eax"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{ctx.ScreenWidth_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{ctx.MouseX_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullscreenPos_Address + 4}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{Entity.OFFSET_Position}]"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{ctx.ScreenHeight_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{ctx.MouseY_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{ctx.MapFullscreenPos_Address + 8}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{Entity.OFFSET_Position + 0x4}]"),

								Instruction.Create("pop eax"),
							}),
						Instruction.Create("_rwualfna:"),
						Instruction.Create("popad")
					});
			InlineHook.Inject(ctx.HContext, ass,
				ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate") + 5, false);
		}*/

		/*public static void ShowInvisiblePlayers_E(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"75 0C C6 83 {AobscanHelper.GetMByteCode(Player.OFFSET_Invisible)} 01 E9") + 8;
			if (a <= 0)
				return;
			byte b = 0;
			NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, a, ref b, 1, 0);
		}
		public static void ShowInvisiblePlayers_D(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				$"75 0C C6 83 {AobscanHelper.GetMByteCode(Player.OFFSET_Invisible)} 00 E9") + 8;
			if (a <= 0)
				return;
			byte b = 1;
			NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, a, ref b, 1, 0);
		}

		public static void HarpToTP_E(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"8B 8D E4 F9 FF FF FF 15") - 5;
			byte[] j = new byte[1];
			NativeFunctions.ReadProcessMemory(ctx.HContext.Handle, a, j, 1, 0);
			if (j[0] != 0xE9)
			{
				var player = ctx.MyPlayer;
				InlineHook.Inject(ctx.HContext,
					AssemblySnippet.FromCode(
						new AssemblyCode[]{
							(Instruction)$"pushad",
							AssemblySnippet.FromClrCall(
								ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main","get_LocalPlayer"),
								null, false),
							(Instruction)$"mov ebx,eax",
							(Instruction)$"push 16",
							(Instruction)$"fild dword ptr [{ctx.TileTargetX_Address}]",
							(Instruction)$"fild dword ptr [esp]",
							(Instruction)$"fmul",
							(Instruction)$"fstp dword ptr [ebx+{Entity.OFFSET_Position}]",

							(Instruction)$"fild dword ptr [{ctx.TileTargetY_Address}]",
							(Instruction)$"fild dword ptr [esp]",
							(Instruction)$"fmul",
							(Instruction)$"fstp dword ptr [ebx+{Entity.OFFSET_Position + 0x4}]",
							(Instruction)$"add esp,4",
							(Instruction)$"popad",
						}),
					a, false);
			}
		}
		public static void HarpToTP_D(GameContext ctx)
		{
			int a = AobscanHelper.Aobscan(
				ctx.HContext.Handle,
				"8B 8D E4 F9 FF FF FF 15") - 5;
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void ImmuneDebuffs_E(GameContext ctx)
		{
			int a = ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Player", "AddBuff");
			byte[] j = new byte[1];
			NativeFunctions.ReadProcessMemory(ctx.HContext.Handle, a, j, 1, 0);
			if (j[0] != 0xE9)
			{
				var player = ctx.MyPlayer;
				InlineHook.Inject(ctx.HContext,
					AssemblySnippet.FromCode(
						new AssemblyCode[]{
							(Instruction)$"pushad",
							(Instruction)$"mov ebx,{ctx.Debuff_Address}",
							(Instruction)$"cmp byte ptr [ebx+edx+8],0",
							(Instruction)$"je end",
							(Instruction)$"popad",
							(Instruction)$"ret 8",
							(Instruction)$"end:",
							(Instruction)$"popad",
						}),
					a, false);
			}
		}
		public static void ImmuneDebuffs_D(GameContext ctx)
		{
			int a = ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Player", "AddBuff");
			InlineHook.FreeHook(ctx.HContext, a);
		}
		public static void SendChat(GameContext ctx, string Text)
		{
			byte[] bs = Encoding.Unicode.GetBytes(Text);
			int strEnd = 0;
			int strMem = NativeFunctions.VirtualAllocEx(ctx.HContext.Handle, 0, Text.Length + 10,
				NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);

			NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, strMem, bs, bs.Length, 0);
			NativeFunctions.WriteProcessMemory(ctx.HContext.Handle, strMem + bs.Length, ref strEnd, 4, 0);

			var mscorlib_AddrHelper = ctx.HContext.GetAddressHelper("mscorlib.dll");
			int ctor = mscorlib_AddrHelper.GetFunctionAddress("System.String", "CtorCharPtr");
			AssemblySnippet asm = AssemblySnippet.FromCode(
				new AssemblyCode[] {
						(Instruction)"push ecx",
						(Instruction)"push edx",
						AssemblySnippet.ConstructString(ctx.HContext, strMem, ctx.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "chatText")),
						(Instruction)$"mov byte ptr [{ctx.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "drawingPlayerChat")}],1",
						(Instruction)$"mov byte ptr [{ctx.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "inputTextEnter")}],1",
						(Instruction)$"mov byte ptr [{ctx.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "chatRelease")}],1",
						(Instruction)"pop edx",
						(Instruction)"pop ecx"
			});

			InlineHook.InjectAndWait(ctx.HContext, asm, ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), true);
			NativeFunctions.VirtualFreeEx(ctx.HContext.Handle, strMem, 0);
		}

		public static void SwingIgnoringTiles_E(GameContext ctx)
		{
			byte s = 0;
			NativeFunctions.ReadProcessMemory(ctx.HContext.Handle,
				ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit"), ref s, 1, 0);
			if (s != 0x55)//已经被修改，不能再hook
				return;
			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				(Instruction)"mov eax,1",
			});
			InlineHook.Inject(ctx.HContext, code, ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit"), false, false);
		}
		public static void SwingIgnoringTiles_D(GameContext ctx)
		{
			int a = ctx.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit");
			InlineHook.FreeHook(ctx.HContext, a);
		}

		public static void SwingingAttacksAll_E(GameContext ctx)
		{
			int a = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x115].StartAddress - 6;
			int b = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x12B].StartAddress;
			byte s = 0;
			NativeFunctions.ReadProcessMemory(ctx.HContext.Handle, a, ref s, 1, 0);
			if (s == 0xE9)//已经被修改，不能再hook
				return;

			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				(Instruction)"mov eax,1",
			});
			InlineHook.Inject(ctx.HContext, code, a, false, false);
			InlineHook.Inject(ctx.HContext, code, b, false, false);
		}
		public static void SwingingAttacksAll_D(GameContext ctx)
		{
			int a = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x115].StartAddress - 6;
			int b = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x12B].StartAddress;
			InlineHook.FreeHook(ctx.HContext, a);
			InlineHook.FreeHook(ctx.HContext, b);
		}


		public static void DisableInvisibility_E(GameContext ctx)
		{
			int a = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "UpdateBuffs", 0x21B].StartAddress;
			int s = 0;
			NativeFunctions.ReadProcessMemory(ctx.HContext.Handle, a + 4, ref s, 4, 0);
			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				(Instruction)"mov byte ptr [esi+0x651],0",
			});
			InlineHook.Inject(ctx.HContext, code, a, false, false);
		}
		public static void DisableInvisibility_D(GameContext ctx)
		{
			int a = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF938].StartAddress;
			int b = (int)ctx.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF962].StartAddress;
			InlineHook.FreeHook(ctx.HContext, a);
			InlineHook.FreeHook(ctx.HContext, b);
		}*/
	}
}
