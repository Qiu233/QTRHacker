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
		private unsafe static extern void ParseAssemble(string code, bool x64, uint IP, byte* dest, UInt32* len);


		private Assembler() { }

		public unsafe static byte[] Assemble(string code, UInt32 IP)
		{
			byte[] result;
			UInt32 len;
			byte* bs = (byte*)Marshal.AllocHGlobal(1024).ToPointer();
			ParseAssemble(code, false, IP, bs, &len);
			result = new byte[len];
			for (int i = 0; i < len; i++)
			{
				result[i] = bs[i];
			}
			Marshal.FreeHGlobal(new IntPtr(bs));
			return result;
		}

	}
}
