using QHackLib.FunctionHelper;
using QHackCLR.Clr;
using QHackCLR.DataTargets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static QHackLib.NativeFunctions;

namespace QHackLib
{
	public sealed class QHackContext : IDisposable
	{
		public int ProcessID { get; }

		public DataTarget DataTarget { get; }
		public ClrRuntime Runtime { get; }

		public Dictionary<ClrModule, CLRHelper> CLRHelpers { get; }

		public CLRHelper BCLHelper => CLRHelpers[Runtime.BaseClassLibrary];

		public DataAccess DataAccess => DataTarget.DataAccess;
		public nuint Handle => DataAccess.ProcessHandle;


		private void InitHelpers()
		{
			CLRHelpers.Clear();
			foreach (var module in Runtime.AppDomain.Modules)
				CLRHelpers[module] = new CLRHelper(this, module);
		}

		private QHackContext(int id)
		{
			GrantPrivilege();
			ProcessID = id;
			DataTarget = DataTarget.AttachToProcess(id);
			Runtime = DataTarget.ClrVersions[0].CreateRuntime();

			CLRHelpers = new Dictionary<ClrModule, CLRHelper>();
			InitHelpers();
		}

		/// <summary>
		/// Ignoring case.
		/// </summary>
		/// <param name="moduleName"></param>
		/// <returns></returns>
		public CLRHelper GetCLRHelper(string moduleName) => CLRHelpers.FirstOrDefault(
			t => t.Key.Name.Equals(moduleName, StringComparison.OrdinalIgnoreCase)
			).Value;

		public static QHackContext Create(int pid) => new(pid);


		private static void GrantPrivilege()
		{
			LUID locallyUniqueIdentifier = new();
			LookupPrivilegeValue(null, "SeDebugPrivilege", ref locallyUniqueIdentifier);
			TOKEN_PRIVILEGES tokenPrivileges = new()
			{
				PrivilegeCount = 1
			};

			LUID_AND_ATTRIBUTES luidAndAtt = new()
			{
				Attributes = SE_PRIVILEGE_ENABLED,
				Luid = locallyUniqueIdentifier
			};
			tokenPrivileges.Privilege = luidAndAtt;

			OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out nuint tokenHandle);
			AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivileges, 1024, IntPtr.Zero, 0);
			CloseHandle(tokenHandle);
		}

		public void Flush()
		{
			Runtime.Flush();
			InitHelpers();
		}

		public void Dispose() => DataTarget.Dispose();
	}
}
