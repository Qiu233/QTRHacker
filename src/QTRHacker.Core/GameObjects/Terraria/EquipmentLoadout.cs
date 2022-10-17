using QHackLib;

namespace QTRHacker.Core.GameObjects.Terraria;

public class EquipmentLoadout : GameObject
{
	public GameObjectArray<Item> Armor => new(Context, InternalObject.Armor);
	public GameObjectArray<Item> Dye => new(Context, InternalObject.Dye);
	public GameObjectArrayV<bool> Hide => new(Context, InternalObject.Hide);

	public EquipmentLoadout(GameContext context, HackObject obj) : base(context, obj)
	{
	}
}
