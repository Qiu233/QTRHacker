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

		public static AssemblySnippet FromCode(IEnumerable<AssemblyCode> code)
		{
			AssemblySnippet s = new AssemblySnippet();
			s.Content.AddRange(code);
			return s;
		}
		/// <summary>
		/// inside loop,[esp] is the iterator
		/// ecx will be changed
		/// </summary>
		/// <param name="body"></param>
		/// <param name="times"></param>
		/// <param name="regProtection"></param>
		/// <returns></returns>
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

		/// <summary>
		/// ecx,edx,eax will be changed
		/// eax will keep the return value
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="strMemPtr">char* pointer of the string to be constructed</param>
		/// <param name="retPtr">the pointer to receive the result</param>
		/// <returns></returns>
		public static AssemblySnippet ConstructString(Context ctx, int strMemPtr, int? retPtr)
		{
			var mscorlib_AddrHelper = ctx.GetAddressHelper("mscorlib.dll");
			int ctor = mscorlib_AddrHelper.GetFunctionAddress("System.String", "CtorCharPtr");
			if (retPtr == null)
				return FromClrCall(ctor, null, false, 0, strMemPtr);
			return FromClrCall(ctor, retPtr, false, 0, strMemPtr);
		}

		/// <summary>
		/// load an assembly
		/// ecx,eax will be changed
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="assemblyFileNamePtr"></param>
		/// <returns></returns>
		public static AssemblySnippet LoadAssembly(Context ctx, int assemblyFileNamePtr)
		{
			var mscorlib_AddrHelper = ctx.GetAddressHelper("mscorlib.dll");
			int loadFrom = mscorlib_AddrHelper.GetFunctionAddress("System.Reflection.Assembly", "LoadFrom");
			return FromClrCall(loadFrom, null, false, assemblyFileNamePtr);
		}

		private static string GetArgumentPassing(int index, object v)
		{
			if (v is string s)
			{
				if (index > 1)
				{
					return "push " + s;
				}
				else if (index == 0)
				{
					return "mov ecx," + s;
				}
				else// if (index == 1)
				{
					return "mov edx," + s;
				}
			}
			else
			{
				if (index > 1)
				{
					return "push " + (Convert.ToInt32(v)).ToString();
				}
				else if (index == 0)
				{
					return "mov ecx," + (Convert.ToInt32(v)).ToString();
				}
				else// if (index == 1)
				{
					return "mov edx," + (Convert.ToInt32(v)).ToString();
				}
			}
		}

		/// <summary>
		/// only for the functions with parameters of ValueType
		/// </summary>
		/// <param name="targetAddr"></param>
		/// <param name="retAddr"></param>
		/// <param name="regProtection"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public static AssemblySnippet FromClrCall(int targetAddr, int? retAddr, bool regProtection, params object[] arguments)
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
				if (v is byte || v is sbyte || v is int|| v is uint || v is char || v is short|| v is ushort|| v is string)
				{
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, v)));
					i++;
				}
				else if (v is float)
				{
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, BitConverter.ToInt32(BitConverter.GetBytes((float)v), 0))));
					i++;
				}
				else if (v is double)
				{
					ulong vv = BitConverter.ToUInt64(BitConverter.GetBytes((double)v), 0);
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)(vv & 0xFFFFFFFFUL))));
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)((vv & 0xFFFFFFFF00000000UL) >> 32))));
					i += 2;
				}
				else if (v is long || v is ulong)
				{
					ulong vv = (ulong)v;
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)(vv & 0xFFFFFFFFUL))));
					s.Content.Add(Instruction.Create(GetArgumentPassing(i, (UInt32)((vv & 0xFFFFFFFF00000000UL) >> 32))));
					i += 2;
				}
				else if (v is bool)
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
