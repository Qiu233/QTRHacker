namespace QTRHacker.ViewModels.PlayerEditor;

public interface ISlotsLayout
{
	int Slots { get; }
	(int Column, int Row) GetSlotLocation(int index);
}
