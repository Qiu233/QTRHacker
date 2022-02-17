using QTRHacker.AssetLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace QTRHacker.Assets
{
	public static class GameImages
	{
		private static readonly Dictionary<string, BitmapImage> ImagesCache = new();
		static GameImages()
		{
			LoadGameImages();
		}
		public static void Touch()
		{

		}

		private static void LoadGameImages()
		{
			LoadGameImage("Items");
		}
		private static void LoadGameImage(string name)
		{
			using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/GameImages/{name}.bin", UriKind.Absolute)).Stream;
			var imgs = BinLoader.ReadBinFromStream(s);
			foreach (var img in imgs)
			{
				BitmapImage bmp = new();
				bmp.CacheOption = BitmapCacheOption.OnDemand;
				bmp.BeginInit();
				bmp.StreamSource = new MemoryStream(img.Value);
				bmp.EndInit();
				ImagesCache[$"{name}.{img.Key}"] = bmp;
			}
		}
		public static BitmapImage GetImage(string key)
		{
			if (ImagesCache.TryGetValue(key, out BitmapImage v))
				return v;
			return null;
		}

		public static BitmapImage GetItemImage(int type)
		{
			return GetImage($"Items.Item_{type}");
		}
	}
}
