using Microsoft.Xna.Framework.Graphics;
using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Res;
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

namespace QTRHacker.NewDimension.PlayerEditor
{
	public class PlayerEditor : TabPage
	{
		private GameContext Context;
		private Form ParentForm;
		private PlayerView MainPlayerView;
		private Panel PropertiesSelectPanel;
		private ColorSelectControl HairColorControl, SkinColorControl, EyeColorControl, ShirtColorControl, UnderShirtColorControl, PantsColorControl, ShoesColorControl;
		private NumericUpDown HairStyleControl;
		private TextBox ManaTextBox, HealthTextBox;
		private readonly Player TargetPlayer;
		public PlayerEditor(GameContext Context, Form ParentForm, Player TargetPlayer, bool Editable)
		{
			this.Context = Context;
			this.ParentForm = ParentForm;
			this.TargetPlayer = TargetPlayer;
			Text = HackContext.CurrentLanguage["Character"];

			MainPlayerView = new PlayerView();
			MainPlayerView.Bounds = new Rectangle(5, 5, 200, 250);
			MainPlayerView.MouseClick += (s, e) =>
			{
				if (e.Button == MouseButtons.Right)
				{
					SaveFileDialog sfd = new SaveFileDialog();
					sfd.Filter = "PNG files(*.png)|*.png";
					if (sfd.ShowDialog(this) == DialogResult.OK)
					{
						var stream = File.Open(sfd.FileName, FileMode.OpenOrCreate);
						var a = MainPlayerView.CreateDTexture(MainPlayerView.GraphicsDevice);
						a.SaveAsPng(stream, a.Width, a.Height);
						stream.Close();
					}
				}
			};

			MainPlayerView.HairType = 0;
			HairColorControl = new ColorSelectControl(HackContext.CurrentLanguage["Hair"]) { Enabled = Editable };
			SkinColorControl = new ColorSelectControl(HackContext.CurrentLanguage["Skin"]) { Enabled = Editable };
			EyeColorControl = new ColorSelectControl(HackContext.CurrentLanguage["Eye"]) { Enabled = Editable };
			ShirtColorControl = new ColorSelectControl(HackContext.CurrentLanguage["Shirt"]) { Enabled = Editable };
			UnderShirtColorControl = new ColorSelectControl(HackContext.CurrentLanguage["UnderShirt"]) { Enabled = Editable };
			PantsColorControl = new ColorSelectControl(HackContext.CurrentLanguage["Pants"]) { Enabled = Editable };
			ShoesColorControl = new ColorSelectControl(HackContext.CurrentLanguage["Shoes"]) { Enabled = Editable };
			HairColorControl.OnColorChanged += (c) => MainPlayerView.HairColor = c;
			HairColorControl.Location = new Point(0, 30);

			SkinColorControl.OnColorChanged += (c) => MainPlayerView.SkinColor = c;
			SkinColorControl.Location = new Point(0, 60);

			EyeColorControl.OnColorChanged += (c) => MainPlayerView.EyeBlackColor = c;
			EyeColorControl.Location = new Point(0, 90);

			ShirtColorControl.OnColorChanged += (c) => MainPlayerView.ShirtColor = c;
			ShirtColorControl.Location = new Point(0, 120);

			UnderShirtColorControl.OnColorChanged += (c) => MainPlayerView.UnderShirtColor = c;
			UnderShirtColorControl.Location = new Point(0, 150);

			PantsColorControl.OnColorChanged += (c) => MainPlayerView.PantsColor = c;
			PantsColorControl.Location = new Point(0, 180);

			ShoesColorControl.OnColorChanged += (c) => MainPlayerView.ShoesColor = c;
			ShoesColorControl.Location = new Point(0, 210);

			HairStyleControl = new NumericUpDown() { Enabled = Editable };
			HairStyleControl.BackColor = Color.FromArgb(120, 120, 120);
			HairStyleControl.TextAlign = HorizontalAlignment.Center;
			HairStyleControl.Bounds = new Rectangle(5, 5, 145, 29);
			HairStyleControl.Maximum = PlayerView.MaxHair - 1;
			HairStyleControl.Minimum = 0;
			HairStyleControl.ValueChanged += (s, e) => MainPlayerView.HairType = (int)HairStyleControl.Value;

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
				Text = HackContext.CurrentLanguage["MaxLife"],
				Location = new Point(160, 8),
				Size = new Size(60, 20),
			};

			HealthTextBox = new TextBox()
			{
				Location = new Point(220, 5),
				Size = new Size(100, 20),
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.FromArgb(120, 120, 120),
				Text = Context.MyPlayer.MaxLife.ToString(),
				Enabled = Editable
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
				Text = HackContext.CurrentLanguage["MaxMana"],
				Location = new Point(160, 33),
				Size = new Size(60, 20),
			};

			ManaTextBox = new TextBox()
			{
				Location = new Point(220, 30),
				Size = new Size(100, 20),
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.FromArgb(120, 120, 120),
				Text = Context.MyPlayer.MaxMana.ToString(),
				Enabled = Editable
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


			MButton RefreshButton = new MButton();
			RefreshButton.Text = HackContext.CurrentLanguage["Refresh"];
			RefreshButton.Bounds = new Rectangle(220, 60, 100, 30);
			RefreshButton.Click += (s, e) =>
			{
				InitData();
			};
			PropertiesSelectPanel.Controls.Add(RefreshButton);

			MButton ConfirmButton = new MButton() { Enabled = Editable };
			ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
			ConfirmButton.Bounds = new Rectangle(220, 93, 100, 30);
			ConfirmButton.Click += (s, e) =>
			{
				ApplyData();
			};
			PropertiesSelectPanel.Controls.Add(ConfirmButton);

			Controls.Add(MainPlayerView);
			Controls.Add(PropertiesSelectPanel);

			InitData();
		}

		private void InitData()
		{

			HairStyleControl.Value = TargetPlayer.Hair;
			HairColorControl.Color = ColorFromIntABGR(TargetPlayer.HairColor);
			SkinColorControl.Color = ColorFromIntABGR(TargetPlayer.SkinColor);
			EyeColorControl.Color = ColorFromIntABGR(TargetPlayer.EyeColor);
			ShirtColorControl.Color = ColorFromIntABGR(TargetPlayer.ShirtColor);
			UnderShirtColorControl.Color = ColorFromIntABGR(TargetPlayer.UnderShirtColor);
			PantsColorControl.Color = ColorFromIntABGR(TargetPlayer.PantsColor);
			ShoesColorControl.Color = ColorFromIntABGR(TargetPlayer.ShoeColor);

			HealthTextBox.Text = TargetPlayer.MaxLife.ToString();
			ManaTextBox.Text = TargetPlayer.MaxMana.ToString();
		}

		private void ApplyData()
		{
			TargetPlayer.Hair = (int)HairStyleControl.Value;
			TargetPlayer.HairColor = IntABGRFromColor(HairColorControl.Color);
			TargetPlayer.SkinColor = IntABGRFromColor(SkinColorControl.Color);
			TargetPlayer.EyeColor = IntABGRFromColor(EyeColorControl.Color);
			TargetPlayer.ShirtColor = IntABGRFromColor(ShirtColorControl.Color);
			TargetPlayer.UnderShirtColor = IntABGRFromColor(UnderShirtColorControl.Color);
			TargetPlayer.PantsColor = IntABGRFromColor(PantsColorControl.Color);
			TargetPlayer.ShoeColor = IntABGRFromColor(ShoesColorControl.Color);

			TargetPlayer.MaxLife = Convert.ToInt32(HealthTextBox.Text);
			TargetPlayer.MaxMana = Convert.ToInt32(ManaTextBox.Text);
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
			Font f = new Font("Arial", 10, FontStyle.Bold);
			this.Size = new Size(150, 27);
			Tip = new Label();
			Tip.Text = text;
			Tip.Bounds = new Rectangle(0, 0, 50, 25);
			Tip.Font = f;
			Tip.TextAlign = ContentAlignment.MiddleCenter;
			ColorPanel = new Panel();
			ColorPanel.Bounds = new Rectangle(50, 1, 21, 21);

			ColorCode = new TextBox();
			ColorCode.BorderStyle = BorderStyle.FixedSingle;
			ColorCode.BackColor = System.Drawing.Color.FromArgb(120, 120, 120);
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
		private struct PlayerModelPart
		{
			public Texture2D Source
			{
				get;
			}
			public int ColorIndex
			{
				get;
			}
			public Microsoft.Xna.Framework.Rectangle Bounds
			{
				get;
			}

			public PlayerModelPart(Texture2D source, int colorIndex, Microsoft.Xna.Framework.Rectangle bounds)
			{
				Source = source;
				ColorIndex = colorIndex;
				Bounds = bounds;
			}
		}
		public const int MaxHair = 163, MaxBody = 14;
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
		private PlayerModelPart[] Models
		{
			get;
			set;
		}
		protected override void Draw()
		{
			GraphicsDevice.Clear(new Microsoft.Xna.Framework.Color(70, 70, 70));

			Batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);

			var tR = new Microsoft.Xna.Framework.Rectangle(0, 0, 200, 250);
			var sR = new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50);
			foreach (var model in Models)
			{
				Batch.Draw(model.Source, tR, model.Bounds, Colors[model.ColorIndex]);
			}
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
			foreach (var model in Models)
			{
				spriteBatch.Draw(model.Source, tR, model.Bounds, Colors[model.ColorIndex]);
			}
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

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.ContentImage.PlayerModelsRaw.bin"))
			{
				var PlayerModels = ResBinFileReader.ReadFromStream(s);
				for (int i = 0; i < BodyTextures.Length; i++)
				{
					var res = new MemoryStream(PlayerModels["Player_0_" + i]);
					BodyTextures[i] = Texture2D.FromStream(GraphicsDevice, res);
					res.Close();
				}
				for (int i = 1; i < HairTextures.Length; i++)
				{
					var res = new MemoryStream(PlayerModels["Player_Hair_" + i]);
					HairTextures[i] = Texture2D.FromStream(GraphicsDevice, res);
					res.Close();
				}
			}

			Models = new PlayerModelPart[]
			{
				new PlayerModelPart(BodyTextures[0], 0, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),
				new PlayerModelPart(BodyTextures[1], 1, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),
				new PlayerModelPart(BodyTextures[2], 2, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),

				new PlayerModelPart(BodyTextures[6], 6, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),

				new PlayerModelPart(BodyTextures[10], 10, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),
				new PlayerModelPart(BodyTextures[11], 11, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),
				new PlayerModelPart(BodyTextures[12], 12, new Microsoft.Xna.Framework.Rectangle(0, 5, 40, 50)),

				new PlayerModelPart(BodyTextures[4], 4, new Microsoft.Xna.Framework.Rectangle(80, 5, 40, 51)),
				new PlayerModelPart(BodyTextures[5], 5, new Microsoft.Xna.Framework.Rectangle(80, 5, 40, 51)),
				new PlayerModelPart(BodyTextures[5], 5, new Microsoft.Xna.Framework.Rectangle(80, 117, 40, 51)),
			};


			Application.Idle += OnIdle;
		}
		private void OnIdle(object sender, EventArgs e)
		{
			Invalidate();
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			Application.Idle -= OnIdle;
		}


	}
}
