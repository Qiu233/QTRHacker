using QHackLib.Memory;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace QTRHacker.Functions.Test;

internal static class AobscanChecks
{
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool VirtualProtect(nuint address, nuint size, uint protection, out uint previous);
	[DllImport("kernel32.dll")]
	private static extern bool VirtualFree(nuint address, nuint size, uint freeType);
	[DllImport("kernel32.dll")]
	private static extern nuint GetCurrentProcess();

	public static unsafe void Verify()
	{
		int position = 0;
		Require(AobscanHelper.Search(new byte[] { 1, 2 }, new byte[] { 1, 2 }, ref position) && position == 0, "whole buffer match");
		position = 0;
		Require(AobscanHelper.Search(new byte[] { 0, 1, 2 }, new byte[] { 1, 2 }, ref position) && position == 1, "last possible match");
		Require(AobscanHelper.Match(new byte[] { 0xAB, 0xCD }, "A*\t?D"), "nibble wildcards and whitespace");
		foreach (string invalid in new[] { "", "0", "GG", "01 Z2", "**" })
		{
			bool rejected = false;
			try { AobscanHelper.GetHexCodeFromString(invalid); }
			catch (ArgumentException) { rejected = true; }
			Require(rejected, "invalid replacement: " + invalid);
		}

		const int chunk = 1024 * 1024;
		const int size = 3 * chunk + 4096;
		nuint memory = MemoryAllocation.Alloc(GetCurrentProcess(), size);
		if (memory == 0) throw new System.ComponentModel.Win32Exception();
		try
		{
			byte[] signature = { 0xA3, 0xD7, 0x61, 0xB9, 0x42, 0xF0, 0xE5, 0x8C };
			int[] offsets = { 128, 4092, 4096 + chunk - 4, size - signature.Length };
			foreach (int offset in offsets) signature.CopyTo(new Span<byte>((void*)(memory + (uint)offset), signature.Length));
			// A real unreadable page must not produce matches or bridge carries.
			signature.CopyTo(new Span<byte>((void*)(memory + 2 * chunk), signature.Length));
			Require(VirtualProtect(memory, 4096, 0x20, out _), "RX region");
			Require(VirtualProtect(memory + 2 * chunk, 4096, 0x01, out _), "no-access page");
			var expected = offsets.Select(offset => memory + (uint)offset).ToArray();
			var exact = AobscanHelper.Aobscan(GetCurrentProcess(), signature).Where(a => a >= memory && a < memory + size).ToArray();
			Require(exact.SequenceEqual(expected), "RX/RWX, region/chunk boundaries, inaccessible page, final bytes");
			var masked = AobscanHelper.Aobscan(GetCurrentProcess(), "A* D7 61 B9 42 F0 E5 ?C").Where(a => a >= memory && a < memory + size).ToArray();
			Require(masked.SequenceEqual(expected), "masked scan has the same bounds");
			new byte[] { 0xAB, 0xCA, 0xAB, 0xCA, 0xAB }.CopyTo(new Span<byte>((void*)(memory + 8192), 5));
			var overlaps = AobscanHelper.Aobscan(GetCurrentProcess(), "AB CA AB").Where(a => a >= memory && a < memory + size).ToArray();
			Require(overlaps.SequenceEqual(new[] { memory + 8192, memory + 8194 }), "overlapping matches");
		}
		finally { VirtualFree(memory, 0, 0x8000); }
		Console.WriteLine("PASS: AOB parsing, end positions, RX/RWX, unreadable pages, overlapping and cross-boundary matches.");
	}

	private static void Require(bool condition, string message)
	{
		if (!condition) throw new InvalidOperationException(message);
	}
}
