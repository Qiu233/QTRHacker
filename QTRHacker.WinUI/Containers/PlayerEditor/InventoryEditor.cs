using Microsoft.UI.Dispatching;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.ViewModels.PlayerEditor.SlotsPages;
using QTRHacker.Views.PlayerEditor;
using StrongInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace QTRHacker.Containers.PlayerEditor;

[Register<InventoryEditorViewModel>(Scope.SingleInstance)]
[Register<ItemPropertiesPanelViewModel>(Scope.SingleInstance)]
[Register<InventorySlotsPanelViewModel>(Scope.SingleInstance)]
[Register<InvPageViewModel>]
[Register<ArmorPageViewModel>]
[Register<ChestPageViewModel>]
[Register<LoadoutPageViewModel>]
public partial class InventoryEditor : IContainer<InventoryEditorViewModel>
{
	[Factory]
	private ItemSlotViewModel CreateSlot(int index)
	{
		return new ItemSlotViewModel(index)
		{
			GroupName = "SlotsGroup@" + GetHashCode()
		};
	}
	[Instance] public Player Player { get; }

	[Instance] public DispatcherQueueTimer UpdateTimer { get; }

	private readonly Owned<InventoryEditorViewModel> owned;

	private InventoryEditor(Player p)
	{
		Player = p;
		UpdateTimer = DispatcherQueue.GetForCurrentThread().CreateTimer();
		UpdateTimer.Interval = TimeSpan.FromMilliseconds(500); // TODO: configurable
		owned = this.Resolve();
	}
	private void Show()
	{
		InventoryEditorWindow window = new(owned.Value);
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
