using QTRHacker.ViewModels.PagePanels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public event DoWorkEventHandler AttachedToGame
		{
			add => MainPageViewModel.AttachedToGameWorker.DoWork += value;
			remove => MainPageViewModel.AttachedToGameWorker.DoWork -= value;
		}

		public event RunWorkerCompletedEventHandler AttachedToGameWorksFinished
		{
			add => MainPageViewModel.AttachedToGameWorker.RunWorkerCompleted += value;
			remove => MainPageViewModel.AttachedToGameWorker.RunWorkerCompleted -= value;
		}
	}
}
