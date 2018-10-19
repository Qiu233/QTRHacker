using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Functions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
		private Panel PropertiesSelectPanel;
		private ColorSelectControl HairColorControl, SkinColorControl, EyeColorControl, ShirtColorControl, UnderShirtColorControl, PantsColorControl, ShoesColorControl;
		private NumericUpDown HairStyleControl;
		private TextBox ManaTextBox, HealthTextBox;
		public PlayerEditor(GameContext Context, Form ParentForm)
		{
			this.Context = Context;
			this.ParentForm = ParentForm;
			Text = "人物";

			PlayerView = new PlayerView();
			PlayerView.Bounds = new Rectangle(5, 5, 200, 250);
			PlayerView.MouseClick += (s, e) =>
			{
				if (e.Button == MouseButtons.Right)
				{
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.Filter = "PNG files(*.png)|*.png";
					if (sfd.ShowDialog(this) == DialogResult.OK)
					{
						var stream = File.Open(sfd.FileName, FileMode.OpenOrCreate);
						var a = PlayerView.CreateDTexture(PlayerView.GraphicsDevice);
						a.SaveAsPng(stream, a.Width, a.Height);
						stream.Close();
					}
				}
			};

			PlayerView.HairType = 0;

			HairColorControl = new ColorSelectControl("头发:");
			HairColorControl.OnColorChanged += (c) => PlayerView.HairColor = c;
			HairColorControl.Location = new Point(0, 30);

			SkinColorControl = new ColorSelectControl("皮肤:");
			SkinColorControl.OnColorChanged += (c) => PlayerView.SkinColor = c;
			SkinColorControl.Location = new Point(0, 60);

			EyeColorControl = new ColorSelectControl("眼睛:");
			EyeColorControl.OnColorChanged += (c) => PlayerView.EyeBlackColor = c;
			EyeColorControl.Location = new Point(0, 90);

			ShirtColorControl = new ColorSelectControl("衬衣:");
			ShirtColorControl.OnColorChanged += (c) => PlayerView.ShirtColor = c;
			ShirtColorControl.Location = new Point(0, 120);

			UnderShirtColorControl = new ColorSelectControl("内衬:");
			UnderShirtColorControl.OnColorChanged += (c) => PlayerView.UnderShirtColor = c;
			UnderShirtColorControl.Location = new Point(0, 150);

			PantsColorControl = new ColorSelectControl("裤子:");
			PantsColorControl.OnColorChanged += (c) => PlayerView.PantsColor = c;
			PantsColorControl.Location = new Point(0, 180);

			ShoesColorControl = new ColorSelectControl("鞋子:");
			ShoesColorControl.OnColorChanged += (c) => PlayerView.ShoesColor = c;
			ShoesColorControl.Location = new Point(0, 210);

			HairStyleControl = new NumericUpDown();
			HairStyleControl.TextAlign = HorizontalAlignment.Center;
			HairStyleControl.Bounds = new Rectangle(5, 5, 145, 29);
			HairStyleControl.Maximum = 133;
			HairStyleControl.Minimum = 0;
			HairStyleControl.ValueChanged += (s, e) => PlayerView.HairType = (int)HairStyleControl.Value;

			PropertiesSelectPanel = new Panel();
			PropertiesSelectPanel.Bounds = new Rectangle(210, 15, 330, 250);

			PropertiesSelectPanel.Controls.Add(HairStyleControl);
			PropertiesSelectPanel.Controls.Add(HairColorControl);
			PropertiesSelectPanel.Controls.Add(SkinColorControl);
			PropertiesSelectPanel.Controls.Add(EyeColorControl);
			PropertiesSelectPanel.Controls.Add(ShirtColorControl);
			PropertiesSelectPanel.Controls.Add(UnderShirtColorControl);
			PropertiesSelectPanel.Controls.Add(PantsColorControl);
			PropertiesSelectPanel.Controls.Add(ShoesColorControl);

			Label HealthTipLabel = new Label()
			{
				Text = Lang.maxLife,
				Location = new Point(160, 8),
				Size = new Size(60, 20)
			};

			HealthTextBox = new TextBox()
			{
				Location = new Point(220, 5),
				Size = new Size(100, 20),
				Text = Context.MyPlayer.MaxLife.ToString()
			};
			HealthTextBox.KeyPress += delegate (object sender, KeyPressEventArgs e)
			{
				if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8)
				{
					e.Handled = true;
				}
			};

			Label ManaTipLabel = new Label()
			{
				Text = Lang.maxMana,
				Location = new Point(160, 33),
				Size = new Size(60, 20)
			};

			ManaTextBox = new TextBox()
			{
				Location = new Point(220, 30),
				Size = new Size(100, 20),
				Text = Context.MyPlayer.MaxMana.ToString()
			};
			ManaTextBox.KeyPress += delegate (object sender, KeyPressEventArgs e)
			{
				if (!Char.IsNumber(e.KeyChar) && e.KeyChar != 8)
				{
					e.Handled = true;
				}
			};
			PropertiesSelectPanel.Controls.Add(HealthTipLabel);
			PropertiesSelectPanel.Controls.Add(HealthTextBox);
			PropertiesSelectPanel.Controls.Add(ManaTipLabel);
			PropertiesSelectPanel.Controls.Add(ManaTextBox);


			Button RefreshButton = new Button();
			RefreshButton.Text = "刷新";
			RefreshButton.Bounds = new Rectangle(220, 60, 100, 30);
			RefreshButton.Click += (s, e) =>
			{
				InitData();
			};
			PropertiesSelectPanel.Controls.Add(RefreshButton);

			Button ConfirmButton = new Button();
			ConfirmButton.Text = "确定";
			ConfirmButton.Bounds = new Rectangle(220, 100, 100, 30);
			ConfirmButton.Click += (s, e) =>
			{
				ApplyData();
			};
			PropertiesSelectPanel.Controls.Add(ConfirmButton);


			Label Intro = new Label();
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Intro.txt"))
				Intro.Text = new StreamReader(stream).ReadToEnd();
			Intro.Bounds = new Rectangle(580, 5, 380, 270);
			Controls.Add(Intro);

			Controls.Add(PlayerView);
			Controls.Add(PropertiesSelectPanel);

			InitData();
		}

		private void InitData()
		{

			HairStyleControl.Value = Context.MyPlayer.Hair;
			HairColorControl.Color = ColorFromIntABGR(Context.MyPlayer.HairColor);
			SkinColorControl.Color = ColorFromIntABGR(Context.MyPlayer.SkinColor);
			EyeColorControl.Color = ColorFromIntABGR(Context.MyPlayer.EyeColor);
			ShirtColorControl.Color = ColorFromIntABGR(Context.MyPlayer.ShirtColor);
			UnderShirtColorControl.Color = ColorFromIntABGR(Context.MyPlayer.UnderShirtColor);
			PantsColorControl.Color = ColorFromIntABGR(Context.MyPlayer.PantsColor);
			ShoesColorControl.Color = ColorFromIntABGR(Context.MyPlayer.ShoesColor);

			HealthTextBox.Text = Context.MyPlayer.MaxLife.ToString();
			ManaTextBox.Text = Context.MyPlayer.MaxMana.ToString();
		}

		private void ApplyData()
		{
			Context.MyPlayer.Hair = (int)HairStyleControl.Value;
			Context.MyPlayer.HairColor = IntABGRFromColor(HairColorControl.Color);
			Context.MyPlayer.SkinColor = IntABGRFromColor(SkinColorControl.Color);
			Context.MyPlayer.EyeColor = IntABGRFromColor(EyeColorControl.Color);
			Context.MyPlayer.ShirtColor = IntABGRFromColor(ShirtColorControl.Color);
			Context.MyPlayer.UnderShirtColor = IntABGRFromColor(UnderShirtColorControl.Color);
			Context.MyPlayer.PantsColor = IntABGRFromColor(PantsColorControl.Color);
			Context.MyPlayer.ShoesColor = IntABGRFromColor(ShoesColorControl.Color);

			Context.MyPlayer.MaxLife = Convert.ToInt32(HealthTextBox.Text);
			Context.MyPlayer.MaxMana = Convert.ToInt32(ManaTextBox.Text);
		}



		public static Microsoft.Xna.Framework.Color ColorFromIntABGR(int i)
		{
			byte a = (byte)((i & 0xFF000000) >> 24);
			byte b = (byte)((i & 0x00FF0000) >> 16);
			byte g = (byte)((i & 0x0000FF00) >> 8);
			byte r = (byte)(i & 0x000000FF);
			var color = new Microsoft.Xna.Framework.Color(r, g, b, a);
			return color;
		}

		public static int IntABGRFromColor(Microsoft.Xna.Framework.Color i)
		{
			return (i.A << 24) + (i.B << 16) + (i.G << 8) + i.R;
		}
	}

	public class ColorSelectControl : UserControl
	{
		private Label Tip;
		private Panel ColorPanel;
		private TextBox ColorCode;
		public override string Text { get => ColorCode.Text; set => ColorCode.Text = value; }
		public event Action<Microsoft.Xna.Framework.Color> OnColorChanged = (c) => { };
		public Microsoft.Xna.Framework.Color Color
		{
			get
			{
				int v = Convert.ToInt32(Text.Trim() == "" ? "0" : Text, 16);
				int r = v >> 16;
				int g = (v & 0x00FF00) >> 8;
				int b = v & 0x0000FF;
				return new Microsoft.Xna.Framework.Color(r, g, b);
			}
			set
			{
				Text = value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
				ColorPanel.BackColor = System.Drawing.Color.FromArgb(value.R, value.G, value.B);
				OnColorChanged(value);
			}
		}
		public ColorSelectControl(string text)
		{
			Font f = new Font("宋体", 10, FontStyle.Bold);
			this.Size = new Size(150, 27);
			Tip = new Label();
			Tip.Text = text;
			Tip.Bounds = new Rectangle(0, 0, 50, 25);
			Tip.Font = f;
			Tip.TextAlign = ContentAlignment.MiddleCenter;
			ColorPanel = new Panel();
			ColorPanel.Bounds = new Rectangle(50, 1, 21, 21);

			ColorCode = new TextBox();
			ColorCode.BorderStyle = BorderStyle.Fixed3D;
			ColorCode.Font = f;
			ColorCode.Bounds = new Rectangle(78, 0, 70, 30);
			ColorCode.KeyPress += (s, e) =>
			{
				if (!((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || e.KeyChar == '\b'))
					e.Handled = true;
			};
			ColorCode.LostFocus += (s, e) =>
			{
				Color = Color;
			};
			Color = Microsoft.Xna.Framework.Color.White;//此时的OnColorChanged事件是空的

			Controls.Add(Tip);
			Controls.Add(ColorPanel);
			Controls.Add(ColorCode);
		}
	}

	public class PlayerView : GraphicsDeviceControl
	{
		private const int MaxHair = 133, MaxBody = 14;
		private Texture2D[] BodyTextures, HairTextures;
		private SpriteBatch Batch;
		private Microsoft.Xna.Framework.Color[] Colors;

		public int HairType
		{
			get; set;
		}

		public Microsoft.Xna.Framework.Color SkinColor
		{
			get => Colors[0];
			set
			{
				Colors[0] = value;
				Colors[5] = value;
			}
		}
		public Microsoft.Xna.Framework.Color HairColor
		{
			get;
			set;
		}
		public Microsoft.Xna.Framework.Color EyeWhiteColor
		{
			get => Colors[1];
			set
			{
				Colors[1] = value;
			}
		}
		public Microsoft.Xna.Framework.Color EyeBlackColor
		{
			get => Colors[2];
			set
			{
				Colors[2] = value;
			}
		}
		public Microsoft.Xna.Framework.Color ShirtColor
		{
			get => Colors[6];
			set
			{
				Colors[6] = value;
			}
		}
		public Microsoft.Xna.Framework.Color UnderShirtColor
		{
			get => Colors[4];
			set
			{
				Colors[4] = value;
			}
		}
		public Microsoft.Xna.Framework.Color PantsColor
		{
			get => Colors[10];
			set
			{
				Colors[10] = value;
				Colors[11] = value;
			}
		}
		public Microsoft.Xna.Framework.Color ShoesColor
		{
			get => Colors[12];
			set
			{
				Colors[12] = value;
			}
		}
		protected override void Draw()
		{
			GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(0.8f, 1f, 0.9f, 1));

			Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);

			var tR = new Microsoft.Xna.Framework.Rectangle(0, 0, 200, 250);
			var sR = new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50);
			Batch.Draw(BodyTextures[0], tR, sR, Colors[0]);
			Batch.Draw(BodyTextures[1], tR, sR, Colors[1]);
			Batch.Draw(BodyTextures[2], tR, sR, Colors[2]);
			Batch.Draw(BodyTextures[4], tR, sR, Colors[4]);
			Batch.Draw(BodyTextures[5], tR, sR, Colors[5]);
			Batch.Draw(BodyTextures[6], tR, sR, Colors[6]);
			Batch.Draw(BodyTextures[10], tR, sR, Colors[10]);
			Batch.Draw(BodyTextures[11], tR, sR, Colors[11]);
			Batch.Draw(BodyTextures[12], tR, sR, Colors[12]);

			Batch.Draw(HairTextures[HairType + 1], tR, sR, HairColor);
			Batch.End();
		}

		public Texture2D CreateDTexture(GraphicsDevice graphcisDevice)
		{

			RenderTarget2D rt = new RenderTarget2D(graphcisDevice, 40, 50);
			graphcisDevice.SetRenderTarget(rt);

			graphcisDevice.Clear(ClearOptions.Target, Microsoft.Xna.Framework.Color.Transparent, 0, 0);

			SpriteBatch spriteBatch = new SpriteBatch(graphcisDevice);
			spriteBatch.Begin();
			var sR = new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50);
			var tR = new Microsoft.Xna.Framework.Rectangle(0, 0, 40, 50);
			spriteBatch.Draw(BodyTextures[0], tR, sR, Colors[0]);
			spriteBatch.Draw(BodyTextures[1], tR, sR, Colors[1]);
			spriteBatch.Draw(BodyTextures[2], tR, sR, Colors[2]);
			spriteBatch.Draw(BodyTextures[4], tR, sR, Colors[4]);
			spriteBatch.Draw(BodyTextures[5], tR, sR, Colors[5]);
			spriteBatch.Draw(BodyTextures[6], tR, sR, Colors[6]);
			spriteBatch.Draw(BodyTextures[10], tR, sR, Colors[10]);
			spriteBatch.Draw(BodyTextures[11], tR, sR, Colors[11]);
			spriteBatch.Draw(BodyTextures[12], tR, sR, Colors[12]);
			spriteBatch.Draw(HairTextures[HairType + 1], tR, sR, HairColor);
			spriteBatch.End();

			graphcisDevice.SetRenderTarget(null);
			spriteBatch.Dispose();

			return rt;
		}

		public PlayerView()
		{

			Colors = new Microsoft.Xna.Framework.Color[MaxBody];
			for (int i = 0; i < Colors.Length; i++)
				Colors[i] = new Microsoft.Xna.Framework.Color(1f, 1f, 1f);
			HairColor = new Microsoft.Xna.Framework.Color(1f, 1f, 1f);
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
				res.Close();
			}

			for (int i = 1; i < HairTextures.Length; i++)
			{
				var res = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.PlayerEditor.Textures.Player_Hair_" + i + ".png");
				HairTextures[i] = Texture2D.FromStream(GraphicsDevice, res);
				res.Close();
			}

			Application.Idle += (s, e) =>
			{
				Invalidate();
			};
		}


	}
}
