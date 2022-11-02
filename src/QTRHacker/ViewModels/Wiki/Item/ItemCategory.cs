namespace QTRHacker.ViewModels.Wiki.Item;

[Flags]
public enum ItemCategory
{
	Others = 0,
	Block = 1, Wall = 2,
	Quest = 4,
	Head = 8, Body = 16, Leg = 32,
	Accessory = 64,
	Melee = 128, Ranged = 256, Magic = 512, Summon = 1024,
	Buff = 2048, Consumable = 4096,
}
