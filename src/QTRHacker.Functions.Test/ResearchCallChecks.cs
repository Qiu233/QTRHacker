using QHackLib.Assemble;
using QTRHacker.Core;
using QTRHacker.Scripts.Functions;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test;

internal static class ResearchCallChecks
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void RunCode();

	public static void Verify()
	{
		if (IntPtr.Size != 4) throw new PlatformNotSupportedException("Run with the x86 test runner.");
		nint memory = VirtualAlloc(0, 4096, 0x3000, 0x40);
		if (memory == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
		try
		{
			nuint start = (nuint)memory, callee = start + 2048, count = start + 3072;
			nuint errors = count + 4, stack = count + 8;
			// Independent callee implementing the CLR x86 ABI: ECX=this, EDX=id,
			// [ESP+4]=teammateName, [ESP+8]=amount; callee pops eight argument bytes.
			byte[] receiver = Assembler.Assemble($@"
cmp ecx, 0x12345678
jne bad_arguments
cmp edx, dword ptr [{count}]
jne bad_arguments
cmp dword ptr [esp+4], 0
jne bad_arguments
cmp dword ptr [esp+8], 9999
je arguments_checked
bad_arguments:
inc dword ptr [{errors}]
arguments_checked:
inc dword ptr [{count}]
xor ecx, ecx
xor edx, edx
ret 8", callee);
			Marshal.Copy(receiver, 0, (nint)callee, receiver.Length);
			var build = typeof(UnlockAllDuplications).GetMethod("BuildUnlockCode", BindingFlags.Static | BindingFlags.NonPublic);
			var loop = (AssemblyCode)build.Invoke(null, new object[] { (nuint)0x12345678, callee });
			byte[] caller = Assembler.Assemble($@"
pushad
mov dword ptr [{stack}], esp
{loop.GetCode()}
cmp esp, dword ptr [{stack}]
je stack_ok
inc dword ptr [{errors}]
stack_ok:
mov esp, dword ptr [{stack}]
popad
ret", start);
			if (caller.Length >= 2048 || receiver.Length >= 1024)
				throw new InvalidOperationException("Research test code exceeds its buffer.");
			Marshal.Copy(caller, 0, memory, caller.Length);
			if (!FlushInstructionCache(-1, memory, 4096)) throw new Win32Exception(Marshal.GetLastWin32Error());
			Marshal.GetDelegateForFunctionPointer<RunCode>(memory)();
			int calls = Marshal.ReadInt32((nint)count), failures = Marshal.ReadInt32((nint)errors);
			if (calls != GameConstants.MaxItemTypes || failures != 0)
				throw new InvalidOperationException($"Research ABI regression failed: calls={calls}, invalid arguments/stack={failures}.");
			Console.WriteLine($"PASS research loop: {calls} calls, ids 0..{calls - 1}, amount=9999, teammateName=null, stack balanced.");
		}
		finally { VirtualFree(memory, 0, 0x8000); }
	}

	[DllImport("kernel32.dll", SetLastError = true)] private static extern nint VirtualAlloc(nint address, nuint size, uint type, uint protect);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool VirtualFree(nint address, nuint size, uint type);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool FlushInstructionCache(nint process, nint address, nuint size);
}
