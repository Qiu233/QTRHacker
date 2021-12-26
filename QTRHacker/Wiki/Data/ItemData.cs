using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Wiki.Data
{
	public class ItemData
	{
		public static bool Initialized
		{
			get;
			private set;
		}
		public static List<ItemData> Data
		{
			get;
			private set;
		}
		public static void InitializeFromJson(JArray ItemInfo)
		{
			if (Initialized)
				return;
			Data = new List<ItemData>();
			foreach (var item in ItemInfo)
			{
				var itm_data = new ItemData();
				Data.Add(itm_data);
				itm_data.Name = item["Name"].ToString();
				itm_data.Type = item["type"].Value<int>();
				itm_data.Rare = item["rare"].Value<int>();
				itm_data.Value = item["value"].Value<int>();

				itm_data.MaxStack = item["maxStack"].Value<int>();

				itm_data.HeadSlot = item["headSlot"].Value<int>();
				itm_data.BodySlot = item["bodySlot"].Value<int>();
				itm_data.LegSlot = item["legSlot"].Value<int>();

				itm_data.Accessory = item["accessory"].Value<bool>();
				itm_data.Melee = item["melee"].Value<bool>();
				itm_data.Ranged = item["ranged"].Value<bool>();
				itm_data.Magic = item["magic"].Value<bool>();
				itm_data.Summon = item["summon"].Value<bool>();
				itm_data.Sentry = item["sentry"].Value<bool>();
				itm_data.Consumable = item["consumable"].Value<bool>();

				itm_data.Pick = item["pick"].Value<int>();
				itm_data.Axe = item["axe"].Value<int>();
				itm_data.Hammer = item["hammer"].Value<int>();
				itm_data.Damage = item["damage"].Value<int>();
				itm_data.Defense = item["defense"].Value<int>();
				itm_data.Crit = item["crit"].Value<int>();
				itm_data.KnockBack = item["knockBack"].Value<int>();
				itm_data.Shoot = item["shoot"].Value<int>();
				itm_data.ShootSpeed = item["shootSpeed"].Value<float>();
				itm_data.UseTime = item["useTime"].Value<int>();
				itm_data.UseAnimation = item["useAnimation"].Value<int>();
				itm_data.HealLife = item["healLife"].Value<int>();
				itm_data.HealMana = item["healMana"].Value<int>();
				itm_data.CreateTile = item["createTile"].Value<int>();
				itm_data.CreateWall = item["createWall"].Value<int>();
				itm_data.PlaceStyle = item["placeStyle"].Value<int>();
				itm_data.TileBoost = item["tileBoost"].Value<int>();
				itm_data.BuffType = item["buffType"].Value<int>();
				itm_data.BuffTime = item["buffTime"].Value<int>();
				itm_data.Mana = item["mana"].Value<int>();
				itm_data.Bait = item["bait"].Value<int>();

				itm_data.QuestItem = item["questItem"].Value<bool>();
			}
			Initialized = true;
		}
		public string Name;
		public int Type;
		public int Rare;
		public int Value;

		public int MaxStack;

		public int HeadSlot;
		public int BodySlot;
		public int LegSlot;

		public bool Accessory;
		public bool Melee;
		public bool Ranged;
		public bool Magic;
		public bool Summon;
		public bool Sentry;
		public bool Consumable;

		public int Pick;
		public int Axe;
		public int Hammer;
		public int Damage;
		public int Defense;
		public int Crit;
		public int KnockBack;
		public int Shoot;
		public float ShootSpeed;
		public int UseTime;
		public int UseAnimation;
		public int HealLife;
		public int HealMana;
		public int CreateTile;
		public int CreateWall;
		public int PlaceStyle;
		public int TileBoost;
		public int BuffType;
		public int BuffTime;
		public int Mana;
		public int Bait;

		public bool QuestItem;
	}
}
