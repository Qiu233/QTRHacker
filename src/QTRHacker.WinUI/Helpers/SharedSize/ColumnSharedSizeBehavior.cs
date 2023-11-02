using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Helpers.SharedSize;

public class ColumnSharedSizeBehavior : Behavior<FrameworkElement>
{
	public static readonly DependencyProperty SharedSizeGroupProperty =
		DependencyProperty.RegisterAttached(nameof(SharedSizeGroup), typeof(string), typeof(ColumnSharedSizeBehavior), new PropertyMetadata(null));

	public string SharedSizeGroup
	{
		get => (string)GetValue(SharedSizeGroupProperty);
		set => SetValue(SharedSizeGroupProperty, value);
	}

	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject.SizeChanged += OnColumnSizeChanged;
	}
	protected override void OnDetaching()
	{
		base.OnDetaching();
		AssociatedObject.SizeChanged -= OnColumnSizeChanged;
	}
	public void OnColumnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (sender is not FrameworkElement o)
			return;
		if (SharedSizeGroup is null)
			return;
		FrameworkElement p = o;
		while (!ColumnSharedSizeHelper.GetIsSharedSizeScope(p))
		{
			if (VisualTreeHelper.GetParent(p) is not FrameworkElement fe)
				break;
			else
				p = fe;
		}
		if (!ColumnSharedSizeHelper.GetIsSharedSizeScope(p))
			return;
		var groups = ColumnSharedSizeHelper.GetGroups(p);
		if(groups is null)
		{
			groups = new();
			ColumnSharedSizeHelper.SetGroups(p, groups);
		}
		if (!groups.ContainsKey(SharedSizeGroup))
			groups[SharedSizeGroup] = new ColumnSharedSizeGroup();
		groups[SharedSizeGroup].Update(o);
	}
}
