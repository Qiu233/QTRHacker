using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using QTRInjectionBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.InjectHook
{
	public class InjectHookHelper
	{
		public static void Process(GameContext Context, string AssemblyFile)
		{
			var hookAddressHelper = Context.HContext.GetAddressHelper(Path.GetFileName(AssemblyFile));
			var assembly = Assembly.LoadFrom(AssemblyFile);

			foreach (var type in assembly.DefinedTypes)
			{
				var methods = type.DeclaredMethods;
				foreach (var method in methods)
				{
					if (method.IsAbstract)
						continue;
					if (!method.IsDefined(typeof(HookAttribute)))
						continue;
					HookAttribute hook = method.GetCustomAttribute<HookAttribute>();
					int targetAddress = hookAddressHelper[type.FullName, method.Name];
					int rawAddress = 0;
					if (hook.HookType == HookType.BEFORE)
					{
						rawAddress = Context.HContext.GetAddressHelper(hook.AssemblyName)[hook.TargetType, hook.TargetMethodName];
					}
					else
					{
						var ah = Context.HContext.GetAddressHelper(hook.AssemblyName);
						var clrmethod = ah.GetClrMethod(hook.TargetType, hook.TargetMethodName);
						int offset = clrmethod.ILOffsetMap.Max(t => t.ILOffset);
						rawAddress = (int)clrmethod.ILOffsetMap.First(t => t.ILOffset == offset).StartAddress;


						//wrong,reserved
						/*int address = (int)clrmethod.ILOffsetMap.First(t => t.ILOffset == offset).StartAddress;
						byte[] j = new byte[16];
						NativeFunctions.ReadProcessMemory(Context.HContext.Handle, address, j, j.Length, 0);
						int u = 0;
						while (u < j.Length)
						{
							if (j[u] == 0xC3)
								break;
							u += Ldasm.GetASMLength(j, u, false);
						}

						rawAddress = address + u + 1;//pass the 'ret'*/
					}
					var snippet = AssemblySnippet.FromCode(new List<AssemblyCode>
					{
						(Instruction)"pushad",
						(Instruction)$"call {targetAddress}",
						(Instruction)"popad"
					});
					byte b = 0;
					NativeFunctions.ReadProcessMemory(Context.HContext.Handle, rawAddress, ref b, 1, 0);
					if (b == 0xE9)//already hooked
						InlineHook.FreeHook(Context.HContext, rawAddress);
					InlineHook.Inject(Context.HContext, snippet, rawAddress, false, true, 1024);
				}
			}
		}
	}
}
