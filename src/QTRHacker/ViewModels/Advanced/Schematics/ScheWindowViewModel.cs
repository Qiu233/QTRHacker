using Microsoft.Win32;
using QTRHacker.Commands;
using QTRHacker.Core;
using QTRHacker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.Advanced.Schematics
{
	public class ScheWindowViewModel : ViewModelBase
	{
		private const string DIR = "./Content/Sches";
		private PatchesManager.STile[,] tiles;
		private readonly HackCommand enableEyeDropperCommand;
		private readonly HackCommand enableBrushCommand;
		private readonly HackCommand enableArrowCommand;
		private readonly HackCommand saveCommand;
		private readonly HackCommand loadCommand;
		private readonly HackCommand flipH;
		private readonly HackCommand flipV;

		private readonly object _UpdateLock = new();

		public PatchesManager.STile[,] Tiles
		{
			get => tiles;
			set
			{
				tiles = value;
				OnPropertyChanged(nameof(Tiles));
			}
		}

		public DispatcherTimer UpdateTimer { get; }

		public HackCommand EnableArrowCommand => enableArrowCommand;
		public HackCommand EnableEyeDropperCommand => enableEyeDropperCommand;
		public HackCommand EnableBrushCommand => enableBrushCommand;
		public HackCommand SaveCommand => saveCommand;
		public HackCommand LoadCommand => loadCommand;
		public HackCommand FlipH => flipH;
		public HackCommand FlipV => flipV;

		public static void SelectArrow()
		{
			HackGlobal.GameContext.Patches.WorldPainter_BrushActive = false;
			HackGlobal.GameContext.Patches.WorldPainter_EyeDropperActive = false;
		}
		public static void SelectEyeDropper()
		{
			HackGlobal.GameContext.Patches.WorldPainter_BrushActive = false;
			HackGlobal.GameContext.Patches.WorldPainter_EyeDropperActive = true;
		}
		public static void SelectBrush()
		{
			HackGlobal.GameContext.Patches.WorldPainter_EyeDropperActive = false;
			HackGlobal.GameContext.Patches.WorldPainter_BrushActive = true;
		}
		public static void Save(PatchesManager.STile[,] tiles)
		{
			SaveFileDialog dialog = new();
			dialog.Filter = "Schematics files (*.sche)|*.sche";
			dialog.InitialDirectory = Path.GetFullPath(DIR);
			if (dialog.ShowDialog() == true)
			{
				SchematicsData data = new(tiles);
				using var s = dialog.OpenFile();
				data.Save(s);
			}
		}

		public static void Load()
		{
			OpenFileDialog dialog = new();
			dialog.Filter = "Schematics files (*.sche)|*.sche";
			dialog.InitialDirectory = Path.GetFullPath(DIR);
			if (dialog.ShowDialog() == true)
			{
				using var s = dialog.OpenFile();
				var sche = SchematicsData.Load(s);
				LoadData(sche.Tiles);
			}
		}

		public static void LoadData(PatchesManager.STile[,] tiles)
		{
			MemoryStream ms = new();
			BinaryWriter bw = new(ms);
			int width = tiles.GetLength(0);
			int height = tiles.GetLength(1);
			bw.Write(width);
			bw.Write(height);
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					bw.Write(QHackLib.Memory.DataHelper.GetBytes(tiles[x, y]));
			byte[] data = ms.ToArray();
			nuint addr = QHackLib.Memory.MemoryAllocation.Alloc(HackGlobal.GameContext.HContext.Handle, (uint)data.Length);
			HackGlobal.GameContext.HContext.DataAccess.WriteBytes(addr, data);
			HackGlobal.GameContext.Patches.WorldPainter_Buffer = addr;
			HackGlobal.GameContext.Patches.WorldPainter_Loading = true;
		}

		public void FlipVertically()
		{
			lock (_UpdateLock)
			{
				int width = Tiles.GetLength(0);
				int height = Tiles.GetLength(1);
				PatchesManager.STile[,] data = new PatchesManager.STile[width, height];
				for (int j = 0; j < height; j++)
					for (int i = 0; i < width; i++)
						data[i, j] = Tiles[i, height - j - 1];
				LoadData(data);
			}
		}
		public void FlipHorizontally()
		{
			lock (_UpdateLock)
			{
				int width = Tiles.GetLength(0);
				int height = Tiles.GetLength(1);
				PatchesManager.STile[,] data = new PatchesManager.STile[width, height];
				for (int i = 0; i < width; i++)
					for (int j = 0; j < height; j++)
						data[i, j] = Tiles[width - i - 1, j];
				LoadData(data);
			}
		}

		public ScheWindowViewModel()
		{
			if (!Directory.Exists(DIR))
				Directory.CreateDirectory(DIR);
			HackGlobal.GameContext.Patches.Init();

			enableArrowCommand = new HackCommand(o => SelectArrow());
			enableEyeDropperCommand = new HackCommand(o => SelectEyeDropper());
			enableBrushCommand = new HackCommand(o => SelectBrush());
			saveCommand = new HackCommand(o =>
			{
				lock (_UpdateLock)
					Save(Tiles);
			});
			loadCommand = new HackCommand(o => Load());
			flipH = new HackCommand(o => FlipHorizontally());
			flipV = new HackCommand(o => FlipVertically());

			UpdateTimer = new();
			UpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.SchesUpdateInterval);
			WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(UpdateTimer, nameof(DispatcherTimer.Tick), UpdateTimer_Tick);
			//Still need to stop the timer, but so far I've found no clean way to do so.
			UpdateTimer.Start();
		}

		private static (int Width, int Height, PatchesManager.STile[] Data) GetDataFromGame()
		{
			var tiles = HackGlobal.GameContext.Patches.WorldPainter_ClipBoard;
			int width = tiles.GetLength(0);
			int height = tiles.GetLength(1);
			var data = tiles.GetAllElements();
			return (width, height, data);
		}

		private void UpdateTimer_Tick(object sender, EventArgs e)
		{
			lock (_UpdateLock)
			{
				if (!HackGlobal.IsActive || !HackGlobal.GameContext.Patches.IsInitialized)
					return;
				(int width, int height, var data) = GetDataFromGame();
				PatchesManager.STile[,] targetData = new PatchesManager.STile[width, height];
				for (int i = 0; i < width; i++)
					for (int j = 0; j < height; j++)
						targetData[i, j] = data[i * height + j];
				Tiles = targetData;
			}
		}
	}
}
