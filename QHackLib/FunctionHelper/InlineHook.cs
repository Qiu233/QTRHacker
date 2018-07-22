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
		[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
		private struct InlineHookInfo
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string Name;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
			public byte[] OriginBytes;
			public bool TimesLimit;
			public UInt32 Times;
			public UInt32 TargetAddress;
			public UInt32 CodeAddress;
			public UInt32 FlagAddress;
		}


		[DllImport("kernel32.dll")]
		private static extern bool ReadProcessMemory
		(
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			ref InlineHookInfo lpBuffer,
			UInt32 nSize,
			UInt32 BytesRead
		);
		[DllImport("kernel32.dll")]
		private static extern bool WriteProcessMemory
		(
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			ref InlineHookInfo lpBuffer,
			UInt32 nSize,
			UInt32 BytesWrite
		);

		private const string Aob = "05 BF 3D 74 4D F3 41 3E 9F B7 0A 01 1B A7 FE BD";

		private Context Context;
		private UInt32 InfoCountAddress;
		private UInt32 InfoAddress;
		public InlineHook(Context ctx)
		{
			Context = ctx;
			/*int addr = AobscanHelper.Aobscan(ctx, Aob);
			if (addr == -1)
			{
				byte[] b = AobscanHelper.GetHexCodeFromString(Aob);
				InfoCountAddress = NativeFunctions.VirtualAllocEx(ctx.Handle, 0, 1024 * 16, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite) + 16;
				NativeFunctions.WriteProcessMemory(ctx.Handle, InfoCountAddress, b, (UInt32)b.Length, 0);
			}
			else
			{
				InfoCountAddress = (UInt32)addr + 16;
			}
			InfoAddress = InfoCountAddress + 4;*/
		}
		/*public int Inject(string hookName, AssemblySnippet snippet, UInt32 targetAddr, bool timesLimit = false, UInt32 times = 1)
		{
			int numberOfHooks = 0;
			NativeFunctions.ReadProcessMemory(Context.Handle, InfoCountAddress, ref numberOfHooks, 4, 0);

			UInt32 sizeOfInfo = (UInt32)Marshal.SizeOf(typeof(InlineHookInfo));
			InlineHookInfo info = new InlineHookInfo();
			UInt32 addr = 0;
			for (int i = 0; i < numberOfHooks; i++)
			{
				addr = (UInt32)(InfoAddress + i * sizeOfInfo);
				ReadProcessMemory(Context.Handle, addr, ref info, sizeOfInfo, 0);
				if (info.Name == hookName)
				{
					return 2;
				}
			}

			info = new InlineHookInfo();
			info.Name = hookName;
			NativeFunctions.ReadProcessMemory(Context.Handle,)

			addr = (UInt32)(InfoAddress + numberOfHooks * sizeOfInfo);
			WriteProcessMemory(Context.Handle, addr, ref info, sizeOfInfo, 0);

			numberOfHooks++;

			NativeFunctions.WriteProcessMemory(Context.Handle, InfoCountAddress, ref numberOfHooks, 4, 0);
			Console.WriteLine(numberOfHooks);
			return 1;
		}*/

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

		public void Inject(AssemblySnippet snippet, UInt32 targetAddr, bool timesLimit = false, UInt32 times = 1)
		{
			byte[] code = new byte[32];
			NativeFunctions.ReadProcessMemory(Context.Handle, targetAddr, code, (UInt32)code.Length, 0);
			byte[] headBytes = GetHeadBytes(code);
			if (!timesLimit)
			{
				UInt32 codeAddr = NativeFunctions.VirtualAllocEx(Context.Handle, 0, 1024, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
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
			}
		}
	}
}
