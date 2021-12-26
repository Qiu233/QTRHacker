using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QHackLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.Terraria
{
	/// <summary>
	/// Wrapper for Terraria.Player
	/// </summary>
	public partial class Player : Entity
	{
		public Player(GameContext ctx, HackObject obj) : base(ctx, obj)
		{

		}

		public const int ITEM_MAX_COUNT = 59;
		public const int INV_MAX_COUNT = 50;
		public const int ARMOR_MAX_COUNT = 20;
		public const int DYE_MAX_COUNT = 10;
		public const int MISC_MAX_COUNT = 5;
		public const int MISCDYE_MAX_COUNT = 5;
		public const int BUFF_MAX_COUNT = 22;
		public const int MAX_PLAYER = 256;

		public void AddBuff(int type, int time, bool quiet = true, bool foodHack = false)
		{
			Context.RunByHookOnDoUpdate(TypedInternalObject.GetMethodCall("Terraria.Player.AddBuff(Int32, Int32, Boolean, Boolean)")
				.Call(true, null, null, new object[] { type, time, quiet, foodHack })).Wait();
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
				var item = MiscEquips[i];
				bw.Write(item.Type);
				bw.Write(item.Stack);
				bw.Write(item.Prefix);
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				var item = MiscDyes[i];
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
				if (type < 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < ARMOR_MAX_COUNT; i++)
			{
				var item = Armor[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type < 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < DYE_MAX_COUNT; i++)
			{
				var item = Dye[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type < 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < MISC_MAX_COUNT; i++)
			{
				var item = MiscEquips[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type < 0) continue;
				item.SetDefaultsAndPrefix(type, prefix);
				item.Stack = stack;
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				var item = MiscDyes[i];
				int type = br.ReadInt32();
				int stack = br.ReadInt32();
				byte prefix = br.ReadByte();
				if (type < 0) continue;
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
				miscArr.Add(JObject.FromObject(MiscEquips[i]));
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				miscDyeArr.Add(JObject.FromObject(MiscDyes[i]));
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
				var item = MiscEquips[i];
				var a = miscArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
			for (int i = 0; i < MISCDYE_MAX_COUNT; i++)
			{
				var item = MiscDyes[i];
				var a = miscDyeArr[i];
				item.SetDefaultsAndPrefix(Convert.ToInt32(a["Type"]), Convert.ToInt32(a["Prefix"]));
				JsonConvert.PopulateObject(a.ToString(), item);
			}
		}
	}
}
