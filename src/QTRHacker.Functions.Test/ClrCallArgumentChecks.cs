using QHackLib.Assemble;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace QTRHacker.Functions.Test;

internal static class ClrCallArgumentChecks
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)] private delegate void RunCode();

	public static void Verify()
	{
		if (IntPtr.Size != 4) throw new PlatformNotSupportedException("Use the x86 test apphost.");
		int cases = 0;
		foreach (uint bits in new[] { 0x80000000u, 0u, 0x7FFFFFFFu, 0xF1234567u, uint.MaxValue })
		{
			// Same argument shape as FromConstructString: null this, char* in EDX.
			Check(new object[] { (nuint)bits }, 0, bits, Array.Empty<uint>(), thisPtr: 0);
			Check(new object[] { bits }, bits, null, Array.Empty<uint>());
			Check(new object[] { unchecked((nint)(int)bits) }, bits, null, Array.Empty<uint>());
			Check(new object[] { unchecked((int)bits) }, bits, null, Array.Empty<uint>());
			// CLR x86 pushes stack arguments left to right: last argument is nearest RET.
			Check(new object[] { 0x11223344u, 0x55667788u, (nuint)bits, bits },
				0x11223344u, 0x55667788u, new[] { bits, bits });
			Check(new object[] { unchecked((nint)(int)bits) }, bits, bits, new[] { bits }, thisPtr: bits, retBuf: bits);
			cases += 6;
		}
		Check(new object[] { (sbyte)-1, ushort.MaxValue, '\u8000', true }, uint.MaxValue, 65535, new[] { 1u, 0x8000u });
		Check(new object[] { null, false, (short)-32768, (byte)255 }, 0, 0, new[] { 255u, 0xFFFF8000u });
		cases += 2;

		// Use a real allocation above 2 GiB so a pointer truncation/bit change
		// cannot pass merely because the generated assembly looks plausible.
		nint high = 0;
		foreach (uint address in new[] { 0x90000000u, 0xA0000000u, 0xB0000000u, 0xC0000000u, 0xD0000000u })
		{
			high = VirtualAlloc(unchecked((nint)(int)address), 4096, 0x3000, 0x04);
			if (high != 0) break;
		}
		if (high == 0) throw new Win32Exception(Marshal.GetLastWin32Error(), "Could not allocate above 2 GiB. Run the LAA-enabled test EXE.");
		try
		{
			byte[] text = Encoding.Unicode.GetBytes("高地址\0");
			Marshal.Copy(text, 0, high, text.Length);
			Check(new object[] { (nuint)high }, 0, (uint)(nuint)high, Array.Empty<uint>(), thisPtr: 0,
				readString: true, result: BitConverter.ToUInt32(text, 0));
			Console.WriteLine($"PASS CLR x86 arguments: {cases} register/stack cases; UInt32/UIntPtr/IntPtr boundary bits preserved; string buffer read at 0x{(nuint)high:X8}; stack and saved registers balanced.");
		}
		finally { VirtualFree(high, 0, 0x8000); }
	}

	private static void Check(object[] arguments, uint? ecx, uint? edx, uint[] stackArguments,
		nuint? thisPtr = null, nuint? retBuf = null, bool readString = false, uint result = 0xCAFEBABE)
	{
		nint memory = VirtualAlloc(0, 4096, 0x3000, 0x40);
		if (memory == 0) throw new Win32Exception(Marshal.GetLastWin32Error());
		try
		{
			nuint start = (nuint)memory, callee = start + 2048, snapshot = start + 3072;
			nuint savedStack = snapshot + 128, error = savedStack + 4, returned = savedStack + 8;
			var receiver = new StringBuilder($"mov dword ptr [{snapshot}], ecx\nmov dword ptr [{snapshot + 4}], edx\n");
			for (int i = 0; i < stackArguments.Length; i++)
				receiver.AppendLine($"mov eax, dword ptr [esp+{4 + 4 * i}]\nmov dword ptr [{snapshot + 8 + (uint)i * 4}], eax");
			receiver.AppendLine(readString ? "mov eax, dword ptr [edx]" : $"mov eax, {result}");
			receiver.AppendLine($"xor ecx, ecx\nxor edx, edx\nret {4 * stackArguments.Length}");
			byte[] receiverBytes = Assembler.Assemble(receiver.ToString(), callee);
			Marshal.Copy(receiverBytes, 0, (nint)callee, receiverBytes.Length);
			var call = AssemblySnippet.FromClrCall(callee, true, thisPtr, retBuf, returned, arguments);
			byte[] caller = Assembler.Assemble($@"
pushad
mov dword ptr [{savedStack}], esp
mov ecx, 0x13579BDF
mov edx, 0x02468ACE
{call.GetCode()}
cmp esp, dword ptr [{savedStack}]
jne bad
cmp ecx, 0x13579BDF
jne bad
cmp edx, 0x02468ACE
je done
bad:
inc dword ptr [{error}]
done:
mov esp, dword ptr [{savedStack}]
popad
ret", start);
			if (caller.Length >= 2048 || receiverBytes.Length >= 1024) throw new InvalidOperationException("Argument test code exceeds buffer.");
			Marshal.Copy(caller, 0, memory, caller.Length);
			if (!FlushInstructionCache(-1, memory, 4096)) throw new Win32Exception(Marshal.GetLastWin32Error());
			Marshal.GetDelegateForFunctionPointer<RunCode>(memory)();
			if (ecx.HasValue) Equal(ecx.Value, snapshot, "ECX");
			if (edx.HasValue) Equal(edx.Value, snapshot + 4, "EDX");
			for (int i = 0; i < stackArguments.Length; i++) Equal(stackArguments[i], snapshot + 8 + (uint)i * 4, "stack argument " + i);
			Equal(0, error, "stack/register restoration");
			Equal(result, returned, "EAX result");
		}
		finally { VirtualFree(memory, 0, 0x8000); }
	}

	private static void Equal(uint expected, nuint address, string label)
	{
		uint actual = unchecked((uint)Marshal.ReadInt32((nint)address));
		if (actual != expected) throw new InvalidOperationException($"{label}: expected 0x{expected:X8}, got 0x{actual:X8}.");
	}
	[DllImport("kernel32.dll", SetLastError = true)] private static extern nint VirtualAlloc(nint address, nuint size, uint type, uint protect);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool VirtualFree(nint address, nuint size, uint type);
	[DllImport("kernel32.dll", SetLastError = true)] private static extern bool FlushInstructionCache(nint process, nint address, nuint size);
}
