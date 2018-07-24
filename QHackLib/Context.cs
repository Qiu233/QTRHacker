using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public class Context
	{

		[DllImport("kernel32.dll")]
		private static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
		[DllImport("kernel32.dll")]
		private static extern int CloseHandle(int dwDesiredAccess);

		private const int PROCESS_ALL_ACCESS = 0x1F0FFF;

		public string ProcessName
		{
			get;
		}
		public int ProcessID
		{
			get;
		}
		public int Handle
		{
			get;
		}
		public FunctionAddressHelper FunctionAddressHelper
		{
			get;
		}
		private Context(string name, int id, int handle, string moduleName)
		{
			this.ProcessName = name;
			this.ProcessID = id;
			this.Handle = handle;
			FunctionAddressHelper = FunctionAddressHelper.Initialize(id, moduleName);
		}

		public static Context Create(int processID)
		{
			string name = Process.GetProcessById((int)processID).ProcessName;
			return new Context(name, processID, OpenProcess(PROCESS_ALL_ACCESS, false, processID), name + ".exe");
		}

		public void Close()
		{
			CloseHandle(ProcessID);
		}
	}
}
