using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	public class AssemblySnippet : AssemblyCode
	{
		#region CLRCall
		public static bool IsPTypePassingMustOnStack(string typeName) => typeName switch
		{
			"System.Int64" or "System.UInt64" or "System.Single" or "System.Double" => true,
			_ => false,
		};

		private unsafe static object[] ProcessUserArgs(object[] userArgs)
		{
			List<object> processedUserArgs = new();
			foreach (var arg in userArgs)
			{
				Type type = arg.GetType();
				if (!type.IsValueType)
					throw new ClrArgsPassingException($"Can only pass game object and value. Type: {type.FullName}");
				if (type.IsPrimitive)
				{
					if ((type == typeof(nuint) || type == typeof(nint)) &&
						(sizeof(nuint) == 4))
						processedUserArgs.Add((uint)(nuint)arg);
					else
						processedUserArgs.Add(arg);//normal
				}
				else
				{
					int size = Marshal.SizeOf(type);
					byte[] data = new byte[size];
					IntPtr ptr = Marshal.AllocHGlobal(size);
					Marshal.StructureToPtr(arg, ptr, false);
					Marshal.Copy(ptr, data, 0, size);
					Marshal.FreeHGlobal(ptr);
					processedUserArgs.Add(data);//normal
				}
			}
			return processedUserArgs.ToArray();
		}

		private static string SetReg(string reg, nuint val)
		{
			if (val == 0)
				return $"xor {reg},{reg}";
			return $"mov {reg},{val}";
		}

		private static AssemblyCode GetArugumentsPassing(nuint? thisPtr, nuint? retBuf, object[] userArgs)
		{
			AssemblySnippet snippet = new();
			int index = 0;
			int reg = 0;
			int stack = 0;
			object[] args = ProcessUserArgs(userArgs);
			if (thisPtr != null)
				snippet.Content.Add((Instruction)SetReg((reg++ == 0 ? "ecx" : "edx"), thisPtr.Value));
			if (retBuf != null)
				snippet.Content.Add((Instruction)SetReg((reg++ == 0 ? "ecx" : "edx"), retBuf.Value));
			foreach (var arg in args)
			{
				Type type = arg.GetType();
				if (type == typeof(byte[]))
				{
					byte[] data = arg as byte[];
					int count = (data.Length + 3) / 4;
					byte[] targetData = ArrayPool<byte>.Shared.Rent(count * 4);
					Array.Clear(targetData, 0, targetData.Length);//0
					Array.Copy(data, targetData, data.Length);
					for (int i = 0; i < count; i++)
						snippet.Content.Add((Instruction)$"push {BitConverter.ToUInt32(targetData, (count - i - 1) * 4)}");
					stack += count;
					ArrayPool<byte>.Shared.Return(targetData);
				}
				else if (type.IsPrimitive)
				{
					if (IsPTypePassingMustOnStack(type.Name))
					{
						if (type.Name == "System.Int64" || type.Name == "System.UInt64")
						{
							byte[] data = BitConverter.GetBytes((ulong)arg);
							uint low = BitConverter.ToUInt32(data, 0);
							uint high = BitConverter.ToUInt32(data, 32);
							snippet.Content.Add((Instruction)$"push {high}");
							snippet.Content.Add((Instruction)$"push {low}");
							stack += 2;
						}
						else if (type.Name == "System.Double")
						{
							byte[] data = BitConverter.GetBytes((double)arg);
							uint low = BitConverter.ToUInt32(data, 0);
							uint high = BitConverter.ToUInt32(data, 32);
							snippet.Content.Add((Instruction)$"push {high}");
							snippet.Content.Add((Instruction)$"push {low}");
							stack += 2;
						}
						else//float
						{
							byte[] data = BitConverter.GetBytes((float)arg);
							snippet.Content.Add((Instruction)$"push {BitConverter.ToUInt32(data, 0)}");
							stack++;
						}
					}
					else
					{
						uint value = Convert.ToUInt32(arg);
						if (reg < 2)
						{
							snippet.Content.Add((Instruction)SetReg(reg++ == 0 ? "ecx" : "edx", value));
						}
						else
						{
							snippet.Content.Add((Instruction)$"push {value}");
							stack++;
						}
					}
				}
				else//ref
					throw new ClrArgsPassingException($"Can only pass game value and byte[]. Type: {type.FullName}");
				index++;
			}
			return snippet;
		}

		public static AssemblySnippet FromClrCall(nuint targetAddr, bool regProtection, nuint? thisPtr, nuint? retBuf, nuint? userEaxBuf, params object[] arguments)
		{
			AssemblySnippet s = new();
			if (regProtection)
			{
				s.Content.Add(Instruction.Create("push ecx"));
				s.Content.Add(Instruction.Create("push edx"));
			}
			s.Content.Add(GetArugumentsPassing(thisPtr, retBuf, arguments));
			s.Content.Add(Instruction.Create($"call {targetAddr}"));
			if (userEaxBuf != null)
				s.Content.Add(Instruction.Create($"mov dword ptr [{userEaxBuf}],eax"));
			if (regProtection)
			{
				s.Content.Add(Instruction.Create("pop edx"));
				s.Content.Add(Instruction.Create("pop ecx"));
			}
			return s;
		}
		#endregion

		#region Thread
		public static AssemblySnippet StartManagedThread(QHackContext ctx, nuint lpCodeAddr, nuint lpwStrName_System_Action)
		{
			nuint getTypeMethod = ctx.BCLHelper.GetFunctionAddress("System.Type",
				t => t.Signature == "System.Type.GetType(System.String)");
			nuint getPtrMethod = ctx.BCLHelper.GetFunctionAddress("System.Runtime.InteropServices.Marshal",
				t => t.Signature == "System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(IntPtr, System.Type)");
			nuint taskRunMethod = ctx.BCLHelper.GetFunctionAddress("System.Threading.Tasks.Task",
				t => t.Signature == "System.Threading.Tasks.Task.Run(System.Action)");
			return FromCode(
					new AssemblyCode[] {
						(Instruction)"pushad",
						FromConstructString(ctx,lpwStrName_System_Action),
						(Instruction)"mov ecx,eax",
						(Instruction)$"call {getTypeMethod}",
						(Instruction)$"mov ecx,{lpCodeAddr}",
						(Instruction)"mov edx,eax",
						(Instruction)$"call {getPtrMethod}",
						(Instruction)"mov ecx,eax",
						(Instruction)$"call {taskRunMethod}",
						(Instruction)"popad",
				});
		}
		#endregion

		public List<AssemblyCode> Content
		{
			get;
		}

		private AssemblySnippet() => Content = new List<AssemblyCode>();

		public static AssemblySnippet FromEmpty() => new();

		public static AssemblySnippet FromASMCode(string asm)
		{
			AssemblySnippet s = new();
			s.Content.AddRange(
				asm.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Select(t => Instruction.Create(t)));
			return s;
		}

		public static AssemblySnippet FromCode(IEnumerable<AssemblyCode> code)
		{
			AssemblySnippet s = new();
			s.Content.AddRange(code);
			return s;
		}

		/// <summary>
		/// Constructs a string from unicode wchar_t*.<br/>
		/// Naked call.<br/>
		/// ecx,edx,eax will be changed.<br/>
		/// eax will keep the return value.
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="strMemPtr">char* pointer of the string to be constructed</param>
		/// <param name="retPtr">the pointer to receive the result</param>
		/// <returns></returns>
		public static AssemblySnippet FromConstructString(QHackContext ctx, nuint strMemPtr)
		{
			nuint ctor = ctx.BCLHelper.GetFunctionAddress("System.String", "CtorCharPtr");
			return FromClrCall(ctor, false, thisPtr: 0, retBuf: null, userEaxBuf: null, strMemPtr);
		}

		/// <summary>
		/// Loads an assembly.<br/>
		/// Naked call.<br/>
		/// ecx,eax will be changed.<br/>
		/// eax will keep the return value(pointer to Assembly).
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="assemblyFileNamePtr">string object containing assembly file name</param>
		/// <returns></returns>
		public static AssemblySnippet FromLoadAssembly(QHackContext ctx, nuint assemblyFileNamePtr)
		{
			nuint loadFrom = ctx.BCLHelper.GetFunctionAddress("System.Reflection.Assembly", "LoadFrom");
			return FromClrCall(loadFrom, false, null, null, null, assemblyFileNamePtr);
		}

		private static readonly Random random = new();
		public static AssemblySnippet Loop(AssemblySnippet body, int times, bool regProtection)
		{
			byte[] lA = new byte[16];
			byte[] lB = new byte[16];
			random.NextBytes(lA);
			random.NextBytes(lB);
			AssemblySnippet s = new();
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

		public override string GetCode() => string.Join('\n', Content);
		public override byte[] GetByteCode(nuint IP) => Assembler.Assemble(GetCode(), IP);

		public override AssemblyCode Copy()
		{
			AssemblySnippet ss = new();
			ss.Content.AddRange(Content);
			return ss;
		}

		internal class ClrArgsPassingException : Exception
		{
			public ClrArgsPassingException(string msg) : base(msg) { }
		}
	}
}
