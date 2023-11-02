using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Helpers;

[ContentProperty(Name = nameof(Templates))]
public class TypeNameDataTemplateSelector : DataTemplateSelector
{
	public Dictionary<string, DataTemplate> Templates { get; } = new Dictionary<string, DataTemplate>();
	protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
	{
		if (item is null)
			return null;
		if (Templates.TryGetValue(item.GetType().Name, out DataTemplate? v))
			return v;
		return null;
	}
}
