using QHackLib.Memory;
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
			"Int64" or "UInt64" or "Single" or "Double" => true,
			_ => false,
		};

		private unsafe static object[] ProcessUserArgs(object[] userArgs)
		{
			List<object> processedUserArgs = new();
			foreach (var arg in userArgs)
			{
				if (arg is null)
				{
					processedUserArgs.Add(0);
					continue;
				}
				Type type = arg.GetType();
				if (!type.IsValueType)
					throw new ClrArgsPassingException($"Can only pass game object and value. Type: {type.FullName}");
				if (type.IsPrimitive)
				{
					if (type == typeof(nuint) && sizeof(nuint) == 4)
						processedUserArgs.Add((uint)(nuint)arg);
					else if (type == typeof(nint) && sizeof(nuint) == 4)
						processedUserArgs.Add((uint)(nint)arg);
					else
						processedUserArgs.Add(arg);//normal
				}
				else
				{
					/*int size = Marshal.SizeOf(type);
					byte[] data = new byte[size];
					IntPtr ptr = Marshal.AllocHGlobal(size);
					Marshal.StructureToPtr(arg, ptr, false);
					Marshal.Copy(ptr, data, 0, size);
					Marshal.FreeHGlobal(ptr);*/
					var data = DataHelper.GetBytes((ValueType)arg);
					processedUserArgs.Add(data);//normal
				}
			}
			return processedUserArgs.ToArray();
		}


		private static AssemblyCode GetArugumentsPassing32(nuint? thisPtr, nuint? retBuf, object[] userArgs)
		{
			static string SetReg(string reg, nint val)
			{
				if (val == 0)
					return $"xor {reg},{reg}";
				return $"mov {reg},{val}";
			}
			AssemblySnippet snippet = new();
			int index = 0;
			int reg = 0;
			int stack = 0;
			object[] args = ProcessUserArgs(userArgs);
			if (thisPtr != null)
				snippet.Content.Add((Instruction)SetReg((reg++ == 0 ? "ecx" : "edx"), (nint)thisPtr.Value));
			if (retBuf != null)
				snippet.Content.Add((Instruction)SetReg((reg++ == 0 ? "ecx" : "edx"), (nint)retBuf.Value));
			foreach (var arg in args)
			{
				Type type = arg.GetType();
				if (type == typeof(byte[]))
				{
					byte[] data = arg as byte[];
					int count = (data.Length + 3) / 4;
					byte[] targetData = new byte[count * 4];
					Array.Clear(targetData, 0, targetData.Length);//0
					Array.Copy(data, targetData, data.Length);
					for (int i = 0; i < count; i++)
						snippet.Content.Add((Instruction)$"push {BitConverter.ToUInt32(targetData, (count - i - 1) * 4)}");
					stack += count;
				}
				else if (type.IsPrimitive)
				{
					if (IsPTypePassingMustOnStack(type.Name))
					{
						if (type.Name == "Int64" || type.Name == "UInt64")
						{
							byte[] data = BitConverter.GetBytes((ulong)arg);
							uint low = BitConverter.ToUInt32(data, 0);
							uint high = BitConverter.ToUInt32(data, 32);
							snippet.Content.Add((Instruction)$"push {high}");
							snippet.Content.Add((Instruction)$"push {low}");
							stack += 2;
						}
						else if (type.Name == "Double")
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
						int value = Convert.ToInt32(arg);
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

		private static AssemblySnippet FromCLRCall32(nuint targetAddr, bool regProtection, nuint? thisPtr, nuint? retBuf, nuint? userEaxBuf, object[] arguments)
		{
			AssemblySnippet s = new();
			if (regProtection)
			{
				s.Content.Add(Instruction.Create("push ecx"));
				s.Content.Add(Instruction.Create("push edx"));
			}
			if (arguments is not null)
				s.Content.Add(GetArugumentsPassing32(thisPtr, retBuf, arguments));
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

		public static AssemblySnippet PreserveReg64()
		{
			AssemblySnippet s = new();
			s.Content.Add(Instruction.Create("push rcx"));
			s.Content.Add(Instruction.Create("push rdx"));
			s.Content.Add(Instruction.Create("push rbx"));//for alignment

			s.Content.Add(Instruction.Create("push r8"));
			s.Content.Add(Instruction.Create("push r9"));

			s.Content.Add(Instruction.Create("sub rsp, 0x10"));
			s.Content.Add(Instruction.Create("movdqu [rsp], xmm0"));

			s.Content.Add(Instruction.Create("sub rsp, 0x10"));
			s.Content.Add(Instruction.Create("movdqu [rsp], xmm1"));

			s.Content.Add(Instruction.Create("sub rsp, 0x10"));
			s.Content.Add(Instruction.Create("movdqu [rsp], xmm2"));

			s.Content.Add(Instruction.Create("sub rsp, 0x10"));
			s.Content.Add(Instruction.Create("movdqu [rsp], xmm3"));
			return s;
		}
		public static AssemblySnippet RestoreReg64()
		{
			AssemblySnippet s = new();
			s.Content.Add(Instruction.Create("movdqu xmm3, [rsp]"));
			s.Content.Add(Instruction.Create("add rsp, 0x10"));

			s.Content.Add(Instruction.Create("movdqu xmm2, [rsp]"));
			s.Content.Add(Instruction.Create("add rsp, 0x10"));

			s.Content.Add(Instruction.Create("movdqu xmm1, [rsp]"));
			s.Content.Add(Instruction.Create("add rsp, 0x10"));

			s.Content.Add(Instruction.Create("movdqu xmm0, [rsp]"));
			s.Content.Add(Instruction.Create("add rsp, 0x10"));

			s.Content.Add(Instruction.Create("pop r9"));
			s.Content.Add(Instruction.Create("pop r8"));

			s.Content.Add(Instruction.Create("pop rbx"));
			s.Content.Add(Instruction.Create("pop rdx"));
			s.Content.Add(Instruction.Create("pop rcx"));
			return s;
		}


		private static AssemblyCode GetArugumentsPassing64(nuint? thisPtr, nuint? retBuf, object[] userArgs, ref int stackSize)
		{
			object[] args = ProcessUserArgs(userArgs);
			AssemblySnippet s = new();
			int off = (args.Length + (thisPtr.HasValue ? 1 : 0) + (retBuf.HasValue ? 1 : 0) - 4) * 8;
			if (off < 0)
				off = 0;
			stackSize += off;
			int localOffset = 0x18 + off;
			int regIndex = 0;
			for (int i = 0; i < args.Length; i++)   //preprocess the structs whose size is not bigger than 8 bytes,
			{
				if (args[i] is byte[] ba)           //they could be simply pushed onto stack as qword value
				{
					if (ba.Length <= 8)
					{
						byte[] tmp = new byte[8];
						Array.Clear(tmp, 0, tmp.Length);//0
						Array.Copy(ba, tmp, ba.Length);
						args[i] = BitConverter.ToUInt64(tmp);
					}
					else
						stackSize += (ba.Length + 7) / 8 * 8;
				}
			}
			if (thisPtr.HasValue)
				AddArg(thisPtr.Value);
			if (retBuf.HasValue)
				AddArg(retBuf.Value);
			foreach (var arg in args)
				AddArg(arg);
			string I() //I(i) -> register to store integral arg(i)
			{
				return regIndex switch
				{
					0 => "rcx",
					1 => "rdx",
					2 => "r8",
					3 => "r9",
					_ => throw new ArgumentException("invalid index")
				};
			}
			string F() //F(i) -> register to store float-point arg(i)
			{
				return regIndex switch
				{
					0 => "xmm0",
					1 => "xmm1",
					2 => "xmm2",
					3 => "xmm3",
					_ => throw new ArgumentException("invalid index")
				};
			}
			string Slot() //Slot(i) -> memory to store arg(i)
			{
				return $"[rsp+{8 * regIndex}]";
			}
			void AddArg(object arg)
			{
				if (arg is byte[] ba) // right here, size of ba is always bigger than 8 bytes, thus cannot be passed 'by' value
				{
					int count = (ba.Length + 7) / 8;
					byte[] targetData = new byte[count * 8];
					Array.Clear(targetData, 0, targetData.Length);//0
					Array.Copy(ba, targetData, ba.Length);
					localOffset += targetData.Length;
					for (int i = 0; i < count; i++)
					{
						s.Content.Add((Instruction)$"mov rax, {BitConverter.ToUInt64(targetData, (count - i - 1) * 8)}");
						s.Content.Add((Instruction)$"mov [rsp+{(localOffset - i * 8)}], rax");
					}
					if (regIndex < 4)
					{
						s.Content.Add((Instruction)$"lea rax, [rsp+{localOffset - (count - 1) * 8}]");
						s.Content.Add((Instruction)$"mov {I()}, rax");
					}
					else
					{
						s.Content.Add((Instruction)$"lea rax, [rsp+{localOffset - (count - 1) * 8}]");
						s.Content.Add((Instruction)$"mov {Slot()}, rax");
					}
				}
				else if (arg is float vf)
				{
					uint urep = BitConverter.ToUInt32(BitConverter.GetBytes(vf));
					if (regIndex < 4)
					{
						s.Add((Instruction)$"mov eax, {urep}");
						s.Add((Instruction)$"movd {F()}, eax");
					}
					else
					{
						s.Add((Instruction)$"mov eax, {urep}");
						s.Add((Instruction)$"mov {Slot()}, eax");
					}
				}
				else if (arg is double vd)
				{
					ulong urep = BitConverter.ToUInt64(BitConverter.GetBytes(vd));
					if (regIndex < 4)
					{
						s.Add((Instruction)$"mov rax, {urep}");
						s.Add((Instruction)$"movq {F()}, rax");
					}
					else
					{
						s.Add((Instruction)$"mov rax, {urep}");
						s.Add((Instruction)$"mov {Slot()}, rax");
					}
				}
				else if (arg is nuint || arg is nint)
				{
					if (regIndex < 4)
					{
						s.Add((Instruction)$"mov {I()}, {arg}");
					}
					else
					{
						s.Add((Instruction)$"mov rax, {arg}");
						s.Add((Instruction)$"mov {Slot()}, rax");
					}
				}
				else
				{
					ulong urep = (ulong)Convert.ToInt64(arg);
					if (regIndex < 4)
					{
						s.Add((Instruction)$"mov {I()}, {urep}");
					}
					else
					{
						s.Add((Instruction)$"mov rax, {urep}");
						s.Add((Instruction)$"mov {Slot()}, rax");
					}
				}
				regIndex++;
			}
			return s;
		}

		private static AssemblySnippet FromCLRCall64(nuint targetAddr, bool regProtection, nuint? thisPtr, nuint? retBuf, nuint? userEaxBuf, object[] arguments)
		{
			int stackSize = 0x20;
			AssemblySnippet s = new();
			if (regProtection)
				s.Content.Add(PreserveReg64());
			AssemblyCode argumentPassing = null;
			if (arguments is not null)
				argumentPassing = GetArugumentsPassing64(thisPtr, retBuf, arguments, ref stackSize);
			s.Content.Add(Instruction.Create($"sub rsp, {AlignSize(stackSize)}"));
			if (argumentPassing != null)
				s.Content.Add(argumentPassing);
			s.Content.Add(Instruction.Create($"mov rax, {targetAddr}"));
			s.Content.Add(Instruction.Create($"call rax"));
			if (userEaxBuf != null)
				s.Content.Add(Instruction.Create($"mov qword ptr [{userEaxBuf}], rax"));
			s.Content.Add(Instruction.Create($"add rsp, {AlignSize(stackSize)}"));
			if (regProtection)
				s.Content.Add(RestoreReg64());
			return s;
			static int AlignSize(int size)
			{
				return ((size + 15) / 16 * 16);
			}
		}

		public static AssemblySnippet FromClrCall(nuint targetAddr, bool regProtection, nuint? thisPtr, nuint? retBuf, nuint? userAXBuf, object[] arguments)
		{
			if (Utils.Is32Bit)
				return FromCLRCall32(targetAddr, regProtection, thisPtr, retBuf, userAXBuf, arguments);
			return FromCLRCall64(targetAddr, regProtection, thisPtr, retBuf, userAXBuf, arguments);
		}
		#endregion

		#region Thread

		private static AssemblySnippet StartManagedThread32(QHackContext ctx, nuint lpCodeAddr, nuint lpwStrName_System_Action)
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
		private static AssemblySnippet StartManagedThread64(QHackContext ctx, nuint lpCodeAddr, nuint lpwStrName_System_Action)
		{
			nuint getTypeMethod = ctx.BCLHelper.GetFunctionAddress("System.Type",
				t => t.Signature == "System.Type.GetType(System.String)");
			nuint getPtrMethod = ctx.BCLHelper.GetFunctionAddress("System.Runtime.InteropServices.Marshal",
				t => t.Signature == "System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(IntPtr, System.Type)");
			nuint taskRunMethod = ctx.BCLHelper.GetFunctionAddress("System.Threading.Tasks.Task",
				t => t.Signature == "System.Threading.Tasks.Task.Run(System.Action)");
			using MemoryAllocation tmp = new(ctx);
			return FromCode(
				new AssemblyCode[] {
					PreserveReg64(),
					FromConstructString(ctx,lpwStrName_System_Action, tmp.AllocationBase),
					(Instruction)$"sub rsp, 0x40",
					(Instruction)$"mov rax,{tmp.AllocationBase}",
					(Instruction)$"mov rcx,[rax]",
					(Instruction)$"mov rax,{getTypeMethod}",
					(Instruction)$"call rax",
					(Instruction)$"mov rcx,{lpCodeAddr}",
					(Instruction)$"mov rdx,rax",
					(Instruction)$"mov rax,{getPtrMethod}",
					(Instruction)$"call rax",
					(Instruction)$"mov rcx,rax",
					(Instruction)$"mov rax,{taskRunMethod}",
					(Instruction)$"call rax",
					(Instruction)$"add rsp, 0x40",
					RestoreReg64()
			});
		}

		public static AssemblySnippet StartManagedThread(QHackContext ctx, nuint lpCodeAddr, nuint lpwStrName_System_Action)
		{
			if (Utils.Is32Bit)
				return StartManagedThread32(ctx, lpCodeAddr, lpwStrName_System_Action);
			return StartManagedThread64(ctx, lpCodeAddr, lpwStrName_System_Action);
		}
		#endregion

		private List<AssemblyCode> Content
		{
			get;
		}

		private AssemblySnippet()
		{
			Content = new();
		}

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
		public static AssemblySnippet FromConstructString(QHackContext ctx, nuint strMemPtr, bool regProtection = false)
		{
			nuint ctor = GetCtorCharPtr(ctx);
			return FromClrCall(ctor, regProtection, thisPtr: 0, retBuf: null, null, new object[] { strMemPtr });
		}

		public static AssemblySnippet FromConstructString(QHackContext ctx, nuint strMemPtr, nuint retPtr, bool regProtection = false)
		{
			nuint ctor = GetCtorCharPtr(ctx);
			return FromClrCall(ctor, regProtection, thisPtr: 0, retBuf: null, retPtr, new object[] { strMemPtr });
		}

		private static nuint GetCtorCharPtr(QHackContext ctx)
		{
			return ctx.BCLHelper.GetFunctionAddress("System.String", 
				t => t.Name == "CtorCharPtr" || t.Signature == "System.String.Ctor(Char*)"); //the latter is for .net core
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
			return FromClrCall(loadFrom, false, null, null, null, new object[] { assemblyFileNamePtr });
		}

		private static AssemblySnippet Loop32(AssemblySnippet body, int times, bool regProtection)
		{
			byte[] lA = new Guid().ToByteArray();
			byte[] lB = new Guid().ToByteArray();
			AssemblySnippet s = new();
			string labelA = "lab_" + string.Concat(lA.Select(t => t.ToString("x2")));
			string labelB = "lab_" + string.Concat(lB.Select(t => t.ToString("x2")));
			if (regProtection)
				s.Content.Add(Instruction.Create("push ecx"));
			s.Content.Add(Instruction.Create("mov ecx,0"));
			s.Content.Add(Instruction.Create($"{labelA}:"));
			s.Content.Add(Instruction.Create("cmp ecx," + times + ""));
			s.Content.Add(Instruction.Create($"jge {labelB}"));
			s.Content.Add(Instruction.Create("push ecx"));
			s.Content.Add(body);
			s.Content.Add(Instruction.Create("pop ecx"));
			s.Content.Add(Instruction.Create("inc ecx"));
			s.Content.Add(Instruction.Create($"jmp {labelA}"));
			s.Content.Add(Instruction.Create($"{labelB}:"));
			if (regProtection)
				s.Content.Add(Instruction.Create("pop ecx"));
			return s;
		}

		private static AssemblySnippet Loop64(AssemblySnippet body, int times, bool regProtection)
		{
			byte[] lA = Guid.NewGuid().ToByteArray();
			byte[] lB = Guid.NewGuid().ToByteArray();
			AssemblySnippet s = new();
			string labelA = "lab_" + string.Concat(lA.Select(t => t.ToString("x2")));
			string labelB = "lab_" + string.Concat(lB.Select(t => t.ToString("x2")));
			if (regProtection)
				s.Content.Add(PreserveReg64());
			s.Content.Add(Instruction.Create("mov rcx, 0"));
			s.Content.Add(Instruction.Create($"{labelA}:"));
			s.Content.Add(Instruction.Create($"cmp rcx, {times}"));
			s.Content.Add(Instruction.Create($"jge {labelB}"));
			s.Content.Add(Instruction.Create("push rcx"));//for alignment
			s.Content.Add(Instruction.Create("push rcx"));
			s.Content.Add(body);
			s.Content.Add(Instruction.Create("pop rcx"));
			s.Content.Add(Instruction.Create("pop rcx"));//for alignment
			s.Content.Add(Instruction.Create("inc rcx"));
			s.Content.Add(Instruction.Create($"jmp {labelA}"));
			s.Content.Add(Instruction.Create($"{labelB}:"));
			if (regProtection)
				s.Content.Add(RestoreReg64());
			return s;
		}

		public static AssemblySnippet Loop(AssemblySnippet body, int times, bool regProtection)
		{
			if (Utils.Is32Bit)
				return Loop32(body, times, regProtection);
			return Loop64(body, times, regProtection);
		}

		public override string GetCode()
		{
			return string.Join('\n', Content);
		}

		public override byte[] GetByteCode(nuint IP) => Assembler.Assemble(GetCode(), IP);

		public void Add(AssemblyCode code)
		{
			Content.Add(code);
		}

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
