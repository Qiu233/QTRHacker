using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class ItemSlots : GameObject
	{
		public Item this[int i]
		{
			get
			{
				ReadFromOffset(0x08 + 0x04 * i, out int v);
				return new Item(Context, v);
			}
		}
		public ItemSlots(GameContext context, int bAddr) : base(context, bAddr)
		{
		}
	}
}
