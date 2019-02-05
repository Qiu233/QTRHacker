using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Res
{
	public class GameResLoader
	{
		public static ImageList ItemImages { get; }
		public static string[] Prefixes { get; }
		public static string[] Items { get; }
		public static string[] Pets { get; }
		public static string[] Mounts { get; }
		public static Dictionary<string, int> PrefixToID { get; }
		public static Dictionary<int, string> IDToItem { get; }
		public static Dictionary<string, int> PetToID { get; }
		public static Dictionary<string, int> MountToID { get; }

		public static Dictionary<string, byte[]> ItemImageData { get; }
		static GameResLoader()
		{
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.ItemImage.bin"))
			{
				ItemImageData = ResBinFileReader.ReadFromStream(s);
				ItemImages = new ImageList();
				foreach (var data in ItemImageData)
				{
					int i = Convert.ToInt32(data.Key.Substring(data.Key.LastIndexOf('_') + 1));
					using (var m = new MemoryStream(data.Value))
					{
						ItemImages.Images.Add(i.ToString(), Image.FromStream(m));
					}
				}
				ItemImages.ColorDepth = ColorDepth.Depth32Bit;
				ItemImages.ImageSize = new Size(20, 20);
			}
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.Prefix.txt"))
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
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.ItemID.txt"))
			{
				string[] t = new StreamReader(s).ReadToEnd().Split('\n');
				Items = new string[t.Length];
				int p = 0;
				IDToItem = new Dictionary<int, string>();
				foreach (var r in t)
				{
					string[] e = r.Split('=');
					int y = Convert.ToInt32(e[1]);
					string u = e[0];
					Items[p++] = u;
					IDToItem[y] = u;
				}
			}
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.Pet.txt"))
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
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Game.Mount.txt"))
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
