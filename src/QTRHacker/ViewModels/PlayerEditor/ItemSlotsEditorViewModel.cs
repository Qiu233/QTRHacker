using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemSlotsEditorViewModel : ViewModelBase
	{
		public delegate Item ItemFromIndexDelegate(int index);


		private int _SelectedIndex;


		public ItemFromIndexDelegate ItemProvider { get; }
		public int SelectedIndex
		{
			get => _SelectedIndex;
			set
			{
				_SelectedIndex = value;
				OnPropertyChanged(nameof(SelectedIndex));
				OnPropertyChanged(nameof(SelectedItem));
			}
		}
		public ISlotsLayout SlotsLayout { get; }

		public Item SelectedItem => ItemProvider(SelectedIndex);

		public ItemPropertiesPanelViewModel ItemPropertiesPanelViewModel { get; }

		public ItemSlotsEditorViewModel(ISlotsLayout layout, ItemFromIndexDelegate itemProvider)
		{
			SlotsLayout = layout;
			ItemProvider = itemProvider;
			ItemPropertiesPanelViewModel = new ItemPropertiesPanelViewModel();
			PropertyChanged += ItemSlotsEditorViewModel_PropertyChanged;
			SelectedIndex = 0;
		}

		private void ItemSlotsEditorViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(SelectedItem))
			{
				ItemPropertiesPanelViewModel.UpdatePropertiesFromItem(SelectedItem);
			}
		}
	}
}
