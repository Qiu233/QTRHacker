using QTRHacker.ViewModels.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QTRHacker.Views.PlayerEditor
{
	public class ItemPropertyTemplateSelector : DataTemplateSelector
	{
		public static readonly ItemPropertyTemplateSelector Instance = new();
		private ItemPropertyTemplateSelector() { }
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is ItemPropertyData property)
			{
				FrameworkElement element = container as FrameworkElement;

				var match = Regex.Match(property.GetType().Name, "ItemPropertyData_([\\w]+)");
				if (match.Success)
				{
					if (element.TryFindResource("DataTemplate_" + match.Groups[1]) is DataTemplate res)
						return res;
				}
			}
			throw new Exception();
		}
	}
}
