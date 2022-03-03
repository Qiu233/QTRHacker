using QTRHacker.Core.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core
{
	public static class Utils
	{
		public static void AddItemStackToInv(this GameContext ctx, int type, int stack)
		{
			var pos = ctx.MyPlayer.Position;
			int num = Item.NewItem(ctx, (int)pos.X, (int)pos.Y, 0, 0, type, stack, false, 0, true);
			NetMessage.SendData(ctx, 21, -1, -1, 0, num, 0, 0, 0, 0, 0, 0);
		}
	}
}
