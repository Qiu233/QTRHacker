namespace QTRHacker.ViewModels.Wiki.Item;

public class JumpToItemEventArgs : EventArgs
{
	public ItemInfo ItemInfo { get; }

	public JumpToItemEventArgs(ItemInfo itemInfo)
	{
		ItemInfo = itemInfo;
	}
}
