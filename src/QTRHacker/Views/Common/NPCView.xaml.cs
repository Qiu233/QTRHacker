using QTRHacker.Assets;
using QTRHacker.Core;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QTRHacker.Views.Common
{
	/// <summary>
	/// NPCView.xaml 的交互逻辑
	/// </summary>
	public partial class NPCView : UserControl
	{
		private int frame;
		public DispatcherTimer Timer { get; }
		public NPCView()
		{
			InitializeComponent();
			Timer = new(DispatcherPriority.Render);
			Timer.Interval = TimeSpan.FromMilliseconds(100);
			Timer.Tick += Timer_Tick;
			Timer.Start();
		}
		private void Timer_Tick(object sender, EventArgs e)
		{
			Draw();
			frame++;
		}

		private void Draw()
		{
			ContentImage.Source = CurrentImage;
		}

		private void Redraw()
		{
			frame = 0;
			Draw();
		}

		~NPCView()
		{
			Timer?.Stop();
		}
		public int NPCType
		{
			get => (int)GetValue(NPCTypeProperty);
			set => SetValue(NPCTypeProperty, value);
		}

		public static readonly DependencyProperty NPCTypeProperty =
			DependencyProperty.Register(nameof(NPCType), typeof(int), typeof(NPCView),
				new PropertyMetadata(0, OnNPCTypeChanged));

		private readonly Dictionary<int, List<ImageSource>> Frames = new();


		public Color Color
		{
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}
		public static readonly DependencyProperty ColorProperty =
			DependencyProperty.Register(nameof(Color), typeof(Color), typeof(NPCView),
				new PropertyMetadata(Colors.White));


		private static void OnNPCTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			if (o is NPCView view)
			{
				int type = (int)args.NewValue;
				view.LoadImage(type);
				view.frame = 0;
				var color = WikiResLoader.NPCDatum[type].Color;
				var rcolor = Color.FromArgb(color.A, color.R, color.G, color.B);
				if (rcolor.A == 0)
					view.Color = Colors.White;
				else
					view.Color = rcolor;
				view.Redraw();
			}
		}

		public ImageSource CurrentImage
		{
			get
			{
				if (!Frames.TryGetValue(NPCType, out var coll))
					return null;
				if (coll.Count == 0)
					return null;
				if (coll.Count <= frame)
					frame = 0;
				return coll[frame];
			}
		}

		private void LoadImage(int type)
		{
			if (Frames.ContainsKey(type))
				return;
			var imgData = GameImages.GetNPCImageData(type);
			if (imgData == null)
				return;
			using var s = new MemoryStream(imgData);

			var bi = new BitmapImage();
			bi.BeginInit();
			bi.CacheOption = BitmapCacheOption.OnLoad;
			bi.StreamSource = s;
			bi.EndInit();

			var list = new List<ImageSource>();
			Frames[type] = list;
			int fs = GameConstants.NPCFrameCount[type];
			int height = (bi.PixelHeight) / fs;
			int max = 20;
			for (int j = 0; j < fs; j++)
			{
				s.Position = 0;
				var img = new BitmapImage();
				img.BeginInit();
				img.CacheOption = BitmapCacheOption.OnLoad;
				img.StreamSource = s;
				img.SourceRect = new Int32Rect(0, j * height + 1, bi.PixelWidth, height - 2);
				img.EndInit();
				double scale = Math.Min((double)max / img.PixelWidth, (double)max / img.PixelHeight);
				//list.Add(new TransformedBitmap(img, new ScaleTransform(scale, scale)));
				list.Add(img);
			}
		}
	}
}
