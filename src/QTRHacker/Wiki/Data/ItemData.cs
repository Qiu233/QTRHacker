using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Wiki.Data
{
	public struct ItemData
	{
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
		public int Shoot;
		public float KnockBack;
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
