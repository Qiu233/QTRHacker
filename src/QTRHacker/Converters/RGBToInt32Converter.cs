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
	internal class RGBToInt32Converter : IValueConverter
	{
		public static readonly RGBToInt32Converter Instance = new();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Color color)
				return color.R << 16 | color.G << 8 | color.B | unchecked((int)0xFF_000000);
			return unchecked((int)0xFF_000000);//black
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is int v)
			{
				byte r = (byte)(v >> 16);
				byte g = (byte)(v >> 8);
				byte b = (byte)(v);
				return Color.FromRgb(r, g, b);
			}
			return Colors.Black;
		}
	}
}
