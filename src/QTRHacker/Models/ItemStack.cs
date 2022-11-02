namespace QTRHacker.Models;

public readonly struct ItemStack
{
	public int Type { get; }
	public int Stack { get; }

	public ItemStack(int type, int stack)
	{
		Type = type;
		Stack = stack;
	}
}
