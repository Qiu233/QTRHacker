using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	internal unsafe static class NativeFunctions
	{
		[Flags]
		internal enum ProtectionType : uint
		{
			PAGE_EXECUTE = 0x00000010,
			PAGE_EXECUTE_READ = 0x00000020,
			PAGE_EXECUTE_READWRITE = 0x00000040,
			PAGE_EXECUTE_WRITECOPY = 0x00000080,
			PAGE_NOACCESS = 0x00000001,
			PAGE_READONLY = 0x00000002,
			PAGE_READWRITE = 0x00000004,
			PAGE_WRITECOPY = 0x00000008,
			PAGE_GUARD = 0x00000100,
			PAGE_NOCACHE = 0x00000200,
			PAGE_WRITECOMBINE = 0x00000400
		}
		[Flags]
		internal enum AllocationType : uint
		{
			MEM_COMMIT = 0x00001000,
			MEM_RESERVE = 0x00002000,
			MEM_DECOMMIT = 0x00004000,
			MEM_RELEASE = 0x00008000,
			MEM_RESET = 0x00080000,
			MEM_PHYSICAL = 0x00400000,
			MEM_TOPDOWN = 0x00100000,
			MEM_WRITEWATCH = 0x00200000,
			MEM_LARGEPAGES = 0x20000000,
		}
		#region natives
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID
		{
			public int LowPart;
			public uint HighPart;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct LUID_AND_ATTRIBUTES
		{
			public LUID Luid;
			public uint Attributes;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct TOKEN_PRIVILEGES
		{
			public int PrivilegeCount;
			public LUID_AND_ATTRIBUTES Privilege;
		}
		internal enum ThreadAccess
		{
			SUSPEND_RESUME = 0x0002,
			THREAD_ALL_ACCESS = 0x1F03FF
		}

		internal const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
		internal const uint TOKEN_QUERY = 0x0008;
		internal const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
		internal const uint SE_PRIVILEGE_ENABLED = 0x00000002;

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern nuint GetCurrentProcess();

		[DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool OpenProcessToken(nuint ProcessHandle, uint DesiredAccesss, out nuint TokenHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID lpLuid);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool AdjustTokenPrivileges(nuint TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, uint ReturnLength);

		[DllImport("kernel32.dll")]
		internal static extern nuint OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool CloseHandle(nuint hObject);

		[DllImport("psapi.dll", CharSet = CharSet.Unicode)]
		internal static extern bool EnumProcessModules(nuint hProcess, nuint* lphModule, uint cb, out uint lpcbNeeded);

		[DllImport("psapi.dll", CharSet = CharSet.Unicode)]
		internal static extern uint GetModuleFileNameEx(nuint hProcess, nuint hModule, [Out] StringBuilder lpBaseName, uint nSize);

#pragma warning disable CA2101
		[DllImport("kernel32", CharSet = CharSet.Ansi)]
		internal static extern nuint LoadLibrary(string lpFileName);

		[DllImport("kernel32", CharSet = CharSet.Ansi)]
		internal static extern nuint GetProcAddress(nuint hModule, string procName);

		[DllImport("kernel32.dll")]
		internal static extern bool FreeLibrary(nuint hModule);

		[DllImport("kernel32.dll")]
		internal static extern bool GetThreadContext(nuint hThread, nuint lpContext);

		[DllImport("kernel32.dll")]
		internal static extern nuint OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		[DllImport("kernel32.dll")]
		internal static extern nuint VirtualAllocEx(
			nuint hProcess,
			nuint lpAddress,
			nuint dwSize,
			AllocationType flAllocationType,
			ProtectionType flProtect);

		[DllImport("kernel32.dll")]
		internal static extern nuint VirtualFreeEx(
			nuint hProcess,
			nuint lpAddress,
			nuint dwSize,
			AllocationType dwFreeType = AllocationType.MEM_RELEASE);

		[DllImport("kernel32.dll")]
		internal unsafe static extern bool ReadProcessMemory
		(
			nuint lpProcess,
			nuint lpBaseAddress,
			void* lpBuffer,
			nuint nSize,
			nuint BytesRead
		);
		[DllImport("kernel32.dll")]
		internal unsafe static extern bool ReadProcessMemory
		(
			nuint lpProcess,
			nuint lpBaseAddress,
			byte[] lpBuffer,
			nuint nSize,
			nuint BytesRead
		);
		[DllImport("kernel32.dll")]
		internal unsafe static extern bool WriteProcessMemory
		(
			nuint lpProcess,
			nuint lpBaseAddress,
			void* lpBuffer,
			nuint nSize,
			nuint BytesWrite
		);
		[DllImport("kernel32.dll")]
		internal unsafe static extern bool WriteProcessMemory
		(
			nuint lpProcess,
			nuint lpBaseAddress,
			byte[] lpBuffer,
			nuint nSize,
			nuint BytesWrite
		);

		#endregion
	}
}
