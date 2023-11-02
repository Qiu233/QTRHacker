using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using QTRHacker.AssetLoaders;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QTRHacker.Assets;

public static class GameImages
{
	private static readonly Dictionary<string, byte[]> ImageDatum = new();
	[ThreadStatic]
	private static Dictionary<string, BitmapImage>? Images;

	private static async Task LoadGameImages()
	{
		await LoadGameImage("Items");
		await LoadGameImage("NPCs");
		await LoadGameImage("Tiles");
		await LoadGameImage("Walls");
	}
	private static async Task LoadGameImage(string name)
	{
		byte[] data = await AssetReader.ReadData($"ms-appx:///Assets/GameImages/{name}.bin");
		var imgs = BinLoader.ReadBinFromStream(new MemoryStream(data, false));
		foreach (var img in imgs)
		{
			string key = $"{name}.{img.Key}";
			ImageDatum[key] = img.Value;
		}
	}
	public static async Task<BitmapImage> GetBitmapAsync(byte[] data)
	{
		var bitmapImage = new BitmapImage();
		using (var stream = new InMemoryRandomAccessStream())
		{
			using (var writer = new DataWriter(stream))
			{
				writer.WriteBytes(data);
				await writer.StoreAsync();
				await writer.FlushAsync();
				writer.DetachStream();
			}

			stream.Seek(0);
			bitmapImage.SetSource(stream);
		}

		return bitmapImage;
	}
	public static async Task<BitmapImage?> GetImage(string key)
	{
		if (!ImageDatum.Any())
			await LoadGameImages();
		Images ??= new();
		if (Images.TryGetValue(key, out BitmapImage? v))
			return v;
		if (!ImageDatum.TryGetValue(key, out byte[]? data))
			return null;
		return Images[key] = await GetBitmapAsync(data);
	}
	public static byte[]? GetImageData(string key)
	{
		if (ImageDatum.TryGetValue(key, out byte[]? v))
			return v;
		return null;
	}

	public static Task<BitmapImage?> GetItemImage(int type) => GetImage($"Items.Item_{type}");
	//public static BitmapImage GetNPCImage(int type) => GetImage($"NPCs.NPC_{type}");
	//public static BitmapImage GetTileImage(int type) => GetImage($"Tiles.Tiles_{type}");
	//public static BitmapImage GetWallImage(int type) => GetImage($"Walls.Wall_{type}");
	//public static byte[] GetItemImageData(int type) => GetImageData($"Items.Item_{type}");
	//public static byte[] GetNPCImageData(int type) => GetImageData($"NPCs.NPC_{type}");
}
