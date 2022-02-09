using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.Localization
{
	public sealed class LocalizationItem : INotifyPropertyChanged, ILocalizationProvider
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private readonly string Key;

		public string Value => LocalizationManager.Instance.GetValue(Key);

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
		}

		public LocalizationItem(string key)
		{
			Key = key;
			LocalizationManager.RegisterCultureChanged(this);
		}
	}
}
