using System.Runtime.InteropServices;

namespace QHackCLR;

public unsafe static partial class NativeMethods
{
	[LibraryImport("kernel32.dll", SetLastError = true)]
	public static partial nuint OpenProcess(ProcessAccessFlags processAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int processId);
	[Flags]
	public enum ProcessAccessFlags : uint
	{
		All = 0x001F0FFF,
		Terminate = 0x00000001,
		CreateThread = 0x00000002,
		VirtualMemoryOperation = 0x00000008,
		VirtualMemoryRead = 0x00000010,
		VirtualMemoryWrite = 0x00000020,
		DuplicateHandle = 0x00000040,
		CreateProcess = 0x000000080,
		SetQuota = 0x00000100,
		SetInformation = 0x00000200,
		QueryInformation = 0x00000400,
		QueryLimitedInformation = 0x00001000,
		Synchronize = 0x00100000
	}

	[LibraryImport("kernel32.dll")]
	public static partial uint GetLastError();

	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool IsWow64Process(
		nuint hProcess,
		[MarshalAs(UnmanagedType.Bool)] out bool wow64Process);
	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool ReadProcessMemory(
		nuint hProcess,
		nuint lpBaseAddress,
		byte* lpBuffer,
		nuint nSize,
		out nuint lpNumberOfBytesRead);
	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool WriteProcessMemory(
		nuint hProcess,
		nuint lpBaseAddress,
		byte* lpBuffer,
		nuint nSize,
		out nuint lpNumberOfBytesWritten);


	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool CloseHandle(nuint Handle);


	[LibraryImport("psapi.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool EnumProcessModules(nuint hProcess, nuint* lphModule, uint cb, out uint lpcbNeeded);

	[LibraryImport("psapi.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool GetModuleFileNameExW(nuint hProcess, nuint hModule, char* lpFileName, uint nSize);

	[LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	public static partial nuint LoadLibraryW(string lpLibFileName);

	[LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf8)]
	public static partial nuint GetProcAddress(nuint hModule, string lpProcName);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	public static partial nuint OpenThread(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwThreadId);

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct FLOATING_SAVE_AREA
	{
		public uint ControlWord;
		public uint StatusWord;
		public uint TagWord;
		public uint ErrorOffset;
		public uint ErrorSelector;
		public uint DataOffset;
		public uint DataSelector;
		public fixed byte RegisterArea[80];
		public uint Spare0;
	}
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct CONTEXT
	{
		public uint ContextFlags;

		public uint Dr0;
		public uint Dr1;
		public uint Dr2;
		public uint Dr3;
		public uint Dr6;
		public uint Dr7;

		public FLOATING_SAVE_AREA FloatSave;

		public uint SegGs;
		public uint SegFs;
		public uint SegEs;
		public uint SegDs;

		public uint Edi;
		public uint Esi;
		public uint Ebx;
		public uint Edx;
		public uint Ecx;
		public uint Eax;

		public uint Ebp;
		public uint Eip;
		public uint SegCs;
		public uint EFlags;
		public uint Esp;
		public uint SegSs;

		public fixed byte ExtendedRegisters[512];
	}
	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool GetThreadContext(
		nuint hThread,
		CONTEXT* lpContext
	);


	public struct CONTEXT_AMD64
	{
		public ulong P1Home;
		public ulong P2Home;
		public ulong P3Home;
		public ulong P4Home;
		public ulong P5Home;
		public ulong P6Home;
		public uint ContextFlags;
		public uint MxCsr;
		public ushort SegCs;
		public ushort SegDs;
		public ushort SegEs;
		public ushort SegFs;
		public ushort SegGs;
		public ushort SegSs;
		public uint EFlags;
		public ulong Dr0;
		public ulong Dr1;
		public ulong Dr2;
		public ulong Dr3;
		public ulong Dr6;
		public ulong Dr7;
		public ulong Rax;
		public ulong Rcx;
		public ulong Rdx;
		public ulong Rbx;
		public ulong Rsp;
		public ulong Rbp;
		public ulong Rsi;
		public ulong Rdi;
		public ulong R8;
		public ulong R9;
		public ulong R10;
		public ulong R11;
		public ulong R12;
		public ulong R13;
		public ulong R14;
		public ulong R15;
		public ulong Rip;
		public fixed ulong DUMMYUNIONNAME[52];
		public fixed ulong VectorRegister[52];
		public ulong VectorControl;
		public ulong DebugControl;
		public ulong LastBranchToRip;
		public ulong LastBranchFromRip;
		public ulong LastExceptionToRip;
		public ulong LastExceptionFromRip;
	}
}
