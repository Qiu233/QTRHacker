using Microsoft.UI.Dispatching;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.ViewModels.Common;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.ViewModels.PlayerEditor.ItemProperties;
using QTRHacker.ViewModels.PlayerEditor.SlotsPages;
using QTRHacker.Views.PlayerEditor;
using StrongInject;

namespace QTRHacker.Containers.PlayerEditor;

[Register<InventoryEditorViewModel>(Scope.SingleInstance)]
[Register<ItemPropertiesPanelViewModel>(Scope.SingleInstance)]

[Register<InventorySlotsPanelViewModel>(Scope.SingleInstance)]
[Register<InvPageViewModel>]
[Register<ArmorPageViewModel>]
[Register<ChestPageViewModel>]
[Register<LoadoutPageViewModel>]

[Register(typeof(ItemPropertyData_TextBox<>))]
[Register(typeof(ItemPropertyData_ComboBox<>))]
[Register(typeof(ItemPropertyData_CheckBox))]
public partial class InventoryEditor : IContainer<InventoryEditorViewModel>
{
	[Factory]
	public ItemSlotViewModel CreateSlot(int index)
	{
		var slot = new ItemSlotViewModel(index)
		{
			GroupName = "SlotsGroup@" + GetHashCode()
		};
		slot.ItemTypeChanged += (s, e) =>
		{
			if (s.IsChecked is true)
				UpdateSelectedItem(s);
		};
		slot.Checked += (s, e) => UpdateSelectedItem(s);
		return slot;
	}

	private async void UpdateSelectedItem(ItemSlotViewModel slot)
	{
		if (ViewModel is null)
			return;
		SelectedItemHolder.SelectedItem = await ViewModel.InventorySlotsPanelViewModel.GetItemBySlotViewModel(slot);
	}

	[Instance] public Player Player { get; }

	[Instance] public DispatcherQueueTimer UpdateTimer { get; }

	[Instance] public SelectedItemHolder SelectedItemHolder { get; } = new SelectedItemHolder();

	private readonly Owned<InventoryEditorViewModel> owned;
	private InventoryEditorViewModel? ViewModel => owned?.Value;

	private InventoryEditor(Player p)
	{
		Player = p;
		UpdateTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
		UpdateTimer.Interval = TimeSpan.FromMilliseconds(500); // TODO: configurable
		owned = this.Resolve();
	}
	private void Show()
	{
		InventoryEditorWindow window = new(ViewModel!);
		window.Closed += Window_Closed;
		window.Activate();
		UpdateTimer.Start();
	}

	private void Window_Closed(object sender, Microsoft.UI.Xaml.WindowEventArgs args)
	{
		UpdateTimer.Stop();
		owned.Dispose();
		Dispose();
	}

	public static void ShowFor(Player p) => new InventoryEditor(p).Show();
}
