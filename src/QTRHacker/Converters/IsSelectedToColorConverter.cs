using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace QTRHacker.Converters
{
	internal class IsSelectedToColorConverter : IValueConverter
	{
		public static readonly IsSelectedToColorConverter Instance = new();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool b)
			{
				return b ? parameter : Colors.Transparent;
			}
			return Colors.Transparent;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
