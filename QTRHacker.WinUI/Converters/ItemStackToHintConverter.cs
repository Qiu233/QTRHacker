using Microsoft.UI.Xaml.Data;
using System.Globalization;

namespace QTRHacker.Converters;

internal class ItemStackToHintConverter : IValueConverter
{
	public static readonly ItemStackToHintConverter Instance = new();
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is int v)
		{
			return v switch
			{
				0 or 1 => string.Empty,
				_ => v.ToString()
			};
		}
		return string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotImplementedException();
	}
}
