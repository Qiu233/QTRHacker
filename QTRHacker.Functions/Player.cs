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

namespace QTRHacker.Functions
{
	public class Player : GameObject
	{
		public const int OFFSET_Life = 0x340;
		public const int OFFSET_MaxLife = 0x338;
		public const int OFFSET_Mana = 0x34c;
		public const int OFFSET_MaxMana = 0x348;
		public const int OFFSET_Active = 0x18;
		public const int OFFSET_X = 0x20;
		public const int OFFSET_Y = 0x24;
		public const int OFFSET_Ghost = 0x549;

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

		public bool Ghost
		{
			get
			{
				ReadFromOffset(OFFSET_Ghost, out bool v);
				return v;
			}
			set => WriteFromOffset(OFFSET_Ghost, value);
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

		public bool Active
		{
			get
			{
				ReadFromOffset(OFFSET_Active, out bool v);
				return v;
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
				Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Player::AddBuff"),
				null,
				true,
				BaseAddress, type, time, quiet);
			InlineHook.InjectAndWait(Context.HContext, snippet, Context.HContext.FunctionAddressHelper.GetFunctionAddress("Terraria.Main::Update"), true);
		}


		public void SaveInventory(Stream file)
		{
			using (BinaryWriter bw = new BinaryWriter(file))
			{
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
			}
		}


		public void LoadInventory(Stream file)
		{
			using (BinaryReader br = new BinaryReader(file))
			{
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
