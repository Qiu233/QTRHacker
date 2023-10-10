using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
	public static readonly BooleanToVisibilityConverter Instance = new();
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is not bool v)
			throw new InvalidDataException();
		if (parameter is string o && o == "inversed")
			v = !v;
		return v ? Visibility.Visible : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		if (value is not Visibility v)
			throw new InvalidDataException();
		if (parameter is string o && o == "inversed")
			return v != Visibility.Visible;
		return v == Visibility.Visible;
	}
}
