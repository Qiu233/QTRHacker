using QHackLib;
using QTRHacker.Functions;
using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QTRHacker.Tile;
using STile = QTRHacker.Functions.PatchesManager.STile;

namespace QTRHacker.PagePanels
{
	public class PagePanel_Sches : PagePanel
	{
		private readonly MButtonStrip ButtonStrip;
		private readonly TileView TileView;
		private bool Activated = false;

		private static STile[,] LoadTilesFromFile(string file)
		{
			var fs = File.Open(file, FileMode.Open);
			BinaryReader br = new BinaryReader(fs);
			int maxX = br.ReadInt32();
			int maxY = br.ReadInt32();
			var tiles = new STile[maxX, maxY];

			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					tiles[x, y] = new STile()
					{
						Type = br.ReadUInt16(),
						Wall = br.ReadByte(),
						Liquid = br.ReadByte(),
						BTileHeader = br.ReadByte(),
						BTileHeader2 = br.ReadByte(),
						BTileHeader3 = br.ReadByte(),
						FrameX = br.ReadInt16(),
						FrameY = br.ReadInt16(),
						STileHeader = br.ReadInt16()
					};
				}
			}
			fs.Close();
			return tiles;
		}

		private static byte[] SerializeTiles(STile[,] tiles)
		{
			int unitSize = Marshal.SizeOf(typeof(STile));
			int memorySize = 4 + 4 + tiles.GetLength(0) * tiles.GetLength(1) * unitSize;
			byte[] bs = new byte[memorySize];
			byte[] tmpS = new byte[unitSize];
			MemoryStream ms = new MemoryStream(bs);
			BinaryWriter bw = new BinaryWriter(ms);
			bw.Write(tiles.GetLength(0));
			bw.Write(tiles.GetLength(1));
			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int y = 0; y < tiles.GetLength(1); y++)
				{
					IntPtr ptr = Marshal.AllocHGlobal(unitSize);
					Marshal.StructureToPtr(tiles[x, y], ptr, false);
					Marshal.Copy(ptr, tmpS, 0, unitSize);
					Marshal.FreeHGlobal(ptr);
					bw.Write(tmpS);
				}
			}
			ms.Close();
			return bs;
		}

		public PagePanel_Sches(int Width, int Height) : base(Width, Height)
		{
			TileView = new TileView();
			TileView.BackColor = Color.FromArgb(40, 40, 40);
			TileView.Bounds = new Rectangle(0, 62, 300, 150);
			Controls.Add(TileView);

			Button arrowButton = new MButton();
			arrowButton.Enabled = false;
			arrowButton.Text = HackContext.CurrentLanguage["Arrow"];
			arrowButton.Bounds = new Rectangle(0, 30, 50, 30);
			arrowButton.Click += (s, e) =>
			{
				HackContext.GameContext.Patches.WorldPainter_BrushActive = false;
				HackContext.GameContext.Patches.WorldPainter_EyeDropperActive = false;
			};
			Controls.Add(arrowButton);

			Button dropperButton = new MButton();
			dropperButton.Enabled = false;
			dropperButton.Text = HackContext.CurrentLanguage["Dropper"];
			dropperButton.Bounds = new Rectangle(50, 30, 50, 30);
			dropperButton.Click += (s, e) =>
			{
				HackContext.GameContext.Patches.WorldPainter_BrushActive = false;
				HackContext.GameContext.Patches.WorldPainter_EyeDropperActive = true;
			};
			Controls.Add(dropperButton);

			Button brushButton = new MButton();
			brushButton.Enabled = false;
			brushButton.Text = HackContext.CurrentLanguage["Brush"];
			brushButton.Bounds = new Rectangle(100, 30, 50, 30);
			brushButton.Click += (s, e) =>
			{
				// because enabling the brush would perform heavy actions,
				// we check the state of brush at first.
				if (HackContext.GameContext.Patches.WorldPainter_BrushActive)
					return;
				HackContext.GameContext.Patches.WorldPainter_EyeDropperActive = false;
				HackContext.GameContext.Patches.WorldPainter_BrushActive = false;
				// with both brush and dropper disabled
				nuint addr = HackContext.GameContext.Patches.WorldPainter_BrushTiles;
				if (addr != 0) // reclaim the memory allocated before
				{
					HackContext.GameContext.Patches.WorldPainter_BrushWidth = 0;
					HackContext.GameContext.Patches.WorldPainter_BrushHeight = 0;
					System.Threading.Thread.Sleep(10);
					HackContext.GameContext.Patches.WorldPainter_BrushTiles = 0;
					QHackLib.Memory.MemoryAllocation.Free(HackContext.GameContext.HContext.Handle, addr);
				}
				System.Threading.Thread.Sleep(10);
				(int width, int height, var data) = GetDataFromGame();
				if (data.Length == 0) return;
				unsafe
				{
					addr = QHackLib.Memory.MemoryAllocation.Alloc(HackContext.GameContext.HContext.Handle, (uint)(data.Length * sizeof(STile) + 64));
				}
				HackContext.GameContext.HContext.DataAccess.Write(addr, data, (uint)data.Length);
				HackContext.GameContext.Patches.WorldPainter_BrushTiles = addr;
				HackContext.GameContext.Patches.WorldPainter_BrushWidth = width;
				HackContext.GameContext.Patches.WorldPainter_BrushHeight = height;
				HackContext.GameContext.Patches.WorldPainter_BrushActive = true; // with everthing's done, enable the brush
			};
			Controls.Add(brushButton);


			Button activateButton = new MButton();
			activateButton.Text = HackContext.CurrentLanguage["Activate"];
			activateButton.Bounds = new Rectangle(0, 0, 50, 30);
			activateButton.Click += (s, e) =>
			{
				arrowButton.Enabled = true;
				dropperButton.Enabled = true;
				brushButton.Enabled = true;
				HackContext.GameContext.Patches.Init();
				Activated = true;
			};
			Controls.Add(activateButton);

			ButtonStrip = new MButtonStrip(80, 30);
			ButtonStrip.Bounds = new Rectangle(215, 132, 80, 210);
			ButtonStrip.Enabled = false;
			//Controls.Add(ButtonStrip);

			System.Timers.Timer timer = new System.Timers.Timer(1000);
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		private static (int Width, int Height, STile[] Data) GetDataFromGame()
		{
			var tiles = HackContext.GameContext.Patches.WorldPainter_DropperTiles;
			int width = tiles.GetLength(0);
			int height = tiles.GetLength(1);
			var data = tiles.GetAllElements();
			return (width, height, data);
		}

		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (HackContext.GameContext == null || Activated == false)
				return;
			(int width, int height, var data) = GetDataFromGame();
			STile[,] targetData = new STile[width, height];
			for (int i = 0; i < width; i++)
				for (int j = 0; j < height; j++)
					targetData[i, j] = data[i * height + j];
			TileView.SetData(targetData);
		}
	}
}
