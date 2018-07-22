using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class AssemblySnippet
	{
		private byte[] Code;

		private AssemblySnippet() { }

		public static AssemblySnippet FromASMCode(string asm)
		{
			AssemblySnippet s = new AssemblySnippet
			{
				Code = Assembler.Assemble(asm)
			};
			return s;
		}
		public static AssemblySnippet FromJmpInstruction(UInt32 jmpAddr, UInt32 targetAddr)
		{
			AssemblySnippet s = new AssemblySnippet
			{
				Code = new byte[5]
			};
			UInt32 off = targetAddr - (jmpAddr + 5);
			s.Code[0] = 0xE9;
			s.Code[1] = (byte)(off & 0x000000FF);
			s.Code[2] = (byte)((off & 0x0000FF00) >> 8);
			s.Code[3] = (byte)((off & 0x00FF0000) >> 16);
			s.Code[4] = (byte)((off & 0xFF000000) >> 24);
			return s;
		}
		public static AssemblySnippet FromCallInstruction(UInt32 callAddr, UInt32 targetAddr)
		{
			AssemblySnippet s = new AssemblySnippet
			{
				Code = new byte[5]
			};
			UInt32 off = targetAddr - (callAddr + 5);
			s.Code[0] = 0xE8;
			s.Code[1] = (byte)(off & 0x000000FF);
			s.Code[2] = (byte)((off & 0x0000FF00) >> 8);
			s.Code[3] = (byte)((off & 0x00FF0000) >> 16);
			s.Code[4] = (byte)((off & 0xFF000000) >> 24);
			return s;
		}
		private static string GetArgumentPassing(int index, ValueType v)
		{
			if (index > 1)
			{
				return "push " + ((int)v).ToString("X8") + "H";
			}
			else if (index == 0)
			{
				return "mov ecx," + ((int)v).ToString("X8") + "H";
			}
			else// if (index == 1)
			{
				return "mov edx," + ((int)v).ToString("X8") + "H";
			}
		}
		public static AssemblySnippet FromDotNetCall(UInt32 addr, UInt32 targetAddr, UInt32 retAddr, params ValueType[] arguments)
		{
			AssemblySnippet s = new AssemblySnippet();
			List<byte> bytes = new List<byte>();
			string asmHead = "push ecx\n" + "push edx\n";
			int i = 0;
			foreach (var v in arguments)
			{
				if (v.GetType() == typeof(byte) || v.GetType() == typeof(sbyte) || v.GetType() == typeof(int) || v.GetType() == typeof(uint) || v.GetType() == typeof(char) || v.GetType() == typeof(short) || v.GetType() == typeof(ushort) || v.GetType() == typeof(long) || v.GetType() == typeof(ulong))
					asmHead += GetArgumentPassing(i, v) + "\n";
				i++;
			}
			bytes.AddRange(Assembler.Assemble(asmHead));
			addr += (UInt32)bytes.Count;
			bytes.AddRange(FromCallInstruction(addr, targetAddr).GetByteCode());
			if (retAddr != 0)
			{
				bytes.AddRange(Assembler.Assemble("mov [" + retAddr + "],eax"));
			}
			if (i > 2)
			{
				byte[] popBytes = Assembler.Assemble("pop");
				for (; i > 2; i--)
					bytes.AddRange(popBytes);
			}
			bytes.AddRange(Assembler.Assemble("pop edx\n" + "pop ecx"));
			s.Code = bytes.ToArray();
			return s;
		}
		public byte[] GetByteCode()
		{
			return Code;
		}
	}
}
