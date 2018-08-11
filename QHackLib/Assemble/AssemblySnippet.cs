using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class AssemblySnippet : AssemblyCode
	{
		private static Random random = new Random();
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

			Instruction[] ss = asm.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).Select(t => Instruction.Create(t)).ToArray();
			s.Content.AddRange(ss);
			return s;
		}

		public static AssemblySnippet Loop(AssemblySnippet body, int times, bool regProtection)
		{

			byte[] lA = new byte[16];
			byte[] lB = new byte[16];
			random.NextBytes(lA);
			random.NextBytes(lB);
			AssemblySnippet s = new AssemblySnippet();
			string labelA = "lab_" + string.Concat(lA.Select(t => t.ToString("x2")));
			string labelB = "lab_" + string.Concat(lB.Select(t => t.ToString("x2")));
			if (regProtection)
				s.Content.Add(Instruction.Create("push ecx"));
			s.Content.Add(Instruction.Create("mov ecx,0"));
			s.Content.Add(Instruction.Create("" + labelA + ":"));
			s.Content.Add(Instruction.Create("cmp ecx," + times + ""));
			s.Content.Add(Instruction.Create("jge " + labelB + ""));
			s.Content.Add(Instruction.Create("push ecx"));
			s.Content.Add(body);
			s.Content.Add(Instruction.Create("pop ecx"));
			s.Content.Add(Instruction.Create("inc ecx"));
			s.Content.Add(Instruction.Create("jmp " + labelA + ""));
			s.Content.Add(Instruction.Create("" + labelB + ":"));
			if (regProtection)
				s.Content.Add(Instruction.Create("pop ecx"));
			return s;
		}

		private static string GetArgumentPassing(int index, object v)
		{
			if (v.GetType() == typeof(string))
			{
				if (index > 1)
				{
					return "push " + (v as string);
				}
				else if (index == 0)
				{
					return "mov ecx," + (v as string);
				}
				else// if (index == 1)
				{
					return "mov edx," + (v as string);
				}
			}
			else
			{
				if (index > 1)
				{
					return "push " + ((int)v).ToString();
				}
				else if (index == 0)
				{
					return "mov ecx," + ((int)v).ToString();
				}
				else// if (index == 1)
				{
					return "mov edx," + ((int)v).ToString();
				}
			}
		}
		public static AssemblySnippet FromDotNetCall(int targetAddr, int? retAddr, bool regProtection, params object[] arguments)
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
				if (v.GetType() == typeof(byte) || v.GetType() == typeof(sbyte) || v.GetType() == typeof(int) || v.GetType() == typeof(uint) || v.GetType() == typeof(char) || v.GetType() == typeof(short) || v.GetType() == typeof(ushort) || v.GetType() == typeof(string))
				{
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, v)));
					i++;
				}
				else if (v.GetType() == typeof(float))
				{
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, BitConverter.ToInt32(BitConverter.GetBytes((float)v), 0))));
					i++;
				}
				else if (v.GetType() == typeof(double))
				{
					ulong vv = BitConverter.ToUInt64(BitConverter.GetBytes((double)v), 0);
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)(vv & 0xFFFFFFFFUL))));
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)((vv & 0xFFFFFFFF00000000UL) >> 32))));
					i += 2;
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
			s.Content.Add(Instruction.Create("call " + ((int)targetAddr).ToString()));
			if (retAddr != null)
				s.Content.Add(Instruction.Create("mov [" + ((int)retAddr).ToString() + "],eax"));
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
