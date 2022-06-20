using QTRHacker.Controls;
using QTRHacker.Core.ProjectileImage;
using QTRHacker.Core.ProjectileImage.RainbowImage;
using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace QTRHacker.ViewModels.Advanced.RainbowFonts
{
	public class RainbowFonts : AdvancedFunction
	{
		public override void ApplyLocalization(string culture)
		{
			Name = culture switch
			{
				"zh" => "彩虹字",
				_ => "Rainbow Fonts",
			};
		}

		public override void Run()
		{
			HackGlobal.LoadRainbowFonts();
			if (!ShowWindow(out string s))
				return;
			RainbowTextDrawer rtd = new(HackGlobal.Characters);
			rtd.DrawString(s, center: new MPointF());
			var ctx = HackGlobal.GameContext;
			var pos = ctx.MyPlayer.Position;
			rtd.Emit(ctx, new MPointF(pos.X, pos.Y));
		}
		private static bool ShowWindow(out string s)
		{
			MWindow window = new();
			window.SizeToContent = SizeToContent.WidthAndHeight;
			window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			window.Title = "Rainbow Fonts";
			window.MinimizeBox = false;
			Grid grid = new();
			window.Content = grid;

			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

			Label tip = new();
			tip.Foreground = new SolidColorBrush(Colors.White);
			tip.Content = "Text:";
			grid.Children.Add(tip);
			Grid.SetColumn(tip, 0);

			TextBox box = new();
			box.Width = 160;
			box.VerticalContentAlignment = VerticalAlignment.Center;
			box.Background = new SolidColorBrush(Color.FromArgb(20, 255, 255, 255));
			box.Foreground = new SolidColorBrush(Colors.White);
			grid.Children.Add(box);
			Grid.SetColumn(box, 1);

			Button btn = new();
			btn.Foreground = new SolidColorBrush(Colors.White);
			btn.Padding = new Thickness(2);
			btn.Content = LocalizationManager.Instance.GetValue("UI.Confirm");
			grid.Children.Add(btn);
			Grid.SetColumn(btn, 2);

			btn.Click += (s, e) =>
			{
				window.DialogResult = true;
				window.Close();
			};

			var result = window.ShowDialog();
			s = box.Text;
			return result == true;
		}
	}
}
