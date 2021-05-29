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
		public enum ProtectionType : uint
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
		public enum AllocationType : uint
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
		public static extern int VirtualAllocEx(int hProcess, int lpAddress, int dwSize, AllocationType flAllocationType, ProtectionType flProtect);

		[DllImport("kernel32.dll")]
		public static extern int VirtualFreeEx(int hProcess, int lpAddress, int dwSize, AllocationType dwFreeType = AllocationType.MEM_RELEASE);


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
		public static extern int CloseHandle(int dwHandle);
		public const int PROCESS_ALL_ACCESS = 0x1F0FFF;

		[DllImport("kernelbase.dll")]
		public static extern int MapViewOfFile3(
			int FileMappingHandle,
			int ProcessHandle,
			ulong Offset,
			int BaseAddress,
			int ViewSize,
			AllocationType AllocationType,
			ProtectionType PageProtection,
			int ExtendedParameters,
			int ParameterCount);
	}
}
