using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor;

public partial class ItemSlotViewModel : ObservableObject
{
	[ObservableProperty]
	private int slotWidth = 50;

	[ObservableProperty]
	private double x;
	[ObservableProperty]
	private double y;

	[ObservableProperty]
	private ImageSource? itemImage;

	[ObservableProperty]
	private int stack;

	[ObservableProperty]
	private bool isSelected;

	public int Index { get; }

	public event EventHandler? Selected;

	public ItemSlotViewModel(int index)
	{
		Index = index;
	}
}
