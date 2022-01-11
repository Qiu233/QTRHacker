using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Res
{
	public static class GameResLoader
	{
		public const string File_Prefix = "QTRHacker.Res.Game.Prefix_en.txt";
		public const string File_Pet = "QTRHacker.Res.Game.Pet_en.txt";
		public const string File_Mount = "QTRHacker.Res.Game.Mount_en.txt";
		private static ImageList ItemImages { get; }
		public static ImageList BuffImages { get; }
		public static Dictionary<string, byte[]> ItemImageData { get; }
		public static Dictionary<string, byte[]> NPCImageData { get; }
		public static Dictionary<string, byte[]> BuffImageData { get; }
		public static Dictionary<string, byte[]> TileImageData { get; }
		public static Dictionary<string, byte[]> WallImageData { get; }
		public static string[] Prefixes { get; }
		public static string[] Pets { get; }
		public static string[] Mounts { get; }
		public static Dictionary<string, int> PrefixToID { get; }
		public static Dictionary<string, int> PetToID { get; }
		public static Dictionary<string, int> MountToID { get; }

		private static Dictionary<string, byte[]> LoadPackedImagesData(string res)
		{
			using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(res);
			return ResBinFileReader.ReadFromStream(s);
		}
		public static Image GetItemImage(int id)
		{
			string key = $"Item_{id}";
			if (ItemImages.Images.ContainsKey(key))
				return ItemImages.Images[key];
			else if (ItemImageData.TryGetValue(key, out byte[] value))
			{
				using var m = new MemoryStream(value);
				ItemImages.Images.Add(key, Image.FromStream(m));
				return ItemImages.Images[key];
			}
			return null;
		}
		static GameResLoader()
		{
			ItemImageData = LoadPackedImagesData("QTRHacker.Res.ContentImage.ItemImages.bin");
			ItemImages = new ImageList();
			ItemImages.ColorDepth = ColorDepth.Depth32Bit;
			ItemImages.ImageSize = new Size(20, 20);
			//(BuffImageData, BuffImages) = LoadPackedImages("QTRHacker.Res.ContentImage.BuffImages.bin");

			NPCImageData = LoadPackedImagesData("QTRHacker.Res.ContentImage.NPCImages.bin");
			TileImageData = LoadPackedImagesData("QTRHacker.Res.ContentImage.TileImages.bin");
			WallImageData = LoadPackedImagesData("QTRHacker.Res.ContentImage.WallImages.bin");

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(File_Prefix))
			{
				string[] t = new StreamReader(s).ReadToEnd().Split('\n');
				Prefixes = new string[t.Length];
				int p = 0;
				PrefixToID = new Dictionary<string, int>();
				foreach (var r in t)
				{
					string[] e = r.Split('=');
					int y = Convert.ToInt32(e[1]);
					string u = e[0];
					Prefixes[p++] = u;
					PrefixToID[u] = y;
				}
			}
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(File_Pet))
			{
				string[] t = new StreamReader(s).ReadToEnd().Split('\n');
				Pets = new string[t.Length];
				int p = 0;
				PetToID = new Dictionary<string, int>();
				foreach (var r in t)
				{
					string[] e = r.Split('=');
					int y = Convert.ToInt32(e[1]);
					string u = e[0];
					Pets[p++] = u;
					PetToID[u] = y;
				}
			}
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(File_Mount))
			{
				string[] t = new StreamReader(s).ReadToEnd().Split('\n');
				Mounts = new string[t.Length];
				int p = 0;
				MountToID = new Dictionary<string, int>();
				foreach (var r in t)
				{
					string[] e = r.Split('=');
					int y = Convert.ToInt32(e[1]);
					string u = e[0];
					Mounts[p++] = u;
					MountToID[u] = y;
				}
			}
		}
	}
}
