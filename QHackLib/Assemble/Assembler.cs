using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class Assembler
	{
		[DllImport("asmjit.dll", CharSet = CharSet.Ansi)]
		private unsafe static extern void ParseAssemble(string code, bool x64, int IP, byte* dest, int* len);


		private Assembler() { }

		public unsafe static byte[] Assemble(string code, int IP)
		{
			byte[] result;
			int len;
			byte* bs = (byte*)Marshal.AllocHGlobal(1024).ToPointer();
			ParseAssemble(code, false, IP, bs, &len);
			result = new byte[len];
			for (int i = 0; i < len; i++)
				result[i] = bs[i];
			Marshal.FreeHGlobal(new IntPtr(bs));
			return result;
		}

	}
}
