namespace QTRHacker.Models.Wiki
{
	public class ItemData
	{
		public int Type { get; set; }
		public int Rare { get; set; }
		public int Value { get; set; }

		public int MaxStack { get; set; }

		public int HeadSlot { get; set; }
		public int BodySlot { get; set; }
		public int LegSlot { get; set; }

		public bool Accessory { get; set; }
		public bool Melee { get; set; }
		public bool Ranged { get; set; }
		public bool Magic { get; set; }
		public bool Summon { get; set; }
		public bool Sentry { get; set; }
		public bool Consumable { get; set; }

		public int Pick { get; set; }
		public int Axe { get; set; }
		public int Hammer { get; set; }
		public int Damage { get; set; }
		public int Defense { get; set; }
		public int Crit { get; set; }
		public int Shoot { get; set; }
		public float KnockBack { get; set; }
		public float ShootSpeed { get; set; }
		public int UseTime { get; set; }
		public int UseAnimation { get; set; }
		public int HealLife { get; set; }
		public int HealMana { get; set; }
		public int CreateTile { get; set; }
		public int CreateWall { get; set; }
		public int PlaceStyle { get; set; }
		public int TileBoost { get; set; }
		public int BuffType { get; set; }
		public int BuffTime { get; set; }
		public int Mana { get; set; }
		public int Bait { get; set; }

		public bool QuestItem { get; set; }
	}
}
