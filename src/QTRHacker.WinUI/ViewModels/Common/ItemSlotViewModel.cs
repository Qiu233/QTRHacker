using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using QTRHacker.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace QTRHacker.ViewModels.Common;

public partial class ItemSlotViewModel : ObservableObject
{
	[ObservableProperty]
	private ImageSource? itemImage;

	private int type;
	private int stack;

	public int Type => type;
	public int Stack => stack;

	[ObservableProperty]
	private int size = 48;

	[ObservableProperty]
	private string? groupName;

	private bool? isChecked;
	public bool? IsChecked
	{
		get => isChecked;
		set
		{
			if (SetProperty(ref isChecked, value))
			{
				if (value is true)
					Checked?.Invoke(this, new EventArgs());
				else
					Unchecked?.Invoke(this, new EventArgs());
			}
		}
	}

	public int Index { get; }

	public event TypedEventHandler<ItemSlotViewModel, int>? ItemTypeChanged;
	public event TypedEventHandler<ItemSlotViewModel, EventArgs>? Checked;
	public event TypedEventHandler<ItemSlotViewModel, EventArgs>? Unchecked;

	public async Task SetItem(int type, int stack)
	{
		if (SetProperty(ref this.type, type, nameof(Type)))
			ItemTypeChanged?.Invoke(this, type);
		SetProperty(ref this.stack, stack, nameof(Stack));
		ItemImage = await GameImages.GetItemImage(type);
	}

	public ItemSlotViewModel(int index)
	{
		Index = index;
	}
}
