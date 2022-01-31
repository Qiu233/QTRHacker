using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace QTRHacker.Converters
{
	[ValueConversion(sourceType: typeof(bool), targetType: typeof(Visibility), ParameterType = typeof(bool))]
	internal class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool v)
			{
				if (v) return Visibility.Visible;
				else if ((bool)parameter) return Visibility.Hidden;
				else return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility v)
			{
				switch (v)
				{
					case Visibility.Visible: return true;
					case Visibility.Collapsed: return false;
					case Visibility.Hidden: return (bool)parameter;
				}
			}
			return false;
		}
	}
}
