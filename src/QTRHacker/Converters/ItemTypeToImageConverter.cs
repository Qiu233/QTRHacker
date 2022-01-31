using QTRHacker.Assets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QTRHacker.Converters
{
	[ValueConversion(sourceType: typeof(int), targetType: typeof(BitmapImage))]
	internal class ItemTypeToImageConverter : IValueConverter
	{
		public static readonly ItemTypeToImageConverter Instance = new();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))// design time
				return null;
			if (value is int v)
			{
				if (v == 0) return null;
				return AssetsLoader.GetItemImage(v);
			}
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
