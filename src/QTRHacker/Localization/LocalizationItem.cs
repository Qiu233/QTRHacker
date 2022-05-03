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
		private readonly LocalizationType Type;

		public string Value => LocalizationManager.Instance.GetValue(Key, Type);

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
		}

		public LocalizationItem(string key, LocalizationType type = LocalizationType.Hack)
		{
			Key = key;
			Type = type;
			LocalizationManager.RegisterLocalizationProvider(this);
		}
	}
}
