using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.Localization
{
	public sealed class LocalizationItem : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private readonly string Key;

		public string Value => LocalizationManager.Instance.GetValue(Key);

		private void OnCultureChanged(object sender, EventArgs args)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
		}

		public LocalizationItem(string key)
		{
			Key = key;
			WeakEventManager<LocalizationManager, EventArgs>.AddHandler(
				LocalizationManager.Instance, nameof(LocalizationManager.CultureChanged), OnCultureChanged);
		}
	}
}
