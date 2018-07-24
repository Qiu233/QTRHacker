using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class AssemblySnippet
	{
		public List<string> Instructions
		{
			get;
		}

		private AssemblySnippet()
		{
			Instructions = new List<string>();
		}

		public static AssemblySnippet FromASMCode(string asm)
		{
			AssemblySnippet s = new AssemblySnippet();

			string[] ss = asm.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			s.Instructions.AddRange(ss);
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
		public static AssemblySnippet FromDotNetCall(UInt32 targetAddr, UInt32? retAddr, params ValueType[] arguments)
		{
			AssemblySnippet s = new AssemblySnippet();
			s.Instructions.Add("push ecx");
			s.Instructions.Add("push edx");
			int i = 0;
			foreach (var v in arguments)
			{
				if (v.GetType() == typeof(byte) || v.GetType() == typeof(sbyte) || v.GetType() == typeof(int) || v.GetType() == typeof(uint) || v.GetType() == typeof(char) || v.GetType() == typeof(short) || v.GetType() == typeof(ushort))
				{
					s.Instructions.Add(GetArgumentPassing(i, v));
					i++;
				}
				else if (v.GetType() == typeof(long) || v.GetType() == typeof(ulong))
				{
					ulong vv = (ulong)v;
					s.Instructions.Add(GetArgumentPassing(i, (UInt32)(vv & 0xFFFFFFFFUL)));
					s.Instructions.Add(GetArgumentPassing(i, (UInt32)((vv & 0xFFFFFFFF00000000UL) >> 32)));
					i += 2;
				}
			}
			s.Instructions.Add("call 0x" + ((int)targetAddr).ToString("X8"));
			if (retAddr != null)
				s.Instructions.Add("mov [0x" + ((int)retAddr).ToString("X8") + "],eax");
			s.Instructions.Add("pop edx");
			s.Instructions.Add("pop ecx");
			return s;
		}
		public string GetSnippet()
		{
			StringBuilder sb = new StringBuilder("");
			Instructions.ForEach(s => sb.Append(s + "\n"));
			return sb.ToString();
		}
		public byte[] GetByteCode(int IP)
		{
			byte[] bs = Assembler.Assemble(GetSnippet(), IP);
			return bs.ToArray();
		}
		public AssemblySnippet Copy()
		{
			AssemblySnippet ss = new AssemblySnippet();
			ss.Instructions.AddRange(Instructions);
			return ss;
		}
	}
}
