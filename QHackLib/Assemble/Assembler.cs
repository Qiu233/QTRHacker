using Keystone;
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
		private Assembler() { }

		public unsafe static byte[] Assemble(string code, int IP)
		{

			using (Engine keystone = new Engine(Architecture.X86, Mode.X32) { ThrowOnError = true })
			{
				EncodedData enc = keystone.Assemble(code, (ulong)IP);
				return enc.Buffer;
			}
		}

	}
}
