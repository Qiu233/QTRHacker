using QHackCLR.Common;
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
		public static unsafe int GetOffset(GameContext context, string module, string type, string field) => (int)context.HContext.GetCLRHelper(module).GetInstanceFieldOffset(type, field) + sizeof(nuint);
		public static unsafe int GetOffset(GameContext context, string type, string field) => (int)context.GameModuleHelper.GetInstanceFieldOffset(type, field) + sizeof(nuint);
		public static unsafe nuint GetFunctionAddress(GameContext context, string type, string func)
		{
			return context.GameModuleHelper.GetFunctionAddress(type, func);
		}
		public static unsafe nuint GetFunctionAddress(GameContext context, string type, Predicate<ClrMethod> methodPredicate)
		{
			return context.GameModuleHelper.GetFunctionAddress(type, methodPredicate);
		}
		public static unsafe nuint GetFunctionAddress(GameContext context, string module, string type, string func)
		{
			return context.HContext.GetCLRHelper(module).GetFunctionAddress(type, func);
		}
		public static unsafe nuint GetFunctionAddress(GameContext context, string module, string type, Predicate<ClrMethod> methodPredicate)
		{
			return context.HContext.GetCLRHelper(module).GetFunctionAddress(type, methodPredicate);
		}
		public static unsafe T Read<T>(GameContext context, nuint addr) where T : unmanaged
		{
			return context.HContext.DataAccess.Read<T>(addr);
		}
		public static unsafe void Write<T>(GameContext context, nuint addr, T value) where T : unmanaged
		{
			context.HContext.DataAccess.Write(addr, value);
		}
		public static void AobReplaceASM(GameContext Context, string asm, string target)
		{
			var addrs = AobscanHelper.AobscanASM(Context.HContext.Handle, asm);
			byte[] code = Assembler.Assemble(target, 0);
			foreach (var addr in addrs)
				Context.HContext.DataAccess.WriteBytes(addr, code);
		}
		public static void AobReplace(GameContext Context, string srcHex, string targetHex)
		{
			var addrs = AobscanHelper.Aobscan(Context.HContext.Handle, srcHex);
			byte[] code = AobscanHelper.GetHexCodeFromString(targetHex);
			foreach (var addr in addrs)
				Context.HContext.DataAccess.WriteBytes(addr, code);
		}
		public static IEnumerable<nuint> Aobscan(GameContext Context, string srcHex)
		{
			return AobscanHelper.Aobscan(Context.HContext.Handle, srcHex);
		}
		public static IEnumerable<nuint> AobscanASM(GameContext Context, string asm)
		{
			return AobscanHelper.AobscanASM(Context.HContext.Handle, asm);
		}
	}
}
