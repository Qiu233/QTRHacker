using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
		public const int OFFSET_UseAnimation = 0x78;
		public const int OFFSET_BuffType = 0xFC;
		public const int OFFSET_BuffTime = 0x100;
		public const int OFFSET_Prefix = 0x147;
		public const int OFFSET_Scale = 0xB8;
		public const int OFFSET_Defense = 0xBC;
		public const int OFFSET_Accessory = 0x12B;
		public const int OFFSET_CreateTile = 0x98;
		public const int OFFSET_CreateWall = 0x9C;
		public const int OFFSET_Width = 0x10;
		public const int OFFSET_Height = 0x14;
		public const int OFFSET_Ammo = 0xE0;
		public const int OFFSET_HeadSlot = 0xC0;
		public const int OFFSET_BodySlot = 0xC4;
		public const int OFFSET_LegSlot = 0xC8;
		public const int OFFSET_BalloonSlot = 0x13A;
		public const int OFFSET_CartTrack = 0x122;
		public const int OFFSET_Consumable = 0x12D;
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
		public const int OFFSET_ShieldSlot = 0x137;
		public const int OFFSET_UniqueStack = 0x145;
		public const int OFFSET_Favorited = 0x129;
		public const int OFFSET_Flame = 0x11f;
		public const int OFFSET_Paint = 0x127;

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

		public byte HeadSlot
		{
			get
			{
				ReadFromOffset(OFFSET_HeadSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HeadSlot, value);
		}

		public byte BodySlot
		{
			get
			{
				ReadFromOffset(OFFSET_BodySlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BodySlot, value);
		}

		public byte LegSlot
		{
			get
			{
				ReadFromOffset(OFFSET_LegSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_LegSlot, value);
		}

		public byte BalloonSlot
		{
			get
			{
				ReadFromOffset(OFFSET_BalloonSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BalloonSlot, value);
		}

		public byte FrontSlot
		{
			get
			{
				ReadFromOffset(OFFSET_FrontSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FrontSlot, value);
		}

		public byte WaistSlot
		{
			get
			{
				ReadFromOffset(OFFSET_WaistSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_WaistSlot, value);
		}

		public byte WingSlot
		{
			get
			{
				ReadFromOffset(OFFSET_WingSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_WingSlot, value);
		}

		public byte BackSlot
		{
			get
			{
				ReadFromOffset(OFFSET_BackSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_BackSlot, value);
		}

		public byte FaceSlot
		{
			get
			{
				ReadFromOffset(OFFSET_FaceSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_FaceSlot, value);
		}

		public byte HandOnSlot
		{
			get
			{
				ReadFromOffset(OFFSET_HandOnSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HandOnSlot, value);
		}

		public byte HandOffSlot
		{
			get
			{
				ReadFromOffset(OFFSET_HandOffSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HandOnSlot, value);
		}

		public byte NeckSlot
		{
			get
			{
				ReadFromOffset(OFFSET_NeckSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_NeckSlot, value);
		}

		public byte Dye
		{
			get
			{
				ReadFromOffset(OFFSET_Dye, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Dye, value);
		}

		public byte ShieldSlot
		{
			get
			{
				ReadFromOffset(OFFSET_ShieldSlot, out byte v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ShieldSlot, value);
		}

		public byte ShoeSlot
		{
			get
			{
				ReadFromOffset(OFFSET_ShoeSlot, out byte v);
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

		public bool Wet
		{
			get
			{
				ReadFromOffset(OFFSET_Wet, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Wet, value);
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

		public bool LavaWet
		{
			get
			{
				ReadFromOffset(OFFSET_LavaWet, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_LavaWet, value);
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
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::SetDefaults"),
				null,
				true,
				BaseAddress, type, false);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}


		/// <summary>
		/// 使用这个函数，效率会优于分别调用SetDefaults和SetPrefix
		/// </summary>
		/// <param name="type"></param>
		/// <param name="prefix"></param>
		public void SetDefaultsAndPrefix(int type, int prefix)
		{
			var a = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::SetDefaults"),
				null,
				false,
				BaseAddress, type, false);
			var b = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::Prefix"),
				null,
				false,
				BaseAddress, prefix);
			var snippet = AssemblySnippet.FromEmpty();
			snippet.Content.Add(Instruction.Create("push ecx"));
			snippet.Content.Add(Instruction.Create("push edx"));
			snippet.Content.Add(a);
			snippet.Content.Add(b);
			snippet.Content.Add(Instruction.Create("pop ecx"));
			snippet.Content.Add(Instruction.Create("pop edx"));

			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);

		}

		public void SetPrefix(int prefix)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::Prefix"),
				null,
				true,
				BaseAddress, prefix);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}

		public static int NewItem(GameContext Context, float x, float y, float width, float height, int type, int stack = 1, bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false)
		{
			int ret = NativeFunctions.VirtualAllocEx(Context.HContext.Handle, 0, 4, NativeFunctions.AllocationType.Commit, NativeFunctions.MemoryProtection.ExecuteReadWrite);
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Item::NewItem"),
				ret,
				true,
				type, stack, y, x, height, width, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
			NativeFunctions.ReadProcessMemory(Context.HContext.Handle, ret, ref ret, 4, 0);
			return ret;
		}

	}
}
