using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QHackLib.Utilities;
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
			while ((addr = AobscanHelper.AobscanASM(Context.HContext, src)) != -1)
			{
				byte[] code = Assembler.Assemble(target, 0);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, addr + offset, code, code.Length, 0);
			}
		}

		public static void InfiniteLife_E(GameContext Context)
		{
			AobReplace(Context, "sub [edx+0x340],eax\ncmp dword [ebp+0x8],-1", "add [edx+0x340],eax");
		}
		public static void InfiniteLife_D(GameContext Context)
		{
			AobReplace(Context, "add [edx+0x340],eax\ncmp dword [ebp+0x8],-1", "sub [edx+0x340],eax");
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
			AobReplace(Context, "dec [eax+0x2B4]\ncmp dword [eax+0x2B4],0", "inc [eax+0x2B4]");
		}
		public static void InfiniteOxygen_D(GameContext Context)
		{
			AobReplace(Context, "inc [eax+0x2B4]\ncmp dword [eax+0x2B4],0", "dec [eax+0x2B4]");
		}

		public static void InfiniteMinion_E(GameContext Context)
		{
			AobReplace(Context, "mov dword [esi+0x214],1\nmov dword [esi+0x440],1", "mov dword [esi+0x214],9999");
		}
		public static void InfiniteMinion_D(GameContext Context)
		{
			AobReplace(Context, "mov dword [esi+0x214],9999\nmov dword [esi+0x440],1", "mov dword [esi+0x214],1");
		}

		public static void InfiniteItem_E(GameContext Context)
		{
			AobReplace(Context, "dec [eax+0x80]\nmov eax,[ebp+0x8]", "nop\nnop\nnop\nnop\nnop\nnop");
		}
		public static void InfiniteItem_D(GameContext Context)
		{
			AobReplace(Context, "nop\nnop\nnop\nnop\nnop\nnop\nmov eax,[ebp+0x8]", "dec [eax+0x80]");
		}

		public static void InfiniteAmmo_E(GameContext Context)
		{
			AobReplace(Context, "dec [ebx+0x80]\ncmp dword [ebx+0x80],0", "nop\nnop\nnop\nnop\nnop\nnop");
		}
		public static void InfiniteAmmo_D(GameContext Context)
		{
			AobReplace(Context, "nop\nnop\nnop\nnop\nnop\nnop\ncmp dword [ebx+0x80],0", "dec [ebx+0x80]");
		}

		public static void InfiniteFly_E(GameContext Context)
		{
			AobReplace(Context, "fstp dword [ecx+0x220]\npop ebp\nret", "nop\nnop\nnop\nnop\nnop\nnop");
		}
		public static void InfiniteFly_D(GameContext Context)
		{
			AobReplace(Context, "nop\nnop\nnop\nnop\nnop\nnop\npop ebp\nret", "fstp dword [ecx+0x220]");
		}

		public static void HighLight_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				@"mov [ebp-0x48],edx
fld dword [esi+0x8]
fld dword [ebp-0x3c]
fcomip st1
fstp st0") + 3;
			if (a <= 0)
				return;
			InlineHook.Inject(Context.HContext,
				AssemblySnippet.FromASMCode(
					@"mov dword [esi+0x8],0x3f800000
mov dword [esi+0x10],0x3f800000
mov dword [esi+0x18],0x3f800000
fld dword [esi+0x8]
fld dword [ebp-0x3c]"
),
					a, false
				);
		}

		public static void HighLight_D(GameContext Context)
		{
			int a = AobscanHelper.Aobscan(Context.HContext, "df f1 dd d8 7a 0a 73 08 d9 46 08 d9 5d c4 eb 2c d9 45 c4 dd 05") - 6;
			if (a <= 0)
				return;
			var ass = Assembler.Assemble(@"fld dword [esi+0x8]
fld dword [ebp-0x3c]", 0);
			int y = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 1, ref y, 4, 0);
			y += a + 5;

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ass, ass.Length, 0);

			InlineHook.FreeHook(Context.HContext, y);
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
				Context.HContext,
				"mov [esi+0x414],edx\ncmp dword [esi+0x370],0");
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode("mov dword [esi+0x410],0x41200000"), a, false);
		}
		public static void LowGravity_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"fldz\nfstp dword [esi+0x410]") + 8;

			int t = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 1, ref t, 4, 0);
			t += a + 5;

			var ass = Assembler.Assemble("mov [esi+0x414],edx", 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ass, ass.Length, 0);

			InlineHook.FreeHook(Context.HContext, t);
		}

		public static void FastSpeed_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"fstp dword [esi+0x3bc]\nmov [esi+0x54b],dl");
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword [esi+0x3bc],0x464b2000\nmov dword [esi+0x3e4],0x464b2000"),
				a, false, false);
		}
		public static void FastSpeed_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"mov [esi+0x54b],dl\nmov [esi+0x54d],dl") - 6;

			int t = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 1, ref t, 4, 0);
			t += a + 5;

			var ass = Assembler.Assemble("fstp dword [esi+0x3bc]", 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, ass, ass.Length, 0);

			InlineHook.FreeHook(Context.HContext, t);
		}

		public static void ProjectileIgnoreTile_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"mov [ebp-0x20],eax\ncmp byte [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, new byte[] { 0x8d }, 1, 0);
		}
		public static void ProjectileIgnoreTile_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"mov [ebp-0x20],eax\ncmp byte [ebx+0xE7],0") + 11;
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, new byte[] { 0x84 }, 1, 0);
		}

		public static void GrabItemFarAway_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"mov [ebp-0x18],eax\ncmp byte [ebx+0x62e],0") + 3;
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
				Context.HContext,
				"mov [ebp-0x18],eax\ncmp byte [ebx+0x62e],0") + 3;
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
				Context.HContext,
				"mov byte [esi+0x5c0],0\nmov byte [esi+0x514],0\nmov byte [esi+0x5aa],0") - 6;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword [esi+0x140],2"),
				a, false, false);
			byte[] bs = { 0x90, 0x90 };

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a - 0x10, bs, bs.Length, 0);
		}
		public static void BonusTwoSlots_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				"mov byte [esi+0x5c0],0\nmov byte [esi+0x514],0\nmov byte [esi+0x5aa],0") - 6;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword [esi+0x140],2"),
				a, false, false);

			int y = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 1, ref y, 4, 0);
			y += a + 5;

			byte[] b = Assembler.Assemble("mov [esi+0x140],edx", 0);
			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, b, b.Length, 0);
			InlineHook.FreeHook(Context.HContext, y);

			byte[] bs = { 0x74, 0x0c };

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a - 0x10, bs, bs.Length, 0);
		}

		public static void GoldHoleDropsBag_E(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				Context.HContext,
				@"push 0
push 0
push 0x49
push 1
push 0
push 0
push 0
push 0") + 2 * 5;
			InlineHook.Inject(Context.HContext, AssemblySnippet.FromASMCode(
				"mov dword [esp+8],3332"),
				a, false);
		}
		public static void GoldHoleDropsBag_D(GameContext Context)
		{
			int a = AobscanHelper.AobscanASM(
				   Context.HContext,
				   @"push 0
push 0
push 0x49
push 1
push 0") + 2 * 5;

			int y = 0;
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 1, ref y, 4, 0);
			y += a + 5;
			Console.WriteLine(y.ToString("X8"));

			byte[] b = Assembler.Assemble(@"push 0
push 0
push 0", 0);

			NativeFunctions.WriteProcessMemory(Context.HContext.Handle, a, b, b.Length, 0);

			InlineHook.FreeHook(Context.HContext, y);
		}
	}
}
