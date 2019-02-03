using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class Entity : GameObject
	{
		public const int OFFSET_WhoAmI = 0x4;
		public const int OFFSET_Active = 0x18;
		public const int OFFSET_Width = 0x10;
		public const int OFFSET_Height = 0x14;
		public const int OFFSET_X = 0x20;
		public const int OFFSET_Y = 0x24;
		public const int OFFSET_VelocityX = 0x28;
		public const int OFFSET_VelocityY = 0x2c;
		public const int OFFSET_OldX = 0x30;
		public const int OFFSET_OldY = 0x34;
		public const int OFFSET_Direction = 0xc;
		public const int OFFSET_OldDirection = 0x8;
		public const int OFFSET_Wet = 0x19;
		public const int OFFSET_HoneyWet = 0x1a;
		public const int OFFSET_WetCount = 0x1b;
		public const int OFFSET_LavaWet = 0x1c;

		public int WhoAmI
		{
			get
			{
				ReadFromOffset(OFFSET_WhoAmI, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_WhoAmI, value);
		}
		public float X
		{
			get
			{
				ReadFromOffset(OFFSET_X, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_X, value);
		}
		public float Y
		{
			get
			{
				ReadFromOffset(OFFSET_Y, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Y, value);
		}
		public float OldX
		{
			get
			{
				ReadFromOffset(OFFSET_OldX, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_OldX, value);
		}
		public float OldY
		{
			get
			{
				ReadFromOffset(OFFSET_OldY, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_OldY, value);
		}
		public float VelocityX
		{
			get
			{
				ReadFromOffset(OFFSET_VelocityX, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_VelocityX, value);
		}
		public float VelocityY
		{
			get
			{
				ReadFromOffset(OFFSET_VelocityY, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_VelocityY, value);
		}
		public int Width
		{
			get
			{
				ReadFromOffset(OFFSET_Width, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Width, value);
		}
		public int Height
		{
			get
			{
				ReadFromOffset(OFFSET_Height, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Height, value);
		}
		public int Direction
		{
			get
			{
				ReadFromOffset(OFFSET_Direction, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Direction, value);
		}
		public int OldDirection
		{
			get
			{
				ReadFromOffset(OFFSET_OldDirection, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_OldDirection, value);
		}
		public bool Active
		{
			get
			{
				ReadFromOffset(OFFSET_Active, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Active, value);
		}
		public bool Wet
		{
			get
			{
				ReadFromOffset(OFFSET_Wet, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Wet, value);
		}
		public bool HoneyWet
		{
			get
			{
				ReadFromOffset(OFFSET_HoneyWet, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HoneyWet, value);
		}
		public byte WetCount
		{
			get
			{
				ReadFromOffset(OFFSET_WetCount, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_WetCount, value);
		}
		public bool LavaWet
		{
			get
			{
				ReadFromOffset(OFFSET_LavaWet, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_LavaWet, value);
		}
		public Entity(GameContext context, int bAddr) : base(context, bAddr)
		{
		}
	}
}
