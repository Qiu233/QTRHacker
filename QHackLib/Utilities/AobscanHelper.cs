using QHackLib.Assemble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Utilities
{
	public class AobscanHelper
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct MEMORY_BASIC_INFORMATION
		{
			public uint BaseAddress;
			public uint AllocationBase;
			public uint AllocationProtect;
			public uint RegionSize;
			public uint State;
			public uint Protect;
			public uint Type;
		}
		private enum AllocationProtect : uint
		{
			PAGE_EXECUTE = 0x00000010,
			PAGE_EXECUTE_READ = 0x00000020,
			PAGE_EXECUTE_READWRITE = 0x00000040,
			PAGE_EXECUTE_WRITECOPY = 0x00000080,
			PAGE_NOACCESS = 0x00000001,
			PAGE_READONLY = 0x00000002,
			PAGE_READWRITE = 0x00000004,
			PAGE_WRITECOPY = 0x00000008,
			PAGE_GUARD = 0x00000100,
			PAGE_NOCACHE = 0x00000200,
			PAGE_WRITECOMBINE = 0x00000400
		}
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory
		(
			UInt32 lpProcess,
			UInt32 lpBaseAddress,
			byte[] lpBuffer,
			UInt32 nSize,
			UInt32 BytesRead
		);

		[DllImport("kernel32.dll")]
		static extern UInt32 VirtualQueryEx
		(
			UInt32 hProcess,
			UInt32 lpAddress,
			out MEMORY_BASIC_INFORMATION lpBuffer,
			UInt32 dwLength
		);

		private static byte Ctoh(char hex)
		{
			if (hex >= '0' && hex <= '9')
				return (byte)(hex - '0');
			if (hex >= 'A' && hex <= 'F')
				return (byte)(hex - 'A' + 10);
			if (hex >= 'a' && hex <= 'f')
				return (byte)(hex - 'a' + 10);
			return 0;
		}
		private static byte[] GetHexCodeFromString(string str)
		{
			List<byte> bs = new List<byte>();

			char[] a = str.ToCharArray();
			byte t = 0;
			bool flag = false;
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != ' ')
				{
					if (flag)
					{
						bs.Add((byte)(t * 0x10 + Ctoh(a[i])));
					}
					t = Ctoh(a[i]);
					flag = !flag;
				}
			}
			return bs.ToArray();
		}
		private static int Memmem(byte[] a, UInt32 alen, byte[] b, UInt32 blen)
		{
			int i, j;
			for (i = 0; i < alen - blen; ++i)
			{
				for (j = 0; j < blen; ++j)
					if (a[i + j] != b[j])
						break;
				if (j >= blen)
					return i;
			}
			return -1;
		}

		public static int AobscanASM(Context ctx, string asm)
		{
			return Aobscan(ctx, Assembler.Assemble(asm));
		}
		public static int Aobscan(Context ctx, string hexCode)
		{
			byte[] bytes = GetHexCodeFromString(hexCode);
			return Aobscan(ctx, hexCode);
		}
		public static int Aobscan(Context ctx, byte[] aob)
		{
			UInt32 i = 0;
			MEMORY_BASIC_INFORMATION mbi;
			while (i < 0x7FFFFFFF)
			{
				UInt32 flag = VirtualQueryEx(ctx.Handle, i, out mbi, 28);
				if (flag != 28)
					break;
				if ((int)mbi.RegionSize <= 0)
					break;
				if (mbi.Protect != (UInt32)AllocationProtect.PAGE_EXECUTE_READWRITE || mbi.State != 0x00001000)
				{
					i += mbi.RegionSize;
					continue;
				}
				byte[] va = new byte[mbi.RegionSize];
				ReadProcessMemory(ctx.Handle, i, va, mbi.RegionSize, 0);
				int r = Memmem(va, mbi.RegionSize, aob, (UInt32)aob.Length);
				if (r >= 0)
				{
					return (int)(i + r);
				}
				i += mbi.RegionSize;
			}
			return -1;
		}
	}
}
