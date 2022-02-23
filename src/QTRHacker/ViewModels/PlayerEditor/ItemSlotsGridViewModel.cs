using QTRHacker.Assets;
using QTRHacker.Controls;
using QTRHacker.Models;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemSlotsGridViewModel : ViewModelBase
	{
		private int selectedIndex = -1;

		public ISlotsLayout SlotsLayout
		{
			get;
		}
		public ObservableCollection<ItemSlotViewModel> Slots { get; } = new();

		public int SelectedIndex
		{
			get => selectedIndex;
			set
			{
				if (selectedIndex == value)
					return;
				if (value < 0 || value >= Slots.Count)
					return;
				selectedIndex = value;
				OnPropertyChanged(nameof(SelectedIndex));
				Slots[selectedIndex].IsSelected = true;
				SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		public event EventHandler SelectedIndexChanged;

		public ItemSlotsGridViewModel(ISlotsLayout slotsLayout)
		{
			SlotsLayout = slotsLayout;

			for (int i = 0; i < SlotsLayout.Slots; i++)
			{
				var (Column, Row) = SlotsLayout.GetSlotLocation(i);
				var slot = new ItemSlotViewModel(i)
				{
					Column = Column,
					Row = Row
				};
				WeakEventManager<ItemSlotViewModel, EventArgs>.AddHandler(slot, nameof(ItemSlotViewModel.Selected), Slot_Selected);
				Slots.Add(slot);
			}
		}

		private void Slot_Selected(object sender, EventArgs e)
		{
			ItemSlotViewModel slot = sender as ItemSlotViewModel;
			SelectedIndex = slot.Index;
		}
	}
}
