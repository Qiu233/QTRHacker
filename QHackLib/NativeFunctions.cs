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
			public uint BaseAddress;
			public uint AllocationBase;
			public uint AllocationProtect;
			public uint RegionSize;
			public uint State;
			public uint Protect;
			public uint Type;
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
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			byte[] lpBuffer,
			UInt32 nSize,
			UInt32 BytesRead
		);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			ref int lpBuffer,
			UInt32 nSize,
			UInt32 BytesRead
		);
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			byte[] lpBuffer,
			UInt32 nSize,
			UInt32 BytesWrite
		);

		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory
		(
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			ref int lpBuffer,
			UInt32 nSize,
			UInt32 BytesWrite
		);

		[DllImport("kernel32.dll")]
		public static extern UInt32 VirtualQueryEx
		(
			UInt32 hProcess,
			UInt32 lpAddress,
			out MEMORY_BASIC_INFORMATION lpBuffer,
			UInt32 dwLength
		);

		[DllImport("kernel32.dll")]
		public static extern UInt32 VirtualAllocEx(UInt32 hProcess, UInt32 lpAddress, UInt32 dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

		[DllImport("kernel32.dll")]
		public static extern UInt32 VirtualFreeEx(UInt32 hProcess, UInt32 lpAddress, UInt32 dwSize, UInt32 dwFreeType = 0x8000);

	}
}
