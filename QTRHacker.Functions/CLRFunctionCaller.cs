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
		public static void Call(GameContext Context, string moduleName, string typeName, string functionName, int hookAddress, params object[] args)
		{
			AddressHelper addrHelper = Context.HContext.GetAddressHelper(moduleName);
			int addr = addrHelper.GetFunctionAddress(typeName, functionName);
			Dictionary<int, int> strAddrs = new Dictionary<int, int>();

			object[] trueArgs = args.Select(t =>
			{
				if (!(t is string))
					return t;
				string str = t as string;
				if (!str.TrimStart().StartsWith("@"))
					return t;
				string trueStr = str.Substring(str.IndexOf("@") + 1);
				int strEnd = 0;
				byte[] bs = Encoding.Unicode.GetBytes(str);
				int maddr = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, bs.Length + 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
				int taddr = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, maddr, bs, bs.Length, 0);
				NativeFunctions.WriteProcessMemory(Context.HContext.Handle, maddr + bs.Length, ref strEnd, 4, 0);
				strAddrs[taddr] = maddr;
				return taddr;
			}).ToArray();


			AssemblySnippet snippet = AssemblySnippet.FromCode(
				new AssemblyCode[] {
					(Instruction)"pushad",
					AssemblySnippet.FromCode(
						strAddrs.Select(t=>AssemblySnippet.ConstructString(
							Context.HContext,t.Value,t.Key
							))),
					AssemblySnippet.FromClrCall(
						Context.HContext.GetAddressHelper(moduleName).GetFunctionAddress(typeName,functionName),null,false,
						trueArgs),
					(Instruction)"popad"
			});

			InlineHook.InjectAndWait(Context.HContext, snippet, hookAddress, true);

			foreach (var addrs in trueArgs)
				NativeFunctions.VirtualFreeEx(Context.HContext.Handle, addr, 0);

		}
	}
}
