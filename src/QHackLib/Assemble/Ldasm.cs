using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib.Assemble
{
	internal static class Ldasm
	{
		public ref struct DATA
		{
			public byte flags;
			public byte rex;
			public byte modrm;
			public byte sib;
			public byte opcd_offset;
			public byte opcd_size;
			public byte disp_offset;
			public byte disp_size;
			public byte imm_offset;
			public byte imm_size;
		}

		public const int F_INVALID = 0x01;
		public const int F_PREFIX = 0x02;
		public const int F_REX = 0x04;
		public const int F_MODRM = 0x08;
		public const int F_SIB = 0x10;
		public const int F_DISP = 0x20;
		public const int F_IMM = 0x40;
		public const int F_RELATIVE = 0x80;

		public const int OP_NONE = 0x00;
		public const int OP_INVALID = 0x80;

		public const int OP_DATA_I8 = 0x01;
		public const int OP_DATA_I16 = 0x02;
		public const int OP_DATA_I16_I32 = 0x04;
		public const int OP_DATA_I16_I32_I64 = 0x08;
		public const int OP_EXTENDED = 0x10;
		public const int OP_RELATIVE = 0x20;
		public const int OP_MODRM = 0x40;
		public const int OP_PREFIX = 0x80;


		private readonly static byte[] flags_table = new byte[]
		{
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_NONE,
			OP_NONE,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_NONE,
			OP_NONE,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_PREFIX,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_PREFIX,
			OP_NONE,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_PREFIX,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_PREFIX,
			OP_NONE,

			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_PREFIX,
			OP_PREFIX,
			OP_PREFIX,
			OP_PREFIX,
			OP_DATA_I16_I32,
			OP_MODRM | OP_DATA_I16_I32,
			OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,

			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I16_I32,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_DATA_I16 | OP_DATA_I16_I32,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_DATA_I8,
			OP_DATA_I16_I32_I64,
			OP_DATA_I8,
			OP_DATA_I16_I32_I64,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_DATA_I8,
			OP_DATA_I16_I32,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,
			OP_DATA_I16_I32_I64,

			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_DATA_I16,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I16_I32,
			OP_DATA_I8 | OP_DATA_I16,
			OP_NONE,
			OP_DATA_I16,
			OP_NONE,
			OP_NONE,
			OP_DATA_I8,
			OP_NONE,
			OP_NONE,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_DATA_I8,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_DATA_I16 | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I8,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_PREFIX,
			OP_NONE,
			OP_PREFIX,
			OP_PREFIX,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM
		};

		private readonly static byte[] flags_table_ex = new byte[]
		{
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_INVALID,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_INVALID,
			OP_NONE,
			OP_INVALID,
			OP_MODRM,
			OP_INVALID,
			OP_MODRM | OP_DATA_I8,        //3Dnow
			
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,
			OP_NONE,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM | OP_EXTENDED,        //SSE5
			OP_INVALID,
			OP_MODRM,
			OP_INVALID,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_INVALID,
			OP_NONE,
			OP_MODRM | OP_EXTENDED,
			OP_INVALID,
			OP_MODRM | OP_EXTENDED | OP_DATA_I8,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,
			OP_INVALID,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_NONE,
			OP_MODRM,
			OP_MODRM,
			OP_INVALID,
			OP_INVALID,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,
			OP_RELATIVE | OP_DATA_I16_I32,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_INVALID,
			OP_INVALID,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_MODRM,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM | OP_DATA_I8,
			OP_MODRM,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,
			OP_NONE,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,

			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_MODRM,
			OP_INVALID,
		};

		private static byte Cflags(byte op)
		{
			return flags_table[op];
		}


		private static byte Cflags_ex(byte op)
		{
			return flags_table_ex[op];
		}


		public unsafe static uint GetInst(in ReadOnlySpan<byte> code, out DATA ld, bool is64)
		{
			fixed (byte* ptr = code)
				return GetInst(ptr, out ld, is64);
		}

		public unsafe static uint GetInst(byte* code, out DATA ld, bool is64)
		{
			ld = new DATA();//init

			byte* p = code;
			byte s, op, f;
			byte rexw, pr_66, pr_67;
			int j = 0;
			s = rexw = pr_66 = pr_67 = 0;

			/* phase 1: parse prefixies */
			while ((Cflags(p[j]) & OP_PREFIX) != 0)
			{
				if (p[j] == 0x66)
					pr_66 = 1;
				if (p[j] == 0x67)
					pr_67 = 1;
				j++; s++;
				ld.flags |= F_PREFIX;
				if (s == 15)
				{
					ld.flags |= F_INVALID;
					return s;
				}
			}

			/* parse REX prefix */
			if (is64 && p[j] >> 4 == 4)
			{
				ld.rex = p[j];
				rexw = (byte)((ld.rex >> 3) & 1);
				ld.flags |= F_REX;
				j++; 
				s++;
			}

			/* can be only one REX prefix */
			if (is64 && p[j] >> 4 == 4)
			{
				ld.flags |= F_INVALID;
				return ++s;
			}

			/* phase 2: parse opcode */
			ld.opcd_offset = (byte)j;
			ld.opcd_size = 1;
			op = p[j++]; 
			s++;

			/* is 2 byte opcode? */
			if (op == 0x0F)
			{
				op = p[j++]; s++;
				ld.opcd_size++;
				f = Cflags_ex(op);
				if ((f & OP_INVALID) != 0)
				{
					ld.flags |= F_INVALID;
					return s;
				}
				/* for SSE instructions */
				if ((f & OP_EXTENDED) != 0)
				{
					op = p[j++]; s++;
					ld.opcd_size++;
				}
			}
			else
			{
				f = Cflags(op);
				/* pr_66 = pr_67 for opcodes A0-A3 */
				if (op >= 0xA0 && op <= 0xA3)
					pr_66 = pr_67;
			}

			/* phase 3: parse ModR/M, SIB and DISP */
			if ((f & OP_MODRM) != 0)
			{
				byte mod = (byte)(p[j] >> 6);
				byte ro = (byte)((p[j] & 0x38) >> 3);
				byte rm = (byte)(p[j] & 7);

				ld.modrm = p[j++]; s++;
				ld.flags |= F_MODRM;

				/* in F6,F7 opcodes immediate data present if R/O == 0 */
				if (op == 0xF6 && (ro == 0 || ro == 1))
					f |= OP_DATA_I8;
				if (op == 0xF7 && (ro == 0 || ro == 1))
					f |= OP_DATA_I16_I32_I64;

				/* is SIB byte exist? */
				if (mod != 3 && rm == 4 && !(!is64 && pr_67 != 0))
				{
					ld.sib = p[j++]; s++;
					ld.flags |= F_SIB;

					/* if base == 5 and mod == 0 */
					if ((ld.sib & 7) == 5 && mod == 0)
					{
						ld.disp_size = 4;
					}
				}

				switch (mod)
				{
					case 0:
						if (is64)
						{
							if (rm == 5)
							{
								ld.disp_size = 4;
								if (is64)
									ld.flags |= F_RELATIVE;
							}
						}
						else if (pr_67 != 0)
						{
							if (rm == 6)
								ld.disp_size = 2;
						}
						else
						{
							if (rm == 5)
								ld.disp_size = 4;
						}
						break;
					case 1:
						ld.disp_size = 1;
						break;
					case 2:
						if (is64)
							ld.disp_size = 4;
						else if (pr_67 != 0)
							ld.disp_size = 2;
						else
							ld.disp_size = 4;
						break;
				}

				if (ld.disp_size != 0)
				{
					ld.disp_offset = (byte)j;
					j += ld.disp_size;
					s += ld.disp_size;
					ld.flags |= F_DISP;
				}
			}

			/* phase 4: parse immediate data */
			if (rexw != 0 && (f & OP_DATA_I16_I32_I64) != 0)
				ld.imm_size = 8;
			else if ((f & OP_DATA_I16_I32) != 0 || (f & OP_DATA_I16_I32_I64) != 0)
				ld.imm_size = (byte)(4 - (pr_66 << 1));

			/* if exist, add OP_DATA_I16 and OP_DATA_I8 size */
			ld.imm_size += (byte)(f & 3);

			if (ld.imm_size != 0)
			{
				s += ld.imm_size;
				ld.imm_offset = (byte)j;
				ld.flags |= F_IMM;
				if ((f & OP_RELATIVE) != 0)
					ld.flags |= F_RELATIVE;
			}

			/* instruction is too long */
			if (s > 15)
				ld.flags |= F_INVALID;

			return s;
		}
	}
}
