using QTRHacker.ViewModels.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				string resKey = "DataTemplate_";
				if (property.PropertyType == typeof(bool))
				{

				}
				else
				{
					resKey += "TextBox";
				}
				if (element.TryFindResource(resKey) is DataTemplate res)
					return res;
			}
			return base.SelectTemplate(item, container);
		}
	}
}
