using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Terraria.ID;

namespace GameWikiResExporter;

internal static class ImageExporter
{
	// XNA ContentManager/SaveAsPng approach adapted from Athari/XnaConvert (see THIRD-PARTY-NOTICES.md).
	public static void Export(string imagesDirectory, string outputDirectory)
	{
		using var window = new Form { ShowInTaskbar = false };
		using var graphics = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef,
			new PresentationParameters
			{
				IsFullScreen = false, DeviceWindowHandle = window.Handle,
				BackBufferWidth = 1, BackBufferHeight = 1
			});
		var services = new GameServiceContainer();
		services.AddService(typeof(IGraphicsDeviceService), new GraphicsService(graphics));
		using var content = new ContentManager(services, imagesDirectory);
		ExportImages(content, imagesDirectory, Path.Combine(outputDirectory, "Items.bin"), "Item", ItemID.Count);
		ExportImages(content, imagesDirectory, Path.Combine(outputDirectory, "NPCs.bin"), "NPC", NPCID.Count);
	}

	private static void ExportImages(ContentManager content, string directory, string path, string prefix, int count)
	{
		var images = new SortedDictionary<string, string>(StringComparer.Ordinal);
		foreach (string file in Directory.GetFiles(directory, prefix + "_*.xnb"))
			images.Add(Path.GetFileNameWithoutExtension(file), Path.GetFileNameWithoutExtension(file));
		for (int id = 0; id < count; id++)
			images[prefix + "_" + id] = prefix + "_" + (prefix == "Item" ? GetItemTextureId(id) : id);

		using var writer = new BinaryWriter(File.Create(path));
		int exported = 0;
		foreach (var image in images)
		{
			string name = image.Key;
			string sourceName = image.Value;
			if (!File.Exists(Path.Combine(directory, sourceName + ".xnb")))
				throw new FileNotFoundException($"Missing texture for {name}.", Path.Combine(directory, sourceName + ".xnb"));
			try
			{
				Texture2D texture = content.Load<Texture2D>(sourceName);
				using var png = new MemoryStream();
				texture.SaveAsPng(png, texture.Width, texture.Height);
				writer.Write(name);
				writer.Write(png.Length);
				png.Position = 0;
				png.CopyTo(writer.BaseStream);
			}
			catch (Exception exception)
			{
				throw new InvalidDataException($"Failed to export {sourceName}.xnb.", exception);
			}
			finally
			{
				content.Unload();
			}
			exported++;
			if (exported % 500 == 0 || exported == images.Count)
				Console.WriteLine($"{prefix} images: {exported}/{images.Count}");
		}
	}

	private static int GetItemTextureId(int id)
	{
		// TextureCopyLoad can contain chains; resolve them to the original texture.
		var visited = new HashSet<int>();
		while (id >= 0 && id < ItemID.Count && visited.Add(id))
		{
			int source = ItemID.Sets.TextureCopyLoad[id];
			if (source < 0)
				return id;
			id = source;
		}
		throw new InvalidDataException($"Invalid or cyclic item texture alias at ID {id}.");
	}

	private sealed class GraphicsService : IGraphicsDeviceService
	{
		public GraphicsDevice GraphicsDevice { get; }
		public GraphicsService(GraphicsDevice device) => GraphicsDevice = device;
		public event EventHandler<EventArgs> DeviceCreated { add { } remove { } }
		public event EventHandler<EventArgs> DeviceDisposing { add { } remove { } }
		public event EventHandler<EventArgs> DeviceReset { add { } remove { } }
		public event EventHandler<EventArgs> DeviceResetting { add { } remove { } }
	}
}
