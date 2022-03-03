using Microsoft.Win32;
using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemSlotsEditorViewModel : ViewModelBase
	{
		private bool updating;
		private readonly Player player;

		public delegate Item ItemFromIndexDelegate(int index);
		public ItemFromIndexDelegate ItemProvider { get; }

		public ItemPropertiesPanelViewModel ItemPropertiesPanelViewModel { get; }
		public ItemSlotsGridViewModel ItemSlotsGridViewModel { get; }

		public bool Updating
		{
			get => updating;
			set
			{
				updating = value;
				OnPropertyChanged(nameof(Updating));
			}
		}

		private readonly RelayCommand applyItemData;
		private readonly RelayCommand initItemData;
		private readonly RelayCommand saveInv;
		private readonly RelayCommand loadInv;
		public RelayCommand ApplyItemData => applyItemData;
		public RelayCommand InitItemData => initItemData;
		public RelayCommand SaveInv => saveInv;
		public RelayCommand LoadInv => loadInv;

		private Item SelectedItem => ItemProvider(ItemSlotsGridViewModel.SelectedIndex);


		public ItemSlotsEditorViewModel(ISlotsLayout layout, Player player, ItemFromIndexDelegate itemProvider, DispatcherTimer updateTimer)
		{
			this.player = player;
			ItemProvider = itemProvider;
			ItemPropertiesPanelViewModel = new ItemPropertiesPanelViewModel();
			ItemSlotsGridViewModel = new(layout);
			ItemSlotsGridViewModel.SelectedIndexChanged += ItemSlotsGridViewModel_SelectedIndexChanged;
			ItemSlotsGridViewModel.SelectedIndex = 0;
			if (updateTimer != null)
				WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(updateTimer, nameof(DispatcherTimer.Tick), Timer_Tick);

			applyItemData = new RelayCommand(o => true, o =>
			{
				ItemPropertiesPanelViewModel.UpdatePropertiesToItem(SelectedItem);
			});
			initItemData = new RelayCommand(o => true, o =>
			{
				InitItem();
				ItemPropertiesPanelViewModel.UpdatePropertiesFromItem(SelectedItem);
			});
			saveInv = new RelayCommand(o => true, o =>
			{
				SaveFileDialog dialog = new();
				dialog.Filter = "inv file|*.inv";
				dialog.InitialDirectory = Path.GetFullPath("./Content/Invs");
				if (dialog.ShowDialog() == true)
				{
					using var s = dialog.OpenFile();
					this.player.SaveInventory(s);
				}
			});
			loadInv = new RelayCommand(o => true, o =>
			{
				OpenFileDialog dialog = new();
				dialog.Filter = "inv file|*.inv";
				dialog.InitialDirectory = Path.GetFullPath("./Content/Invs");
				if (dialog.ShowDialog() == true)
				{
					using var s = dialog.OpenFile();
					this.player.LoadInventory(s);
				}
			});

			Update();
		}

		private void InitItem()
		{
			int type = (int)ItemPropertiesPanelViewModel.GetValue("Type");
			if (type == 0)
				return;
			int stack = (int)ItemPropertiesPanelViewModel.GetValue("Stack");
			stack = stack == 0 ? 1 : stack;
			byte prefix = (byte)ItemPropertiesPanelViewModel.GetValue("Prefix");
			var item = SelectedItem;
			item.SetDefaultsAndPrefix(type, prefix);
			item.Stack = stack;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (!Updating)
				return;
			Update();
		}

		private void ItemSlotsGridViewModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			ItemPropertiesPanelViewModel.UpdatePropertiesFromItem(ItemProvider(ItemSlotsGridViewModel.SelectedIndex));
		}

		public void Update()
		{
			foreach (var slot in ItemSlotsGridViewModel.Slots)
			{
				var item = ItemProvider(slot.Index);
				if (item == null)
					continue;
				slot.ItemImage = GameImages.GetItemImage(item.Type);
				slot.Stack = item.Stack;
			}
		}
	}
}
