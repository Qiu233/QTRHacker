using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class Player : GameObject
	{
		public const int OFFSET_Life = 0x340;
		public const int OFFSET_MaxLife = 0x338;
		public const int OFFSET_Mana = 0x34c;
		public const int OFFSET_MaxMana = 0x348;
		public const int OFFSET_X = 0x20;
		public const int OFFSET_Y = 0x24;

		public const int OFFSET_INV = 0xBC, OFFSET_ARMOR = 0x98, OFFSET_DYE = 0x9C, OFFSET_MISC = 0xA0, OFFSET_MISCDYE = 0xA4;
		public const int ITEM_MAX_COUNT = 59, INV_MAX_COUNT = 50, ARMOR_MAX_COUNT = 20, DYE_MAX_COUNT = 10, MISC_MAX_COUNT = 5, MISCDYE_MAX_COUNT = 5;
		public const int BUFF_MAX_COUNT = 22;

		public int Life
		{
			get
			{
				ReadFromOffset(OFFSET_Life, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Life, value);
		}

		public int MaxLife
		{
			get
			{
				ReadFromOffset(OFFSET_MaxLife, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_MaxLife, value);
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

		public int MaxMana
		{
			get
			{
				ReadFromOffset(OFFSET_MaxMana, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_MaxMana, value);
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

		public ItemSlots Inventory
		{
			get
			{
				ReadFromOffset(OFFSET_INV, out int v);
				return new ItemSlots(Context, v);
			}
		}

		public ItemSlots Armor
		{
			get
			{
				ReadFromOffset(OFFSET_ARMOR, out int v);
				return new ItemSlots(Context, v);
			}
		}

		public ItemSlots Dye
		{
			get
			{
				ReadFromOffset(OFFSET_DYE, out int v);
				return new ItemSlots(Context, v);
			}
		}

		public ItemSlots Misc
		{
			get
			{
				ReadFromOffset(OFFSET_MISC, out int v);
				return new ItemSlots(Context, v);
			}
		}

		public ItemSlots MiscDye
		{
			get
			{
				ReadFromOffset(OFFSET_MISCDYE, out int v);
				return new ItemSlots(Context, v);
			}
		}



		public Player(GameContext context, int bAddr) : base(context, bAddr)
		{

		}


		public void AddBuff(int type, int time, bool quiet)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Player::AddBuff"),
				null,
				true,
				BaseAddress, type, time, quiet);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}
	}
}
