using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using QTRHacker.NewDimension.Configs;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.PagePanels;
using QTRHacker.NewDimension.Res;
using QTRHacker.NewDimension.Wiki;
using QTRHacker.NewDimension.XNAControls;

namespace QTRHacker.NewDimension
{
	public partial class MainForm : Form
	{
		private Color FormBack = Color.FromArgb(45, 45, 48);
		private Point Drag_MousePos;
		private Panel MainPanel, ButtonsPanel, ContentPanel;
		private PictureBox MinButton, CloseButton;
		public static readonly Color ButtonNormalColor = Color.Transparent;
		public static readonly Color ButtonHoverColor = Color.FromArgb(70, 70, 80);
		private PagePanel MainPagePanel, BasicPagePanel, PlayerPagePanel,
			ProjectilePagePanel, ScriptsPagePanel, SchesPagePanel,
			MiscPagePanel, ChatSenderPanel, AimBotPagePanel,
			AboutPagePanel;
		public static MainForm MainFormInstance { get; private set; }
		public static Dictionary<string, Config> Configs = new Dictionary<string, Config>();
		public static ScriptRuntime QHScriptRuntime { get; private set; }
		public static ScriptEngine QHScriptEngine { get; private set; }
		public static Languages.Processor CurrentLanguage;
		public static PageGroup Group1, Group2;
		public static PageGroup ExpandedGroup;
		public static int ButtonsPanelWidth = 100;
		private int GroupsIndex = 0;
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
		}
		public MainForm()
		{
			CheckForIllegalCrossThreadCalls = false;

			if (System.Diagnostics.Process.GetProcessesByName("QTRHacker").Length > 1)
			{
				MessageBox.Show("请先关闭已打开的修改器\n如果已关闭且出现此提示，请在任务管理器检查是否有进程QTRHacker.exe滞留");
				Environment.Exit(0);
			}

			CreateDirectoriesAndFiles();
			LoadConfigs();
#if ENG
			CurrentLanguage = Languages.Processor.GetLanguage("en");
#else
			CurrentLanguage = Languages.Processor.GetLanguage("zh-cn");
#endif

			MainFormInstance = this;

			InitializeComponent();
			BackColor = FormBack;

			QHScriptRuntime = Python.CreateRuntime();
			QHScriptEngine = QHScriptRuntime.GetEngine("Python");
			var paths = QHScriptEngine.GetSearchPaths();
			paths.Add(Path.GetFullPath(".\\Scripts\\"));
			QHScriptEngine.SetSearchPaths(paths);

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
			ButtonsPanel.Bounds = new Rectangle(0, 0, ButtonsPanelWidth, MainPanel.Height);
			ButtonsPanel.BackColor = Color.FromArgb(255, 90, 90, 90);
			MainPanel.Controls.Add(ButtonsPanel);



			Image img_MainPage = null, img_Basic = null, img_Player = null,
				img_Projectile = null, img_Misc = null, img_Scripts = null,
				img_Sche = null, img_ChatSender = null, img_AimBot = null,
				img_About = null;
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_171"]))
				img_MainPage = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_990"]))
				img_Basic = Image.FromStream(st);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Image.player.png"))
				img_Player = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_42"]))
				img_Projectile = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_518"]))
				img_Scripts = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3124"]))
				img_Misc = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_109"]))
				img_About = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_531"]))
				img_ChatSender = Image.FromStream(st);


			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3"]))
				img_Sche = Image.FromStream(st);
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_164"]))
				img_AimBot = Image.FromStream(st);

			ContentPanel = new Panel();
			ContentPanel.Bounds = new Rectangle(ButtonsPanel.Width, 0, MainPanel.Width - ButtonsPanel.Width, MainPanel.Height);
			MainPanel.Controls.Add(ContentPanel);

			int pageWidth = ContentPanel.Width;

			MainPagePanel = new PagePanel_MainPage(pageWidth, MainPanel.Height);
			AboutPagePanel = new PagePanel_About(pageWidth, MainPanel.Height);
			BasicPagePanel = new PagePanel_Basic(pageWidth, MainPanel.Height);
			PlayerPagePanel = new PagePanel_Player(pageWidth, MainPanel.Height);
			ProjectilePagePanel = new PagePanel_Projectile(pageWidth, MainPanel.Height);
			ScriptsPagePanel = new PagePanel_Scripts(pageWidth, MainPanel.Height);
			MiscPagePanel = new PagePanel_Misc(pageWidth, MainPanel.Height);
			ChatSenderPanel = new PagePanel_ChatSender(pageWidth, MainPanel.Height);


			//SchesPagePanel = new PagePanel_Sches(pageWidth, MainPanel.Height);
			//AimBotPagePanel = new PagePanel_AimBot(pageWidth, MainPanel.Height);


			ExpandedGroup = Group1 = AddGroup();
			Group2 = AddGroup();


			AddButton(Group1, CurrentLanguage["Basic"], img_Basic, BasicPagePanel).Enabled = false;
			AddButton(Group1, CurrentLanguage["Players"], img_Player, PlayerPagePanel).Enabled = false;
			AddButton(Group1, CurrentLanguage["Projectiles"], img_Projectile, ProjectilePagePanel).Enabled = false;
			AddButton(Group1, CurrentLanguage["Scripts"], img_Scripts, ScriptsPagePanel).Enabled = false;
			AddButton(Group1, CurrentLanguage["ChatSender"], img_ChatSender, ChatSenderPanel).Enabled = false;
			AddButton(Group1, CurrentLanguage["Miscs"], img_Misc, MiscPagePanel).Enabled = false;
#if ENG
#else
			AddButton(Group1, CurrentLanguage["About"], img_About, AboutPagePanel).Enabled = true;
#endif
			AddButton(Group1, CurrentLanguage["MainPage"], img_MainPage, MainPagePanel).Selected = true;

			//AddButton(Group2, CurrentLanguage["Sches"], img_Sche, SchesPagePanel).Enabled = false;
			//AddButton(Group2, CurrentLanguage["AimBot"], img_AimBot, AimBotPagePanel).Enabled = false;

			Icon = ConvertToIcon(img_Basic, true);

			
		}

		public void OnInitialized()
		{
			foreach (var c in ButtonsPanel.Controls)
			{
				foreach (var cc in ((Control)c).Controls)
				{
					(cc as Control).Enabled = true;
				}
			}
			(Group1.Controls[0] as ImageButton).Selected = true;
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

		private PageGroup AddGroup()
		{
			PageGroup group = new PageGroup();
			if (GroupsIndex == 0)
			{
				group.Location = new Point(ButtonsPanel.Width - 100, 0);
				group.Expanded = true;
			}
			else
			{
				group.Location = new Point(ButtonsPanel.Width - 100 - 32 * GroupsIndex);
				group.Expanded = false;
			}
			group.Height = ButtonsPanel.Height;
			ButtonsPanel.Controls.Add(group);
			GroupsIndex++;
			return group;
		}

		private ImageButton AddButton(PageGroup group, string Text, Image Icon, Control Content)
		{
			return group.AddButton(Text, Icon, Content, (s, e) =>
			 {
				 ImageButton bs = (s as ImageButton);
				 if (!bs.Selected)
					 return;
				 PageGroup previousGroup = ExpandedGroup;
				 PageGroup targetGroup = bs.Parent as PageGroup;

				 foreach (var i in ExpandedGroup.Controls)
					 if (i is ImageButton ii && i != bs)
						 ii.Selected = false;

				 Point tmpP = previousGroup.Location;
				 previousGroup.Location = targetGroup.Location;
				 targetGroup.Location = tmpP;

				 Size tmpS = previousGroup.Size;
				 previousGroup.Size = targetGroup.Size;
				 targetGroup.Size = tmpS;

				 ExpandedGroup = targetGroup;

				 if (Content == null)
					 return;
				 ContentPanel.Controls.Clear();
				 ContentPanel.Controls.Add(Content);
			 });
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

		private void CreateDirectoriesAndFiles()
		{
			if (!Directory.Exists(".\\Projs"))
				Directory.CreateDirectory(".\\Projs");
			if (!Directory.Exists(".\\Scripts"))
				Directory.CreateDirectory(".\\Scripts");
			if (!Directory.Exists(".\\Sches"))
				Directory.CreateDirectory(".\\Sches");
			if (!Directory.Exists(".\\ChatTemplates"))
				Directory.CreateDirectory(".\\ChatTemplates");
			if (!Directory.Exists(".\\RainbowFonts"))
				Directory.CreateDirectory(".\\RainbowFonts");
		}
		private void LoadConfigs()
		{
			if (!Directory.Exists(".\\configs"))
				Directory.CreateDirectory(".\\configs");
			var ts = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t.Namespace == "QTRHacker.NewDimension.Configs" && t.Name.StartsWith("CFG_"));
			ts.ToList().ForEach(t =>
			{
				LoadConfig(t.Name, out Config c, t.AsType());
				Configs[t.Name] = c;
			});
		}
		private void LoadConfig(string name, out Config obj, Type t)
		{
			if (!t.IsSubclassOf(typeof(Config)))
				throw new Exception("Unexpected config type");
			string s = $".\\configs\\{name}.json";
			if (File.Exists(s))
			{
				obj = JsonConvert.DeserializeObject(File.ReadAllText(s), t) as Config;
			}
			else
			{
				obj = t.GetConstructor(new Type[] { }).Invoke(null) as Config;
				SaveConfig(name, obj);
			}
		}
		private void SaveConfig(string name, object obj)
		{
			string s = $".\\configs\\{name}.json";
			File.WriteAllText(s, JsonConvert.SerializeObject(obj, Formatting.Indented));
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			Environment.Exit(0);//防止线程滞留
		}
	}
}
