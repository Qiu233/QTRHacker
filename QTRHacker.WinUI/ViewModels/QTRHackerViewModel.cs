using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels;

public class QTRHackerViewModel : ObservableObject
{
	public MainPageViewModel MainPageViewModel { get; }
	public PlayersPageViewModel PlayersPageViewModel { get; }
	public QTRHackerViewModel(MainPageViewModel mainPage, PlayersPageViewModel playersPage)
	{
		MainPageViewModel = mainPage;
		PlayersPageViewModel = playersPage;
	}
}
