using QHackLib.Assemble;
using QTRHacker.Scripts.Functions;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test;

internal static class MapCallChecks
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)] private delegate void RunCode();

	public static void Verify()
	{
		if (IntPtr.Size != 4) throw new PlatformNotSupportedException("Run with the x86 test runner.");
		foreach (var size in new[] { (81, 81), (83, 85), (4200, 1200), (6400, 1800), (8400, 2400) })
			Verify(size.Item1, size.Item2);
	}

	private static void Verify(int width, int height)
	{
		nint memory = VirtualAlloc(0, 4096, 0x3000, 0x40);
		if (memory == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
		try
		{
			nuint start = (nuint)memory, callee = start + 2048;
			nuint calls = start + 3072, errors = calls + 4, expectedX = calls + 8, expectedY = calls + 12, stack = calls + 16;
			Marshal.WriteInt32((nint)expectedX, 40);
			Marshal.WriteInt32((nint)expectedY, 40);
			// Independent receiver checks every coordinate in vanilla's visible
			// rectangle and implements UpdateLighting's managed x86 calling convention.
			byte[] receiver = Assembler.Assemble($@"
cmp ecx, 0x12345678
jne bad
cmp edx, dword ptr [{expectedX}]
jne bad
mov eax, dword ptr [esp+8]
cmp eax, dword ptr [{expectedY}]
jne bad
cmp dword ptr [esp+4], 255
je checked
bad:
inc dword ptr [{errors}]
checked:
inc dword ptr [{calls}]
inc dword ptr [{expectedY}]
cmp dword ptr [{expectedY}], {height - 40}
jne next
mov dword ptr [{expectedY}], 40
inc dword ptr [{expectedX}]
next:
xor ecx, ecx
xor edx, edx
mov eax, 1
ret 8", callee);
			Marshal.Copy(receiver, 0, (nint)callee, receiver.Length);
			var build = typeof(RevealTheWholeMap).GetMethod("BuildRevealCode", BindingFlags.Static | BindingFlags.NonPublic);
			var loop = (AssemblySnippet)build.Invoke(null, new object[] { (nuint)0x12345678, callee, width, height });
			byte[] caller = Assembler.Assemble($@"
pushad
mov dword ptr [{stack}], esp
mov ecx, 0x13579BDF
mov edx, 0x02468ACE
{loop.GetCode()}
cmp esp, dword ptr [{stack}]
jne bad_return
cmp ecx, 0x13579BDF
jne bad_return
cmp edx, 0x02468ACE
je returned
bad_return:
inc dword ptr [{errors}]
returned:
mov esp, dword ptr [{stack}]
popad
ret", start);
			if (caller.Length >= 2048 || receiver.Length >= 1024) throw new InvalidOperationException("Map test code exceeds its buffer.");
			Marshal.Copy(caller, 0, memory, caller.Length);
			if (!FlushInstructionCache(-1, memory, 4096)) throw new Win32Exception(Marshal.GetLastWin32Error());
			Marshal.GetDelegateForFunctionPointer<RunCode>(memory)();
			int count = Marshal.ReadInt32((nint)calls), bad = Marshal.ReadInt32((nint)errors);
			int finalX = Marshal.ReadInt32((nint)expectedX), finalY = Marshal.ReadInt32((nint)expectedY);
			if (count != (width - 80) * (height - 80) || bad != 0 || finalX != width - 40 || finalY != 40)
				throw new InvalidOperationException($"Map loop failed: calls={count}, errors={bad}, final=({finalX},{finalY}).");
			Console.WriteLine($"PASS map {width}x{height}: {count} calls, x=40..{width - 41}, y=40..{height - 41}, this/x/y/light correct, stack and saved registers balanced.");
		}
		finally { VirtualFree(memory, 0, 0x8000); }
	}

	[DllImport("kernel32.dll", SetLastError = true)] private static extern nint VirtualAlloc(nint address, nuint size, uint type, uint protect);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool VirtualFree(nint address, nuint size, uint type);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool FlushInstructionCache(nint process, nint address, nuint size);
}
