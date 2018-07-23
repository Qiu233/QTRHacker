using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class Instruction
	{
		public string Code;
		public Instruction(string code)
		{
			this.Code = code;
		}
		public byte[] GetByteCode(UInt32 IP)
		{
			return Assembler.Assemble(Code, IP);
		}
	}
}
