using QHackCLR.Common;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class CLRFunctionCaller
	{
		public static void Call(GameContext Context, string moduleName, string typeName, string functionName, params object[] args)
		{
			CLRHelper addrHelper = Context.HContext.GetCLRHelper(moduleName);
			nuint addr = addrHelper.GetFunctionAddress(typeName, functionName);
			Call(Context, addr, args);
		}

		public static void Call(GameContext Context, ClrMethod method, params object[] args)
		{
			Call(Context, method.NativeCode, args);
		}

		public static void Call(GameContext Context, nuint targetAddr, params object[] args)
		{
			AssemblySnippet snippet = AssemblySnippet.FromCode(
				new AssemblyCode[] {
					(Instruction)"pushad",
					AssemblySnippet.FromClrCall(
						targetAddr,false,null,null,null,
						args),
					(Instruction)"popad"
			});
			Context.RunByHookOnUpdate(snippet);

		}
	}
}