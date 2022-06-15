using QTRHacker.ViewModels.PagePanels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels
{
	public sealed class MainWindowViewModel : ViewModelBase
	{
		public DirectFunctionsPageViewModel DirectFunctionsPageViewModel { get; }
		public PlayersPageViewModel PlayersPageViewModel { get; }
		public AdvancedPageViewModel AdvancedPageViewModel { get; }
		public AboutPageViewModel AboutPageViewModel { get; }
		public MainPageViewModel MainPageViewModel { get; }
		public MainWindowViewModel()
		{
			PlayersPageViewModel = new();
			DirectFunctionsPageViewModel = new();
			AdvancedPageViewModel = new();

			PlayersPageViewModel.RegisterHackInitEvent();
			DirectFunctionsPageViewModel.RegisterHackInitEvent();
			AdvancedPageViewModel.RegisterHackInitEvent();

			AboutPageViewModel = new();

			MainPageViewModel = new();
			MainPageViewModel.IsSelected = true;

			MainPageViewModel.AttachedToGame += () =>
			{
				DirectFunctionsPageViewModel.Load();
				if (MainPageViewModel.IsSelected)
					PlayersPageViewModel.IsSelected = true;
			};
		}

	}
}
