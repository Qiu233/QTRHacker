using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class ItemSlots : GameObjectArray<Item>
	{
		public ItemSlots(GameContext context, int bAddr) : base(context, bAddr)
		{
		}
	}
}
