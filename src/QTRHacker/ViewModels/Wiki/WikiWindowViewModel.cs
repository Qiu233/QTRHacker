using QTRHacker.ViewModels.Wiki.Item;
using QTRHacker.ViewModels.Wiki.NPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki
{
	public class WikiWindowViewModel : ViewModelBase
	{
		public ItemPageViewModel ItemPageViewModel { get; } = new();
		public NPCPageViewModel NPCPageViewModel { get; } = new();

		public WikiWindowViewModel()
		{
		}
	}
}
