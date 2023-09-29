using QHackCLR.Common;
using QHackCLR.Entities;
using QHackLib;
using QHackLib.Assemble;

namespace QTRHacker.Core;

public class CLRFunctionCaller
{
	public static void Call(GameContext Context, string moduleName, string typeName, string functionName, params object[] args)
	{
		CLRHelper addrHelper = Context.HContext.GetCLRHelper(moduleName);
		nuint addr = addrHelper.GetFunctionAddress(typeName, functionName);
		Call(Context, addr, args);
	}

	public static void Call(GameContext Context, CLRMethod method, params object[] args)
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
		Context.RunByHookUpdate(snippet);

	}
}