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
		} = new Dictionary<int, Texture2D>();
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
		public System.Timers.Timer StateTimer
		{
			get;
		}
		private SpriteBatch Batch;
		public NPCView()
		{
			State = 0;
			StateTimer = new System.Timers.Timer(100);
			StateTimer.Elapsed += StateTimer_Tick;

			FramesPlayList = new Dictionary<int, List<Microsoft.Xna.Framework.Rectangle>>();
		}


		private void StateTimer_Tick(object sender, EventArgs e)
		{
			Invalidate();
			State++;
			if (State >= GameConstants.NPCFrameCount[NPCType])
				State = 0;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!disposing)
				return;
			foreach (var texture in Frames.Values)
				texture?.Dispose();
		}

		protected override void Initialize()
		{
			Batch = new SpriteBatch(GraphicsDevice);

			State = 0;
			StateTimer.Start();
		}

		private Texture2D GetNPCTexture(int npcType)
		{
			if (Frames.TryGetValue(npcType, out Texture2D t))
				return t;
			if (GameResLoader.NPCImageData.TryGetValue($"NPC_{npcType}", out byte[] value))
			{
				using var s = new MemoryStream(value);
				Frames[npcType] = Texture2D.FromStream(GraphicsDevice, s);
			}
			FramesPlayList[npcType] = new List<Microsoft.Xna.Framework.Rectangle>();
			int fs = GameConstants.NPCFrameCount[npcType];
			int height = (Frames[npcType].Height) / fs;
			for (int j = 0; j < fs; j++)
				FramesPlayList[npcType].Add(new Microsoft.Xna.Framework.Rectangle(0, j * height + 1, Frames[npcType].Width, height - 2));
			return Frames[npcType];
		}

		protected override void Draw()
		{
			GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(BackColor.R, BackColor.G, BackColor.B));
			Batch.Begin();
			var texture = GetNPCTexture(NPCType);
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
			var color = NPCTabPage.NPCDatum[NPCType].Color;
			var rcolor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
			if (rcolor.A == 0)
				Batch.Draw(texture, dest, FramesPlayList[NPCType][State], Microsoft.Xna.Framework.Color.White);
			else
				Batch.Draw(texture, dest, FramesPlayList[NPCType][State], rcolor);
			Batch.End();
		}
	}
}
