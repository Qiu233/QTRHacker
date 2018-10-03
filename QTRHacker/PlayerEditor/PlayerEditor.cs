using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsGraphicsDevice;

namespace QTRHacker.PlayerEditor
{
	public class PlayerEditor : TabPage
	{
		private GameContext Context;
		private Form ParentForm;
		private PlayerView PlayerView;
		public PlayerEditor(GameContext Context, Form ParentForm)
		{
			this.Context = Context;
			this.ParentForm = ParentForm;
			Text = "玩家";

			PlayerView = new PlayerView();
			PlayerView.Bounds = new Rectangle(5, 5, 150, 250);

			Controls.Add(PlayerView);
		}
	}

	public class PlayerView : GraphicsDeviceControl
	{
		private const int MaxHair = 133, MaxBody = 14;
		private Texture2D[] BodyTextures, HairTextures;
		private SpriteBatch Batch;
		protected override void Draw()
		{
			GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(0.8f, 1f, 0.9f, 1));

			Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);

			for (int i = 0; i < BodyTextures.Length; i++)
			{
				Batch.Draw(BodyTextures[i], new Microsoft.Xna.Framework.Rectangle(0, 0, 150, 250), new Microsoft.Xna.Framework.Rectangle(5, 5, 30, 50), new Microsoft.Xna.Framework.Color(1f, 1f, 1f));
			}
			Batch.Draw(HairTextures[1], new Microsoft.Xna.Framework.Rectangle(0, 0, 150, 250), new Microsoft.Xna.Framework.Rectangle(5, 5, 30, 50), new Microsoft.Xna.Framework.Color(1f, 1f, 1f));
			Batch.End();
		}

		protected override void Initialize()
		{

			Batch = new SpriteBatch(GraphicsDevice);
			BodyTextures = new Texture2D[MaxBody];
			HairTextures = new Texture2D[MaxHair + 1];
			for (int i = 0; i < BodyTextures.Length; i++)
			{
				var res = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.PlayerEditor.Textures.Player_0_" + i + ".png");
				BodyTextures[i] = Texture2D.FromStream(GraphicsDevice, res);
			}

			for (int i = 1; i < HairTextures.Length; i++)
			{
				var res = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.PlayerEditor.Textures.Player_Hair_" + i + ".png");
				HairTextures[i] = Texture2D.FromStream(GraphicsDevice, res);
			}
		}
	}
}
