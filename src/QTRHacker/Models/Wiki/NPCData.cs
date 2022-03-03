using QTRHacker.Core.GameObjects.ValueTypeRedefs.Xna;

namespace QTRHacker.Models.Wiki
{
	public class NPCData
	{
		public int Type { get; set; }
		public int AiStyle { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public float Value { get; set; }
		public bool TownNPC { get; set; }
		public bool Friendly { get; set; }
		public bool Boss { get; set; }
		public int DefDamage { get; set; }
		public int DefDefense { get; set; }
		public int LifeMax { get; set; }
		public float KnockBackResist { get; set; }
		public Color Color { get; set; }
	}
}
