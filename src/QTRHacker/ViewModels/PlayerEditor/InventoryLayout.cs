namespace QTRHacker.ViewModels.PlayerEditor;

public sealed class InventoryLayout : ISlotsLayout
{
	public int Slots => 50;

	public (int Column, int Row) GetSlotLocation(int index)
	{
		return new(index % 10, index / 10);
	}
}
