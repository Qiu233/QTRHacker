using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public class NativeFunctions
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct MEMORY_BASIC_INFORMATION
		{
			public int BaseAddress;
			public int AllocationBase;
			public int AllocationProtect;
			public int RegionSize;
			public int State;
			public int Protect;
			public int Type;
		}
		public enum AllocationProtect : uint
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

		public enum AllocationType
		{
			Commit = 0x1000,
			Reserve = 0x2000,
			Decommit = 0x4000,
			Release = 0x8000,
			Reset = 0x80000,
			Physical = 0x400000,
			TopDown = 0x100000,
			WriteWatch = 0x200000,
			LargePages = 0x20000000
		}

		public enum MemoryProtection
		{
			Execute = 0x10,
			ExecuteRead = 0x20,
			ExecuteReadWrite = 0x40,
			ExecuteWriteCopy = 0x80,
			NoAccess = 0x01,
			ReadOnly = 0x02,
			ReadWrite = 0x04,
			WriteCopy = 0x08,
			GuardModifierflag = 0x100,
			NoCacheModifierflag = 0x200,
			WriteCombineModifierflag = 0x400
		}
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			byte[] lpBuffer,
			int nSize,
			int BytesRead
		);
		
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			byte[] lpBuffer,
			int nSize,
			int BytesWrite
		);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref int lpBuffer,
			int nSize,
			int BytesRead
		);
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref int lpBuffer,
			int nSize,
			int BytesWrite
		);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref bool lpBuffer,
			int nSize,
			int BytesRead
		);
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref bool lpBuffer,
			int nSize,
			int BytesWrite
		);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref float lpBuffer,
			int nSize,
			int BytesRead
		);
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref float lpBuffer,
			int nSize,
			int BytesWrite
		);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref byte lpBuffer,
			int nSize,
			int BytesRead
		);
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			int lpProcess,
			int lpBaseAddress,
			ref byte lpBuffer,
			int nSize,
			int BytesWrite
		);

		[DllImport("kernel32.dll")]
		public static extern int VirtualQueryEx
		(
			int hProcess,
			int lpAddress,
			out MEMORY_BASIC_INFORMATION lpBuffer,
			int dwLength
		);

		[DllImport("kernel32.dll")]
		public static extern int VirtualAllocEx(int hProcess, int lpAddress, int dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

		[DllImport("kernel32.dll")]
		public static extern int VirtualFreeEx(int hProcess, int lpAddress, int dwSize, int dwFreeType = 0x8000);


		public static byte[] ReadFromMultiOffsets(int hProcess, int addr, int len, params int[] offsets)
		{
			byte[] result = new byte[len];
			int v = addr;
			for (int i = 0; i < offsets.Length - 1; i++)
			{
				ReadProcessMemory(hProcess, v + offsets[i], ref v, 4, 0);
			}
			ReadProcessMemory(hProcess, v + offsets[offsets.Length - 1], result, len, 0);
			return result;
		}

		public static void WriteFromMultiOffsets(int hProcess, int addr, byte[] value, params int[] offsets)
		{
			int v = addr;
			for (int i = 0; i < offsets.Length - 1; i++)
			{
				ReadProcessMemory(hProcess, v + offsets[i], ref v, 4, 0);
			}
			WriteProcessMemory(hProcess, v + offsets[offsets.Length - 1], value, value.Length, 0);
		}

		[DllImport("kernel32")]
		public static extern IntPtr CreateRemoteThread(
		  int hProcess,
		  int lpThreadAttributes,
		  int dwStackSize,
		  int lpStartAddress, // raw Pointer into remote process
		  int lpParameter,
		  int dwCreationFlags,
		  out int lpThreadId
		);



		[DllImport("kernel32.dll")]
		public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
		[DllImport("kernel32.dll")]
		public static extern int CloseHandle(int dwDesiredAccess);
		public const int PROCESS_ALL_ACCESS = 0x1F0FFF;
	}
}
