using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	[GameFieldOffsetTypeName("Terraria.Chest")]
	public class Chest : GameObject
	{
		[GameFieldOffsetFieldName("item")]
		public static int OFFSET_Item = 0x4;
		public ItemSlots Item
		{
			get => new ItemSlots(Context, ReadFromOffset<int>(OFFSET_Item));
		}
		public Chest(GameContext context, int bAddr) : base(context, bAddr)
		{

		}
	}
}
