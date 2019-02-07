using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.Tile")]
	public class Tile : GameObject
	{
		[GameFieldOffsetFieldName("type")]
		public static int OFFSET_Type = 0x4;
		[GameFieldOffsetFieldName("wall")]
		public static int OFFSET_Wall = 0xc;
		[GameFieldOffsetFieldName("liquid")]
		public static int OFFSET_Liquid = 0xd;
		[GameFieldOffsetFieldName("sTileHeader")]
		public static int OFFSET_STileHeader = 0x6;
		[GameFieldOffsetFieldName("bTileHeader")]
		public static int OFFSET_BTileHeader = 0xe;
		[GameFieldOffsetFieldName("bTileHeader2")]
		public static int OFFSET_BTileHeader2 = 0xf;
		[GameFieldOffsetFieldName("bTileHeader3")]
		public static int OFFSET_BTileHeader3 = 0x10;
		[GameFieldOffsetFieldName("frameX")]
		public static int OFFSET_FrameX = 0x8;
		[GameFieldOffsetFieldName("frameY")]
		public static int OFFSET_FrameY = 0xa;

		
		public const int Liquid_Honey = 2;
		public const int Liquid_Lava = 1;
		public const int Liquid_Water = 0;

		public const int Type_Halfbrick = 1;
		public const int Type_SlopeDownLeft = 3;
		public const int Type_SlopeDownRight = 2;
		public const int Type_SlopeUpLeft = 5;
		public const int Type_SlopeUpRight = 4;
		public const int Type_Solid = 0;


		public ushort Type
		{
			get
			{
				ReadFromOffset(OFFSET_Type, out ushort v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Type, value);
		}

		public byte Wall
		{
			get
			{
				ReadFromOffset(OFFSET_Wall, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Wall, value);
		}

		public byte Liquid
		{
			get
			{
				ReadFromOffset(OFFSET_Liquid, out byte v);
				return v;
			}
			set=>WriteFromOffset(OFFSET_Liquid, value);
		}

		public short STileHeader
		{
			get
			{
				ReadFromOffset(OFFSET_STileHeader, out short v);
				return v;
			}
			set => WriteFromOffset(OFFSET_STileHeader, value);
		}


		public byte BTileHeader
		{
			get
			{
				ReadFromOffset(OFFSET_BTileHeader, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BTileHeader, value);
		}

		public byte BTileHeader2
		{
			get
			{
				ReadFromOffset(OFFSET_BTileHeader2, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BTileHeader2, value);
		}

		public byte BTileHeader3
		{
			get
			{
				ReadFromOffset(OFFSET_BTileHeader3, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BTileHeader3, value);
		}

		public short FrameX
		{
			get
			{
				ReadFromOffset(OFFSET_FrameX, out short v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FrameX, value);
		}

		public short FrameY
		{
			get
			{
				ReadFromOffset(OFFSET_FrameY, out short v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FrameY, value);
		}

		public bool InActive()
		{
			return (this.STileHeader & 64) == 64;
		}
		
		public void InActive(bool inActive)
		{
			if (inActive)
			{
				this.STileHeader |= 64;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 65471);
		}

		public bool Lava()
		{
			return (this.BTileHeader & 32) == 32;
		}
		
		public void Lava(bool lava)
		{
			if (lava)
			{
				this.BTileHeader = ((byte)((this.BTileHeader & 159) | 32));
				return;
			}
			this.BTileHeader &= 223;
		}

		public bool Honey()
		{
			return (this.BTileHeader & 64) == 64;
		}
		
		public void Honey(bool honey)
		{
			if (honey)
			{
				this.BTileHeader = ((byte)((this.BTileHeader & 159) | 64));
				return;
			}
			this.BTileHeader &= 191;
		}

		public byte LiquidType()
		{
			return (byte)((this.BTileHeader & 96) >> 5);
		}
		
		public void LiquidType(int liquidType)
		{
			if (liquidType == 0)
			{
				this.BTileHeader &= 159;
				return;
			}
			if (liquidType == 1)
			{
				this.Lava(true);
				return;
			}
			if (liquidType == 2)
			{
				this.Honey(true);
			}
		}

		public bool SkipLiquid()
		{
			return (this.BTileHeader3 & 16) == 16;
		}
		
		public void SkipLiquid(bool skipLiquid)
		{
			if (skipLiquid)
			{
				this.BTileHeader3 |= 16;
				return;
			}
			this.BTileHeader3 &= 239;
		}

		public byte Slope()
		{
			return (byte)((this.STileHeader & 28672) >> 12);
		}
		
		public void Slope(byte slope)
		{
			this.STileHeader = (short)(((int)this.STileHeader & 36863) | (int)(slope & 7) << 12);
		}

		public bool TopSlope()
		{
			byte b = this.Slope();
			return b == 1 || b == 2;
		}

		public byte WallColor()
		{
			return (byte)(this.BTileHeader & 31);
		}
		
		public void WallColor(byte wallColor)
		{
			if (wallColor > 30)
			{
				wallColor = 30;
			}
			this.BTileHeader = ((byte)((this.BTileHeader & 224) | wallColor));
		}

		public byte WallFrameNumber()
		{
			return (byte)((this.BTileHeader2 & 192) >> 6);
		}
		
		public void WallFrameNumber(byte wallFrameNumber)
		{
			this.BTileHeader2 = (byte)((int)(this.BTileHeader2 & 63) | (int)(wallFrameNumber & 3) << 6);
		}

		public int WallFrameX()
		{
			return (int)((this.BTileHeader2 & 15) * 36);
		}
		
		public void WallFrameX(int wallFrameX)
		{
			this.BTileHeader2 = (byte)((int)(this.BTileHeader2 & 240) | (wallFrameX / 36 & 15));
		}
		
		public int WallFrameY()
		{
			return (int)((this.BTileHeader3 & 7) * 36);
		}
		
		public void WallFrameY(int wallFrameY)
		{
			this.BTileHeader3 = (byte)((int)(this.BTileHeader3 & 248) | (wallFrameY / 36 & 7));
		}
		
		public bool Wire()
		{
			return (this.STileHeader & 128) == 128;
		}
		
		public void Wire(bool wire)
		{
			if (wire)
			{
				this.STileHeader |= 128;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 65407);
		}
		
		public bool Wire2()
		{
			return (this.STileHeader & 256) == 256;
		}
		
		public void Wire2(bool wire2)
		{
			if (wire2)
			{
				this.STileHeader |= 256;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 65279);
		}
		
		public bool Wire3()
		{
			return (this.STileHeader & 512) == 512;
		}
		
		public void Wire3(bool wire3)
		{
			if (wire3)
			{
				this.STileHeader |= 512;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 65023);
		}
		
		public bool Wire4()
		{
			return (this.BTileHeader & 128) == 128;
		}
		
		public void Wire4(bool wire4)
		{
			if (wire4)
			{
				this.BTileHeader |= 128;
				return;
			}
			this.BTileHeader &= 127;
		}

		public bool Active()
		{
			return (this.STileHeader & 32) == 32;
		}
		
		public void Active(bool active)
		{
			if (active)
			{
				this.STileHeader |= 32;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 65503);
		}

		public bool Actuator()
		{
			return (this.STileHeader & 2048) == 2048;
		}
		
		public void Actuator(bool actuator)
		{
			if (actuator)
			{
				this.STileHeader |= 2048;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 63487);
		}

		public int BlockType()
		{
			if (this.HalfBrick())
			{
				return 1;
			}
			int num = (int)this.Slope();
			if (num > 0)
			{
				num++;
			}
			return num;
		}

		public bool BottomSlope()
		{
			byte b = this.Slope();
			return b == 3 || b == 4;
		}
		
		public bool CheckingLiquid()
		{
			return (this.BTileHeader3 & 8) == 8;
		}
		
		public void CheckingLiquid(bool checkingLiquid)
		{
			if (checkingLiquid)
			{
				this.BTileHeader3 |= 8;
				return;
			}
			this.BTileHeader3 &= 247;
		}

		public void ClearEverything()
		{
			this.Type = 0;
			this.Wall = 0;
			this.Liquid = 0;
			this.STileHeader = 0;
			this.BTileHeader = 0;
			this.BTileHeader2 = 0;
			this.BTileHeader3 = 0;
			this.FrameX = 0;
			this.FrameY = 0;
		}

		public void ClearMetadata()
		{
			this.Liquid = 0;
			this.STileHeader = 0;
			this.BTileHeader = 0;
			this.BTileHeader2 = 0;
			this.BTileHeader3 = 0;
			this.FrameX = 0;
			this.FrameY = 0;
		}
		public bool HalfBrick()
		{
			return (this.STileHeader & 1024) == 1024;
		}
		
		public void HalfBrick(bool halfBrick)
		{
			if (halfBrick)
			{
				this.STileHeader |= 1024;
				return;
			}
			this.STileHeader = (short)((int)this.STileHeader & 64511);
		}

		public byte FrameNumber()
		{
			return (byte)((this.BTileHeader2 & 48) >> 4);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00293BF9 File Offset: 0x00291DF9
		public void FrameNumber(byte frameNumber)
		{
			this.BTileHeader2 = (byte)((int)(this.BTileHeader2 & 207) | (int)(frameNumber & 3) << 4);
		}

		public bool HasSameSlope(Tile tile)
		{
			return (this.STileHeader & 29696) == (tile.STileHeader & 29696);
		}

		public Tile(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
	}
}
