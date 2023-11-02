using CommunityToolkit.WinUI.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using QTRHacker.ViewModels.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor.ItemProperties;

public class ItemPropertyTemplateSelector : DataTemplateSelector
{
	public ItemPropertyTemplateSelector() { }
	private readonly Dictionary<string, DataTemplate> templates = new();
	public Dictionary<string, DataTemplate> Templates => templates;
	protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
	{
		if (item is not ItemPropertyData property)
			throw new ArgumentOutOfRangeException(nameof(item));

		var match = Regex.Match(property.GetType().Name, "ItemPropertyData_([\\w]+)");
		if (match.Success)
		{
			string name = "DataTemplate_" + match.Groups[1];
			if (Templates.TryGetValue(name, out DataTemplate? res))
				return res;
		}
		return null;
	}
}
