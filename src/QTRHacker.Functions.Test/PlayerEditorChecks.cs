using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.ViewModels.PlayerEditor;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace QTRHacker.Functions.Test;

internal static class PlayerEditorChecks
{
	public static void Verify(Player player)
	{
		var application = Application.Current ?? new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
		application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/QTRHacker;component/Styles/Themes/Dark.xaml", UriKind.Relative) });
		application.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/QTRHacker;component/Styles/Controls.xaml", UriKind.Relative) });
		HackGlobal.LoadConfig();
		for (int i = 0; i < player.Loadouts.Length; i++)
			Console.WriteLine($"Loadout {i}: armor={player.Loadouts[i].Armor.Length}, dye={player.Loadouts[i].Dye.Length}.");

		var editor = new PlayerEditorWindowViewModel(player);
		PlayerEditorWindow window = null;
		try
		{
			var itemEditors = new[] {
				editor.InventoryEditorViewModel, editor.ArmorEditorViewModel,
				editor.PiggyBankViewModel, editor.SafeViewModel, editor.ForgeViewModel, editor.VoidVaultViewModel,
				editor.Loadout1ViewModel, editor.Loadout2ViewModel, editor.Loadout3ViewModel,
			};
			int selectedSlots = 0;
			foreach (var itemEditor in itemEditors)
			{
				foreach (var slot in itemEditor.ItemSlotsGridViewModel.Slots)
				{
					itemEditor.ItemSlotsGridViewModel.SelectedIndex = slot.Index;
					var item = itemEditor.ItemProvider(slot.Index);
					if ((int)itemEditor.ItemPropertiesPanelViewModel.GetValue("Type") != item.Type)
						throw new InvalidOperationException("Selecting an item slot displayed the wrong item.");
					selectedSlots++;
				}
				itemEditor.Update();
			}
			var loadoutEditors = new[] { editor.Loadout1ViewModel, editor.Loadout2ViewModel, editor.Loadout3ViewModel };
			for (int loadoutIndex = 0; loadoutIndex < loadoutEditors.Length; loadoutIndex++)
			{
				var loadout = player.Loadouts[loadoutIndex];
				var expected = loadout.Armor.Concat(loadout.Dye).Select(item => item.BaseAddress).ToArray();
				var actual = loadoutEditors[loadoutIndex].ItemSlotsGridViewModel.Slots
					.Select(slot => loadoutEditors[loadoutIndex].ItemProvider(slot.Index).BaseAddress).ToArray();
				if (!actual.SequenceEqual(expected))
					throw new InvalidOperationException($"Loadout {loadoutIndex} slots do not map to its armor and dye arrays.");
			}

			window = new PlayerEditorWindow { DataContext = editor };
			var tabs = ((Grid)window.Content).Children.OfType<TabControl>().Single();
			var tabEditors = new[] {
				editor.InventoryEditorViewModel, editor.ArmorEditorViewModel,
				editor.PiggyBankViewModel, editor.SafeViewModel, editor.ForgeViewModel, editor.VoidVaultViewModel,
				null, editor.Loadout1ViewModel, editor.Loadout2ViewModel, editor.Loadout3ViewModel,
			};
			editor.UpdateTimer.Interval = TimeSpan.FromMilliseconds(30);
			for (int index = 0; index < tabs.Items.Count; index++)
			{
				tabs.SelectedIndex = index;
				Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
				foreach (var itemEditor in itemEditors)
					if (itemEditor.Updating != ReferenceEquals(itemEditor, tabEditors[index]))
						throw new InvalidOperationException($"Tab {index} enabled updates for the wrong item editor.");
				// Change only the UI cache, then verify that the real timer refreshes the active tab.
				foreach (var slot in itemEditors.SelectMany(itemEditor => itemEditor.ItemSlotsGridViewModel.Slots))
					slot.Stack = int.MinValue;
				WaitForUpdates(editor.UpdateTimer);
				foreach (var itemEditor in itemEditors)
					foreach (var slot in itemEditor.ItemSlotsGridViewModel.Slots)
						if ((slot.Stack != int.MinValue) != ReferenceEquals(itemEditor, tabEditors[index]))
							throw new InvalidOperationException($"Tab {index} did not refresh only its own item slots.");
			}
			Console.WriteLine($"Player editor checks passed: initialization, {selectedSlots} slot selections, all 3 loadout mappings, and {tabs.Items.Count} tab bindings and timer refreshes.");
		}
		finally
		{
			editor.UpdateTimer.Stop();
			window?.Close();
		}
	}

	private static void WaitForUpdates(DispatcherTimer timer)
	{
		var frame = new DispatcherFrame();
		int updates = 0;
		EventHandler onTick = (_, _) =>
		{
			if (++updates == 2)
				frame.Continue = false;
		};
		var timeout = new DispatcherTimer { Interval = TimeSpan.FromSeconds(15) };
		timeout.Tick += (_, _) => frame.Continue = false;
		timer.Tick += onTick;
		try
		{
			timeout.Start();
			Dispatcher.PushFrame(frame);
			if (updates != 2)
				throw new TimeoutException("Player editor timer did not finish the integration check.");
		}
		finally
		{
			timeout.Stop();
			timer.Tick -= onTick;
		}
	}
}
