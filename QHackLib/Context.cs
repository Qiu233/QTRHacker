using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public class Context
	{

		[DllImport("kernel32.dll")]
		private static extern UInt32 OpenProcess(UInt32 dwDesiredAccess, bool bInheritHandle, UInt32 dwProcessId);
		[DllImport("kernel32.dll")]
		private static extern UInt32 CloseHandle(UInt32 dwDesiredAccess);

		private const UInt32 PROCESS_ALL_ACCESS = 0x1F0FFF;

		public string ProcessName
		{
			get;
		}
		public UInt32 ProcessID
		{
			get;
		}
		public UInt32 Handle
		{
			get;
		}
		private Context(string name, UInt32 id, UInt32 handle)
		{
			this.ProcessName = name;
			this.ProcessID = id;
			this.Handle = handle;
		}

		public static Context Create(string processName, UInt32 processID)
		{
			return new Context(processName, processID, OpenProcess(PROCESS_ALL_ACCESS, false, processID));
		}

		public void Close()
		{
			CloseHandle(ProcessID);
		}
	}
}
