using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using QTRHacker.NewDimension.Configs;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.PagePanels;
using QTRHacker.NewDimension.Res;

namespace QTRHacker.NewDimension
{
	public partial class MainForm : Form
	{
		private Color FormBack = Color.FromArgb(45, 45, 48);
		private Label VersionLabel;
		private Point Drag_MousePos;
		private Panel MainPanel, ButtonsPanel, ContentPanel;
		private PictureBox MinButton, CloseButton;
		private int ButtonsNumber = 0;
		public static readonly Color ButtonNormalColor = Color.Transparent;
		public static readonly Color ButtonHoverColor = Color.FromArgb(70, 70, 80);
		private PagePanel MainPagePanel, BasicPagePanel, PlayerPagePanel, ProjectilePagePanel, GameDataPagePanel;
		public static MainForm MainFormInstance { get; private set; }
		public static CFG_ProjDrawer Config_ProjDrawer;
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
		}
		public MainForm()
		{
			if (System.Diagnostics.Process.GetProcessesByName("QTRHackerND").Length > 1)
			{
				Environment.Exit(0);
			}
			MainFormInstance = this;
			InitializeComponent();
			BackColor = FormBack;

			MainPanel = new Panel();
			MainPanel.BackColor = Color.FromArgb(30, 30, 30);
			MainPanel.Bounds = new Rectangle(2, 30, this.Width - 4, this.Height - 32);
			this.Controls.Add(MainPanel);

			MinButton = new PictureBox();
			MinButton.Click += (s, e) => WindowState = FormWindowState.Minimized;
			MinButton.MouseEnter += (s, e) => MinButton.BackColor = ButtonHoverColor;
			MinButton.MouseLeave += (s, e) => MinButton.BackColor = ButtonNormalColor;
			MinButton.Bounds = new Rectangle(Width - 64, -1, 32, 32);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.min.png"))
				MinButton.Image = Image.FromStream(st);

			CloseButton = new PictureBox();
			CloseButton.MouseEnter += (s, e) => CloseButton.BackColor = ButtonHoverColor;
			CloseButton.MouseLeave += (s, e) => CloseButton.BackColor = ButtonNormalColor;
			CloseButton.Click += (s, e) => Dispose();
			CloseButton.Bounds = new Rectangle(Width - 32, -1, 32, 32);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.close.png"))
				CloseButton.Image = Image.FromStream(st);

			Controls.Add(MinButton);
			Controls.Add(CloseButton);

			ButtonsPanel = new Panel();
			ButtonsPanel.Bounds = new Rectangle(0, 0, 100, MainPanel.Height);
			ButtonsPanel.BackColor = Color.FromArgb(50, 255, 255, 255);
			MainPanel.Controls.Add(ButtonsPanel);

			VersionLabel = new Label();
			VersionLabel.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			VersionLabel.BackColor = Color.Transparent;
			VersionLabel.Bounds = new Rectangle(3, 350, 100, 20);
			ButtonsPanel.Controls.Add(VersionLabel);


			Image img_MainPage = null;
			Image img_Basic = null;
			Image img_Player = null;
			Image img_Projectile = null;
			Image img_GameData = null;
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_171"]))
				img_MainPage = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_990"]))
				img_Basic = Image.FromStream(st);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.player.png"))
				img_Player = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_42"]))
				img_Projectile = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3124"]))
				img_GameData = Image.FromStream(st);

			ContentPanel = new Panel();
			ContentPanel.Bounds = new Rectangle(100, 0, MainPanel.Width - 100, MainPanel.Height);
			MainPanel.Controls.Add(ContentPanel);

			MainPagePanel = new PagePanel_MainPage(MainPanel.Width - 100, MainPanel.Height);
			BasicPagePanel = new PagePanel_Basic(MainPanel.Width - 100, MainPanel.Height);
			PlayerPagePanel = new PagePanel_Player(MainPanel.Width - 100, MainPanel.Height);
			ProjectilePagePanel = new PagePanel_Projectile(MainPanel.Width - 100, MainPanel.Height);
			GameDataPagePanel = new PagePanel_GameData(MainPanel.Width - 100, MainPanel.Height);


			AddButton("基础功能", img_Basic, BasicPagePanel).Enabled = false;
			AddButton("玩家", img_Player, PlayerPagePanel).Enabled = false;
			AddButton("弹幕管理", img_Projectile, ProjectilePagePanel).Enabled = false;
			AddButton("游戏数据", img_GameData, GameDataPagePanel).Enabled = false;


			AddButton("主页", img_MainPage, MainPagePanel).Selected = true;


			Icon = ConvertToIcon(img_Basic, true);


			LoadConfigs();
		}

		public void OnInitialized()
		{
			foreach (var c in ButtonsPanel.Controls)
			{
				(c as Control).Enabled = true;
			}
			foreach (var i in ButtonsPanel.Controls)
			{
				if (i is ImageButton)
				{
					(i as ImageButton).Selected = true;
					break;
				}
			}
		}

		/// <summary>
		/// 这个算法是从网上找到的，来源：http://www.cnblogs.com/ahdung/p/ConvertToIcon.html
		/// 作者是AhDung
		/// </summary>
		/// <param name="image"></param>
		/// <param name="nullTonull"></param>
		/// <returns></returns>
		public static Icon ConvertToIcon(Image image, bool nullTonull = false)
		{
			if (image == null)
			{
				if (nullTonull) { return null; }
				throw new ArgumentNullException("image");
			}
			using (MemoryStream msImg = new MemoryStream(), msIco = new MemoryStream())
			{
				image.Save(msImg, ImageFormat.Png);
				using (var bin = new BinaryWriter(msIco))
				{
					//写图标头部
					bin.Write((short)0);           //0-1保留
					bin.Write((short)1);           //2-3文件类型。1=图标, 2=光标
					bin.Write((short)1);           //4-5图像数量（图标可以包含多个图像）

					bin.Write((byte)image.Width);  //6图标宽度
					bin.Write((byte)image.Height); //7图标高度
					bin.Write((byte)0);            //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
					bin.Write((byte)0);            //9保留。必须为0
					bin.Write((short)0);           //10-11调色板
					bin.Write((short)32);          //12-13位深
					bin.Write((int)msImg.Length);  //14-17位图数据大小
					bin.Write(22);                 //18-21位图数据起始字节
												   //写图像数据
					bin.Write(msImg.ToArray());
					bin.Flush();
					bin.Seek(0, SeekOrigin.Begin);
					return new Icon(msIco);
				}
			}
		}

		private ImageButton AddButton(string Text, Image Icon, Control Content)
		{
			ImageButton b = new ImageButton();
			b.Location = new Point(0, 30 * (ButtonsNumber++));
			b.Image = Icon;
			b.Text = Text;
			ButtonsPanel.Controls.Add(b);
			b.OnSelected += (s, e) =>
			{
				ImageButton bs = (s as ImageButton);
				if (!bs.Selected)
					return;
				foreach (var i in ButtonsPanel.Controls)
					if (i is ImageButton && i != bs)
						(i as ImageButton).Selected = false;
				if (Content == null)
					return;
				ContentPanel.Controls.Clear();
				ContentPanel.Controls.Add(Content);
			};
			b.Click += (s, e) =>
			{
				ImageButton bs = (s as ImageButton);
				if (bs.Selected)
					return;
				bs.Selected = true;
			};
			return b;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawString(Text, new Font("Arial", 16f), Brushes.White, new Point(5, 3));
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left)
			{
				Drag_MousePos = e.Location;
			}
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left)
			{
				this.Top = MousePosition.Y - Drag_MousePos.Y;
				this.Left = MousePosition.X - Drag_MousePos.X;
			}
		}

		private void LoadConfigs()
		{
			if (!Directory.Exists(".\\configs"))
				Directory.CreateDirectory(".\\configs");
			LoadConfig("CFG_ProjDrawer", out Config_ProjDrawer);
		}
		private void LoadConfig<T>(string name, out T obj) where T : new()
		{
			string s = $".\\configs\\{name}.json";
			if (File.Exists(s))
			{
				obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(s));
			}
			else
			{
				obj = new T();
				SaveConfig(name, obj);
			}
		}
		private void SaveConfig(string name, object obj)
		{
			string s = $".\\configs\\{name}.json";
			File.WriteAllText(s, JsonConvert.SerializeObject(obj, Formatting.Indented));
		}
	}
}
