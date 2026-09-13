using QTRHacker.Core.GameObjects.Terraria;

namespace QTRHacker.Core;

public static class Utils
{
	public static void AddItemStackToInv(this GameContext ctx, int type, int stack)
	{
		var pos = ctx.MyPlayer.Position;
		Item.RequestNewItem(ctx, pos, type, stack, ownership: NewItemOwnership.ReserveForLocalPlayer);
	}
}
