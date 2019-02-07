using Newtonsoft.Json;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.Item")]
	public class Item : Entity
	{
		[GameFieldOffsetFieldName("type")]
		public static int OFFSET_Type = 0x6C;
		[GameFieldOffsetFieldName("netID")]
		public static int OFFSET_Type2 = 0x6c + 0xA0;
		[GameFieldOffsetFieldName("stack")]
		public static int OFFSET_Stack = 0x80;
		[GameFieldOffsetFieldName("fishingPole")]
		public static int OFFSET_FishingPole = 0x58;
		[GameFieldOffsetFieldName("bait")]
		public static int OFFSET_Bait = 0x5C;
		[GameFieldOffsetFieldName("knockBack")]
		public static int OFFSET_KnockBack = 0xA8;
		[GameFieldOffsetFieldName("shoot")]
		public static int OFFSET_Shoot = 0xD8;
		[GameFieldOffsetFieldName("shootSpeed")]
		public static int OFFSET_ShootSpeed = 0xDC;
		[GameFieldOffsetFieldName("crit")]
		public static int OFFSET_Crit = 0x110;
		[GameFieldOffsetFieldName("damage")]
		public static int OFFSET_Damage = 0xA4;
		[GameFieldOffsetFieldName("healLife")]
		public static int OFFSET_HealLife = 0xAC;
		[GameFieldOffsetFieldName("healMana")]
		public static int OFFSET_HealMana = 0xB0;
		[GameFieldOffsetFieldName("useTime")]
		public static int OFFSET_UseTime = 0x7C;
		[GameFieldOffsetFieldName("pick")]
		public static int OFFSET_Pick = 0x88;
		[GameFieldOffsetFieldName("axe")]
		public static int OFFSET_Axe = 0x8C;
		[GameFieldOffsetFieldName("hammer")]
		public static int OFFSET_Hammer = 0x90;
		[GameFieldOffsetFieldName("tileBoost")]
		public static int OFFSET_TileBoost = 0x94;
		[GameFieldOffsetFieldName("autoReuse")]
		public static int OFFSET_AutoReuse = 0x12E;
		[GameFieldOffsetFieldName("useAnimation")]
		public static int OFFSET_UseAnimation = 0x78;
		[GameFieldOffsetFieldName("buffType")]
		public static int OFFSET_BuffType = 0xFC;
		[GameFieldOffsetFieldName("buffTime")]
		public static int OFFSET_BuffTime = 0x100;
		[GameFieldOffsetFieldName("prefix")]
		public static int OFFSET_Prefix = 0x147;
		[GameFieldOffsetFieldName("scale")]
		public static int OFFSET_Scale = 0xB8;
		[GameFieldOffsetFieldName("defense")]
		public static int OFFSET_Defense = 0xBC;
		[GameFieldOffsetFieldName("accessory")]
		public static int OFFSET_Accessory = 0x12B;
		[GameFieldOffsetFieldName("createTile")]
		public static int OFFSET_CreateTile = 0x98;
		[GameFieldOffsetFieldName("createWall")]
		public static int OFFSET_CreateWall = 0x9C;
		[GameFieldOffsetFieldName("ammo")]
		public static int OFFSET_Ammo = 0xE0;
		[GameFieldOffsetFieldName("headSlot")]
		public static int OFFSET_HeadSlot = 0xC0;
		[GameFieldOffsetFieldName("bodySlot")]
		public static int OFFSET_BodySlot = 0xC4;
		[GameFieldOffsetFieldName("legSlot")]
		public static int OFFSET_LegSlot = 0xC8;
		[GameFieldOffsetFieldName("balloonSlot")]
		public static int OFFSET_BalloonSlot = 0x13A;
		[GameFieldOffsetFieldName("cartTrack")]
		public static int OFFSET_CartTrack = 0x122;
		[GameFieldOffsetFieldName("consumable")]
		public static int OFFSET_Consumable = 0x12D;
		[GameFieldOffsetFieldName("frontSlot")]
		public static int OFFSET_FrontSlot = 0x133;
		[GameFieldOffsetFieldName("glowMask")]
		public static int OFFSET_GlowMask = 0x11C;
		[GameFieldOffsetFieldName("makeNPC")]
		public static int OFFSET_MakeNPC = 0x118;
		[GameFieldOffsetFieldName("mana")]
		public static int OFFSET_Mana = 0xF0;
		[GameFieldOffsetFieldName("maxStack")]
		public static int OFFSET_MaxStack = 0x84;
		[GameFieldOffsetFieldName("noUseGraphic")]
		public static int OFFSET_NoUseGraphic = 0x13D;
		[GameFieldOffsetFieldName("useStyle")]
		public static int OFFSET_UseStyle = 0x74;
		[GameFieldOffsetFieldName("placeStyle")]
		public static int OFFSET_PlaceStyle = 0xA0;
		[GameFieldOffsetFieldName("rare")]
		public static int OFFSET_Rare = 0xD4;
		[GameFieldOffsetFieldName("reuseDelay")]
		public static int OFFSET_ReuseDelay = 0x114;
		[GameFieldOffsetFieldName("tileWand")]
		public static int OFFSET_TileWand = 0x54;
		[GameFieldOffsetFieldName("useAmmo")]
		public static int OFFSET_UseAmmo = 0xE4;
		[GameFieldOffsetFieldName("value")]
		public static int OFFSET_Value = 0xF8;
		[GameFieldOffsetFieldName("waistSlot")]
		public static int OFFSET_WaistSlot = 0x135;
		[GameFieldOffsetFieldName("wingSlot")]
		public static int OFFSET_WingSlot = 0x136;
		[GameFieldOffsetFieldName("lifeRegen")]
		public static int OFFSET_LifeRegen = 0xE8;
		[GameFieldOffsetFieldName("backSlot")]
		public static int OFFSET_BackSlot = 0x132;
		[GameFieldOffsetFieldName("faceSlot")]
		public static int OFFSET_FaceSlot = 0x139;
		[GameFieldOffsetFieldName("handOnSlot")]
		public static int OFFSET_HandOnSlot = 0x130;
		[GameFieldOffsetFieldName("handOffSlot")]
		public static int OFFSET_HandOffSlot = 0x131;
		[GameFieldOffsetFieldName("holdStyle")]
		public static int OFFSET_HoldStyle = 0x70;
		[GameFieldOffsetFieldName("magic")]
		public static int OFFSET_Magic = 0x149;
		[GameFieldOffsetFieldName("mech")]
		public static int OFFSET_Mech = 0x120;
		[GameFieldOffsetFieldName("melee")]
		public static int OFFSET_Melee = 0x148;
		[GameFieldOffsetFieldName("noMelee")]
		public static int OFFSET_NoMelee = 0x13E;
		[GameFieldOffsetFieldName("neckSlot")]
		public static int OFFSET_NeckSlot = 0x138;
		[GameFieldOffsetFieldName("ranged")]
		public static int OFFSET_Ranged = 0x14A;
		[GameFieldOffsetFieldName("shoeSlot")]
		public static int OFFSET_ShoeSlot = 0x134;
		[GameFieldOffsetFieldName("material")]
		public static int OFFSET_Material = 0x142;
		[GameFieldOffsetFieldName("sentry")]
		public static int OFFSET_Sentry = 0x14D;
		[GameFieldOffsetFieldName("mountType")]
		public static int OFFSET_MountType = 0x104;
		[GameFieldOffsetFieldName("hairDye")]
		public static int OFFSET_HairDye = 0x11A;
		[GameFieldOffsetFieldName("dye")]
		public static int OFFSET_Dye = 0x124;
		[GameFieldOffsetFieldName("questItem")]
		public static int OFFSET_QuestItem = 0x11E;
		[GameFieldOffsetFieldName("thrown")]
		public static int OFFSET_Thrown = 0x14B;
		[GameFieldOffsetFieldName("instanced")]
		public static int OFFSET_Instanced = 0x128;
		[GameFieldOffsetFieldName("expertOnly")]
		public static int OFFSET_ExpertOnly = 0x125;
		[GameFieldOffsetFieldName("expert")]
		public static int OFFSET_Expert = 0x126;
		[GameFieldOffsetFieldName("summon")]
		public static int OFFSET_Summon = 0x14C;
		[GameFieldOffsetFieldName("noWet")]
		public static int OFFSET_NoWet = 0x143;
		[GameFieldOffsetFieldName("vanity")]
		public static int OFFSET_Vanity = 0x141;
		[GameFieldOffsetFieldName("channel")]
		public static int OFFSET_Channel = 0x12A;
		[GameFieldOffsetFieldName("manaIncrease")]
		public static int OFFSET_ManaIncrease = 0xEC;
		[GameFieldOffsetFieldName("release")]
		public static int OFFSET_Release = 0xF4;
		[GameFieldOffsetFieldName("alpha")]
		public static int OFFSET_Alpha = 0xB4;
		[GameFieldOffsetFieldName("potion")]
		public static int OFFSET_Potion = 0x12C;
		[GameFieldOffsetFieldName("useTurn")]
		public static int OFFSET_UseTurn = 0x12F;
		[GameFieldOffsetFieldName("buy")]
		public static int OFFSET_Buy = 0x13F;
		[GameFieldOffsetFieldName("shieldSlot")]
		public static int OFFSET_ShieldSlot = 0x137;
		[GameFieldOffsetFieldName("uniqueStack")]
		public static int OFFSET_UniqueStack = 0x145;
		[GameFieldOffsetFieldName("favorited")]
		public static int OFFSET_Favorited = 0x129;
		[GameFieldOffsetFieldName("flame")]
		public static int OFFSET_Flame = 0x11f;
		[GameFieldOffsetFieldName("paint")]
		public static int OFFSET_Paint = 0x127;

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
			set => WriteFromOffset(OFFSET_Stack, value);
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

		public int Bait
		{
			get
			{
				ReadFromOffset(OFFSET_FishingPole, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FishingPole, value);
		}

		public float KnockBack
		{
			get
			{
				ReadFromOffset(OFFSET_KnockBack, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_KnockBack, value);
		}

		public int Shoot
		{
			get
			{
				ReadFromOffset(OFFSET_Shoot, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Shoot, value);
		}

		public float ShootSpeed
		{
			get
			{
				ReadFromOffset(OFFSET_ShootSpeed, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ShootSpeed, value);
		}

		public int Crit
		{
			get
			{
				ReadFromOffset(OFFSET_Crit, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Crit, value);
		}

		public int Damage
		{
			get
			{
				ReadFromOffset(OFFSET_Damage, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Damage, value);
		}

		public int HealLife
		{
			get
			{
				ReadFromOffset(OFFSET_HealLife, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HealLife, value);
		}

		public int HealMana
		{
			get
			{
				ReadFromOffset(OFFSET_HealMana, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HealMana, value);
		}

		public int UseTime
		{
			get
			{
				ReadFromOffset(OFFSET_UseTime, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UseTime, value);
		}

		public int Pick
		{
			get
			{
				ReadFromOffset(OFFSET_Pick, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Pick, value);
		}

		public int Axe
		{
			get
			{
				ReadFromOffset(OFFSET_Axe, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Axe, value);
		}

		public int Hammer
		{
			get
			{
				ReadFromOffset(OFFSET_Hammer, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Hammer, value);
		}

		public int TileBoost
		{
			get
			{
				ReadFromOffset(OFFSET_TileBoost, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_TileBoost, value);
		}

		public bool AutoReuse
		{
			get
			{
				ReadFromOffset(OFFSET_AutoReuse, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_AutoReuse, value);
		}

		public int UseAnimation
		{
			get
			{
				ReadFromOffset(OFFSET_UseAnimation, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UseAnimation, value);
		}

		public int BuffType
		{
			get
			{
				ReadFromOffset(OFFSET_BuffType, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BuffType, value);
		}

		public int BuffTime
		{
			get
			{
				ReadFromOffset(OFFSET_BuffTime, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BuffTime, value);
		}

		public byte Prefix
		{
			get
			{
				ReadFromOffset(OFFSET_Prefix, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Prefix, value);
		}

		public float Scale
		{
			get
			{
				ReadFromOffset(OFFSET_Scale, out float v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Scale, value);
		}

		public int Defense
		{
			get
			{
				ReadFromOffset(OFFSET_Defense, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Defense, value);
		}

		public bool Accessory
		{
			get
			{
				ReadFromOffset(OFFSET_Accessory, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Accessory, value);
		}

		public int CreateTile
		{
			get
			{
				ReadFromOffset(OFFSET_CreateTile, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_CreateTile, value);
		}

		public int CreateWall
		{
			get
			{
				ReadFromOffset(OFFSET_CreateWall, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_CreateWall, value);
		}

		public int Ammo
		{
			get
			{
				ReadFromOffset(OFFSET_Ammo, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Ammo, value);
		}

		public bool CartTrack
		{
			get
			{
				ReadFromOffset(OFFSET_CartTrack, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_CartTrack, value);
		}

		public bool Consumable
		{
			get
			{
				ReadFromOffset(OFFSET_Consumable, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Consumable, value);
		}

		public bool NoUseGraphic
		{
			get
			{
				ReadFromOffset(OFFSET_NoUseGraphic, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_NoUseGraphic, value);
		}

		public sbyte HeadSlot
		{
			get
			{
				ReadFromOffset(OFFSET_HeadSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HeadSlot, value);
		}

		public sbyte BodySlot
		{
			get
			{
				ReadFromOffset(OFFSET_BodySlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BodySlot, value);
		}

		public sbyte LegSlot
		{
			get
			{
				ReadFromOffset(OFFSET_LegSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_LegSlot, value);
		}

		public sbyte BalloonSlot
		{
			get
			{
				ReadFromOffset(OFFSET_BalloonSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BalloonSlot, value);
		}

		public sbyte FrontSlot
		{
			get
			{
				ReadFromOffset(OFFSET_FrontSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FrontSlot, value);
		}

		public sbyte WaistSlot
		{
			get
			{
				ReadFromOffset(OFFSET_WaistSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_WaistSlot, value);
		}

		public sbyte WingSlot
		{
			get
			{
				ReadFromOffset(OFFSET_WingSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_WingSlot, value);
		}

		public sbyte BackSlot
		{
			get
			{
				ReadFromOffset(OFFSET_BackSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BackSlot, value);
		}

		public sbyte FaceSlot
		{
			get
			{
				ReadFromOffset(OFFSET_FaceSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FaceSlot, value);
		}

		public sbyte HandOnSlot
		{
			get
			{
				ReadFromOffset(OFFSET_HandOnSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HandOnSlot, value);
		}

		public sbyte HandOffSlot
		{
			get
			{
				ReadFromOffset(OFFSET_HandOffSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HandOnSlot, value);
		}

		public sbyte NeckSlot
		{
			get
			{
				ReadFromOffset(OFFSET_NeckSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_NeckSlot, value);
		}

		public sbyte Dye
		{
			get
			{
				ReadFromOffset(OFFSET_Dye, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Dye, value);
		}

		public sbyte ShieldSlot
		{
			get
			{
				ReadFromOffset(OFFSET_ShieldSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ShieldSlot, value);
		}

		public sbyte ShoeSlot
		{
			get
			{
				ReadFromOffset(OFFSET_ShoeSlot, out sbyte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ShoeSlot, value);
		}

		public byte Paint
		{
			get
			{
				ReadFromOffset(OFFSET_Paint, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Paint, value);
		}

		public short GlowMask
		{
			get
			{
				ReadFromOffset(OFFSET_GlowMask, out short v);
				return v;
			}
			set => WriteFromOffset(OFFSET_GlowMask, value);
		}

		public short MakeNPC
		{
			get
			{
				ReadFromOffset(OFFSET_MakeNPC, out short v);
				return v;
			}
			set => WriteFromOffset(OFFSET_MakeNPC, value);
		}

		public int Mana
		{
			get
			{
				ReadFromOffset(OFFSET_Mana, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Mana, value);
		}

		public int MaxStack
		{
			get
			{
				ReadFromOffset(OFFSET_MaxStack, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_MaxStack, value);
		}

		public int UseStyle
		{
			get
			{
				ReadFromOffset(OFFSET_UseStyle, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UseStyle, value);
		}

		public int PlaceStyle
		{
			get
			{
				ReadFromOffset(OFFSET_PlaceStyle, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_PlaceStyle, value);
		}

		public int HoldStyle
		{
			get
			{
				ReadFromOffset(OFFSET_HoldStyle, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HoldStyle, value);
		}

		public int Rare
		{
			get
			{
				ReadFromOffset(OFFSET_Rare, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Rare, value);
		}

		public int ReuseDelay
		{
			get
			{
				ReadFromOffset(OFFSET_ReuseDelay, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ReuseDelay, value);
		}

		public int TileWand
		{
			get
			{
				ReadFromOffset(OFFSET_TileWand, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_TileWand, value);
		}

		public int UseAmmo
		{
			get
			{
				ReadFromOffset(OFFSET_UseAmmo, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UseAmmo, value);
		}

		public int Value
		{
			get
			{
				ReadFromOffset(OFFSET_Value, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Value, value);
		}

		public bool Magic
		{
			get
			{
				ReadFromOffset(OFFSET_Magic, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Magic, value);
		}

		public bool Mech
		{
			get
			{
				ReadFromOffset(OFFSET_Mech, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Mech, value);
		}

		public bool Melee
		{
			get
			{
				ReadFromOffset(OFFSET_Melee, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Melee, value);
		}

		public bool Ranged
		{
			get
			{
				ReadFromOffset(OFFSET_Ranged, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Ranged, value);
		}

		public bool NoMelee
		{
			get
			{
				ReadFromOffset(OFFSET_NoMelee, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_NoMelee, value);
		}

		public bool Material
		{
			get
			{
				ReadFromOffset(OFFSET_Material, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Material, value);
		}

		public bool Sentry
		{
			get
			{
				ReadFromOffset(OFFSET_Sentry, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Sentry, value);
		}

		public bool QuestItem
		{
			get
			{
				ReadFromOffset(OFFSET_QuestItem, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_QuestItem, value);
		}

		public bool Thrown
		{
			get
			{
				ReadFromOffset(OFFSET_Thrown, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Thrown, value);
		}

		public bool Instanced
		{
			get
			{
				ReadFromOffset(OFFSET_Instanced, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Instanced, value);
		}

		public bool ExpertOnly
		{
			get
			{
				ReadFromOffset(OFFSET_ExpertOnly, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ExpertOnly, value);
		}

		public bool Expert
		{
			get
			{
				ReadFromOffset(OFFSET_Expert, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Expert, value);
		}

		public bool Summon
		{
			get
			{
				ReadFromOffset(OFFSET_Summon, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Summon, value);
		}

		public bool NoWet
		{
			get
			{
				ReadFromOffset(OFFSET_NoWet, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_NoWet, value);
		}

		public bool Vanity
		{
			get
			{
				ReadFromOffset(OFFSET_Vanity, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Vanity, value);
		}

		public bool Channel
		{
			get
			{
				ReadFromOffset(OFFSET_Channel, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Channel, value);
		}

		public bool Potion
		{
			get
			{
				ReadFromOffset(OFFSET_Potion, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Potion, value);
		}

		public bool UseTurn
		{
			get
			{
				ReadFromOffset(OFFSET_UseTurn, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UseTurn, value);
		}

		public bool Buy
		{
			get
			{
				ReadFromOffset(OFFSET_Buy, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Buy, value);
		}

		public bool UniqueStack
		{
			get
			{
				ReadFromOffset(OFFSET_UniqueStack, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UniqueStack, value);
		}

		public bool Favorited
		{
			get
			{
				ReadFromOffset(OFFSET_Favorited, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Favorited, value);
		}

		public bool Flame
		{
			get
			{
				ReadFromOffset(OFFSET_Flame, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Flame, value);
		}

		public int MountType
		{
			get
			{
				ReadFromOffset(OFFSET_MountType, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_MountType, value);
		}


		public short HairDye
		{
			get
			{
				ReadFromOffset(OFFSET_HairDye, out short v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HairDye, value);
		}

		public int ManaIncrease
		{
			get
			{
				ReadFromOffset(OFFSET_ManaIncrease, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ManaIncrease, value);
		}

		public int Release
		{
			get
			{
				ReadFromOffset(OFFSET_Release, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Release, value);
		}

		public int Alpha
		{
			get
			{
				ReadFromOffset(OFFSET_Alpha, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Alpha, value);
		}


		public Item(GameContext context, int bAddr) : base(context, bAddr)
		{

		}

		public void SetDefaults(int type)
		{
			var snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Item", "SetDefaults"),
				null,
				true,
				BaseAddress, type, false);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}


		/// <summary>
		/// 使用这个函数，效率会优于分别调用SetDefaults和SetPrefix
		/// </summary>
		/// <param name="type"></param>
		/// <param name="prefix"></param>
		public void SetDefaultsAndPrefix(int type, int prefix)
		{
			var snippet = AssemblySnippet.FromCode(
				new AssemblyCode[] {
					Instruction.Create("push ecx"),
					Instruction.Create("push edx"),
					AssemblySnippet.FromDotNetCall(
						Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Item","SetDefaults"),
						null, false,
						BaseAddress, type, false),
					AssemblySnippet.FromDotNetCall(
						Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Item","Prefix"),
						null,false,
						BaseAddress, prefix),
					Instruction.Create("pop edx"),
					Instruction.Create("pop ecx")
				});

			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);

		}

		public void SetPrefix(int prefix)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Item", "Prefix"),
				null,
				true,
				BaseAddress, prefix);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}

		public static int NewItem(GameContext Context, float x, float y, float width, float height, int type, int stack = 1,
			bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false)
		{
			int ret = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Item", "NewItem"),
				ret,
				true,
				type, stack, y, x, height, width, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, ret, ref ret, 4, 0);
			NativeFunctions.VirtualFreeEx(Context.HContext.Handle, ret, 0);
			return ret;
		}

	}
}
