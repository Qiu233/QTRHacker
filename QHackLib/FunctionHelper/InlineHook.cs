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
		[StructLayout(LayoutKind.Sequential)]
		private struct InlineHookInfo
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string Name;
			public bool TimesLimit;
			public UInt32 Times;
		}
		private const string Aob = "05 BF 3D 74 4D F3 41 3E 9F B7 0A 01 1B A7 FE BD";

		private Context Context;
		private UInt32 InfoAddress;
		public InlineHook(Context ctx)
		{
			Context = ctx;
			int addr = AobscanHelper.Aobscan(ctx, Aob);
			if (addr == -1)
			{
				byte[] b = AobscanHelper.GetHexCodeFromString(Aob);
				InfoAddress = NativeFunctions.VirtualAllocEx(ctx.Handle, 0, 1024 * 16, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite) + 16;
				NativeFunctions.WriteProcessMemory(ctx.Handle, InfoAddress, b, (UInt32)b.Length, 0);
			}
			else
			{
				InfoAddress = (UInt32)addr + 16;
			}
			Console.WriteLine("{0:X8}", InfoAddress);
		}
		public void Inject(string hookName, bool timesLimit, UInt32 times)
		{

		}
	}
}
