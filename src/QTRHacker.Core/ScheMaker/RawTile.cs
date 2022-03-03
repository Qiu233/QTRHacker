using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ScheMaker
{
	[StructLayout(LayoutKind.Sequential)]
	public struct RawTile : ICloneable
	{
		public ushort Type;
		public byte Wall;
		public byte Liquid;
		public byte BTileHeader;
		public byte BTileHeader2;
		public byte BTileHeader3;
		public short FrameX;
		public short FrameY;
		public short STileHeader;

		public byte Color()
		{
			return (byte)(STileHeader & 31);
		}

		public void Color(byte color)
		{
			if (color > 30)
			{
				color = 30;
			}
			STileHeader = (short)(STileHeader & 65504 | color);
		}

		public object Clone()
		{
			return new RawTile()
			{
				Type = Type,
				Wall = Wall,
				Liquid = Liquid,
				BTileHeader = BTileHeader,
				BTileHeader2 = BTileHeader2,
				BTileHeader3 = BTileHeader3,
				FrameX = FrameX,
				FrameY = FrameY,
				STileHeader = STileHeader
			};
		}

		public bool InActive()
		{
			return (STileHeader & 64) == 64;
		}

		public void InActive(bool inActive)
		{
			if (inActive)
			{
				STileHeader |= 64;
				return;
			}
			STileHeader = (short)(STileHeader & 65471);
		}

		public bool Lava()
		{
			return (BTileHeader & 32) == 32;
		}

		public void Lava(bool lava)
		{
			if (lava)
			{
				BTileHeader = (byte)(BTileHeader & 159 | 32);
				return;
			}
			BTileHeader &= 223;
		}

		public bool Honey()
		{
			return (BTileHeader & 64) == 64;
		}

		public void Honey(bool honey)
		{
			if (honey)
			{
				BTileHeader = (byte)(BTileHeader & 159 | 64);
				return;
			}
			BTileHeader &= 191;
		}

		public byte LiquidType()
		{
			return (byte)((BTileHeader & 96) >> 5);
		}

		public void LiquidType(int liquidType)
		{
			if (liquidType == 0)
			{
				BTileHeader &= 159;
				return;
			}
			if (liquidType == 1)
			{
				Lava(true);
				return;
			}
			if (liquidType == 2)
			{
				Honey(true);
			}
		}

		public bool SkipLiquid()
		{
			return (BTileHeader3 & 16) == 16;
		}

		public void SkipLiquid(bool skipLiquid)
		{
			if (skipLiquid)
			{
				BTileHeader3 |= 16;
				return;
			}
			BTileHeader3 &= 239;
		}

		public byte Slope()
		{
			return (byte)((STileHeader & 28672) >> 12);
		}

		public void Slope(byte slope)
		{
			STileHeader = (short)(STileHeader & 36863 | (slope & 7) << 12);
		}

		public bool TopSlope()
		{
			byte b = Slope();
			return b == 1 || b == 2;
		}

		public byte WallColor()
		{
			return (byte)(BTileHeader & 31);
		}

		public void WallColor(byte wallColor)
		{
			if (wallColor > 30)
			{
				wallColor = 30;
			}
			BTileHeader = (byte)(BTileHeader & 224 | wallColor);
		}

		public byte WallFrameNumber()
		{
			return (byte)((BTileHeader2 & 192) >> 6);
		}

		public void WallFrameNumber(byte wallFrameNumber)
		{
			BTileHeader2 = (byte)(BTileHeader2 & 63 | (wallFrameNumber & 3) << 6);
		}

		public int WallFrameX()
		{
			return (BTileHeader2 & 15) * 36;
		}

		public void WallFrameX(int wallFrameX)
		{
			BTileHeader2 = (byte)(BTileHeader2 & 240 | wallFrameX / 36 & 15);
		}

		public int WallFrameY()
		{
			return (BTileHeader3 & 7) * 36;
		}

		public void WallFrameY(int wallFrameY)
		{
			BTileHeader3 = (byte)(BTileHeader3 & 248 | wallFrameY / 36 & 7);
		}

		public bool Wire()
		{
			return (STileHeader & 128) == 128;
		}

		public void Wire(bool wire)
		{
			if (wire)
			{
				STileHeader |= 128;
				return;
			}
			STileHeader = (short)(STileHeader & 65407);
		}

		public bool Wire2()
		{
			return (STileHeader & 256) == 256;
		}

		public void Wire2(bool wire2)
		{
			if (wire2)
			{
				STileHeader |= 256;
				return;
			}
			STileHeader = (short)(STileHeader & 65279);
		}

		public bool Wire3()
		{
			return (STileHeader & 512) == 512;
		}

		public void Wire3(bool wire3)
		{
			if (wire3)
			{
				STileHeader |= 512;
				return;
			}
			STileHeader = (short)(STileHeader & 65023);
		}

		public bool Wire4()
		{
			return (BTileHeader & 128) == 128;
		}

		public void Wire4(bool wire4)
		{
			if (wire4)
			{
				BTileHeader |= 128;
				return;
			}
			BTileHeader &= 127;
		}

		public bool Active()
		{
			return (STileHeader & 32) == 32;
		}

		public void Active(bool active)
		{
			if (active)
			{
				STileHeader |= 32;
				return;
			}
			STileHeader = (short)(STileHeader & 65503);
		}

		public bool Actuator()
		{
			return (STileHeader & 2048) == 2048;
		}

		public void Actuator(bool actuator)
		{
			if (actuator)
			{
				STileHeader |= 2048;
				return;
			}
			STileHeader = (short)(STileHeader & 63487);
		}

		public int BlockType()
		{
			if (HalfBrick())
			{
				return 1;
			}
			int num = Slope();
			if (num > 0)
			{
				num++;
			}
			return num;
		}

		public bool BottomSlope()
		{
			byte b = Slope();
			return b == 3 || b == 4;
		}

		public bool CheckingLiquid()
		{
			return (BTileHeader3 & 8) == 8;
		}

		public void CheckingLiquid(bool checkingLiquid)
		{
			if (checkingLiquid)
			{
				BTileHeader3 |= 8;
				return;
			}
			BTileHeader3 &= 247;
		}

		public void ClearEverything()
		{
			Type = 0;
			Wall = 0;
			Liquid = 0;
			STileHeader = 0;
			BTileHeader = 0;
			BTileHeader2 = 0;
			BTileHeader3 = 0;
			FrameX = 0;
			FrameY = 0;
		}

		public void ClearMetadata()
		{
			Liquid = 0;
			STileHeader = 0;
			BTileHeader = 0;
			BTileHeader2 = 0;
			BTileHeader3 = 0;
			FrameX = 0;
			FrameY = 0;
		}
		public bool HalfBrick()
		{
			return (STileHeader & 1024) == 1024;
		}

		public void HalfBrick(bool halfBrick)
		{
			if (halfBrick)
			{
				STileHeader |= 1024;
				return;
			}
			STileHeader = (short)(STileHeader & 64511);
		}

		public byte FrameNumber()
		{
			return (byte)((BTileHeader2 & 48) >> 4);
		}

		public void FrameNumber(byte frameNumber)
		{
			BTileHeader2 = (byte)(BTileHeader2 & 207 | (frameNumber & 3) << 4);
		}

		public bool HasSameSlope(RawTile tile)
		{
			return (STileHeader & 29696) == (tile.STileHeader & 29696);
		}
	}
}
