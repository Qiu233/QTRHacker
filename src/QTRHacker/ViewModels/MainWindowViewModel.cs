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

			AttachedToGame += MainWindowViewModel_AttachedToGame;
			AttachedToGameWorksFinished += MainWindowViewModel_AttachedToGameWorksFinished;
		}

		private void MainWindowViewModel_AttachedToGame(object sender, DoWorkEventArgs e)
		{
			DateTime t0 = DateTime.Now;
			DirectFunctionsPageViewModel.LoadAllFunctions();
			DateTime t1 = DateTime.Now;
			HackGlobal.Logging.Log("Time used by loading scripts: " + (t1 - t0).TotalMilliseconds);
		}

		private void MainWindowViewModel_AttachedToGameWorksFinished(object sender, RunWorkerCompletedEventArgs e)
		{
			DateTime t0 = DateTime.Now;
			DirectFunctionsPageViewModel.UpdateUI();
			DateTime t1 = DateTime.Now;
			HackGlobal.Logging.Log("Time used by updating UI: " + (t1 - t0).TotalMilliseconds);
			PlayersPageViewModel.IsSelected = true;
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
