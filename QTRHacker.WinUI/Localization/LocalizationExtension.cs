using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;

namespace QTRHacker.Localization;

public class LocalizationExtension : MarkupExtension
{
	public string? Key
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

	protected override object ProvideValue(IXamlServiceProvider serviceProvider)
	{
		if (Key is null)
			return new InvalidDataException("Key is null");
		Binding binding = new();
		binding.Path = new PropertyPath(nameof(LocalizationItem.Value));
		binding.Source = new LocalizationItem(Key, Type);
		binding.Mode = BindingMode.OneWay;
		return binding;
	}
}
