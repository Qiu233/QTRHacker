using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemSlotsEditorViewModel
	{
		public delegate Item ItemFromIndexDelegate(int index);

		public int SelectedIndex { get; }
		public ISlotsLayout SlotsLayout { get; }
		public ItemFromIndexDelegate ItemProvider { get; }

		public ItemSlotsEditorViewModel(ISlotsLayout layout, ItemFromIndexDelegate itemProvider)
		{
			SlotsLayout = layout;
			ItemProvider = itemProvider;
			SelectedIndex = 0;
		}
	}
}
