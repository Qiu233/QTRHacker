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
		public static int GetOffset(GameContext context, string module, string type, string field)
		{
			return (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field);
		}
		public static int GetOffset(GameContext context, string type, string field)
		{
			return (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field);
		}
		public static void AobReplaceASM(GameContext Context, string src, string target)
		{
			var addrs = AobscanHelper.Aobscan(Context.HContext.Handle, AobscanHelper.GetHexCodeFromString(src));
			byte[] code = Assembler.Assemble(target, 0);
			foreach (var addr in addrs)
				Context.HContext.DataAccess.WriteBytes(addr, code);
		}
		public static void AobReplace(GameContext Context, string srcHex, string targetHex)
		{
			var addrs = AobscanHelper.Aobscan(Context.HContext.Handle, AobscanHelper.GetHexCodeFromString(srcHex));
			byte[] code = AobscanHelper.GetHexCodeFromString(targetHex);
			foreach (var addr in addrs)
				Context.HContext.DataAccess.WriteBytes(addr, code);
		}

		public static void InfiniteLife_E(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "statLife");
			AobReplaceASM(Context, $"sub [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"add [edx+{off}],eax");
		}
		public static void InfiniteLife_D(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "statLife");
			AobReplaceASM(Context, $"add [edx+{off}],eax\ncmp dword ptr [ebp+0x8],-1", $"sub [edx+{off}],eax");
		}

		public static void InfiniteMana_E(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "statMana");
			AobReplaceASM(Context, $"sub [esi+{off}],edi", $"add [esi+{off}],edi");
			AobReplaceASM(Context, $"sub [esi+{off}],eax", $"add [esi+{off}],eax");
		}
		public static void InfiniteMana_D(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "statMana");
			AobReplaceASM(Context, $"add [esi+{off}],edi", $"sub [esi+{off}],edi");
			AobReplaceASM(Context, $"add [esi+{off}],eax", $"sub [esi+{off}],eax");
		}

		public static void InfiniteOxygen_E(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "breath");
			AobReplaceASM(Context, $"dec dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"inc dword ptr [eax+{off}]");
		}
		public static void InfiniteOxygen_D(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "breath");
			AobReplaceASM(Context, $"inc dword ptr [eax+{off}]\ncmp dword ptr [eax+{off}],0", $"dec dword ptr [eax+{off}]");
		}

		public static void InfiniteMinion_E(GameContext Context)
		{
			int offA = GetOffset(Context, "Terraria.Player", "maxMinions");
			int offB = GetOffset(Context, "Terraria.Player", "maxTurrets");
			AobReplaceASM(Context, $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1", $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999");
		}
		public static void InfiniteMinion_D(GameContext Context)
		{
			int offA = GetOffset(Context, "Terraria.Player", "maxMinions");
			int offB = GetOffset(Context, "Terraria.Player", "maxTurrets");
			AobReplaceASM(Context, $"mov dword ptr [esi+{offA}],9999\nmov dword ptr [esi+{offB}],9999", $"mov dword ptr [esi+{offA}],1\nmov dword ptr [esi+{offB}],1");
		}

		public static void InfiniteItemAmmo_E(GameContext Context)
		{
			AobReplace(Context, "FF 88 B0 00 00 00 8B 45 E0 83 B8", "90 90 90 90 90 90");//dec dword ptr [eax+0xB0]\nmov eax,[ebp-0x20]\ncmp
			AobReplace(Context, "FF 89 B0 00 00 00 8B 45 0C 8B 55 F4", "90 90 90 90 90 90");//dec dword ptr [ecx+0xB0]\nmov eax,[ebp+0xC]\nmov edx[ebp-0xC]
		}
		public static void InfiniteItemAmmo_D(GameContext Context)
		{
			AobReplace(Context, "90 90 90 90 90 90 8B 45 E0 83 B8", "FF 88 B0 00 00 00");
			AobReplace(Context, "90 90 90 90 90 90 8B 45 0C 8B 55 F4", "FF 89 B0 00 00 00");
		}

		public static void InfiniteFly_E(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "wingTime");
			AobReplace(Context, $"D9 99 {AobscanHelper.GetMByteCode(off)} 80 B9 F7060000 00", "90 90 90 90 90 90");
		}
		public static void InfiniteFly_D(GameContext Context)
		{
			int off = GetOffset(Context, "Terraria.Player", "wingTime");
			AobReplace(Context, "90 90 90 90 90 90 80 B9 F7060000 00", $"D9 99 {AobscanHelper.GetMByteCode(off)}");
		}

		/*public static void HighLight_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				@"C7 ** ** ******** D9 07 D9 45 F0 DF F1 DD D8 7A", true);
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext,
				AssemblySnippet.FromASMCode(
					@"mov dword ptr[ebp-0x10],0x3F800000
mov dword ptr[ebp-0x14],0x3F800000
mov dword ptr[ebp-0x18],0x3F800000"
),
					a + 7, false
				);
		}

		public static void HighLight_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(Context.HContext.Handle, "C7 ** ** ******** E9 ** ** ** ** DF F1 DD D8 7A", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a + 7);
		}*/

		public static void GhostMode_E(GameContext Context)
		{
			Context.MyPlayer.Ghost = true;
		}
		public static void GhostMode_D(GameContext Context)
		{
			Context.MyPlayer.Ghost = false;
		}
		/*
		public static void LowGravity_E(GameContext Context)
		{
			int offA = GetOffset(Context, "Terraria.Player", "slowFall");
			int offB = GetOffset(Context, "Terraria.Player", "findTreasure");
			nuint a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(offA)} 88 96 {AobscanHelper.GetMByteCode(offB)}").FirstOrDefault();
			if (a == 0)
				return;
			_ = new InlineHook(Context.HContext, AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offA}],1"),
				new HookParameters(a, 4096, false, false));
		}
		public static void LowGravity_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(GetOffset(Context, "Terraria.Player", "findTreasure"))}", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}*/

		/*public static void FastSpeed_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"D9 E8 D9 9E {AobscanHelper.GetMByteCode(Player.OFFSET_MoveSpeed)} 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_BoneArmor)}");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{Player.OFFSET_MoveSpeed}],0x41A00000"),
				a, false, false);
		}
		public static void FastSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"E9 ******** 90 90 90 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_BoneArmor)}", true);
			if (a <= 0) return;
			InlineHook.FreeHook(Context.HContext, a);
		}*/

		/*[Obsolete("for the game of old version")]
		public static void ProjectileIgnoreTile_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [ebp-0x20],eax\ncmp byte ptr [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, new byte[] { 0x8d }, 1, 0);
		}
		[Obsolete("for the game of old version")]
		public static void ProjectileIgnoreTile_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [ebp-0x20],eax\ncmp byte ptr [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, new byte[] { 0x84 }, 1, 0);
		}*/

		public static void GrabItemFarAway_E(GameContext Context)
		{
			nuint a = Context.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
			if (Context.HContext.DataAccess.Read<byte>(a) == 0xE9)
				return;
			InlineHook.Hook(Context.HContext, AssemblySnippet.FromASMCode(
				"mov eax,1000\nret"),
				new HookParameters(a, 4096, false, false));
		}
		public static void GrabItemFarAway_D(GameContext Context)
		{
			nuint a = Context.GameModuleHelper["Terraria.Player", "GetItemGrabRange"];
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void BonusTwoSlots_E(GameContext Context)
		{
			nuint a = Context.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
			if (Context.HContext.DataAccess.Read<byte>(a) == 0xE9)
				return;
			InlineHook.Hook(Context.HContext,
				AssemblySnippet.FromASMCode(
				"mov eax,1\nret"),
				new HookParameters(a, 4096, false, false));
		}
		public static void BonusTwoSlots_D(GameContext Context)
		{
			nuint a = Context.GameModuleHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void GoldHoleDropsBag_E(GameContext Context)
		{
			nuint a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
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
			InlineHook.Hook(Context.HContext,
				AssemblySnippet.FromASMCode(
				"mov dword ptr [esp+8],3332"),
				new HookParameters(a, 4096, false, false));
		}
		public static void GoldHoleDropsBag_D(GameContext Context)
		{
			nuint a = AobscanHelper.AobscanASM(
				   Context.HContext.Handle,
				   @"push 0
push 0
push 0x49
push 1
push 0").FirstOrDefault();
			if (a == 0)
				return;
			a += 2 * 5;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void SlimeGunBurn_E(GameContext Context)
		{
			nuint a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"8b 85 b8 f3 ff ff 89 45 cc 8b 45 cc 40").FirstOrDefault();
			if (a == 0)
				return;
			a -= 0x1a;
			InlineHook.Hook(Context.HContext,
				AssemblySnippet.FromASMCode(
				"mov dword ptr [esp+8],216000\nmov edx,0x99"),
				new HookParameters(a, 4096, false, false));
		}
		public static void SlimeGunBurn_D(GameContext Context)
		{
			nuint a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "8b 85 b8 f3 ff ff 89 45 cc 8b 45 cc 40").FirstOrDefault();
			if (a == 0)
				return;
			a -= 0x1a;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void FishOnlyCrates_E(GameContext Context)
		{
			nuint a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"8B 45 0C C6 00 00 8B 45 08 C6 00 00 B9").FirstOrDefault();
			if (a == 0)
				return;
			a += 11;
			Context.HContext.DataAccess.Write<byte>(a, 1);
		}
		public static void FishOnlyCrates_D(GameContext Context)
		{
			nuint a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"8B 45 0C C6 00 00 8B 45 08 C6 00 01 B9").FirstOrDefault();
			if (a == 0)
				return;
			a += 11;
			Context.HContext.DataAccess.Write<byte>(a, 0);
		}

		public static void EnableAllRecipes_E(GameContext Context)
		{
			var helper = Context.GameModuleHelper;
			Context.HContext.DataAccess.Write<byte>(
				helper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
				0xC3);
			helper.SetStaticFieldValue("Terraria.Main", "numAvailableRecipes", 3000);
			var array = new GameObjectArrayV<int>(Context, helper.GetStaticHackObject("Terraria.Main", "availableRecipe"));
			int len = array.Length;
			for (int i = 0; i < len; i++)
				array[i] = i;
		}
		public static void EnableAllRecipes_D(GameContext Context)
		{
			Context.HContext.DataAccess.Write<byte>(
				Context.GameModuleHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
				0x55);
		}

		public static void StrengthenVampireKnives_E(GameContext Context)
		{
			nuint a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"81 F9 21060000").FirstOrDefault();
			if (a == 0)
				return;
			a += 18;
			Context.HContext.DataAccess.Write<int>(a, 100);
		}
		public static void StrengthenVampireKnives_D(GameContext Context)
		{
			nuint a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"81 F9 21060000").FirstOrDefault();
			if (a == 0)
				return;
			a += 18;
			Context.HContext.DataAccess.Write<int>(a, 4);
		}
		/*
		public static void SuperRange_E(GameContext Context)
		{
			//int a = (int)Context.HContext.MainAddressHelper.GetFunctionInstruction("Terraria.Player", "ResetEffects", 0x08AE).StartAddress;
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"C7 05 ******** 05000000 C7 05 ******** 04000000 A1", true);
			if (a <= 0)
				return;
			int b = a + 6;
			int c = a + 16;
			int v = 0x1000;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, b, ref v, 4, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, c, ref v, 4, 0);
		}
		public static void SuperRange_D(GameContext Context)
		{
			//int a = (int)Context.HContext.MainAddressHelper.GetFunctionInstruction("Terraria.Player", "ResetEffects", 0x08AE).StartAddress;
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"C7 05 ******** 00100000 C7 05 ******** 00100000 A1", true);
			if (a <= 0)
				return;
			int b = a + 6;
			int c = a + 16;
			int v1 = 5;
			int v2 = 4;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, b, ref v1, 4, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, c, ref v2, 4, 0);
		}
		*/
		/*public static void FastTileAndWallSpeed_E(GameContext Context)
		{
			int offA = GetOffset(Context, "Terraria.Player", "wallSpeed");
			int offB = GetOffset(Context, "Terraria.Player", "tileSpeed");
			nuint a = AobscanHelper.Aobscan(
			Context.HContext.Handle,
			$"D9 E8 D9 9E {AobscanHelper.GetMByteCode(offA)} D9 E8 D9 9E {AobscanHelper.GetMByteCode(offB)} 88 96").FirstOrDefault();
			if (a == 0) return;

			InlineHook.Hook(Context.HContext,
				AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offA})],0x41200000"),
				new HookParameters(a, 4096, false, false));

			InlineHook.Hook(Context.HContext,
				AssemblySnippet.FromASMCode(
				$"mov dword ptr [esi+{offB}],0x41200000"),
				new HookParameters(a+8, 4096, false, false));
		}
		public static void FastTileAndWallSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "E9 ******** 90 90 90 E9 ******** 90 90 90 88 96", true);
			if (a <= 0) return;
			InlineHook.FreeHook(Context.HContext, a);
			InlineHook.FreeHook(Context.HContext, a + 8);
		}*/

		/*public static void MachinicalRulerEffect_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_RulerGrid)} C6 86 {AobscanHelper.GetMByteCode(Player.OFFSET_RulerLine)} 01");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				$"mov byte ptr [esi+{Player.OFFSET_RulerGrid}],0x1"),
				a, false, false);
		}
		public static void MachinicalRulerEffect_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   $"E9 ******** 90 C6 86 {AobscanHelper.GetMByteCode(Player.OFFSET_RulerLine)} 01", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ShowCircuit_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_InfoAccMechShowWires)} 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_AccJarOfSouls)}");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				$"mov byte ptr [esi+{Player.OFFSET_InfoAccMechShowWires}],0x1"),
				a, false, false);
		}
		public static void ShowCircuit_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   $"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_AccJarOfSouls)}", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ShadowDodge_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_ShadowDodge)} 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_PalladiumRegen)}");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				$"mov byte ptr [esi+{Player.OFFSET_ShadowDodge}],1"),
				a, false, false);
		}
		public static void ShadowDodge_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   $"E9 ******** 90 88 96 {AobscanHelper.GetMByteCode(Player.OFFSET_PalladiumRegen)}", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}*/

		public static void RevealMap(GameContext Context)
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
									Context.GameModuleHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"), false, Context.Map.BaseAddress, null, null,
									Array.Empty<object>())
							}),
						Context.MaxTilesY, false),
					Context.MaxTilesX, false));
			asm.Content.Add(Instruction.Create("pop edx"));
			asm.Content.Add(Instruction.Create("pop ecx"));

			Context.RunByHookOnUpdate(asm);
			Context.RefreshMap = true;
		}


		/*
		public static void DropLavaOntoPlayers(GameContext Context)
		{
			var players = Context.Players;
			for (int i = 0; i < players.Length; i++)
			{
				var p = Context.Players[i];
				if (p.Active)
				{
					int x = (int)Math.Round(p.Position.X / 16);
					int y = (int)Math.Round(p.Position.Y / 16);
					var tile = Context.Tile[x, y];
					tile.LiquidType(1);
					tile.Liquid = 255;
					WorldGen.SquareTileFrame(Context, x, y, true);
					if (Context.NetMode == 1)
						NetMessage.SendWater(Context, x, y);
				}
			}
		}

		public static void RightClickToTP(GameContext Context)
		{
			byte s = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), ref s, 1, 0);
			if (s == 0xE9)//已经被修改，不能再hook
				return;
			var ass = AssemblySnippet.FromCode(
					new AssemblyCode[] {
						Instruction.Create("pushad"),
						Instruction.Create($"cmp byte ptr [{Context.MapFullScreen_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{Context.MouseRight_Address}],0"),
						Instruction.Create("je _rwualfna"),
						Instruction.Create($"cmp byte ptr [{Context.MouseRightRelease_Address}],0"),
						Instruction.Create("je _rwualfna"),
						AssemblySnippet.FromCode(
							new AssemblyCode[]{
								Instruction.Create($"mov byte ptr [{Context.MapFullScreen_Address}],0"),
								Instruction.Create($"mov byte ptr [{Context.MouseRightRelease_Address}],0"),
								AssemblySnippet.FromClrCall(
									Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main","get_LocalPlayer"), null, false),
								Instruction.Create("mov ebx,eax"),
								Instruction.Create("push eax"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{Context.ScreenWidth_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{Context.MouseX_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{Context.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{Context.MapFullscreenPos_Address + 4}]"),
								Instruction.Create("fsubrp"),
								Instruction.Create("mov dword ptr [esp],16"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fmulp"),
								Instruction.Create($"fstp dword ptr [ebx+{Entity.OFFSET_Position}]"),
								Instruction.Create("mov dword ptr [esp],2"),
								Instruction.Create($"fild dword ptr [{Context.ScreenHeight_Address}]"),
								Instruction.Create("fild dword ptr [esp]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fild dword ptr [{Context.MouseY_Address}]"),
								Instruction.Create("fsubp"),
								Instruction.Create($"fld dword ptr [{Context.MapFullScreenScale_Address}]"),
								Instruction.Create("fdivp"),
								Instruction.Create($"fld dword ptr [{Context.MapFullscreenPos_Address + 8}]"),
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
			InlineHook.Inject(Context.HContext, ass,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate") + 5, false);
		}*/

		/*public static void ShowInvisiblePlayers_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"75 0C C6 83 {AobscanHelper.GetMByteCode(Player.OFFSET_Invisible)} 01 E9") + 8;
			if (a <= 0)
				return;
			byte b = 0;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ref b, 1, 0);
		}
		public static void ShowInvisiblePlayers_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				$"75 0C C6 83 {AobscanHelper.GetMByteCode(Player.OFFSET_Invisible)} 00 E9") + 8;
			if (a <= 0)
				return;
			byte b = 1;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ref b, 1, 0);
		}

		public static void HarpToTP_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"8B 8D E4 F9 FF FF FF 15") - 5;
			byte[] j = new byte[1];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, j, 1, 0);
			if (j[0] != 0xE9)
			{
				var player = Context.MyPlayer;
				InlineHook.Inject(Context.HContext,
					AssemblySnippet.FromCode(
						new AssemblyCode[]{
							(Instruction)$"pushad",
							AssemblySnippet.FromClrCall(
								Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main","get_LocalPlayer"),
								null, false),
							(Instruction)$"mov ebx,eax",
							(Instruction)$"push 16",
							(Instruction)$"fild dword ptr [{Context.TileTargetX_Address}]",
							(Instruction)$"fild dword ptr [esp]",
							(Instruction)$"fmul",
							(Instruction)$"fstp dword ptr [ebx+{Entity.OFFSET_Position}]",

							(Instruction)$"fild dword ptr [{Context.TileTargetY_Address}]",
							(Instruction)$"fild dword ptr [esp]",
							(Instruction)$"fmul",
							(Instruction)$"fstp dword ptr [ebx+{Entity.OFFSET_Position + 0x4}]",
							(Instruction)$"add esp,4",
							(Instruction)$"popad",
						}),
					a, false);
			}
		}
		public static void HarpToTP_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"8B 8D E4 F9 FF FF FF 15") - 5;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ImmuneDebuffs_E(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Player", "AddBuff");
			byte[] j = new byte[1];
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, j, 1, 0);
			if (j[0] != 0xE9)
			{
				var player = Context.MyPlayer;
				InlineHook.Inject(Context.HContext,
					AssemblySnippet.FromCode(
						new AssemblyCode[]{
							(Instruction)$"pushad",
							(Instruction)$"mov ebx,{Context.Debuff_Address}",
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
		public static void ImmuneDebuffs_D(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Player", "AddBuff");
			InlineHook.FreeHook(Context.HContext, a);
		}
		public static void SendChat(GameContext Context, string Text)
		{
			byte[] bs = Encoding.Unicode.GetBytes(Text);
			int strEnd = 0;
			int strMem = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, Text.Length + 10,
				NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, strMem, bs, bs.Length, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, strMem + bs.Length, ref strEnd, 4, 0);

			var mscorlib_AddrHelper = Context.HContext.GetAddressHelper("mscorlib.dll");
			int ctor = mscorlib_AddrHelper.GetFunctionAddress("System.String", "CtorCharPtr");
			AssemblySnippet asm = AssemblySnippet.FromCode(
				new AssemblyCode[] {
						(Instruction)"push ecx",
						(Instruction)"push edx",
						AssemblySnippet.ConstructString(Context.HContext, strMem, Context.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "chatText")),
						(Instruction)$"mov byte ptr [{Context.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "drawingPlayerChat")}],1",
						(Instruction)$"mov byte ptr [{Context.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "inputTextEnter")}],1",
						(Instruction)$"mov byte ptr [{Context.HContext.MainAddressHelper.GetStaticFieldAddress("Terraria.Main", "chatRelease")}],1",
						(Instruction)"pop edx",
						(Instruction)"pop ecx"
			});

			InlineHook.InjectAndWait(Context.HContext, asm, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), true);
			NativeFunctions.VirtualFreeEx(Context.HContext.Handle, strMem, 0);
		}

		public static void SwingIgnoringTiles_E(GameContext Context)
		{
			byte s = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit"), ref s, 1, 0);
			if (s != 0x55)//已经被修改，不能再hook
				return;
			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				(Instruction)"mov eax,1",
			});
			InlineHook.Inject(Context.HContext, code, Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit"), false, false);
		}
		public static void SwingIgnoringTiles_D(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit");
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void SwingingAttacksAll_E(GameContext Context)
		{
			int a = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x115].StartAddress - 6;
			int b = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x12B].StartAddress;
			byte s = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, ref s, 1, 0);
			if (s == 0xE9)//已经被修改，不能再hook
				return;

			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				(Instruction)"mov eax,1",
			});
			InlineHook.Inject(Context.HContext, code, a, false, false);
			InlineHook.Inject(Context.HContext, code, b, false, false);
		}
		public static void SwingingAttacksAll_D(GameContext Context)
		{
			int a = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x115].StartAddress - 6;
			int b = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck_MeleeHitNPCs", 0x12B].StartAddress;
			InlineHook.FreeHook(Context.HContext, a);
			InlineHook.FreeHook(Context.HContext, b);
		}


		public static void DisableInvisibility_E(GameContext Context)
		{
			int a = (int)Context.HContext.MainAddressHelper["Terraria.Player", "UpdateBuffs", 0x21B].StartAddress;
			int s = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 4, ref s, 4, 0);
			var code = AssemblySnippet.FromCode(new AssemblyCode[] {
				(Instruction)"mov byte ptr [esi+0x651],0",
			});
			InlineHook.Inject(Context.HContext, code, a, false, false);
		}
		public static void DisableInvisibility_D(GameContext Context)
		{
			int a = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF938].StartAddress;
			int b = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF962].StartAddress;
			InlineHook.FreeHook(Context.HContext, a);
			InlineHook.FreeHook(Context.HContext, b);
		}*/
	}
}
