using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Windows;
using System.Windows.Input;
using Windows.UI;

namespace QTRHacker.Controls;

public class ItemSlot : RadioButton
{
	public ImageSource ItemImageSource
	{
		get => (ImageSource)GetValue(ItemImageSourceProperty);
		set => SetValue(ItemImageSourceProperty, value);
	}
	public static readonly DependencyProperty ItemImageSourceProperty =
		DependencyProperty.Register(nameof(ItemImageSource), typeof(ImageSource), typeof(ItemSlot), new PropertyMetadata(null, (s, e) => ((ItemSlot)s).ReevaluateItemStackVisibility()));


	public int ItemStack
	{
		get => (int)GetValue(ItemStackProperty);
		set => SetValue(ItemStackProperty, value);
	}
	public static readonly DependencyProperty ItemStackProperty =
		DependencyProperty.Register(nameof(ItemStack), typeof(int), typeof(ItemSlot), new PropertyMetadata(0, (s, e) => ((ItemSlot)s).ReevaluateItemStackVisibility()));

	public Color TintColor
	{
		get => (Color)GetValue(TintColorProperty);
		set => SetValue(TintColorProperty, value);
	}
	public static readonly DependencyProperty TintColorProperty =
		DependencyProperty.Register(nameof(TintColor), typeof(Color), typeof(ItemSlot), new PropertyMetadata(Colors.White));

	public Brush SelectedBorderBrush
	{
		get => (Brush)GetValue(SelectedBorderBrushProperty);
		set => SetValue(SelectedBorderBrushProperty, value);
	}
	public static readonly DependencyProperty SelectedBorderBrushProperty =
		DependencyProperty.Register(nameof(SelectedBorderBrush), typeof(Brush), typeof(ItemSlot),
			new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x88, 0x00))));

	private void ReevaluateItemStackVisibility()
	{
		if (ItemStack == 0 || ItemImageSource is null)
			VisualStateManager.GoToState(this, "StackInvisible", false);
		else
			VisualStateManager.GoToState(this, "StackVisible", false);
	}

	public ItemSlot()
	{
		this.DefaultStyleKey = typeof(ItemSlot);
	}

	protected override void OnPointerPressed(PointerRoutedEventArgs e)
	{
		base.OnPointerPressed(e);
		//var p = e.GetCurrentPoint(this).Properties;
		//if (!p.IsRightButtonPressed)
		//	return;
		IsChecked = true;
	}
}
