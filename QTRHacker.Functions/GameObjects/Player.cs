using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QHackLib;
using QHackLib.Assemble;
using QHackLib.FunctionHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.Player")]
	public class Player : Entity
	{
		[GameFieldOffsetFieldName("statLife")]
		public static int OFFSET_Life = 0x340;
		[GameFieldOffsetFieldName("statLifeMax")]
		public static int OFFSET_MaxLife = 0x338;
		[GameFieldOffsetFieldName("statMana")]
		public static int OFFSET_Mana = 0x34c;
		[GameFieldOffsetFieldName("statManaMax")]
		public static int OFFSET_MaxMana = 0x348;
		[GameFieldOffsetFieldName("ghost")]
		public static int OFFSET_Ghost = 0x549;
		[GameFieldOffsetFieldName("buffType")]
		public static int OFFSET_BuffType = 0xAC;
		[GameFieldOffsetFieldName("buffTime")]
		public static int OFFSET_BuffTime = 0xB0;

		[GameFieldOffsetFieldName("inventory")]
		public static int OFFSET_INV = 0xBC;
		[GameFieldOffsetFieldName("armor")]
		public static int OFFSET_ARMOR = 0x98;
		[GameFieldOffsetFieldName("dye")]
		public static int OFFSET_DYE = 0x9C;
		[GameFieldOffsetFieldName("miscEquips")]
		public static int OFFSET_MISC = 0xA0;
		[GameFieldOffsetFieldName("miscDyes")]
		public static int OFFSET_MISCDYE = 0xA4;
		[GameFieldOffsetFieldName("bank")]
		public static int OFFSET_Bank = 0xC4;
		[GameFieldOffsetFieldName("bank2")]
		public static int OFFSET_Bank2 = 0xC8;
		[GameFieldOffsetFieldName("bank3")]
		public static int OFFSET_Bank3 = 0xCC;
		[GameFieldOffsetFieldName("hair")]
		public static int OFFSET_Hair = 0x3F4;
		[GameFieldOffsetFieldName("hairColor")]
		public static int OFFSET_HairColor = 0x73C;
		[GameFieldOffsetFieldName("skinColor")]
		public static int OFFSET_SkinColor = 0x740;
		[GameFieldOffsetFieldName("eyeColor")]
		public static int OFFSET_EyeColor = 0x744;
		[GameFieldOffsetFieldName("shirtColor")]
		public static int OFFSET_ShirtColor = 0x748;
		[GameFieldOffsetFieldName("underShirtColor")]
		public static int OFFSET_UnderShirtColor = 0x74C;
		[GameFieldOffsetFieldName("pantsColor")]
		public static int OFFSET_PantsColor = 0x750;
		[GameFieldOffsetFieldName("shoeColor")]
		public static int OFFSET_ShoeColor = 0x754;



		public const int ITEM_MAX_COUNT = 59;
		public const int INV_MAX_COUNT = 50;
		public const int ARMOR_MAX_COUNT = 20;
		public const int DYE_MAX_COUNT = 10;
		public const int MISC_MAX_COUNT = 5;
		public const int MISCDYE_MAX_COUNT = 5;
		public const int BUFF_MAX_COUNT = 22;
		public const int MAX_PLAYER = 256;

		public Chest Bank
		{
			get
			{
				ReadFromOffset(OFFSET_Bank, out int v);
				return new Chest(Context, v);
			}
		}
		public Chest Bank2
		{
			get
			{
				ReadFromOffset(OFFSET_Bank2, out int v);
				return new Chest(Context, v);
			}
		}
		public Chest Bank3
		{
			get
			{
				ReadFromOffset(OFFSET_Bank3, out int v);
				return new Chest(Context, v);
			}
		}

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


		public bool Ghost
		{
			get
			{
				ReadFromOffset(OFFSET_Ghost, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Ghost, value);
		}

		public int Hair
		{
			get
			{
				ReadFromOffset(OFFSET_Hair, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Hair, value);
		}
		public int HairColor
		{
			get
			{
				ReadFromOffset(OFFSET_HairColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_HairColor, value);
		}
		public int SkinColor
		{
			get
			{
				ReadFromOffset(OFFSET_SkinColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_SkinColor, value);
		}
		public int EyeColor
		{
			get
			{
				ReadFromOffset(OFFSET_EyeColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_EyeColor, value);
		}
		public int ShirtColor
		{
			get
			{
				ReadFromOffset(OFFSET_ShirtColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ShirtColor, value);
		}
		public int UnderShirtColor
		{
			get
			{
				ReadFromOffset(OFFSET_UnderShirtColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_UnderShirtColor, value);
		}
		public int PantsColor
		{
			get
			{
				ReadFromOffset(OFFSET_PantsColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_PantsColor, value);
		}
		public int ShoeColor
		{
			get
			{
				ReadFromOffset(OFFSET_ShoeColor, out int v);
				return v;
			}
			set => WriteFromOffset(OFFSET_ShoeColor, value);
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

		public string Name
		{
			get
			{
				ReadFromOffset(0x70, out int a);
				int b = 0;
				NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 0x4, ref b, 4, 0);
				byte[] c = new byte[b * 2];
				NativeFunctions.ReadProcessMemory(Context.HContext.Handle, a + 0x8, c, c.Length, 0);
				return Encoding.Unicode.GetString(c);
			}
		}


		public Player(GameContext context, int bAddr) : base(context, bAddr)
		{

		}


		public void AddBuff(int type, int time, bool quiet = false)
		{
			AssemblySnippet snippet = AssemblySnippet.FromDotNetCall(
				Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Player", "AddBuff"),
				null,
				true,
				BaseAddress, type, time, quiet);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.AddressHelper.GetFunctionAddress("Terraria.Main", "Update"), true);
		}


		public void SaveInventory(Stream file)
		{
			BinaryWriter bw = new BinaryWriter(file);
			for (int i = 0; i < ITEM_MAX_COUNT; i++)
			{
				var item = Inventory[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < ARMOR_MAX_COUNT; i++)
			{
				var item = Armor[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < DYE_MAX_COUNT; i++)
			{
				var item = Dye[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < MISC_MAX_COUNT; i++)
			{
				var item = Misc[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				var item = MiscDye[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			bw.Flush();
		}


		public void LoadInventory(Stream file)
		{
			BinaryReader br = new BinaryReader(file);
			for (int i = 0; i < ITEM_MAX_COUNT; i++)
			{
				var item = Inventory[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type <= 0 && item.Type <= 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < ARMOR_MAX_COUNT; i++)
			{
				var item = Armor[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type <= 0 && item.Type <= 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < DYE_MAX_COUNT; i++)
			{
				var item = Dye[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type <= 0 && item.Type <= 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < MISC_MAX_COUNT; i++)
			{
				var item = Misc[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type <= 0 && item.Type <= 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				var item = MiscDye[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type <= 0 && item.Type <= 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}

		}

		public string SerializeInventoryWithProperties()
		{
			JArray arr = new JArray();
			JArray invArr = new JArray();
			JArray armArr = new JArray();
			JArray dyeArr = new JArray();
			JArray miscArr = new JArray();
			JArray miscDyeArr = new JArray();

			arr.Add(invArr);
			arr.Add(armArr);
			arr.Add(dyeArr);
			arr.Add(miscArr);
			arr.Add(miscDyeArr);

			for (int i = 0; i < ITEM_MAX_COUNT; i++)
			{
				invArr.Add(JObject.FromObject(Inventory[i]));
			}
			for (int i = 0; i < ARMOR_MAX_COUNT; i++)
			{
				armArr.Add(JObject.FromObject(Armor[i]));
			}
			for (int i = 0; i < DYE_MAX_COUNT; i++)
			{
				dyeArr.Add(JObject.FromObject(Dye[i]));
			}
			for (int i = 0; i < MISC_MAX_COUNT; i++)
			{
				miscArr.Add(JObject.FromObject(Misc[i]));
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				miscDyeArr.Add(JObject.FromObject(MiscDye[i]));
			}
			return arr.ToString(Formatting.Indented);
		}

		public void DeserializeInventoryWithProperties(string json)
		{
			JArray o = JArray.Parse(json);
			JArray invArr = (JArray)o[0];
			JArray armArr = (JArray)o[1];
			JArray dyeArr = (JArray)o[2];
			JArray miscArr = (JArray)o[3];
			JArray miscDyeArr = (JArray)o[4];


			for (int i = 0; i < ITEM_MAX_COUNT; i++)
			{
				var item = Inventory[i];
				var a = invArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
			for (int i = 0; i < ARMOR_MAX_COUNT; i++)
			{
				var item = Armor[i];
				var a = armArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
			for (int i = 0; i < DYE_MAX_COUNT; i++)
			{
				var item = Dye[i];
				var a = dyeArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
			for (int i = 0; i < MISC_MAX_COUNT; i++)
			{
				var item = Misc[i];
				var a = miscArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				var item = MiscDye[i];
				var a = miscDyeArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
		}
	}
}
