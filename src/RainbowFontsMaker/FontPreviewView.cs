using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Core.ProjectileImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsGraphicsDevice;

namespace RainbowFontsMaker
{
	public class FontPreviewView : GraphicsDeviceControl
	{
		public ProjImage Image
		{
			get;
			set;
		} = null;
		public SpriteBatch Batch
		{
			get;
			private set;
		}
		private Texture2D ProjTexture
		{
			get;
			set;
		}
		private Texture2D VerticalLine
		{
			get;
			set;
		}
		private Texture2D HorizontalLine
		{
			get;
			set;
		}

		public FontPreviewView()
		{
			Size = new System.Drawing.Size(150, 240);
		}
		protected override void Draw()
		{
			GraphicsDevice.Clear(new Color(145, 140, 145));

			Batch.Begin();
			Batch.Draw(VerticalLine, new Vector2(Width / 2 - 1, 0), Color.BlueViolet);
			Batch.Draw(HorizontalLine, new Vector2(0, Height / 2 - 1), Color.BlueViolet);
			Batch.End();
			
			if (Image == null)
				return;
			Batch.Begin();
			foreach (var p in Image.Projs)
			{
				if (p.ProjType != 251)
					continue;
				var pos = new Vector2(p.Location.X, p.Location.Y) + new Vector2(Width / 2, Height / 2);
				DrawPoint(pos, new Vector2(p.Speed.X, p.Speed.Y));
			}
			Batch.End();
		}

		private void DrawPointRaw(Vector2 pos, Vector2 direction)
		{
			Batch.Draw(ProjTexture, pos, null, new Color(255, 255, 255, 120), (float)Math.PI / 2 + (float)Math.Atan2(direction.Y, direction.X), new Vector2(16, 16), 1.25f, SpriteEffects.None, 0);
		}

		private void DrawPoint(Vector2 pos, Vector2 direction)
		{
			var unit = direction / direction.Length();
			var unitp = new Vector2(-unit.Y, unit.X);
			DrawPointRaw(pos - unit * 13, direction);
		}

		protected override void Initialize()
		{
			Batch = new SpriteBatch(GraphicsDevice);
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RainbowFontsMaker.Res.Projectile_251.png"))
				ProjTexture = Texture2D.FromStream(GraphicsDevice, stream);
			VerticalLine = new Texture2D(GraphicsDevice, 2, Height);
			HorizontalLine = new Texture2D(GraphicsDevice, Width, 2);
			Color[] colors1 = new Color[Height * 2];
			for (int i = 0; i < colors1.Length; i++)
				colors1[i] = Color.White;
			Color[] colors2 = new Color[Width * 2];
			for (int i = 0; i < colors2.Length; i++)
				colors2[i] = Color.White;
			VerticalLine.SetData(colors1);
			HorizontalLine.SetData(colors2);
			Application.Idle += Application_Idle;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			ProjTexture.Dispose();
			VerticalLine.Dispose();
			HorizontalLine.Dispose();
		}

		private void Application_Idle(object sender, EventArgs e)
		{
			Invalidate();
		}

	}
}
