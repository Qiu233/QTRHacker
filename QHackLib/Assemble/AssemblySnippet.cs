using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class AssemblySnippet
	{
		private List<Instruction> Instructions;

		private AssemblySnippet()
		{
			Instructions = new List<Instruction>();
		}

		public static AssemblySnippet FromASMCode(string asm)
		{
			AssemblySnippet s = new AssemblySnippet();

			string[] ss = asm.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			s.Instructions.AddRange(ss.Select(t => new Instruction(t)));
			return s;
		}
		private static string GetArgumentPassing(int index, ValueType v)
		{
			if (index > 1)
			{
				return "push " + ((int)v).ToString("X8");
			}
			else if (index == 0)
			{
				return "mov ecx," + ((int)v).ToString("X8");
			}
			else// if (index == 1)
			{
				return "mov edx," + ((int)v).ToString("X8");
			}
		}
		public static AssemblySnippet FromDotNetCall(UInt32 targetAddr, UInt32 retAddr, params ValueType[] arguments)
		{
			AssemblySnippet s = new AssemblySnippet();
			s.Instructions.Add(new Instruction("push ecx"));
			s.Instructions.Add(new Instruction("push edx"));
			int i = 0;
			foreach (var v in arguments)
			{
				if (v.GetType() == typeof(byte) || v.GetType() == typeof(sbyte) || v.GetType() == typeof(int) || v.GetType() == typeof(uint) || v.GetType() == typeof(char) || v.GetType() == typeof(short) || v.GetType() == typeof(ushort) || v.GetType() == typeof(long) || v.GetType() == typeof(ulong))
					s.Instructions.Add(new Instruction(GetArgumentPassing(i, v)));
				i++;
			}
			s.Instructions.Add(new Instruction("call " + ((int)targetAddr).ToString("X8")));
			s.Instructions.Add(new Instruction("mov [" + ((int)retAddr).ToString("X8") + "],eax"));
			if (i > 2)
			{
				s.Instructions.Add(new Instruction("add esp," + ((i - 2) * 4).ToString("X8")));
			}
			s.Instructions.Add(new Instruction("pop edx"));
			s.Instructions.Add(new Instruction("pop ecx"));
			return s;
		}
		public List<Instruction> GetInstructions()
		{
			return Instructions;
		}
		public byte[] GetByteCode(UInt64 IP)
		{
			List<byte> bs = new List<byte>();
			List<Instruction> s = GetInstructions();
			foreach (var v in s)
			{
				byte[] y = Assembler.AssembleSingleInstruction(v.Code, IP);
				bs.AddRange(y);
				IP += (UInt32)y.Count();
			}
			return bs.ToArray();
		}
	}
}
