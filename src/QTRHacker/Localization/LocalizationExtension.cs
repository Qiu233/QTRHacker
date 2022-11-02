using System.Windows.Data;
using System.Windows.Markup;

namespace QTRHacker.Localization;

public class LocalizationExtension : MarkupExtension
{
	public string Key
	{
		get;
		set;
	}
	public LocalizationType Type
	{
		get;
		set;
	}

	public LocalizationExtension(string key)
	{
		Key = key;
	}

	public LocalizationExtension()
	{
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		Binding binding = new(nameof(LocalizationItem.Value));
		binding.Source = new LocalizationItem(Key, Type);
		binding.Mode = BindingMode.OneWay;
		return binding.ProvideValue(serviceProvider);
	}
}
