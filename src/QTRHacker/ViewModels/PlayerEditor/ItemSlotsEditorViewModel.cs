using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public RelayCommand ApplyItemData => applyItemData;
		public RelayCommand InitItemData => initItemData;

		private Item SelectedItem => ItemProvider(ItemSlotsGridViewModel.SelectedIndex);


		public ItemSlotsEditorViewModel(ISlotsLayout layout, ItemFromIndexDelegate itemProvider, DispatcherTimer updateTimer)
		{
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
			initItemData= new RelayCommand(o => true, o =>
			{
				InitItem();
				ItemPropertiesPanelViewModel.UpdatePropertiesFromItem(SelectedItem);
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
