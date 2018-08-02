using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class AssemblySnippet:AssemblyCode
	{
		public List<AssemblyCode> Content
		{
			get;
		}

		private AssemblySnippet()
		{
			Content = new List<AssemblyCode>();
		}

		public static AssemblySnippet FromEmpty()
		{
			return new AssemblySnippet();
		}

		public static AssemblySnippet FromASMCode(string asm)
		{
			AssemblySnippet s = new AssemblySnippet();

			Instruction[] ss = asm.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(t=>Instruction.Create(t)).ToArray();
			s.Content.AddRange(ss);
			return s;
		}
		private static string GetArgumentPassing(int index, ValueType v)
		{
			if (index > 1)
			{
				return "push 0x" + ((int)v).ToString("X8");
			}
			else if (index == 0)
			{
				return "mov ecx,0x" + ((int)v).ToString("X8");
			}
			else// if (index == 1)
			{
				return "mov edx,0x" + ((int)v).ToString("X8");
			}
		}
		public static AssemblySnippet FromDotNetCall(int targetAddr, int? retAddr, bool regProtection, params ValueType[] arguments)
		{
			AssemblySnippet s = new AssemblySnippet();
			if (regProtection)
			{
				s.Content.Add(Instruction.Create("push ecx"));
				s.Content.Add(Instruction.Create("push edx"));
			}
			int i = 0;
			foreach (var v in arguments)
			{
				if (v.GetType() == typeof(byte) || v.GetType() == typeof(sbyte) || v.GetType() == typeof(int) || v.GetType() == typeof(uint) || v.GetType() == typeof(char) || v.GetType() == typeof(short) || v.GetType() == typeof(ushort))
				{
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, v)));
					i++;
				}
				else if (v.GetType() == typeof(long) || v.GetType() == typeof(ulong))
				{
					ulong vv = (ulong)v;
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)(vv & 0xFFFFFFFFUL))));
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)((vv & 0xFFFFFFFF00000000UL) >> 32))));
					i += 2;
				}
				else if (v.GetType() == typeof(bool))
				{
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (bool)v ? 1 : 0)));
					i++;
				}
			}
			s.Content.Add(Instruction.Create("call 0x" + ((int)targetAddr).ToString("X8")));
			if (retAddr != null)
				s.Content.Add(Instruction.Create("mov [0x" + ((int)retAddr).ToString("X8") + "],eax"));
			if (regProtection)
			{
				s.Content.Add(Instruction.Create("pop edx"));
				s.Content.Add(Instruction.Create("pop ecx"));
			}
			return s;
		}
		public override string GetCode()
		{
			StringBuilder sb = new StringBuilder("");
			Content.ForEach(s => sb.Append(s.GetCode() + "\n"));
			return sb.ToString();
		}
		public override byte[] GetByteCode(int IP)
		{
			List<byte> bs = new List<byte>();
			bs.AddRange(Assembler.Assemble(GetCode(), IP));
			return bs.ToArray();
		}
		public AssemblySnippet Copy()
		{
			AssemblySnippet ss = new AssemblySnippet();
			ss.Content.AddRange(Content);
			return ss;
		}
	}
}
