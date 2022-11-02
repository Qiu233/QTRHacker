using System.Globalization;
using System.Windows.Data;

namespace QTRHacker.Converters;

internal class EnumConverter : IValueConverter
{
	public static readonly EnumConverter Instance = new();
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return Enum.Parse(parameter as Type, value.ToString());
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return System.Convert.ChangeType(value, targetType);
	}
}
