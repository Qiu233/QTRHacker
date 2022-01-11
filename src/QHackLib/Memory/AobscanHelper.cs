using QHackCLR.DataTargets;
using QHackLib.Assemble;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Memory
{
	public unsafe static class AobscanHelper
	{
		internal static readonly int SIZE_MBI = sizeof(NativeFunctions.MEMORY_BASIC_INFORMATION);

		public static string GetMByteCode(int i) => $"{i & 0xFF:X2}{(i >> 8) & 0xFF:X2}{(i >> 16) & 0xFF:X2}{(i >> 24) & 0xFF:X2}";

		private static byte Ctoh(char hex) => hex switch
		{
			>= '0' and <= '9' => (byte)(hex - '0'),
			>= 'A' and <= 'F' => (byte)(hex - 'A' + 10),
			>= 'a' and <= 'f' => (byte)(hex - 'a' + 10),
			_ => 0
		};

		public static byte[] GetHexCodeFromString(string str)
		{
			var src = str.Where(c => !char.IsWhiteSpace(c)).Select(c => Ctoh(c));
			return (src.Count() % 2) == 0
				? src.Where((c, i) => i % 2 == 0).Zip(src.Where((c, i) => i % 2 == 1), (i, j) => (byte)((i * 0x10) + j)).ToArray()
				: throw new ArgumentException("Not a valid hex string. A hex string should have a even length.", nameof(str));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Match(ReadOnlySpan<byte> src, ReadOnlySpan<byte> sub)
		{
			for (int i = 0; i < src.Length; i++)
				if (src[i] != sub[i])
					return false;
			return true;
		}

		public static bool Match(ReadOnlySpan<byte> src, string sub)
		{
			for (int i = 0; i < src.Length; i++)
				if (src[i] != sub[i])
					return false;
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="sub"></param>
		/// <param name="pos"></param>
		/// <returns>-1 if nothing was found</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Search(ReadOnlySpan<byte> src, ReadOnlySpan<byte> sub, ref int pos)
		{
			int bLen = sub.Length;
			int len = src.Length - sub.Length;
			for (; pos < len; pos++)
				if (Match(src.Slice(pos, bLen), sub))
					return true;
			return false;
		}

		public static IEnumerable<nuint> Aobscan(nuint handle, string hexStr) => Aobscan(handle, GetHexCodeFromString(hexStr));

		public static IEnumerable<nuint> AobscanASM(nuint handle, string asm) => Aobscan(handle, Assembler.Assemble(asm, 0));

		public static IEnumerable<nuint> Aobscan(nuint handle, byte[] aob)
		{
			List<nuint> result = new();
			Traverse(handle, mbi =>
			{
				if (!mbi.Protect.HasFlag(NativeFunctions.ProtectionType.PAGE_EXECUTE_READWRITE)
				|| !mbi.State.HasFlag(NativeFunctions.AllocationType.MEM_COMMIT))
				{
					return;
				}
				byte[] va = ArrayPool<byte>.Shared.Rent((int)mbi.RegionSize);
				NativeFunctions.ReadProcessMemory(handle, mbi.BaseAddress, va, mbi.RegionSize, 0);
				int pos = 0;
				while (Search(va, aob, ref pos))
					result.Add(mbi.BaseAddress + (uint)(pos++));
				ArrayPool<byte>.Shared.Return(va);
			});
			return result;
		}

		private static void Traverse(nuint handle, MemoryTraverse traverse)
		{
			nuint addr = 0;
			while (true)
			{
				if (NativeFunctions.VirtualQueryEx(handle, addr, out NativeFunctions.MEMORY_BASIC_INFORMATION mbi, SIZE_MBI) != SIZE_MBI
					|| mbi.RegionSize == 0)
					break;
				traverse(mbi);
				addr = mbi.BaseAddress + mbi.RegionSize;
			}
		}

		private delegate void MemoryTraverse(NativeFunctions.MEMORY_BASIC_INFORMATION mbi);
	}
}
