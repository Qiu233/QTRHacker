using QTRHacker.ViewModels.PagePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels
{
	public sealed class MainWindowViewModel : ViewModelBase
	{
		public DirectFunctionsPageViewModel DirectFunctionsPageViewModel { get; }
		public PlayersPageViewModel PlayersPageViewModel { get; }
		public MainPageViewModel MainPageViewModel { get; }
		public MainWindowViewModel()
		{
			DirectFunctionsPageViewModel = new DirectFunctionsPageViewModel();
			PlayersPageViewModel = new PlayersPageViewModel();
			MainPageViewModel = new MainPageViewModel();
		}
	}
}
