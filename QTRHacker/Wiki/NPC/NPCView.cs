using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Functions;
using QTRHacker.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsGraphicsDevice;

namespace QTRHacker.Wiki.NPC
{
	public class NPCView : GraphicsDeviceControl
	{
		public Dictionary<int, Texture2D> Frames
		{
			get;
		}
		public Dictionary<int, List<Microsoft.Xna.Framework.Rectangle>> FramesPlayList
		{
			get;
			set;
		}
		private int _NPCType;
		public int NPCType
		{
			get => _NPCType;
			set
			{
				_NPCType = value;
				State = 0;
			}
		}
		public int State
		{
			get;
			set;
		}
		public Timer StateTimer
		{
			get;
		}
		private SpriteBatch Batch
		{
			get;
			set;
		}
		private int __timer = 0;
		public NPCView()
		{
			State = 0;
			StateTimer = new Timer();
			StateTimer.Interval = 80;
			StateTimer.Tick += StateTimer_Tick;

			Frames = new Dictionary<int, Texture2D>();
			FramesPlayList = new Dictionary<int, List<Microsoft.Xna.Framework.Rectangle>>();
		}


		private void StateTimer_Tick(object sender, EventArgs e)
		{
			Invalidate();
			State++;
			if (State >= FramesPlayList[NPCType].Count)
				State = 0;
		}

		private void OnIdle(object sender, EventArgs e)
		{
			if (__timer >= 1000000)
				__timer = 0;
			if (__timer % 10 == 0)
				Invalidate();
			__timer++;
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Application.Idle -= OnIdle;
		}

		protected override void Initialize()
		{
			Batch = new SpriteBatch(GraphicsDevice);

			/*for (int i = 0; i < NPC.NPCTabPage.NPCName_en.Count; i++)
			{
				using (var s = new MemoryStream(GameResLoader.NPCImageData[$"NPC_{i}"]))
				{
					Frames[i] = Texture2D.FromStream(GraphicsDevice, s);
				}
				FramesPlayList[i] = new List<Microsoft.Xna.Framework.Rectangle>();
				int fs = GameConstants.NPCFrameCount[i];
				int height = (Frames[i].Height) / fs;
				for (int j = 0; j < fs; j++)
				{
					FramesPlayList[i].Add(new Microsoft.Xna.Framework.Rectangle(0, j * height + 1, Frames[i].Width, height - 2));
				}
			}

			Application.Idle += OnIdle;*/
			State = 0;
			StateTimer.Start();
		}

		protected override void Draw()
		{
			GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(255, 255, 255));
			Batch.Begin();
			var dest = new Microsoft.Xna.Framework.Rectangle();
			var src = FramesPlayList[NPCType][State];
			if (src.Width - Width >= src.Height - Height)
			{
				dest.X = 10;
				dest.Width = Width - 20;
				float scale = (float)dest.Width / src.Width;
				dest.Height = (int)(src.Height * scale);
				dest.Y = Height / 2 - dest.Height / 2;
			}
			else
			{
				dest.Y = 10;
				dest.Height = Height - 20;
				float scale = (float)dest.Height / src.Height;
				dest.Width = (int)(src.Width * scale);
				dest.X = Width / 2 - dest.Width / 2;
			}
			var color = NPCTabPage.NPCInfo[NPCType]["Color"];
			var rcolor = new Microsoft.Xna.Framework.Color(color["R"].ToObject<int>(), color["G"].ToObject<int>(), color["B"].ToObject<int>(), color["A"].ToObject<int>());
			if (rcolor.A == 0)
				Batch.Draw(Frames[NPCType], dest, FramesPlayList[NPCType][State], Microsoft.Xna.Framework.Color.White);
			else
				Batch.Draw(Frames[NPCType], dest, FramesPlayList[NPCType][State], rcolor);
			Batch.End();
		}
	}
}
