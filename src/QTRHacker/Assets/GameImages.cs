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
		private static readonly Dictionary<string, BitmapImage> Images = new();
		private static readonly Dictionary<string, byte[]> ImageDatum = new();
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
			LoadGameImage("NPCs");
			LoadGameImage("Tiles");
			LoadGameImage("Walls");
		}
		private static void LoadGameImage(string name)
		{
			using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/GameImages/{name}.bin", UriKind.Absolute)).Stream;
			var imgs = BinLoader.ReadBinFromStream(s);
			foreach (var img in imgs)
			{
				string key = $"{name}.{img.Key}";
				var data = ImageDatum[key] = new byte[img.Value.Length];
				Array.Copy(img.Value, data, img.Value.Length);
				BitmapImage bmp = new();
				bmp.CacheOption = BitmapCacheOption.OnDemand;
				bmp.BeginInit();
				bmp.StreamSource = new MemoryStream(img.Value);
				bmp.EndInit();
				Images[key] = bmp;
			}
		}
		public static BitmapImage GetImage(string key)
		{
			if (Images.TryGetValue(key, out BitmapImage v))
				return v;
			return null;
		}
		public static byte[] GetImageData(string key)
		{
			if (ImageDatum.TryGetValue(key, out byte[] v))
				return v;
			return null;
		}

		public static BitmapImage GetItemImage(int type) => GetImage($"Items.Item_{type}");
		public static BitmapImage GetNPCImage(int type) => GetImage($"NPCs.NPC_{type}");
		public static BitmapImage GetTileImage(int type) => GetImage($"Tiles.Tiles_{type}");
		public static BitmapImage GetWallImage(int type) => GetImage($"Walls.Wall_{type}");
		public static byte[] GetItemImageData(int type) => GetImageData($"Items.Item_{type}");
		public static byte[] GetNPCImageData(int type) => GetImageData($"NPCs.NPC_{type}");
	}
}
