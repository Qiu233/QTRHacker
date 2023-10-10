using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor;

public class ItemSlotsGridViewModel : ObservableObject
{
	public ObservableCollection<ItemSlotViewModel> Slots { get; } = new();
	public ItemSlotsGridViewModel()
	{
	}

}
