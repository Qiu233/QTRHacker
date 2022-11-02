using QTRHacker.ViewModels.Wiki.Item;
using QTRHacker.ViewModels.Wiki.NPC;

namespace QTRHacker.ViewModels.Wiki;

public class WikiWindowViewModel : ViewModelBase
{
	public ItemPageViewModel ItemPageViewModel { get; } = new();
	public NPCPageViewModel NPCPageViewModel { get; } = new();

	public WikiWindowViewModel()
	{
	}
}
