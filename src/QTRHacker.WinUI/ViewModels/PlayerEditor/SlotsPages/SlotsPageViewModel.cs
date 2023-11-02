using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Assets;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using QTRHacker.Models;
using QTRHacker.ViewModels.Common;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public abstract class SlotsPageViewModel : ObservableObject
{
	private readonly LocalizationItem HeaderLI;
	public string Header => HeaderLI.Value;
	public SlotsPageViewModel(string key)
	{
		HeaderLI = new($"InventoryEditor.SlotsTabs.{key}");
		HeaderLI.ValueChanged += (s, e) => OnPropertyChanged(nameof(Header));
	}

	public abstract Task Update();

	public abstract Task<Item> GetItem(int id);

	public abstract IEnumerable<ItemSlotViewModel> Slots { get; }

	protected static async void UpdateItemStack(ItemSlotViewModel vm, int type, int stack)
	{
		await vm.SetItem(type, stack);
	}
}
