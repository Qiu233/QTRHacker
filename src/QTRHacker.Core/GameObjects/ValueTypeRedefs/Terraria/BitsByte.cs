using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects.ValueTypeRedefs.Terraria
{
	[StructLayout(LayoutKind.Sequential)]
	public struct BitsByte
	{
		private static bool Null;
		public byte Value;
		public BitsByte(bool b1 = false, bool b2 = false, bool b3 = false, bool b4 = false, bool b5 = false, bool b6 = false, bool b7 = false, bool b8 = false)
		{
			Value = 0;
			this[0] = b1;
			this[1] = b2;
			this[2] = b3;
			this[3] = b4;
			this[4] = b5;
			this[5] = b6;
			this[6] = b7;
			this[7] = b8;
		}

		public void ClearAll()
		{
			Value = 0;
		}

		public void SetAll()
		{
			Value = byte.MaxValue;
		}

		public bool this[int key]
		{
			get
			{
				return (Value & 1 << key) != 0;
			}
			set
			{
				if (value)
				{
					Value |= (byte)(1 << key);
					return;
				}
				Value &= (byte)~(byte)(1 << key);
			}
		}

		public void Retrieve(ref bool b0)
		{
			Retrieve(ref b0, ref Null, ref Null, ref Null, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1)
		{
			Retrieve(ref b0, ref b1, ref Null, ref Null, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2)
		{
			Retrieve(ref b0, ref b1, ref b2, ref Null, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref b6, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6, ref bool b7)
		{
			b0 = this[0];
			b1 = this[1];
			b2 = this[2];
			b3 = this[3];
			b4 = this[4];
			b5 = this[5];
			b6 = this[6];
			b7 = this[7];
		}

		public static implicit operator byte(BitsByte bb)
		{
			return bb.Value;
		}

		public static implicit operator BitsByte(byte b)
		{
			return new BitsByte
			{
				Value = b
			};
		}

		public static BitsByte[] ComposeBitsBytesChain(bool optimizeLength, params bool[] flags)
		{
			int i = flags.Length;
			int num = 0;
			while (i > 0)
			{
				num++;
				i -= 7;
			}
			BitsByte[] array = new BitsByte[num];
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < flags.Length; j++)
			{
				array[num3][num2] = flags[j];
				num2++;
				if (num2 == 7 && num3 < num - 1)
				{
					array[num3][num2] = true;
					num2 = 0;
					num3++;
				}
			}
			if (optimizeLength)
			{
				int num4 = array.Length - 1;
				while (array[num4] == 0 && num4 > 0)
				{
					array[num4 - 1][7] = false;
					num4--;
				}
				Array.Resize(ref array, num4 + 1);
			}
			return array;
		}

		public static BitsByte[] DecomposeBitsBytesChain(BinaryReader reader)
		{
			List<BitsByte> list = new List<BitsByte>();
			BitsByte item;
			do
			{
				item = reader.ReadByte();
				list.Add(item);
			}
			while (item[7]);
			return list.ToArray();
		}
	}
}
