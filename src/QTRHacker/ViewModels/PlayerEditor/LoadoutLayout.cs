namespace QTRHacker.ViewModels.PlayerEditor;

public class LoadoutLayout : ISlotsLayout
{
	public int Slots => 30;

	public (int Column, int Row) GetSlotLocation(int index)
	{
		return new(index % 10, index / 10);
	}
}
