using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QTRHacker.Converters;

[ValueConversion(typeof(bool), typeof(Visibility), ParameterType = typeof(InvertableBooleanToVisibilityConverter.Mode))]
public class InvertableBooleanToVisibilityConverter : IValueConverter
{
	public static readonly InvertableBooleanToVisibilityConverter Instance = new();
	enum Mode
	{
		Normal, Inverted
	}

	public object Convert(object value, Type targetType,
						  object parameter, CultureInfo culture)
	{
		var boolValue = (bool)value;
		var direction = parameter is not null ? (Mode)Enum.Parse(typeof(Mode), (string)parameter) : Mode.Normal;

		if (direction == Mode.Inverted)
			return !boolValue ? Visibility.Visible : Visibility.Collapsed;

		return boolValue ? Visibility.Visible : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType,
		object parameter, CultureInfo culture)
	{
		return null;
	}
}
