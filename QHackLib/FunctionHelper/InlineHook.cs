﻿using QHackLib.Assemble;
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
		private InlineHook() { }

		private static byte[] GetHeadBytes(byte[] code)
		{
			IntPtr ptr3 = Marshal.AllocHGlobal(code.Length);
			Marshal.Copy(code, 0, ptr3, code.Length);
			UInt32 len = 0;
			unsafe
			{
				Ldasm.ldasm_data data = new Ldasm.ldasm_data();
				byte* p = (byte*)ptr3.ToPointer();
				byte* i = p;
				while (i - p < 5)
				{
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

		public static Tuple<int, int, int, byte[]> Inject(Context Context, AssemblySnippet snippet, int targetAddr, bool once, int codeSize = 1024)
		{
			int codeAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			int compAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			int flagAddr = 0;

			AssemblySnippet a = AssemblySnippet.FromEmpty();
			a.Content.Add(Instruction.Create("mov dword [0x" + compAddr.ToString("X8") + "],1"));
			if (once)
			{
				flagAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
				NativeFunctions.WriteProcessMemory(Context.Handle, flagAddr, ref once, 4, 0);
				a.Content.Add(Instruction.Create("cmp dword [0x" + flagAddr.ToString("X8") + "],0"));
				a.Content.Add(Instruction.Create("jle a"));
			}
			a.Content.Add(snippet);
			if (once)
			{

				a.Content.Add(Instruction.Create("dec [0x" + flagAddr.ToString("X8") + "]"));
				a.Content.Add(Instruction.Create("a:"));
			}
			byte[] code = new byte[32];
			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr, code, code.Length, 0);


			byte[] headBytes = GetHeadBytes(code);

			int addr = codeAddr;
			byte[] snippetBytes = a.GetByteCode(addr);


			NativeFunctions.WriteProcessMemory(Context.Handle, addr, snippetBytes, snippetBytes.Length, 0);
			addr += snippetBytes.Length;

			NativeFunctions.WriteProcessMemory(Context.Handle, addr, headBytes, headBytes.Length, 0);
			addr += headBytes.Length;


			byte[] compBytes = Assembler.Assemble("mov dword [0x" + compAddr.ToString("X8") + "],0", addr);
			NativeFunctions.WriteProcessMemory(Context.Handle, addr, compBytes, compBytes.Length, 0);
			addr += compBytes.Length;


			byte[] jmpBackBytes = Assembler.Assemble("jmp 0x" + (targetAddr + headBytes.Length).ToString("X8"), addr);
			NativeFunctions.WriteProcessMemory(Context.Handle, addr, jmpBackBytes, jmpBackBytes.Length, 0);
			addr += jmpBackBytes.Length;


			byte[] jmpToBytesRaw = Assembler.Assemble("jmp 0x" + codeAddr.ToString("X8"), targetAddr);
			byte[] jmpToBytes = new byte[headBytes.Length];
			for (int i = 0; i < 5; i++)
				jmpToBytes[i] = jmpToBytesRaw[i];
			for (int i = 5; i < headBytes.Length; i++)
				jmpToBytes[i] = 0x90;//nop
			NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, jmpToBytes, jmpToBytes.Length, 0);
			return new Tuple<int, int, int, byte[]>(codeAddr, flagAddr, compAddr, headBytes);
		}
	}
}