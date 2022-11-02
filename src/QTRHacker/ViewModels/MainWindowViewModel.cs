using QTRHacker.ViewModels.PagePanels;

namespace QTRHacker.ViewModels;

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
