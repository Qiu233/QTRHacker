using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QTRHacker.Controls;

public class ItemSlot : RadioButton
{
	public ImageSource ItemImageSource
	{
		get => (ImageSource)GetValue(ItemImageSourceProperty);
		set => SetValue(ItemImageSourceProperty, value);
	}
	public static readonly DependencyProperty ItemImageSourceProperty =
		DependencyProperty.Register(nameof(ItemImageSource), typeof(ImageSource), typeof(ItemSlot));


	public int ItemStack
	{
		get => (int)GetValue(ItemStackProperty);
		set => SetValue(ItemStackProperty, value);
	}
	public static readonly DependencyProperty ItemStackProperty =
		DependencyProperty.Register(nameof(ItemStack), typeof(int), typeof(ItemSlot), new PropertyMetadata(0));

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
			new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0xFF, 0x88, 0x00))));


	public ItemSlot()
	{
	}
	static ItemSlot()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemSlot), new FrameworkPropertyMetadata(typeof(ItemSlot)));
	}

	protected override void OnMouseDown(MouseButtonEventArgs e)
	{
		base.OnMouseDown(e);
		if (e.RightButton != MouseButtonState.Pressed)
			return;
		IsChecked = true;
	}
}
