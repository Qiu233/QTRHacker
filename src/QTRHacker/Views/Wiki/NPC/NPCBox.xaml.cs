using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Assets;
using QTRHacker.Core;
using QTRHacker.EventManagers;
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

namespace QTRHacker.Views.Wiki.NPC
{
	/// <summary>
	/// NPCBox.xaml 的交互逻辑
	/// </summary>
	public partial class NPCBox : UserControl, IWeakEventListener
	{
		private readonly Dictionary<int, Texture2D> NPCTextures = new();
		private readonly Dictionary<int, List<Microsoft.Xna.Framework.Rectangle>> FramesPlayList = new();
		public int Frame
		{
			get => (int)GetValue(FrameProperty);
			set => SetValue(FrameProperty, value);
		}

		public static readonly DependencyProperty FrameProperty =
			DependencyProperty.Register(nameof(Frame), typeof(int), typeof(NPCBox));

		public int NPCType
		{
			get => (int)GetValue(NPCTypeProperty);
			set => SetValue(NPCTypeProperty, value);
		}

		public static readonly DependencyProperty NPCTypeProperty =
			DependencyProperty.Register(nameof(NPCType), typeof(int), typeof(NPCBox),
				new PropertyMetadata(0, OnNPCTypeChanged));

		private SpriteBatch batch;

		public SpriteBatch Batch => batch;

		private Texture2D Texture;
		private List<Microsoft.Xna.Framework.Rectangle> Frames;

		public NPCBox()
		{
			InitializeComponent();
			XnaControl.Initialize += NPCView_Initialize;
			XnaControl.Update += NPCView_Update;
			XnaControl.Draw += NPCView_Draw;
		}

		private static void OnNPCTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			if (o is NPCBox view)
				view.LoadNPCTexture(view.NPCType);
		}
		private void NPCView_Initialize()
		{
			batch = new SpriteBatch(XnaControl.GraphicsDevice);
			LoadNPCTexture(NPCType);
			RenderingEventManager.AddListener(this);
			DispatcherTimer timer = new();
			timer.Interval = TimeSpan.FromMilliseconds(100);
			timer.Tick += (s, e) =>
			{
				if (++Frame >= Frames.Count)
					Frame = 0;
			};
			timer.Start();
		}

		private void NPCView_Update(Microsoft.Xna.Framework.GameTime obj)
		{
			if (Frames == null)
			{
				LoadNPCTexture(NPCType);
			}
		}

		private void LoadNPCTexture(int npcType)
		{
			if (!XnaControl.IsXNAInitialized)
				return;
			if (NPCTextures.TryGetValue(npcType, out Texture2D t))
			{
				Texture = t;
				Frames = FramesPlayList[npcType];
				return;
			}
			var imgData = GameImages.GetNPCImageData(npcType);
			if (imgData != null)
			{
				using var s = new MemoryStream(imgData);
				Texture = NPCTextures[npcType] = Texture2D.FromStream(XnaControl.GraphicsDevice, s);
			}
			Frames = FramesPlayList[npcType] = new List<Microsoft.Xna.Framework.Rectangle>();
			int fs = GameConstants.NPCFrameCount[npcType];
			int height = (NPCTextures[npcType].Height) / fs;
			for (int j = 0; j < fs; j++)
				Frames.Add(new Microsoft.Xna.Framework.Rectangle(0, j * height + 1, NPCTextures[npcType].Width, height - 2));
		}

		private void NPCView_Draw()
		{
			if (Frame >= Frames.Count)
				Frame = 0;
			XnaControl.GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(0xC8, 0xC8, 0xC8));
			Batch.Begin();
			var src = Frames[Frame];
			var dest = new Microsoft.Xna.Framework.Rectangle();
			if (src.Width - ActualWidth >= src.Height - ActualHeight)
			{
				dest.X = 10;
				dest.Width = (int)(ActualWidth - 20);
				float scale = (float)dest.Width / src.Width;
				dest.Height = (int)(src.Height * scale);
				dest.Y = (int)(ActualHeight / 2 - dest.Height / 2);
			}
			else
			{
				dest.Y = 10;
				dest.Height = (int)(ActualHeight - 20);
				float scale = (float)dest.Height / src.Height;
				dest.Width = (int)(src.Width * scale);
				dest.X = (int)(ActualWidth / 2 - dest.Width / 2);
			}
			var color = WikiResLoader.NPCDatum[NPCType].Color;
			var rcolor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
			if (rcolor.A == 0)
				Batch.Draw(Texture, dest, src, Microsoft.Xna.Framework.Color.White);
			else
				Batch.Draw(Texture, dest, src, rcolor);
			Batch.End();
		}

		public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			if (managerType == typeof(RenderingEventManager))
			{
				XnaControl.Render();
				return true;
			}
			return false;
		}
	}
}
