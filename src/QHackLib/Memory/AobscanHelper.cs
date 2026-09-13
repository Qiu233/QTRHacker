using QHackLib.Assemble;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace QHackLib.Memory;

public unsafe static class AobscanHelper
{
	internal static readonly int SIZE_MBI = sizeof(NativeFunctions.MEMORY_BASIC_INFORMATION);
	private const int ChunkSize = 1024 * 1024;

	public static string GetMByteCode(int i) => $"{i & 0xFF:X2}{(i >> 8) & 0xFF:X2}{(i >> 16) & 0xFF:X2}{(i >> 24) & 0xFF:X2}";

	public static byte[] GetHexCodeFromString(string str)
	{
		var (bytes, masks) = Parse(str);
		if (masks.Any(mask => mask != 0xFF))
			throw new ArgumentException("Replacement bytes cannot contain wildcards.", nameof(str));
		return bytes;
	}

	private static (byte[] Bytes, byte[] Masks) Parse(string pattern)
	{
		ArgumentNullException.ThrowIfNull(pattern);
		string hex = string.Concat(pattern.Where(c => !char.IsWhiteSpace(c)));
		if (hex.Length == 0 || hex.Length % 2 != 0)
			throw new ArgumentException("A hex pattern must contain a nonzero, even number of nibbles.", nameof(pattern));
		byte[] bytes = new byte[hex.Length / 2];
		byte[] masks = new byte[bytes.Length];
		for (int i = 0; i < hex.Length; i++)
		{
			char c = hex[i];
			if (c is '*' or '?') continue;
			int value = c switch
			{
				>= '0' and <= '9' => c - '0',
				>= 'A' and <= 'F' => c - 'A' + 10,
				>= 'a' and <= 'f' => c - 'a' + 10,
				_ => throw new ArgumentException($"Invalid hex digit '{c}'.", nameof(pattern))
			};
			int shift = i % 2 == 0 ? 4 : 0;
			bytes[i / 2] |= (byte)(value << shift);
			masks[i / 2] |= (byte)(0xF << shift);
		}
		return (bytes, masks);
	}

	public static bool Match(ReadOnlySpan<byte> src, ReadOnlySpan<byte> sub) => src.SequenceEqual(sub);
	public static bool Match(ReadOnlySpan<byte> src, string sub)
	{
		var (bytes, masks) = Parse(sub);
		return src.Length == bytes.Length && Matches(src, bytes, masks);
	}

	public static bool Search(ReadOnlySpan<byte> src, ReadOnlySpan<byte> sub, ref int pos)
	{
		if (sub.IsEmpty) throw new ArgumentException("An empty pattern cannot be scanned.", nameof(sub));
		if (pos < 0) throw new ArgumentOutOfRangeException(nameof(pos));
		for (; pos <= src.Length - sub.Length; pos++)
			if (src.Slice(pos, sub.Length).SequenceEqual(sub)) return true;
		return false;
	}

	private static bool Matches(ReadOnlySpan<byte> src, byte[] bytes, byte[] masks)
	{
		for (int i = 0; i < bytes.Length; i++)
			if ((src[i] & masks[i]) != bytes[i]) return false;
		return true;
	}

	public static IEnumerable<nuint> AobscanASM(nuint handle, string asm) => Aobscan(handle, Assembler.Assemble(asm, 0));
	public static IEnumerable<nuint> AobscanMatch(nuint handle, string hexCode) => Aobscan(handle, hexCode);
	public static IEnumerable<nuint> Aobscan(nuint handle, string src)
	{
		var (bytes, masks) = Parse(src);
		return Scan(handle, bytes, masks);
	}

	public static IEnumerable<nuint> Aobscan(nuint handle, byte[] aob)
	{
		ArgumentNullException.ThrowIfNull(aob);
		if (aob.Length == 0) throw new ArgumentException("An empty pattern cannot be scanned.", nameof(aob));
		return Scan(handle, aob, Enumerable.Repeat((byte)0xFF, aob.Length).ToArray());
	}

	private static List<nuint> Scan(nuint handle, byte[] pattern, byte[] masks)
	{
		List<nuint> result = new();
		byte[] buffer = ArrayPool<byte>.Shared.Rent(checked(ChunkSize + pattern.Length - 1));
		int carry = 0;
		nuint nextRead = 0;
		try
		{
			nuint address = 0;
			while (NativeFunctions.VirtualQueryEx(handle, address, out var mbi, SIZE_MBI) == SIZE_MBI && mbi.RegionSize != 0)
			{
				uint protection = (uint)mbi.Protect;
				uint access = protection & 0xFF;
				// JIT code may be RX, RWX or executable copy-on-write. Never read
				// guard/no-access pages or inspect bytes outside the actual read.
				bool readableCode = access is 0x20 or 0x40 or 0x80;
				if (mbi.State == NativeFunctions.AllocationType.MEM_COMMIT && readableCode && (protection & 0x100) == 0)
				{
					for (nuint offset = 0; offset < mbi.RegionSize;)
					{
						nuint current = mbi.BaseAddress + offset;
						if (current != nextRead) carry = 0;
						int requested = (int)Math.Min((ulong)(mbi.RegionSize - offset), (ulong)ChunkSize);
						nuint bytesRead = 0;
						fixed (byte* data = buffer)
							NativeFunctions.ReadProcessMemory(handle, current, data + carry, (uint)requested, (nuint)(&bytesRead));
						int validLength = carry + (int)bytesRead;
						for (int pos = 0; pos <= validLength - pattern.Length; pos++)
							if (Matches(buffer.AsSpan(pos, pattern.Length), pattern, masks))
								result.Add(current - (uint)carry + (uint)pos);
						// Keep enough bytes for matches crossing chunks or adjacent
						// readable regions, including overlapping matches.
						carry = Math.Min(pattern.Length - 1, validLength);
						buffer.AsSpan(validLength - carry, carry).CopyTo(buffer);
						nextRead = current + bytesRead;
						if (bytesRead != (nuint)requested) carry = 0;
						offset += (uint)requested;
					}
				}
				else carry = 0;
				nuint next = mbi.BaseAddress + mbi.RegionSize;
				if (next <= address) break;
				address = next;
			}
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(buffer);
		}
		return result;
	}
}
