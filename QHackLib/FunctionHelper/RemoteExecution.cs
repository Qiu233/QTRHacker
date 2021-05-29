using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.FunctionHelper
{
	public class RemoteExecution : IDisposable
	{
		private int FlagAddress;
		public int Address
		{
			get;
		}
		public Context Context
		{
			get;
		}
		public int Thread
		{
			get;
			private set;
		}
		private RemoteExecution(Context c, AssemblySnippet asm)
		{
			Thread = 0;
			Context = c;
			FlagAddress = NativeFunctions.VirtualAllocEx(c.Handle, 0, 4, NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
			int z = 0;
			NativeFunctions.WriteProcessMemory(Context.Handle, FlagAddress, ref z, 4, 0);
			Address = NativeFunctions.VirtualAllocEx(c.Handle, 0, 1024, NativeFunctions.AllocationType.MEM_COMMIT, NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE);
			List<byte> code = new List<byte>();
			byte[] b = asm.GetByteCode(Address);
			code.AddRange(b);
			code.AddRange(Assembler.Assemble("inc [0x" + FlagAddress.ToString("X8") + "]", 0));
			code.AddRange(Assembler.Assemble("ret", 0));

			NativeFunctions.WriteProcessMemory(c.Handle, Address, code.ToArray(), code.Count, 0);
		}
		public static RemoteExecution Create(Context c, AssemblySnippet asm)
		{
			return new RemoteExecution(c, asm);
		}
		public void Execute()
		{
			NativeFunctions.CreateRemoteThread(Context.Handle, 0, 0, Address, 0, 0, out int th);
			Thread = th;
		}

		public void Close()
		{
			int v = 0;
			while (v == 0 && Thread != 0)
				NativeFunctions.ReadProcessMemory(Context.Handle, FlagAddress, ref v, 4, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, Address, 0);
			NativeFunctions.VirtualFreeEx(Context.Handle, FlagAddress, 0);
		}

		public void Dispose()
		{
			Close();
		}
	}
}
