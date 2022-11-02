using System.Globalization;
using System.Windows.Data;

namespace QTRHacker.Converters;

internal class EqualityConverter : IValueConverter
{
	public static readonly EqualityConverter Instance = new();
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value.Equals(parameter);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
