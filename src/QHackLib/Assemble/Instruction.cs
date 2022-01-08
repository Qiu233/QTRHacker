using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class Instruction : AssemblyCode
	{
		public string Content
		{
			get;
		}
		private Instruction(string code) => Content = code;
		public static Instruction Create(string code) => new(code);
		public override string GetCode() => Content;
		public override byte[] GetByteCode(nuint ip) => Assembler.Assemble(Content, ip);
		public override AssemblyCode Copy() => new Instruction(Content);
		public static explicit operator Instruction(string s) => Create(s);
	}
}
