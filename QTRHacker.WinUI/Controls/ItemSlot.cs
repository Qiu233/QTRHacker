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
		DependencyProperty.Register(nameof(ItemImageSource), typeof(ImageSource), typeof(ItemSlot), new PropertyMetadata(null, (s, e) => ((ItemSlot)s).ReEvaluateItemStackVisibility()));


	public int ItemStack
	{
		get => (int)GetValue(ItemStackProperty);
		set => SetValue(ItemStackProperty, value);
	}
	public static readonly DependencyProperty ItemStackProperty =
		DependencyProperty.Register(nameof(ItemStack), typeof(int), typeof(ItemSlot), new PropertyMetadata(0, (s, e) => ((ItemSlot)s).ReEvaluateItemStackVisibility()));

	public Color TintColor
	{
		get => (Color)GetValue(TintColorProperty);
		set => SetValue(TintColorProperty, value);
	}
	public static readonly DependencyProperty TintColorProperty =
		DependencyProperty.Register(nameof(TintColor), typeof(Color), typeof(ItemSlot), new PropertyMetadata(Colors.White));

	private void ReEvaluateItemStackVisibility()
	{
		if (ItemStack == 0 || ItemImageSource is null)
			VisualStateManager.GoToState(this, "StackInvisible", false);
		else
			VisualStateManager.GoToState(this, "StackVisible", false);
	}

	public ItemSlot()
	{
		DefaultStyleKey = typeof(ItemSlot);
	}

	// user can re-select other items without need to release pointer first
	protected override void OnPointerMoved(PointerRoutedEventArgs e)
	{
		base.OnPointerMoved(e);
		var p = e.GetCurrentPoint(this).Properties;
		if ((p.IsLeftButtonPressed || p.IsRightButtonPressed) && IsChecked is not true)
		{
			Focus(FocusState.Programmatic);
			IsChecked = true;
		}
	}

	protected override void OnPointerPressed(PointerRoutedEventArgs e)
	{
		// don't call to base implementation, as it would capture pointer, preventing other items be re-selected
		if (IsChecked is not true)
		{
			Focus(FocusState.Programmatic);
			IsChecked = true;
		}
	}
}
