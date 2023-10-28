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

public static class ColumnSharedSizeHelper
{
	public static readonly DependencyProperty IsSharedSizeScopeProperty =
		DependencyProperty.RegisterAttached(nameof(ColumnSharedSizeGroup), typeof(bool), typeof(UIElement), new PropertyMetadata(false));

	public static readonly DependencyProperty GroupsProperty =
		DependencyProperty.RegisterAttached(nameof(ColumnSharedSizeGroup), typeof(Dictionary<string, ColumnSharedSizeGroup>), typeof(UIElement),
			new PropertyMetadata(null));

	private static readonly DependencyProperty SharedSizeGroupProperty =
		DependencyProperty.RegisterAttached("SharedSizeGroup", typeof(string), typeof(UIElement), new PropertyMetadata(null));

	public static Dictionary<string, ColumnSharedSizeGroup> GetGroups(DependencyObject o)
	{
		return (Dictionary<string, ColumnSharedSizeGroup>)o.GetValue(GroupsProperty);
	}
	
	public static void SetGroups(DependencyObject o, Dictionary<string, ColumnSharedSizeGroup> groups)
	{
		o.SetValue(GroupsProperty, groups);
	}

	public static void SetIsSharedSizeScope(DependencyObject o, bool group) => o.SetValue(IsSharedSizeScopeProperty, group);
	public static bool GetIsSharedSizeScope(DependencyObject o) => (bool)o.GetValue(IsSharedSizeScopeProperty);


	public static void SetSharedSizeGroup(DependencyObject o, string group)
	{
		o.SetValue(SharedSizeGroupProperty, group);
		var bs = Interaction.GetBehaviors(o);
		bs.OfType<ColumnSharedSizeBehavior>().ToList().ForEach(t => bs.Remove(t));
		bs.Add(new ColumnSharedSizeBehavior() { SharedSizeGroup = group });
	}
	public static string GetSharedSizeGroup(DependencyObject o)
	{
		return (string)o.GetValue(SharedSizeGroupProperty);
	}
}
