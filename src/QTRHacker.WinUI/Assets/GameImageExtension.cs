using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using QTRHacker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Assets;

internal class GameImageExtension : MarkupExtension
{
	public string? Key { get; set; }
	public GameImageExtension(string key) => Key = key;
	public GameImageExtension() { }
	protected override object ProvideValue(IXamlServiceProvider serviceProvider)
	{
		if (Key is null)
			throw new InvalidDataException("Key is null");
		//var result = GameImages.GetImage(Key).Result;
		//if (result is null)
		//	throw new InvalidDataException("Key not found: " + Key);
		var tcn = new TaskCompletionNotifier<BitmapImage?>(GameImages.GetImage(Key));
		return new Binding
		{
			Source = tcn,
			Path = new PropertyPath(nameof(TaskCompletionNotifier<BitmapImage?>.Result))
		};
	}
}
internal class GameImageItemExtension : MarkupExtension
{
	public int Type { get; set; }
	public GameImageItemExtension(int type) => Type = type;
	public GameImageItemExtension() { }
	protected override object ProvideValue(IXamlServiceProvider serviceProvider)
	{
		//var result = GameImages.GetImage($"Items.Item_{Type}").Result;
		//if (result is null)
		//	throw new InvalidDataException("Key not found: " + Type);
		//return result;
		var tcn = new TaskCompletionNotifier<BitmapImage?>(GameImages.GetImage($"Items.Item_{Type}"));
		return new Binding
		{
			Source = tcn,
			Path = new PropertyPath(nameof(TaskCompletionNotifier<BitmapImage?>.Result))
		};
	}
}