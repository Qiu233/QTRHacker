using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.Entity")]
	public class Entity : GameObject
	{
		[GameFieldOffsetFieldName("whoAmI")]
		public static int OFFSET_WhoAmI = 0x4;
		[GameFieldOffsetFieldName("active")]
		public static int OFFSET_Active = 0x18;
		[GameFieldOffsetFieldName("width")]
		public static int OFFSET_Width = 0x10;
		[GameFieldOffsetFieldName("height")]
		public static int OFFSET_Height = 0x14;
		[GameFieldOffsetFieldName("position")]
		public static int OFFSET_Position = 0x20;
		[GameFieldOffsetFieldName("velocity")]
		public static int OFFSET_Velocity = 0x28;
		[GameFieldOffsetFieldName("oldPosition")]
		public static int OFFSET_OldPosition = 0x30;
		[GameFieldOffsetFieldName("direction")]
		public static int OFFSET_Direction = 0xc;
		[GameFieldOffsetFieldName("oldDirection")]
		public static int OFFSET_OldDirection = 0x8;
		[GameFieldOffsetFieldName("wet")]
		public static int OFFSET_Wet = 0x19;
		[GameFieldOffsetFieldName("honeyWet")]
		public static int OFFSET_HoneyWet = 0x1a;
		[GameFieldOffsetFieldName("wetCount")]
		public static int OFFSET_WetCount = 0x1b;
		[GameFieldOffsetFieldName("lavaWet")]
		public static int OFFSET_LavaWet = 0x1c;

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
				ReadFromOffset(OFFSET_Position, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Position, value);
		}
		public float Y
		{
			get
			{
				ReadFromOffset(OFFSET_Position + 0x4, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Position + 0x4, value);
		}
		public float OldX
		{
			get
			{
				ReadFromOffset(OFFSET_OldPosition, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_OldPosition, value);
		}
		public float OldY
		{
			get
			{
				ReadFromOffset(OFFSET_OldPosition + 0x4, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_OldPosition + 0x4, value);
		}
		public float VelocityX
		{
			get
			{
				ReadFromOffset(OFFSET_Velocity, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Velocity, value);
		}
		public float VelocityY
		{
			get
			{
				ReadFromOffset(OFFSET_Velocity + 0x4, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Velocity + 0x4, value);
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
