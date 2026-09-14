using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QHackCLR.TestTarget;

public class HighAddressProbe
{
	public static int Value = 42;
	public static object Root = new HighAddressProbe();
	public int Number = 73;
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int ReadValue() => Value;
}

internal static class HighAddressFixture
{
	public static int[] HighArray;
	[StructLayout(LayoutKind.Sequential)]
	private struct MemoryInformation
	{
		public UIntPtr BaseAddress, AllocationBase;
		public uint AllocationProtect;
		public UIntPtr RegionSize;
		public uint State, Protect, Type;
	}
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern UIntPtr VirtualQuery(UIntPtr address, out MemoryInformation info, UIntPtr size);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr VirtualAlloc(UIntPtr address, UIntPtr size, uint allocationType, uint protect);
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool VirtualFree(IntPtr address, UIntPtr size, uint freeType);

	public static void Run()
	{
		if (IntPtr.Size != 4) throw new InvalidOperationException("This fixture requires x86.");
		var reservations = new List<IntPtr>();
		byte[] assemblyBytes = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
		try
		{
			// Only reserve unused virtual addresses in this disposable fixture, without
			// committing RAM. Subsequent CLR loader heaps must grow above 2 GiB.
			ulong cursor = 0x10000;
			while (cursor < 0x80000000)
			{
				if (VirtualQuery((UIntPtr)cursor, out var info, (UIntPtr)Marshal.SizeOf<MemoryInformation>()) == UIntPtr.Zero)
					throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
				ulong end = Math.Min(info.BaseAddress.ToUInt64() + info.RegionSize.ToUInt64(), 0x80000000);
				if (end <= cursor) throw new InvalidOperationException("Memory query did not advance.");
				ulong start = (cursor + 0xFFFF) & ~0xFFFFUL;
				if (info.State == 0x10000 && start < end)
				{
					var block = VirtualAlloc((UIntPtr)start, (UIntPtr)(end - start), 0x2000, 1);
					if (block != IntPtr.Zero) reservations.Add(block);
				}
				cursor = end;
			}
			for (int i = 0; i < 512; i++)
			{
				// Loading from bytes creates a separate loader context each time.
				// Use PE-backed metadata so DAC can also inspect field/method names.
				var assembly = Assembly.Load(assemblyBytes);
				var type = assembly.GetType("QHackCLR.TestTarget.HighAddressProbe", true);
				if (((Func<int>)Delegate.CreateDelegate(typeof(Func<int>), type.GetMethod("ReadValue")))() != 42)
					throw new InvalidOperationException("Loaded probe did not initialize.");
			}
			// Force a new LOH segment, beyond any low-address segments reserved at
			// CLR startup. Keep this real managed object stable for DAC queries.
			HighArray = new int[8 * 1024 * 1024];
			HighArray[0] = 55;
			HighArray[HighArray.Length - 1] = 77;
			var pin = GCHandle.Alloc(HighArray, GCHandleType.Pinned);
			try
			{
				if (unchecked((uint)pin.AddrOfPinnedObject().ToInt32()) < 0x80000000u)
					throw new InvalidOperationException("Fixture did not produce a high-address array.");
				Console.WriteLine(Process.GetCurrentProcess().Id);
				Console.ReadLine();
			}
			finally { pin.Free(); }
		}
		finally
		{
			foreach (var reservation in reservations) VirtualFree(reservation, UIntPtr.Zero, 0x8000);
		}
	}
}
