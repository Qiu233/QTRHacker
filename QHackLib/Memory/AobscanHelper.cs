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
		[StructLayout(LayoutKind.Sequential)]
		private struct MEMORY_BASIC_INFORMATION
		{
			public nuint BaseAddress;
			public nuint AllocationBase;
			public uint AllocationProtect;
			public nuint RegionSize;
			public uint State;
			public DataAccess.ProtectionType Protect;
			public uint Type;
		}
		[DllImport("kernel32.dll")]
		private static extern int VirtualQueryEx
		(
			nuint hProcess,
			nuint lpAddress,
			out MEMORY_BASIC_INFORMATION lpBuffer,
			int dwLength
		);

		internal static readonly int SIZE_MBI = sizeof(MEMORY_BASIC_INFORMATION);

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
		public static bool Match(in ReadOnlySpan<byte> src, in ReadOnlySpan<byte> sub)
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
		public static int Search(in ReadOnlySpan<byte> src, in ReadOnlySpan<byte> sub, ref int pos)
		{
			int bLen = sub.Length;
			int len = src.Length - sub.Length;
			for (; pos < len; pos++)
				if (Match(src.Slice(pos, bLen), sub))
					return pos;
			return -1;
		}

		public static IReadOnlyList<nuint> Aobscan(nuint handle, in ReadOnlySpan<byte> aob)
		{
			List<nuint> result = new();
			nuint addr = 0;
			while (true)
			{
				int size = VirtualQueryEx(handle, addr, out MEMORY_BASIC_INFORMATION mbi, SIZE_MBI);
				if (size != SIZE_MBI || mbi.RegionSize <= 0)
					break;
				if (!mbi.Protect.HasFlag(DataAccess.ProtectionType.PAGE_EXECUTE_READWRITE)
					|| !((DataAccess.AllocationType)mbi.State).HasFlag(DataAccess.AllocationType.MEM_COMMIT))
				{
					addr = mbi.BaseAddress + mbi.RegionSize;
					continue;
				}
				byte[] va = ArrayPool<byte>.Shared.Rent((int)mbi.RegionSize);
				DataAccess.ReadProcessMemory(handle, mbi.BaseAddress, va, mbi.RegionSize, 0);
				int pos = 0;
				if (Search(va, aob, ref pos) >= 0)
					result.Add(mbi.BaseAddress + (uint)pos);
				ArrayPool<byte>.Shared.Return(va);
				addr = mbi.BaseAddress + mbi.RegionSize;
			}
			return result;
		}
	}
}
