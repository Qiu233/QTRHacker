using QHackLib.Assemble;
using QHackLib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public class InlineHook
	{
		private static readonly Object thisLock = new Object();
		private const int CodeOffset = 64;
		private InlineHook() { }

		public static byte[] GetHeadBytes(byte[] code)
		{
			IntPtr ptr3 = Marshal.AllocHGlobal(code.Length);
			Marshal.Copy(code, 0, ptr3, code.Length);
			UInt32 len = 0;
			unsafe
			{
				byte* p = (byte*)ptr3.ToPointer();
				byte* i = p;
				while (i - p < 5)
				{
					Ldasm.ldasm_data data = new Ldasm.ldasm_data();
					UInt32 t = Ldasm.ldasm(i, ref data, false);
					i += t;
				}
				len = (UInt32)(i - p);
			}
			Marshal.FreeHGlobal(ptr3);
			byte[] v = new byte[len];
			for (int i = 0; i < len; i++)
			{
				v[i] = code[i];
			}
			return v;
		}

		public static void InjectAndWait(Context Context, AssemblySnippet snippet, int targetAddr, bool once)
		{
			var t = Inject(Context, snippet, targetAddr, once);
			System.Threading.Thread.Sleep(10);
			while (true)
			{
				int y = 0;
				NativeFunctions.ReadProcessMemory(Context.Handle, t.Item3, ref y, 4, 0);
				if (y == 0)
				{
					if (t.Item2 == 0)
					{
						NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, t.Item4, t.Item4.Length, 0);
						NativeFunctions.VirtualFreeEx(Context.Handle, t.Item1, 0);
						NativeFunctions.VirtualFreeEx(Context.Handle, t.Item3, 0);
						return;
					}
					else
					{
						NativeFunctions.ReadProcessMemory(Context.Handle, t.Item2, ref y, 4, 0);
						if (y == 0)
						{
							NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, t.Item4, t.Item4.Length, 0);
							NativeFunctions.VirtualFreeEx(Context.Handle, t.Item1, 0);
							NativeFunctions.VirtualFreeEx(Context.Handle, t.Item2, 0);
							NativeFunctions.VirtualFreeEx(Context.Handle, t.Item3, 0);
							return;
						}
					}
				}
			}
		}

		public static void FreeHook(Context Context, int targetAddr)
		{
			int t = 0, y = 0;

			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr, ref t, 4, 0);
			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr + 4, ref y, 4, 0);

			NativeFunctions.VirtualFreeEx(Context.Handle, targetAddr, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, t, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, y, 0);
		}


		/// <summary>
		/// 这个函数被lock了，无法被多个线程同时调用，预防了一些错误
		/// </summary>
		/// <param name="Context"></param>
		/// <param name="snippet"></param>
		/// <param name="targetAddr"></param>
		/// <param name="once"></param>
		/// <param name="execRaw"></param>
		/// <param name="codeSize"></param>
		/// <returns></returns>
		public static Tuple<int, int, int, byte[]> Inject(Context Context, AssemblySnippet snippet, int targetAddr, bool once, bool execRaw = true, int codeSize = 1024)
		{
			lock (thisLock)
			{
				int codeAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
				int compAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
				int flagAddr = 0;

				AssemblySnippet a = AssemblySnippet.FromEmpty();
				a.Content.Add(Instruction.Create("__$$__:"));//very begin
				a.Content.Add(Instruction.Create("mov dword ptr [" + compAddr + "],1"));
				if (once)
				{
					flagAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
					NativeFunctions.WriteProcessMemory(Context.Handle, flagAddr, ref once, 4, 0);
					a.Content.Add(Instruction.Create("cmp dword ptr [" + flagAddr + "],0"));
					a.Content.Add(Instruction.Create("jle jalkjflakjl"));
				}
				a.Content.Add(snippet);
				if (once)
				{
					a.Content.Add(Instruction.Create("dec dword ptr [" + flagAddr + "]"));
					a.Content.Add(Instruction.Create("jalkjflakjl:"));
				}
				byte[] code = new byte[32];
				NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr, code, code.Length, 0);


				byte[] headBytes = GetHeadBytes(code);


				NativeFunctions.WriteProcessMemory(Context.Handle, codeAddr, ref flagAddr, 4, 0);
				NativeFunctions.WriteProcessMemory(Context.Handle, codeAddr + 4, ref compAddr, 4, 0);

				int addr = codeAddr + CodeOffset;
				byte[] snippetBytes = a.GetByteCode(addr);


				NativeFunctions.WriteProcessMemory(Context.Handle, addr, snippetBytes, snippetBytes.Length, 0);
				addr += snippetBytes.Length;

				if (execRaw)
				{
					NativeFunctions.WriteProcessMemory(Context.Handle, addr, headBytes, headBytes.Length, 0);
					addr += headBytes.Length;
				}

				byte[] compBytes = Assembler.Assemble("mov dword ptr [" + compAddr + "],0", addr);
				NativeFunctions.WriteProcessMemory(Context.Handle, addr, compBytes, compBytes.Length, 0);
				addr += compBytes.Length;


				byte[] jmpBackBytes = Assembler.Assemble("jmp " + (targetAddr + headBytes.Length), addr);
				NativeFunctions.WriteProcessMemory(Context.Handle, addr, jmpBackBytes, jmpBackBytes.Length, 0);
				addr += jmpBackBytes.Length;
				
				byte[] jmpToBytesRaw = Assembler.Assemble("jmp " + (codeAddr + CodeOffset), targetAddr);
				byte[] jmpToBytes = new byte[headBytes.Length];
				for (int i = 0; i < 5; i++)
					jmpToBytes[i] = jmpToBytesRaw[i];
				for (int i = 5; i < headBytes.Length; i++)
					jmpToBytes[i] = 0x90;//nop
				//Console.WriteLine(codeAddr.ToString("X8"));
				//Console.ReadKey();
				NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, jmpToBytes, jmpToBytes.Length, 0);
				return new Tuple<int, int, int, byte[]>(codeAddr, flagAddr, compAddr, headBytes);
			}
		}
	}
}
