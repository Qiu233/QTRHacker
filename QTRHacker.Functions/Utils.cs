using QHackLib;
using QHackLib.Assemble;
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
	}
}
