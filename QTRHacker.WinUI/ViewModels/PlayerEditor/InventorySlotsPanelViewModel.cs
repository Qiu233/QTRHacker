using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.ViewModels.PlayerEditor.SlotsPages;
using StrongInject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
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

	public ChestPageViewModel Piggy { get; }
	public ChestPageViewModel Safe { get; }
	public ChestPageViewModel Forge { get; }
	public ChestPageViewModel VoidVault { get; }


	public InventorySlotsPanelViewModel(
		Func<InvPageViewModel> inv,
		Func<ArmorPageViewModel> armor,
		Func<string, Chest, ChestPageViewModel> chest,
		Func<string, EquipmentLoadout, LoadoutPageViewModel> loadout,
		Player p,
		DispatcherQueueTimer updater)
	{
		InvPageViewModel = inv();
		ArmorPageViewModel = armor();
		Piggy = chest("Piggy", p.Bank);
		Safe = chest("Safe", p.Bank2);
		Forge = chest("Forge", p.Bank3);
		VoidVault = chest("Void Vault", p.Bank4);

		pages.Add(InvPageViewModel);
		pages.Add(ArmorPageViewModel);
		pages.Add(Piggy);
		pages.Add(Safe);
		pages.Add(Forge);
		pages.Add(VoidVault);

		for (int i = 0; i < p.Loadouts.Length; i++)
			pages.Add(loadout($"Loadout {i}", p.Loadouts[i]));

		updater.Tick += Updater_Tick;
		SelectedPage = InvPageViewModel;
	}

	private async void Updater_Tick(DispatcherQueueTimer sender, object args)
	{
		if (SelectedPage is null)
			return;
		await SelectedPage.Update();
	}
}
