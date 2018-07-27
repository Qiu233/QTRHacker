using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class Item : GameObject
	{
		public const int OFFSET_Type = 0x6C;
		public const int OFFSET_Type2 = 0x6c + 0xA0;
		public const int OFFSET_Stack = 0x80;
		public const int OFFSET_FishingPole = 0x58;
		public const int OFFSET_Bait = 0x5C;
		public const int OFFSET_KnockBack = 0xA8;
		public const int OFFSET_Shoot = 0xD8;
		public const int OFFSET_ShootSpeed = 0xDC;
		public const int OFFSET_Crit = 0x110;
		public const int OFFSET_Damage = 0xA4;
		public const int OFFSET_HealLife = 0xAC;
		public const int OFFSET_HealMana = 0xB0;
		public const int OFFSET_UseTime = 0x7C;
		public const int OFFSET_Pick = 0x88;
		public const int OFFSET_Axe = 0x8C;
		public const int OFFSET_Hammer = 0x90;
		public const int OFFSET_TileBoost = 0x94;
		public const int OFFSET_AutoReuse = 0x12E;
		public const int OFFSET_WaveTime = 0x78;
		public const int OFFSET_BuffID = 0xFC;
		public const int OFFSET_BuffTime = 0x100;
		public const int OFFSET_Prefix = 0x147;
		public const int OFFSET_Scale = 0xB8;
		public const int OFFSET_Defense = 0xBC;
		public const int OFFSET_Equippable = 0x12B;
		public const int OFFSET_TileID = 0x98;
		public const int OFFSET_Width = 0x10;
		public const int OFFSET_Height = 0x14;
		public const int OFFSET_Ammo = 0xE0;
		public const int OFFSET_HeadSlot = 0xC0;
		public const int OFFSET_BodySlot = 0xC4;
		public const int OFFSET_LegSlot = 0xC8;
		public const int OFFSET_BalloonSlot = 0x13A;
		public const int OFFSET_CartTrack = 0x122;
		public const int OFFSET_Consumable = 0x12D;
		public const int OFFSET_WallID = 0x9C;
		public const int OFFSET_FrontSlot = 0x133;
		public const int OFFSET_GlowMask = 0x11C;
		public const int OFFSET_MakeNPC = 0x118;
		public const int OFFSET_Mana = 0xF0;
		public const int OFFSET_MaxStack = 0x81;
		public const int OFFSET_NoUseGraphic = 0x13D;
		public const int OFFSET_UseStyle = 0x74;
		public const int OFFSET_PlaceStyle = 0xA0;
		public const int OFFSET_Rare = 0xD4;
		public const int OFFSET_ReuseDelay = 0x114;
		public const int OFFSET_TileWand = 0x54;
		public const int OFFSET_UseAmmo = 0xE4;
		public const int OFFSET_Value = 0xF8;
		public const int OFFSET_WaistSlot = 0x135;
		public const int OFFSET_WingSlot = 0x136;
		public const int OFFSET_LifeRegen = 0xE8;
		public const int OFFSET_BackSlot = 0x132;
		public const int OFFSET_FaceSlot = 0x139;
		public const int OFFSET_HandOnSlot = 0x130;
		public const int OFFSET_HandOffSlot = 0x131;
		public const int OFFSET_HoldStyle = 0x70;
		public const int OFFSET_Magic = 0x149;
		public const int OFFSET_Mech = 0x120;
		public const int OFFSET_Melee = 0x148;
		public const int OFFSET_NoMelee = 0x13E;
		public const int OFFSET_NeckSlot = 0x138;
		public const int OFFSET_Ranged = 0x14A;
		public const int OFFSET_ShoeSlot = 0x134;
		public const int OFFSET_Material = 0x142;
		public const int OFFSET_Sentry = 0x14D;
		public const int OFFSET_MountType = 0x104;
		public const int OFFSET_HairDye = 0x11A;
		public const int OFFSET_Dye = 0x124;
		public const int OFFSET_QuestItem = 0x11E;
		public const int OFFSET_Thrown = 0x14B;
		public const int OFFSET_Instanced = 0x128;
		public const int OFFSET_ExpertOnly = 0x125;
		public const int OFFSET_Expert = 0x126;
		public const int OFFSET_Summon = 0x14C;
		public const int OFFSET_NoWet = 0x143;
		public const int OFFSET_Vanity = 0x141;
		public const int OFFSET_Wet = 0x19;
		public const int OFFSET_WetCount = 0x1B;
		public const int OFFSET_LavaWet = 0x1C;
		public const int OFFSET_Channel = 0x12A;
		public const int OFFSET_ManaIncrease = 0xEC;
		public const int OFFSET_Release = 0xF4;
		public const int OFFSET_Active = 0x18;
		public const int OFFSET_Alpha = 0xB4;
		public const int OFFSET_Potion = 0x12C;
		public const int OFFSET_UseTurn = 0x12F;
		public const int OFFSET_Buy = 0x13F;
		public const int OFFSET_ShiledSlot = 0x137;
		public const int OFFSET_UniqeStack = 0x145;
		public const int OFFSET_Favorited = 0x129;
		public const int OFFSET_Flame = 0x11f;

		public int Type
		{
			get
			{
				ReadFromOffset(OFFSET_Type, out int v);
				return v;
			}
			set
			{
				WriteFromOffset(OFFSET_Type, value);
				WriteFromOffset(OFFSET_Type2, value);
			}
		}

		public int Stack
		{
			get
			{
				ReadFromOffset(OFFSET_Stack, out int v);
				return v;
			}
			set=> WriteFromOffset(OFFSET_Stack, value);
		}

		public int FishingPole
		{
			get
			{
				ReadFromOffset(OFFSET_FishingPole, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FishingPole, value);
		}

		public Item(GameContext context, int bAddr) : base(context, bAddr)
		{

		}
	}
}
