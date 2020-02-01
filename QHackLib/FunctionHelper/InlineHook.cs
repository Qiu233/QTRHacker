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
		public struct HookInfo
		{
			public int CodeAddress;
			public int ComparisonFlagAddress;
			public int ComparisonInstructionAddress;
			public byte[] RawCodeBytes;

			public HookInfo(int codeAddress, int comparisonFlagAddress, int comparisonInstructionAddress, byte[] rawCodeBytes)
			{
				CodeAddress = codeAddress;
				ComparisonFlagAddress = comparisonFlagAddress;
				ComparisonInstructionAddress = comparisonInstructionAddress;
				RawCodeBytes = rawCodeBytes;
			}
		}
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
				NativeFunctions.ReadProcessMemory(Context.Handle, t.ComparisonInstructionAddress, ref y, 4, 0);
				if (y == 0)
				{
					if (t.ComparisonFlagAddress == 0)
					{
						NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, t.RawCodeBytes, t.RawCodeBytes.Length, 0);
						NativeFunctions.VirtualFreeEx(Context.Handle, t.CodeAddress, 0);
						NativeFunctions.VirtualFreeEx(Context.Handle, t.ComparisonInstructionAddress, 0);
						return;
					}
					else
					{
						NativeFunctions.ReadProcessMemory(Context.Handle, t.ComparisonFlagAddress, ref y, 4, 0);
						if (y == 0)
						{
							NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, t.RawCodeBytes, t.RawCodeBytes.Length, 0);
							NativeFunctions.VirtualFreeEx(Context.Handle, t.CodeAddress, 0);
							NativeFunctions.VirtualFreeEx(Context.Handle, t.ComparisonFlagAddress, 0);
							NativeFunctions.VirtualFreeEx(Context.Handle, t.ComparisonInstructionAddress, 0);
							return;
						}
					}
				}
			}
		}

		public static void FreeHook(Context Context, int targetAddr)
		{
			int t = 0, y = 0, j = 0, k = targetAddr;
			int headLen = 0;
			byte[] head;

			byte h = 0;
			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr, ref h, 1, 0);
			if (h != 0xE9) return;

			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr + 1, ref j, 4, 0);
			k += j + 5 - CodeOffset;
			NativeFunctions.ReadProcessMemory(Context.Handle, k, ref t, 4, 0);
			NativeFunctions.ReadProcessMemory(Context.Handle, k + 4, ref y, 4, 0);
			NativeFunctions.ReadProcessMemory(Context.Handle, k + 8, ref headLen, 4, 0);
			head = new byte[headLen];
			NativeFunctions.ReadProcessMemory(Context.Handle, k + 0xc, head, headLen, 0);

			NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, head, headLen, 0);

			NativeFunctions.VirtualFreeEx(Context.Handle, targetAddr, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, t, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, y, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, k, 0);
		}


		private static byte[] ProcessJmps(byte[] b, int rawAddr, int targetAddr)
		{

			IntPtr ptr3 = Marshal.AllocHGlobal(b.Length);
			Marshal.Copy(b, 0, ptr3, b.Length);
			unsafe
			{
				byte* p = (byte*)ptr3;
				byte* i = p;
				while (i - p < b.Length)
				{
					if (*i == 0xe9 || *i == 0xe8)//jmp or call
						*((int*)(i + 1)) += rawAddr - targetAddr;//move the call
					Ldasm.ldasm_data data = new Ldasm.ldasm_data();
					uint t = Ldasm.ldasm(i, ref data, false);
					i += t;
				}
			}
			byte[] result = new byte[b.Length];
			Marshal.Copy(ptr3, result, 0, b.Length);
			Marshal.FreeHGlobal(ptr3);
			return result;
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
		public static HookInfo Inject(Context Context, AssemblySnippet snippet, int targetAddr, bool once, bool execRaw = true, int codeSize = 1024)
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
				int headLen = headBytes.Length;

				NativeFunctions.WriteProcessMemory(Context.Handle, codeAddr, ref flagAddr, 4, 0);//0-4
				NativeFunctions.WriteProcessMemory(Context.Handle, codeAddr + 4, ref compAddr, 4, 0);//4-8
				NativeFunctions.WriteProcessMemory(Context.Handle, codeAddr + 8, ref headLen, 4, 0);//8-c
				NativeFunctions.WriteProcessMemory(Context.Handle, codeAddr + 0xc, headBytes, headLen, 0);//c-1c

				int addr = codeAddr + CodeOffset;
				byte[] snippetBytes = a.GetByteCode(addr);


				NativeFunctions.WriteProcessMemory(Context.Handle, addr, snippetBytes, snippetBytes.Length, 0);
				addr += snippetBytes.Length;

				if (execRaw)
				{
					byte[] bs = ProcessJmps(headBytes, targetAddr, addr);
					NativeFunctions.WriteProcessMemory(Context.Handle, addr, bs, bs.Length, 0);
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
				NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, jmpToBytes, jmpToBytes.Length, 0);
				return new HookInfo(codeAddr, flagAddr, compAddr, headBytes);
			}
		}
	}
}
