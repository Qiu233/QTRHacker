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

		public void Inject(AssemblySnippet snippet, UInt32 targetAddr, bool timesLimit = false, UInt32 times = 1)
		{
			if(!timesLimit)
			{

			}
		}
	}
}
