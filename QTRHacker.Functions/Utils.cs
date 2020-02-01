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
		public static void AobReplace(GameContext Context, string src, string target, int offset = 0)
		{
			int addr = 0;
			while ((addr = AobscanHelper.AobscanASM(Context.HContext.Handle, src)) != -1)
			{
				byte[] code = Assembler.Assemble(target, 0);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, addr + offset, code, code.Length, 0);
			}
		}

		public static void InfiniteLife_E(GameContext Context)
		{
			AobReplace(Context, "sub [edx+0x340],eax\ncmp dword ptr [ebp+0x8],-1", "add [edx+0x340],eax");
		}
		public static void InfiniteLife_D(GameContext Context)
		{
			AobReplace(Context, "add [edx+0x340],eax\ncmp dword ptr [ebp+0x8],-1", "sub [edx+0x340],eax");
		}

		public static void InfiniteMana_E(GameContext Context)
		{
			AobReplace(Context, "sub [esi+0x344],edi", "add [esi+0x344],edi");
			AobReplace(Context, "sub [edx+0x344],eax", "add [edx+0x344],eax");
		}
		public static void InfiniteMana_D(GameContext Context)
		{
			AobReplace(Context, "add [esi+0x344],edi", "sub [esi+0x344],edi");
			AobReplace(Context, "add [edx+0x344],eax", "sub [edx+0x344],eax");
		}

		public static void InfiniteOxygen_E(GameContext Context)
		{
			AobReplace(Context, "dec dword ptr [eax+0x2B4]\ncmp dword ptr [eax+0x2B4],0", "inc dword ptr [eax+0x2B4]");
		}
		public static void InfiniteOxygen_D(GameContext Context)
		{
			AobReplace(Context, "inc dword ptr [eax+0x2B4]\ncmp dword ptr [eax+0x2B4],0", "dec dword ptr [eax+0x2B4]");
		}

		public static void InfiniteMinion_E(GameContext Context)
		{
			AobReplace(Context, "mov dword ptr [esi+0x214],1\nmov dword ptr [esi+0x440],1", "mov dword ptr [esi+0x214],9999");
		}
		public static void InfiniteMinion_D(GameContext Context)
		{
			AobReplace(Context, "mov dword ptr [esi+0x214],9999\nmov dword ptr [esi+0x440],1", "mov dword ptr [esi+0x214],1");
		}

		public static void InfiniteItem_E(GameContext Context)
		{
			AobReplace(Context, "dec dword ptr [eax+0x80]\nmov eax,[ebp+0x8]", "nop\nnop\nnop\nnop\nnop\nnop");
		}
		public static void InfiniteItem_D(GameContext Context)
		{
			AobReplace(Context, "nop\nnop\nnop\nnop\nnop\nnop\nmov eax,[ebp+0x8]", "dec dword ptr [eax+0x80]");
		}

		public static void InfiniteAmmo_E(GameContext Context)
		{
			AobReplace(Context, "dec dword ptr [ebx+0x80]\ncmp dword ptr [ebx+0x80],0", "nop\nnop\nnop\nnop\nnop\nnop");
		}
		public static void InfiniteAmmo_D(GameContext Context)
		{
			AobReplace(Context, "nop\nnop\nnop\nnop\nnop\nnop\ncmp dword ptr [ebx+0x80],0", "dec dword ptr [ebx+0x80]");
		}

		public static void InfiniteFly_E(GameContext Context)
		{
			AobReplace(Context, "fstp dword ptr [ecx+0x220]\npop ebp\nret", "nop\nnop\nnop\nnop\nnop\nnop");
		}
		public static void InfiniteFly_D(GameContext Context)
		{
			AobReplace(Context, "nop\nnop\nnop\nnop\nnop\nnop\npop ebp\nret", "fstp dword ptr [ecx+0x220]");
		}

		public static void HighLight_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				@"mov [ebp-0x48],edx
fld dword ptr [esi+0x8]
fld dword ptr [ebp-0x3c]
fcomip st(1)
fstp st(0)") + 3;
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext,
				AssemblySnippet.FromASMCode(
					@"mov dword ptr [esi+0x8],0x3f800000
mov dword ptr [esi+0x10],0x3f800000
mov dword ptr [esi+0x18],0x3f800000
fld dword ptr [esi+0x8]
fld dword ptr [ebp-0x3c]"
),
					a, false
				);
		}

		public static void HighLight_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(Context.HContext.Handle, "df f1 dd d8 7a 0a 73 08 d9 46 08 d9 5d c4 eb 2c d9 45 c4 dd 05") - 6;
			if (a <= 0)
				return;
			InlineHook.FreeHook(Context.HContext, a);
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
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [esi+0x414],edx\ncmp dword ptr [esi+0x370],0");
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode("mov dword ptr [esi+0x410],0x41200000"), a, false);
		}
		public static void LowGravity_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"fldz\nfstp dword ptr [esi+0x410]") + 8;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void FastSpeed_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"fstp dword ptr [esi+0x3bc]\nmov [esi+0x54b],dl");
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x3bc],0x464b2000\nmov dword ptr [esi+0x3e4],0x464b2000"),
				a, false, false);
		}
		public static void FastSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [esi+0x54b],dl\nmov [esi+0x54d],dl") - 6;
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
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [ebp-0x18],eax\ncmp byte ptr [ebx+0x62e],0") + 3;
			int b = a + 0x7;
			int c = a + 0xf;
			int d = a + 0x14;
			int e = a + 0x17;
			int y = 0;
			int t = 1000;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, e, ref y, 4, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, y, ref t, 4, 0);
			byte[] bs = { 0x90, 0x90 };
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, b, bs, bs.Length, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, c, bs, bs.Length, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, d, bs, bs.Length, 0);

		}
		public static void GrabItemFarAway_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov [ebp-0x18],eax\ncmp byte ptr [ebx+0x62e],0") + 3;
			int b = a + 0x7;
			int c = a + 0xf;
			int d = a + 0x14;
			byte[] bs = { 0x74, 0x15 };
			byte[] cs = { 0x7C, 0x0D };
			byte[] ds = { 0x7F, 0x08 };
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, b, bs, bs.Length, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, c, cs, cs.Length, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, d, ds, ds.Length, 0);
		}

		public static void BonusTwoSlots_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov byte ptr [esi+0x5c0],0\nmov byte ptr [esi+0x514],0\nmov byte ptr [esi+0x5aa],0") - 6;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x140],2"),
				a, false, false);
			byte[] bs = { 0x90, 0x90 };

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a - 0x10, bs, bs.Length, 0);
		}
		public static void BonusTwoSlots_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext.Handle,
				"mov byte ptr [esi+0x5c0],0\nmov byte ptr [esi+0x514],0\nmov byte ptr [esi+0x5aa],0") - 6;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [esi+0x140],2"),
				a, false, false);

			InlineHook.FreeHook(Context.HContext, a);

			byte[] bs = { 0x74, 0x0c };

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a - 0x10, bs, bs.Length, 0);
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
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"33 c9 89 4c 90 08 42 3b") + 0x13;
			int max = 2000;
			int v = 0, y = max;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a, ref v, 4, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, v, ref y, 4, 0);

			NativeFunctions.ReadProcessMemory(Context.HContext.Handle,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Recipe", "FindRecipes") + 0x1c,
				ref v, 4, 0);
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, v, ref y, 4, 0);

			for (int i = 0; i < max; i++)
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
				"81 78 6c 21 06 00 00 0f 85") + 0x13;
			int v = 100;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ref v, 4, 0);
		}
		public static void StrengthenVampireKnives_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"81 78 6c 21 06 00 00 0f 85") + 0x13;
			int v = 4;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ref v, 4, 0);
		}

		public static void SuperRange_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"89 44 8A 08 41 3B");
			int b = a + 0x1a;
			int c = a + 0x24;
			int v = 999;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, b, ref v, 4, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, c, ref v, 4, 0);
		}
		public static void SuperRange_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"89 44 8A 08 41 3B");
			int b = a + 0x1a;
			int c = a + 0x24;
			int v1 = 5;
			int v2 = 4;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, b, ref v1, 4, 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, c, ref v2, 4, 0);
		}

		public static void FastTileSpeed_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"d9 98 c8 03 00 00 8b 85 30 f0 ff ff d9");
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword ptr [eax+0x3c8],0x3e800000"),
				a, false, false);
		}
		public static void FastTileSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "8b 85 30 f0 ff ff d9 80 c4 03 00 00") - 6;

			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void MachinicalRulerEffect_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"d9 9e c0 03 00 00 88 96 f0 05 00 00") + 12;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x5f6],0x1"),
				a, false, false);
		}
		public static void MachinicalRulerEffect_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "d9 9e c0 03 00 00 88 96 f0 05 00 00") + 12;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void RulerEffect_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"88 96 F8 05 00 00 88 96 F9 05 00 00") - 6;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x5f7],0x1"),
				a, false, false);
		}
		public static void RulerEffect_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "88 96 F8 05 00 00 88 96 F9 05 00 00") - 6;

			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ShowCircuit_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"88 96 1D 06 00 00 88 96 1E 06 00 00") - 6;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x62a],0x1"),
				a, false, false);
		}
		public static void ShowCircuit_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "88 96 F8 05 00 00 88 96 F9 05 00 00") - 6;
			InlineHook.FreeHook(Context.HContext, a);
		}

		public static void ShadowDodge_E(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				Context.HContext.Handle,
				"00 00 88 96 33 05 00 00 88 96 A9 05 00 00") - 4;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov byte ptr [esi+0x532],0x1"),
				a, false, false);
		}
		public static void ShadowDodge_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(
				   Context.HContext.Handle,
				   "88 96 33 05 00 00 88 96 A9 05 00 00") - 6;
			InlineHook.FreeHook(Context.HContext, a);
		}

		[Obsolete("Not so safe to call this")]
		public static void RevealMap_Obsoleted(GameContext Context)
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

		public static void RevealMap(GameContext Context)
		{
			AssemblySnippet asm = AssemblySnippet.FromClrCall(
				Context.HContext.GetAddressHelper("TRInjections.dll").GetFunctionAddress("TRInjections.Utils", "RevealMap"), null, true);

			InlineHook.InjectAndWait(Context.HContext, asm,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate"), true);
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

		[Obsolete("Not so safe to call this")]
		public static void RightClickToTP_Obsolete(GameContext Context)
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
		public static void RightClickToTP(GameContext Context)
		{
			byte s = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle,
				Context.HContext.MainAddressHelper.GetFunctionAddress("Terraria.Main", "DoUpdate") + 5, ref s, 1, 0);
			if (s == 0xE9)//已经被修改，不能再hook
				return;

			AssemblySnippet asm = AssemblySnippet.FromClrCall(
				Context.HContext.GetAddressHelper("TRInjections.dll").GetFunctionAddress("TRInjections.Utils", "RightClickTPCheck"), null, true);
			InlineHook.Inject(Context.HContext, asm,
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

	}
}
