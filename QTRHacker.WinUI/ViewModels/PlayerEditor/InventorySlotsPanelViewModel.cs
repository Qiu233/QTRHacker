using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using QTRHacker.ViewModels.PlayerEditor.SlotsPages;
using StrongInject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor;

public partial class InventorySlotsPanelViewModel : ObservableObject
{
	private readonly ObservableCollection<SlotsPageViewModel> pages = new();
	public IEnumerable<SlotsPageViewModel> Pages => pages;

	[ObservableProperty]
	private SlotsPageViewModel? selectedPage;

	public InvPageViewModel InvPageViewModel { get; }
	public ArmorPageViewModel ArmorPageViewModel { get; }

	public InventorySlotsPanelViewModel(
		Func<InvPageViewModel> inv,
		Func<ArmorPageViewModel> armor,
		Func<string, ChestPageViewModel> chest, DispatcherQueueTimer updater)
	{
		InvPageViewModel = inv();
		ArmorPageViewModel = armor();

		pages.Add(InvPageViewModel);
		//pages.Add(ArmorPageViewModel);
		updater.Tick += (s, e) =>
		{
			Update();
		};
		SelectedPage = InvPageViewModel;
	}
	public async void Update()
	{
		if (SelectedPage is null)
			return;
		await SelectedPage.Update();
	}
}
