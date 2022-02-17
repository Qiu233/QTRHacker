using QTRHacker.Models;

namespace QTRHacker.ViewModels.Wiki.Item
{
	public class ItemStackInfo : ViewModelBase
	{
		public ItemInfo ItemInfo { get; }
		public int Stack { get; }

		public ItemStackInfo(ItemInfo itemInfo, int stack)
		{
			ItemInfo = itemInfo;
			Stack = stack;
		}
		public ItemStackInfo(ItemStack stack)
		{
			ItemInfo = new ItemInfo(stack.Type);
			Stack = stack.Stack;
		}
	}
}
