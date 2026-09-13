using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Terraria;

namespace QTRHacker.Patches.Test;

internal sealed class HighAddressChecks : IDisposable
{
	[StructLayout(LayoutKind.Sequential)]
	private struct MemoryInfo
	{
		public uint BaseAddress, AllocationBase, AllocationProtect, RegionSize, State, Protect, Type;
	}
	[DllImport("kernel32.dll")]
	private static extern uint VirtualQuery(uint address, out MemoryInfo info, uint length);
	[DllImport("kernel32.dll")]
	private static extern IntPtr VirtualAlloc(IntPtr address, uint size, uint type, uint protect);
	[DllImport("kernel32.dll")]
	private static extern bool VirtualFree(IntPtr address, uint size, uint type);
	private readonly List<IntPtr> reservations = new List<IntPtr>();

	public HighAddressChecks()
	{
		// Keep the original AI below 2 GB, then make replacement JIT code cross
		// the signed-pointer boundary. Old MonoMod emitted x64 FF25 jumps here.
		RuntimeHelpers.PrepareMethod(typeof(Projectile).GetMethod("AI").MethodHandle);
		for (uint address = 0x10000; address < 0x80000000;)
		{
			if (VirtualQuery(address, out var info, (uint)Marshal.SizeOf(typeof(MemoryInfo))) == 0) break;
			ulong end = (ulong)info.BaseAddress + info.RegionSize;
			if (info.State == 0x10000 && end <= 0x80000000)
			{
				uint start = (info.BaseAddress + 0xFFFF) & 0xFFFF0000;
				if (start < end)
				{
					var allocation = VirtualAlloc(new IntPtr(unchecked((int)start)), (uint)end - start, 0x2000, 1);
					if (allocation != IntPtr.Zero) reservations.Add(allocation);
				}
			}
			if (end <= address) break;
			address = (uint)end;
		}
		var high = VirtualAlloc(IntPtr.Zero, 4096, 0x3000, 0x04);
		if (high == IntPtr.Zero || high.ToInt32() >= 0)
		{
			if (high != IntPtr.Zero) VirtualFree(high, 0, 0x8000);
			Dispose();
			throw new InvalidOperationException("High-address fixture needs a large-address-aware x86 executable.");
		}
		VirtualFree(high, 0, 0x8000);
	}

	public void Dispose()
	{
		foreach (var address in reservations) VirtualFree(address, 0, 0x8000);
		reservations.Clear();
	}

	public static void VerifyJump()
	{
		var entry = typeof(Projectile).GetMethod("AI").MethodHandle.GetFunctionPointer();
		int destination = unchecked(entry.ToInt32() + 5 + Marshal.ReadInt32(entry, 1));
		if (entry.ToInt32() < 0 || Marshal.ReadByte(entry) != 0xE9 || destination >= 0)
			throw new InvalidOperationException("Expected an x86 relative jump from below 2 GB to replacement code above 2 GB.");
		Console.WriteLine("PASS: x86 detour crosses 2 GB without emitting an x64 jump");
	}
}
