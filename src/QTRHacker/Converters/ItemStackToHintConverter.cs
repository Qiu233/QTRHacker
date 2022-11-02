using System.Globalization;
using System.Windows.Data;

namespace QTRHacker.Converters;

[ValueConversion(sourceType: typeof(int), targetType: typeof(string))]
internal class ItemStackToHintConverter : IValueConverter
{
	public static readonly ItemStackToHintConverter Instance = new();
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
