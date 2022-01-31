using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace QTRHacker.Localization
{
	public class LocalizationExtension : MarkupExtension
	{
		[ConstructorArgument("Key")]
		public string Key
		{
			get;
			set;
		}

		public LocalizationExtension(string key)
		{
			Key = key;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			Binding binding = new(nameof(LocalizationItem.Value));
			binding.Source = new LocalizationItem(Key);
			return binding.ProvideValue(serviceProvider);
		}
	}
}
