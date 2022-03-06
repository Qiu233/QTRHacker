using QTRHacker.Commands;
using QTRHacker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.Advanced.Schematics
{
	public class ScheWindowViewModel : ViewModelBase
	{
		private PatchesManager.STile[,] tiles;
		private readonly HackCommand enableEyeDropperCommand;
		private readonly HackCommand enableBrushCommand;
		private readonly HackCommand enableArrowCommand;

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

		public ScheWindowViewModel()
		{
			HackGlobal.GameContext.Patches.Init();

			enableArrowCommand = new HackCommand(o => 
			{
				HackGlobal.GameContext.Patches.WorldPainter_BrushActive = false;
				HackGlobal.GameContext.Patches.WorldPainter_EyeDropperActive = false;
			});
			enableEyeDropperCommand = new HackCommand(o =>
			{
				HackGlobal.GameContext.Patches.WorldPainter_BrushActive = false;
				HackGlobal.GameContext.Patches.WorldPainter_EyeDropperActive = true;
			});
			enableBrushCommand = new HackCommand(o =>
			{
				HackGlobal.GameContext.Patches.WorldPainter_EyeDropperActive = false;
				HackGlobal.GameContext.Patches.WorldPainter_BrushActive = true;
			});


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
