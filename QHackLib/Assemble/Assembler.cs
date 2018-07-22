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
		[DllImport("XEDWrapper.dll", CharSet = CharSet.Ansi)]
		private unsafe static extern void GetAsmCode(string code, byte** mcode, UInt64 IP, int* len);


		private Assembler() { }

		public unsafe static byte[] AssembleSingleInstruction(string code, UInt64 IP)
		{
			byte[] result;
			byte* mcode;
			int len;
			GetAsmCode(code, &mcode, IP, &len);
			result = new byte[len];
			for (int i = 0; i < len; i++)
			{
				result[i] = mcode[i];
			}
			return result;
		}

		public static byte[] AssembleInstructionBlock(string code, UInt64 IP)
		{
			List<byte> v = new List<byte>();
			string[] s = code.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var ss in s)
			{
				byte[] y = AssembleSingleInstruction(ss, IP);
				v.AddRange(y);
				IP += (UInt32)y.Count();
			}
			return v.ToArray();
		}
	}
}
