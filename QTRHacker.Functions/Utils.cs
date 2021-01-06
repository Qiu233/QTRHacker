using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;
using QTRHacker.Functions.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class Utils
	{
		public static void AobReplaceASM(GameContext Context, string src, string target, int offset = 0)
		{
			int addr = 0;
			while ((addr = AobscanHelper.AobscanASM(Context.HContext.Handle, src)) != -1)
			{
				byte[] code = Assembler.Assemble(target, 0);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, addr + offset, code, code.Length, 0);
			}
		}
		public static void AobReplace(GameContext Context, string srcHex, string targetHex, int offset = 0, bool matching = false)
		{
			int addr = 0;
			while ((addr = AobscanHelper.Aobscan(Context.HContext.Handle, srcHex, matching)) != -1)
			{
				byte[] code = AobscanHelper.GetHexCodeFromString(targetHex);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, addr + offset, code, code.Length, 0);
			}
		}

		public static void InfiniteLife_E(GameContext Context)
		{
			AobReplaceASM(Context, "sub [edx+0x3B4],eax\ncmp dword ptr [ebp+0x8],-1", "add [edx+0x3B4],eax");
		}
		public static void InfiniteLife_D(GameContext Context)
		{
			AobReplaceASM(Context, "add [edx+0x3B4],eax\ncmp dword ptr [ebp+0x8],-1", "sub [edx+0x3B4],eax");
		}

		public static void InfiniteMana_E(GameContext Context)
		{
			AobReplaceASM(Context, "sub [esi+0x3B8],edi", "add [esi+0x3B8],edi");
			AobReplaceASM(Context, "sub [esi+0x3B8],eax", "add [esi+0x3B8],eax");
		}
		public static void InfiniteMana_D(GameContext Context)
		{
			AobReplaceASM(Context, "add [esi+0x3B8],edi", "sub [esi+0x3B8],edi");
			AobReplaceASM(Context, "add [esi+0x3B8],eax", "sub [esi+0x3B8],eax");
		}

		public static void InfiniteOxygen_E(GameContext Context)
		{
			AobReplaceASM(Context, "dec dword ptr [eax+0x318]\ncmp dword ptr [eax+0x318],0", "inc dword ptr [eax+0x318]");
		}
		public static void InfiniteOxygen_D(GameContext Context)
		{
			AobReplaceASM(Context, "inc dword ptr [eax+0x318]\ncmp dword ptr [eax+0x318],0", "dec dword ptr [eax+0x318]");
		}

		public static void InfiniteMinion_E(GameContext Context)
		{
			AobReplaceASM(Context, "mov dword ptr [esi+0x270],1\nmov dword ptr [esi+0x4B4],1", "mov dword ptr [esi+0x270],9999\nmov dword ptr [esi+0x4B4],9999");
		}
		public static void InfiniteMinion_D(GameContext Context)
		{
			AobReplaceASM(Context, "mov dword ptr [esi+0x270],9999\nmov dword ptr [esi+0x4B4],9999", "mov dword ptr [esi+0x270],1\nmov dword ptr [esi+0x4B4],1");
		}

		public static void InfiniteItemAmmo_E(GameContext Context)
		{
			//AobReplaceASM(Context, "dec dword ptr [eax+0xA8]\nmov eax,[ebp-0x20]", "nop\nnop\nnop\nnop\nnop\nnop");
			//AobReplaceASM(Context, "dec dword ptr [ecx+0xA8]\nmov eax,[ebp+0xC]", "nop\nnop\nnop\nnop\nnop\nnop");
			AobReplace(Context, "FF 88 A8 00 00 00 8B 45 E0 83 B8", "90 90 90 90 90 90");//dec dword ptr [eax+0xA8]\nmov eax,[ebp-0x20]\ncmp
			AobReplace(Context, "FF 89 A8 00 00 00 8B 45 0C 8B 55 F4", "90 90 90 90 90 90");//dec dword ptr [ecx+0xA8]\nmov eax,[ebp+0xC]\nmov edx[ebp-0xC]
		}
		public static void InfiniteItemAmmo_D(GameContext Context)
		{
			//AobReplaceASM(Context, "nop\nnop\nnop\nnop\nnop\nnop\nmov eax,[ebp-0x20]", "dec dword ptr [eax+0xA8]");
			//AobReplaceASM(Context, "nop\nnop\nnop\nnop\nnop\nnop\nmov eax,[ebp+0xC]", "dec dword ptr [ecx+0xA8]");
			AobReplace(Context, "90 90 90 90 90 90 8B 45 E0 83 B8", "FF 88 A8 00 00 00");
			AobReplace(Context, "90 90 90 90 90 90 8B 45 0C 8B 55 F4", "FF 89 A8 00 00 00");
		}

		public static void InfiniteFly_E(GameContext Context)
		{
			int addr = AobscanHelper.Aobscan(Context.HContext.Handle, "89 86 90020000 80 BF");
			if (addr <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromCode(
				new AssemblyCode[]
				{
					(Instruction)"mov dword ptr [esi+0x290],100000"
				}), addr, false, false);
		}
		public static void InfiniteFly_D(GameContext Context)
		{
			int addr = AobscanHelper.Aobscan(Context.HContext.Handle, "E9 ******** 90 80 BF", true);
			if (addr <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, addr);
		}

		public static void HighLight_E(GameContext Context)
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
		}

		public static void GhostMode_E(GameContext Context)
		{
			Context.MyPlayer.Ghost = true;
		}
		public static void GhostMode_D(GameContext Context)
		{
			Context.MyPlayer.Ghost = false;
		}

		public static void LowGravity_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"88 96 69070000 88 96 6A070000");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x769],1"),
				a, false, false);
		}
		public static void LowGravity_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"E9 ******** 90 88 96 6A070000", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void FastSpeed_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"D9 E8 D9 9E 0C040000 88 96 15060000");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x40C],0x41A00000"),
				a, false, false);
		}
		public static void FastSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"E9 ******** 90 90 90 88 96 15060000", true);
			if (a <= 0) return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ProjectileIgnoreTile_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [ebp-0x20],eax\ncmp byte ptr [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, new byte[] { 0x8d }, 1, 0);
		}
		public static void ProjectileIgnoreTile_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [ebp-0x20],eax\ncmp byte ptr [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, new byte[] { 0x84 }, 1, 0);
		}

		public static void GrabItemFarAway_E(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper["Terraria.Player", "GetItemGrabRange"];
			byte d = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, ref d, 1, 0);
			if (d == 0xE9) return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
			"mov eax,1000\nret"),
			a, false, false);
		}
		public static void GrabItemFarAway_D(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper["Terraria.Player", "GetItemGrabRange"];
			byte d = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, ref d, 1, 0);
			if (d == 0xE9)
				InlineHook.FreeHook(Context.HContext, a);
		}

		public static void BonusTwoSlots_E(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
			byte d = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, ref d, 1, 0);
			if (d == 0xE9) return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
			"mov eax,1\nret"),
			a, false, false);
		}
		public static void BonusTwoSlots_D(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper["Terraria.Player", "IsAValidEquipmentSlotForIteration"];
			byte d = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, ref d, 1, 0);
			if (d == 0xE9)
				InlineHook.FreeHook(Context.HContext, a);
		}

		public static void GoldHoleDropsBag_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				@"push 0
push 0
push 0x49
push 1
push 0
push 0
push 0
push 0") + 2 * 5;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esp+8],3332"),
				a, false);
		}
		public static void GoldHoleDropsBag_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				   Context.HContext.Handle,
				   @"push 0
push 0
push 0x49
push 1
push 0") + 2 * 5;

			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void SlimeGunBurn_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"8b 85 b8 f3 ff ff 89 45 cc 8b 45 cc 40") - 0x1a;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esp+8],216000\nmov edx,0x99"),
				a, false, false);
		}
		public static void SlimeGunBurn_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "8b 85 b8 f3 ff ff 89 45 cc 8b 45 cc 40") - 0x1a;

			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void FishOnlyCrates_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"0f 8d 4F 01 00 00 8b 45");
			var bs = AobscanHelper.GetHexCodeFromString("90 90 90 90 90 90");
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, bs, bs.Length, 0);
		}
		public static void FishOnlyCrates_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"90 90 90 90 90 90 8B 45 A8 0B 45 A4");
			var bs = AobscanHelper.GetHexCodeFromString("0f 8d 4F 01 00 00 8b 45");
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, bs, bs.Length, 0);
		}

		public static void EnableAllRecipes_E(GameContext Context)
		{
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
				new byte[] { 0xC3 }, 1, 0);
			int y = 2000;
			var addrHelper = Context.HContext.MainAddressHelper;
			addrHelper.SetStaticFieldValue("Terraria.Main", "numAvailableRecipes", 2000);

			y = addrHelper.GetStaticFieldValue<int>("Terraria.Main", "availableRecipe");
			for (int i = 0; i < 2000; i++)
			{
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, y + 0x8 + i * 4, ref i, 4, 0);
			}

		}
		public static void EnableAllRecipes_D(GameContext Context)
		{
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes"),
				new byte[] { 0x55 }, 1, 0);
		}

		public static void StrengthenVampireKnives_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"81 F9 21060000") + 18;
			int v = 100;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ref v, 4, 0);
		}
		public static void StrengthenVampireKnives_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"81 F9 21060000") + 18;
			int v = 4;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ref v, 4, 0);
		}

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

		public static void FastTileAndWallSpeed_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
			Context.HContext.Handle,
			"D9 E8 D9 9E 14040000 D9 E8 D9 9E 18040000 88 96");
			if (a <= 0) return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x414],0x41200000"),
				a, false, false);
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x418],0x41200000"),
				a + 8, false, false);
		}
		public static void FastTileAndWallSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "E9 ******** 90 90 90 E9 ******** 90 90 90 88 96", true);
			if (a <= 0) return;
			InlineHook.FreeHook(Context.HContext, a);
			InlineHook.FreeHook(Context.HContext, a + 8);
		}

		public static void MachinicalRulerEffect_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"88 96 FD060000 C6 86 FE060000 01");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x6FD],0x1"),
				a, false, false);
		}
		public static void MachinicalRulerEffect_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "E9 ******** 90 C6 86 FE060000 01", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ShowCircuit_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"88 96 36070000 88 96 27070000");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x736],0x1"),
				a, false, false);
		}
		public static void ShowCircuit_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "E9 ******** 90 88 96 27070000", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ShadowDodge_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"88 96 F3050000");
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x5F3],1"),
				a, false, false);
		}
		public static void ShadowDodge_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "E9 ******** 90 88 96 F4050000", true);
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void RevealMap(GameContext Context)
		{
			AssemblySnippet asm = AssemblySnippet.FromEmpty();
			asm.Content.Add(Instruction.Create("push ecx"));
			asm.Content.Add(Instruction.Create("push edx"));
			asm.Content.Add(
				AssemblySnippet.Loop(
					AssemblySnippet.Loop(
						AssemblySnippet.FromClrCall(
							Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Map.WorldMap", "UpdateLighting"), null, false,
							Context.Map.BaseAddress, "[esp+4]", "[esp]", 255),
						Context.MaxTilesY, false),
					Context.MaxTilesX, false));
			asm.Content.Add(Instruction.Create("pop edx"));
			asm.Content.Add(Instruction.Create("pop ecx"));

			InlineHook.InjectAndWait(Context.HContext, asm,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), true);
			Context.RefreshMap = true;
		}



		public static void DropLavaOntoPlayers(GameContext Context)
		{
			for (int i = 0; i < Player.MAX_PLAYER; i++)
			{
				var p = Context.Players[i];
				if (p.Active)
				{
					int x = (int)Math.Round(p.X / 16);
					int y = (int)Math.Round(p.Y / 16);
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
		}

		public static void ShowInvisiblePlayers_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"75 37 8d 45 e8 83 ec 08 f3 0f 7e 00");
			byte[] b = new byte[] { 0x90, 0x90 };
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, b, 2, 0);
		}
		public static void ShowInvisiblePlayers_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"90 90 8d 45 e8 83 ec 08 f3 0f 7e 00");
			byte[] b = new byte[] { 0x75, 0x37 };
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, b, 2, 0);
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
				NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);

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

		public static void SwingIgnoringTils_E(GameContext Context)
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
		public static void SwingIgnoringTils_D(GameContext Context)
		{
			int a = Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Collision", "CanHit");
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void SwingingAttacksAll_E(GameContext Context)
		{
			int a = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF938].StartAddress;
			int b = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF962].StartAddress;
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
			int a = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF938].StartAddress;
			int b = (int)Context.HContext.MainAddressHelper["Terraria.Player", "ItemCheck", 0xF962].StartAddress;
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
		}

	}
}
