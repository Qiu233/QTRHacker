using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemPropertyData : ViewModelBase, IDataErrorInfo
	{
		private object _Value;
		private readonly Localization.LocalizationItem LocalizationItem;

		public string Key { get; }
		public string Tip => LocalizationItem.Value;
		public PropertyInfo ItemProperty { get; }
		public Type PropertyType => ItemProperty.PropertyType;
		public object Value
		{
			get => _Value;
			set
			{
				_Value = value;
				OnPropertyChanged(nameof(Value));
			}
		}

		public string Error
		{
			get
			{
				object res = null;
				try
				{
					res = Convert.ChangeType(Value, PropertyType);
				}
				catch
				{

				}
				if (res == null)
					return $"Cannot convert {Value} to {PropertyType}";
				return null;
			}
		}

		public string this[string columnName] => Error;

		public void UpdateFromItem(Item item)
		{
			Value = ItemProperty.GetValue(item);
		}

		public void UpdateToItem(Item item)
		{
			ItemProperty.SetValue(item, Value);
		}

		public ItemPropertyData(string key)
		{
			Key = key;
			ItemProperty = typeof(Item).GetProperty(Key);
			if (ItemProperty == null)
				throw new Exception($"No such property: {Key}");//TODO: replace it with a user exception
			LocalizationItem = new Localization.LocalizationItem($"UI.ItemProperties.{Key}");
			LocalizationItem.PropertyChanged += LocalizationItem_PropertyChanged;
		}

		private void LocalizationItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(nameof(Tip));
		}
	}
}
