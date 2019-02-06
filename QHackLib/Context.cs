using Microsoft.Diagnostics.Runtime;
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
	public class Context : IDisposable
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct LUID
		{
			public int LowPart;
			public uint HighPart;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct LUID_AND_ATTRIBUTES
		{
			public LUID Luid;
			public uint Attributes;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct TOKEN_PRIVILEGES
		{
			public int PrivilegeCount;
			public LUID_AND_ATTRIBUTES Privilege;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetCurrentProcess();

		[DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccesss, out IntPtr TokenHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean CloseHandle(IntPtr hObject);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, [MarshalAs(UnmanagedType.Struct)] ref LUID lpLuid);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges, [MarshalAs(UnmanagedType.Struct)]ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, uint ReturnLength);


		private const uint TOKEN_QUERY = 0x0008;
		private const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
		private const uint SE_PRIVILEGE_ENABLED = 0x00000002;

		private static void GrantPrivilege()
		{
			bool flag;
			LUID locallyUniqueIdentifier = new LUID();
			flag = LookupPrivilegeValue(null, "SeDebugPrivilege", ref locallyUniqueIdentifier);
			TOKEN_PRIVILEGES tokenPrivileges = new TOKEN_PRIVILEGES();
			tokenPrivileges.PrivilegeCount = 1;

			LUID_AND_ATTRIBUTES luidAndAtt = new LUID_AND_ATTRIBUTES();
			// luidAndAtt.Attributes should be SE_PRIVILEGE_ENABLED to enable privilege
			luidAndAtt.Attributes = SE_PRIVILEGE_ENABLED;
			luidAndAtt.Luid = locallyUniqueIdentifier;
			tokenPrivileges.Privilege = luidAndAtt;

			IntPtr tokenHandle = IntPtr.Zero;
			flag = OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out tokenHandle);
			flag = AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 1024, IntPtr.Zero, 0);
			flag = CloseHandle(tokenHandle);
		}

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

		public DataTarget DataTarget
		{
			get;
		}
		public ClrRuntime Runtime
		{
			get;
		}
		public AddressHelper AddressHelper
		{
			get;
		}

		private Context(string name, int id, int handle, string moduleName)
		{
			GrantPrivilege();
			ProcessName = name;
			ProcessID = id;
			Handle = handle;
			DataTarget = DataTarget.AttachToProcess(id, 2000, AttachFlag.Passive);
			Runtime = DataTarget.ClrVersions[0].CreateRuntime();
			AddressHelper = new AddressHelper(this, moduleName);//这句必须最后执行，因为需要用到Context里面的一点信息
		}

		public AddressHelper CreateFunctionAddressHelper(string subModuleName)
		{
			return new AddressHelper(this, subModuleName);
		}

		public static Context Create(int processID)
		{
			string name = Process.GetProcessById(processID).ProcessName;
			return new Context(name, processID, OpenProcess(PROCESS_ALL_ACCESS, false, processID), name + ".exe");
		}

		public void Close()
		{
			DataTarget.Dispose();
			CloseHandle(ProcessID);
		}

		public void Dispose()
		{
			Close();
		}
	}
}
