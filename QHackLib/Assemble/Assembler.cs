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
		[DllImport("KeystoneWrapper", CharSet = CharSet.Ansi)]
		private unsafe static extern int GetAsmCode(string code, byte** mcode, int* len);

		[DllImport("KeystoneWrapper", CharSet = CharSet.Ansi)]
		private unsafe static extern int FreeCode(byte* mcode);

		private Assembler() { }

		public unsafe static byte[] Assemble(string code)
		{
			byte[] result;
			byte* mcode;
			int len;
			GetAsmCode(code, &mcode, &len);
			result = new byte[len];
			for (int i = 0; i < len; i++)
			{
				result[i] = mcode[i];
			}
			FreeCode(mcode);
			return result;
		}
	}
}
