using System.Windows.Media;

namespace QTRHacker.ViewModels.PlayerEditor;

public class ItemSlotViewModel : ViewModelBase
{
	private int slotWidth = 50;
	private int slotMargin = 2;
	private int row;
	private int column;
	private ImageSource itemImage;
	private int stack;
	private bool isSelected;

	public int Index { get; }

	public int SlotWidth
	{
		get => slotWidth;
		set
		{
			slotWidth = value;
			OnPropertyChanged(nameof(SlotWidth));
		}
	}

	public int SlotMargin
	{
		get => slotMargin;
		set
		{
			slotMargin = value;
			OnPropertyChanged(nameof(SlotMargin));
		}
	}

	public int Row
	{
		get => row;
		set
		{
			row = value;
			OnPropertyChanged(nameof(Row));
		}
	}
	public int Column
	{
		get => column;
		set
		{
			column = value;
			OnPropertyChanged(nameof(Column));
		}
	}
	public ImageSource ItemImage
	{
		get => itemImage;
		set
		{
			itemImage = value;
			OnPropertyChanged(nameof(ItemImage));
		}
	}
	public int Stack
	{
		get => stack;
		set
		{
			stack = value;
			OnPropertyChanged(nameof(Stack));
		}
	}

	public bool IsSelected
	{
		get => isSelected;
		set
		{
			isSelected = value;
			OnPropertyChanged(nameof(IsSelected));
			if (IsSelected)
				Selected?.Invoke(this, EventArgs.Empty);
		}
	}

	public event EventHandler Selected;

	public ItemSlotViewModel(int index)
	{
		Index = index;
	}
}
