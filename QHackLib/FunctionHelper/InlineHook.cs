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
		private const string Aob = "05 BF 3D 74 4D F3 41 3E 9F B7 0A 01 1B A7 FE BD";

		private Context Context;
		public InlineHook(Context ctx)
		{
			Context = ctx;
		}

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

		public UInt32 Inject(AssemblySnippet snippet, UInt32 targetAddr, UInt32 codeSize = 1024)
		{
			byte[] code = new byte[32];
			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr, code, (UInt32)code.Length, 0);
			byte[] headBytes = GetHeadBytes(code);
			UInt32 codeAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, codeSize, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			UInt32 addr = codeAddr;
			byte[] snippetBytes = snippet.GetByteCode(addr);
			NativeFunctions.WriteProcessMemory(Context.Handle, addr, snippetBytes, (UInt32)snippetBytes.Length, 0);
			addr += (UInt32)snippetBytes.Length;
			NativeFunctions.WriteProcessMemory(Context.Handle, addr, headBytes, (UInt32)headBytes.Length, 0);
			addr += (UInt32)headBytes.Length;
			byte[] jmpBackBytes = Assembler.AssembleSingleInstruction("jmp " + (targetAddr + headBytes.Length).ToString("X8"), addr);
			NativeFunctions.WriteProcessMemory(Context.Handle, addr, jmpBackBytes, (UInt32)jmpBackBytes.Length, 0);
			byte[] jmpToBytesRaw = Assembler.AssembleSingleInstruction("jmp " + codeAddr.ToString("X8"), targetAddr);
			byte[] jmpToBytes = new byte[headBytes.Length];
			for (int i = 0; i < 5; i++)
				jmpToBytes[i] = jmpToBytesRaw[i];
			for (int i = 5; i < headBytes.Length; i++)
				jmpToBytes[i] = 0x90;//nop
			NativeFunctions.WriteProcessMemory(Context.Handle, targetAddr, jmpToBytes, (UInt32)jmpToBytes.Length, 0);
			return codeAddr;
		}
	}
}
