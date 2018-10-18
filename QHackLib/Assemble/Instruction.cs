using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class Instruction : AssemblyCode
	{
		public string Code
		{
			get;
		}
		private Instruction(string code)
		{
			this.Code = code;
		}
		public static Instruction Create(string code)
		{
			return new Instruction(code);
		}
		public override string GetCode()
		{
			return Code;
		}
		public override byte[] GetByteCode(int ip)
		{
			return Assembler.Assemble(Code, ip);
		}
		public static explicit operator Instruction(string s)
		{
			return Instruction.Create(s);
		}
	}
}
