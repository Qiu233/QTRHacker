namespace QTRHacker.ViewModels.PlayerEditor;

public sealed class BankLayout : ISlotsLayout
{
	public int Slots => 40;

	public (int Column, int Row) GetSlotLocation(int index)
	{
		return new(index % 10, index / 10);
	}
}
