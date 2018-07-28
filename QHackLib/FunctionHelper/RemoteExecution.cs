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
			Context = c;
			Address = NativeFunctions.VirtualAllocEx(c.Handle, 0, 1024, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			byte[] code = asm.GetByteCode(Address, true);
			NativeFunctions.WriteProcessMemory(c.Handle, Address, code, code.Length, 0);
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
			NativeFunctions.VirtualFreeEx(Context.Handle, Address, 0);
		}

		public void Dispose()
		{
			Close();
		}
	}
}
