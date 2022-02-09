using QHackLib.Assemble;
using QHackLib.Memory;
using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Scripts
{
	public static class ScriptHelper
	{
		public unsafe static int GetOffset(GameContext context, string module, string type, string field)
		{
			return (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
		}
		public unsafe static int GetOffset(GameContext context, string type, string field)
		{
			return (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
		}
		public static void AobReplaceASM(GameContext Context, string src, string target)
		{
			var addrs = AobscanHelper.Aobscan(Context.HContext.Handle, Assembler.Assemble(src, 0));
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

	}
}
