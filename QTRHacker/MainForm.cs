/*
 * Created by SharpDevelop.
 * User: jianqiu
 * Date: 2015/9/12
 * Time: 15:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Resources;
using System.IO;
using System.Linq;
using System.Reflection;
using QTRHacker.Functions;

namespace QTRHacker
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private bool flag = false;
		private Label status;
		private int hProcess;
		public const int PROCESS_ALL_ACCESS = 0x1F0FFF;

		public delegate void HackFunc(GameContext c);
		private Button Extra, Script;
		private ExtraForm ExtraHack = null;
		private ScriptForm ScriptForm = null;
		private TabControl mainTab;
		private TabPage buttonTabPage1;
		private TabPage buttonTabPage2;
		private TabPage buttonTabPage3;
		private TabPage buttonTabPage4;
		private TabPage buttonTabPage5;
		private TabPage buttonTabPage6;

		internal Dictionary<string, TabPage> tabs = new Dictionary<string, TabPage>();
		internal Dictionary<string, int> indexes = new Dictionary<string, int>();

		private List<Plugin> Plugins = new List<Plugin>();

		public static ImageList item_images = new ImageList();
		private string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString() + "---" + gameVersion;
		public int processID;
		public static MainForm mainWindow;
		private Image cross;
		public static Resources resource = new Resources();
		public static string gameVersion = "1.3.5.3";
		public const int MAX_ITEMS_1353 = 3930;
		private const int row = 12;
		public static GameContext Context;
		public static bool CanHack => !(Context == null || Context.MyPlayer.BaseAddress <= 0);

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}
		[DllImport("User32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);
		[DllImport("User32.dll")]
		public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);
		[DllImport("User32.dll")]
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
		[DllImport("kernel32.dll")]
		public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);


		public TabPage RegisterTab(string Text)
		{
			TabPage tp = new TabPage(Text);
			tp.BackColor = Color.LightGray;
			tabs[Text] = tp;
			indexes[Text] = 0;
			mainTab.Controls.Add(tp);
			return tp;
		}

		public void InitControls()
		{
			MemoryStream ms = new MemoryStream(resource.ItemImage);
			BinaryReader br = new BinaryReader(ms);
			while (true)
			{
				if (br.PeekChar() == -1) break;
				string name = br.ReadString();
				long length = br.ReadInt64();
				byte[] data = br.ReadBytes((int)length);
				MemoryStream imgms = new MemoryStream(data);
				Image img = Image.FromStream(imgms);
				item_images.Images.Add(name, img);
			}
			br.Close();
			ms.Close();
			item_images.ColorDepth = ColorDepth.Depth32Bit;
			item_images.ImageSize = new Size(20, 20);

			{
				var none = item_images.Images[0];//just to load all images
			}

			status = new Label()
			{
				Text = Lang.none,
				Location = new Point(0, 50),
				Size = new Size(190, 25),
				Font = new Font("Arial", 10)
			};
			this.Controls.Add(status);

			Extra = new Button()
			{
				Location = new Point(250, 50),
				Size = new Size(50, 25),
				Text = Lang.extra
			};
			Extra.Click += delegate (object sender, EventArgs e)
			 {
				 if (!CanHack)
				 {
					 MessageBox.Show(Lang.nonePlayerBase);
					 return;
				 }
				 if (ExtraHack == null)
				 {
					 ExtraHack = new ExtraForm(Context);
					 ExtraHack.Show(this);
					 ExtraHack.Location = new Point(Location.X + Width, Location.Y);
					 Extra.Font = new Font("Arial", 10, FontStyle.Bold);
				 }
				 else
				 {
					 ExtraHack.Dispose();
					 ExtraHack = null;
					 Extra.Font = new Font("Arial", 10);
				 }
			 };
			this.Controls.Add(Extra);

			Script = new Button()
			{
				Location = new Point(200, 50),
				Size = new Size(50, 25),
				Text = "脚本"
			};
			Script.Click += delegate (object sender, EventArgs e)
			{
				if (ScriptForm == null)
				{
					ScriptForm = new ScriptForm(Context);
					ScriptForm.Show(this);
					Script.Font = new Font("Arial", 10, FontStyle.Bold);
				}
				else
				{
					if (ScriptForm.Visible)
					{
						ScriptForm.Visible = false;
						Script.Font = new Font("Arial", 10);
					}
					else
					{
						ScriptForm.Visible = true;
						Script.Font = new Font("Arial", 10, FontStyle.Bold);
					}
				}
			};
			this.Controls.Add(Script);



			mainTab = new MTabControl()
			{
				Location = new Point(0, 75),
				Size = new Size(300, 500 - 105),
				SelectedIndex = 0
			};
			buttonTabPage1 = RegisterTab("1");

			buttonTabPage2 = RegisterTab("2");

			buttonTabPage6 = RegisterTab("3");

			buttonTabPage3 = RegisterTab(Lang.Event);

			buttonTabPage4 = RegisterTab(Lang.builder);

			buttonTabPage5 = RegisterTab(Lang.eff);


			this.Controls.Add(mainTab);



			AddButton(buttonTabPage1, Lang.infLife, 0, Utils.InfiniteLife_E, Utils.InfiniteLife_D);
			AddButton(buttonTabPage1, Lang.infOxygen, 1, Utils.InfiniteOxygen_E, Utils.InfiniteOxygen_D);
			AddButton(buttonTabPage1, Lang.infSummon, 2, Utils.InfiniteMinion_E, Utils.InfiniteMinion_D);
			AddButton(buttonTabPage1, Lang.infMana, 3, Utils.InfiniteMana_E, Utils.InfiniteMana_D);
			AddButton(buttonTabPage1, Lang.infItemAndAmmo, 4, (ctx) => { Utils.InfiniteItem_E(ctx); Utils.InfiniteAmmo_E(ctx); }, (ctx) => { Utils.InfiniteItem_D(ctx); Utils.InfiniteAmmo_D(ctx); });
			AddButton(buttonTabPage1, Lang.infFly, 5, Utils.InfiniteFly_E, Utils.InfiniteFly_D);
			//AddButton(buttonTabPage1, Lang.immuneStoned, 6, HackFunctions.immuneBuff, HackFunctions.De_immuneBuff);
			AddButton(buttonTabPage1, Lang.highLight, 7, Utils.HighLight_E, Utils.HighLight_D);
			AddButton(buttonTabPage1, Lang.ghostMode, 8, Utils.GhostMode_E, Utils.GhostMode_D);
			/*AddButton(buttonTabPage1, Lang.respawnAtOnce, 9, HackFunctions.NoRespawnTime, HackFunctions.De_NoRespawnTime);
			AddButton(buttonTabPage1, Lang.attackThroughWalls, 10, HackFunctions.AttackThroughWalls, HackFunctions.De_AttackThroughWalls);
			AddButton(buttonTabPage1, Lang.noPotionDelay, 11, HackFunctions.NoPotionDelay, HackFunctions.De_NoPotionDelay);*/

			AddButton(buttonTabPage2, Lang.decreaseGravity, 0, Utils.LowGravity_E, Utils.LowGravity_D);
			AddButton(buttonTabPage2, Lang.increaseSpeed, 1, Utils.FastSpeed_E, Utils.FastSpeed_D);
			//AddButton(buttonTabPage2, Lang.killAllNPC, 2, HackFunctions.KillAllNPC, HackFunctions.De_KillAllNPC);
			AddButton(buttonTabPage2, Lang.projectileThroughWalls, 3, Utils.ProjectileIgnoreTile_E, Utils.ProjectileIgnoreTile_D);
			AddButton(buttonTabPage2, Lang.superPick, 4, Utils.GrabItemFarAway_E, Utils.GrabItemFarAway_D);
			AddButton(buttonTabPage2, Lang.extraTwoSlots, 5, Utils.BonusTwoSlots_E, Utils.BonusTwoSlots_D);
			AddButton(buttonTabPage2, Lang.goldHoleDropBag, 6, Utils.GoldHoleDropsBag_E, Utils.GoldHoleDropsBag_D);
			AddButton(buttonTabPage2, Lang.slimeGunBurn, 7, Utils.SlimeGunBurn_E, Utils.SlimeGunBurn_D);
			AddButton(buttonTabPage2, Lang.fishOnlyCrates, 8, Utils.FishOnlyCrates_E, Utils.FishOnlyCrates_D);
			//AddButton(buttonTabPage2, Lang.killAllScreen, 9, HackFunctions.KillAllScreen, HackFunctions.De_KillAllScreen);
			AddButton(buttonTabPage2, Lang.allRecipe, 10, Utils.EnableAllRecipes_E, Utils.EnableAllRecipes_D);
			AddButton(buttonTabPage2, Lang.strengthen_Vampire_Knives, 11, Utils.StrengthenVampireKnives_E, Utils.StrengthenVampireKnives_D);


			//AddButton(buttonTabPage6, Lang.blockAttacking, 0, HackFunctions.BlockAttacking, HackFunctions.De_BlockAttacking, true);
			AddButton(buttonTabPage6, Lang.burnAllNPC, 1, (Context) =>
			{
				int i = 0;
				Form p = new Form();
				ProgressBar pb = new ProgressBar();
				Label tip = new Label(), percent = new Label();
				tip.Text = "Burning NPCS...";
				tip.Location = new Point(0, 0);
				tip.Size = new Size(150, 30);
				tip.TextAlign = ContentAlignment.MiddleCenter;
				percent.Location = new Point(150, 0);
				percent.Size = new Size(50, 30);
				percent.TextAlign = ContentAlignment.MiddleCenter;
				System.Timers.Timer timer = new System.Timers.Timer(1);
				p.FormBorderStyle = FormBorderStyle.FixedSingle;
				p.ClientSize = new Size(300, 60);
				p.ControlBox = false;
				pb.Location = new Point(0, 30);
				pb.Size = new Size(300, 30);
				pb.Maximum = NPC.MAXNUMBER;
				pb.Minimum = 0;
				pb.Value = 0;
				p.Controls.Add(tip);
				p.Controls.Add(percent);
				p.Controls.Add(pb);
				timer.Elapsed += (sender, e) =>
				{
					pb.Value = i;
					percent.Text = pb.Value + "/" + pb.Maximum;
					if (i >= pb.Maximum) p.Dispose();
				};
				timer.Start();
				p.Show();
				p.Location = new Point(MainForm.mainWindow.Location.X + MainForm.mainWindow.Width / 2 - p.ClientSize.Width / 2, MainForm.mainWindow.Location.Y + MainForm.mainWindow.Height / 2 - p.ClientSize.Height / 2);
				new Thread(() =>
				{
					this.Enabled = false;
					if (ExtraForm.Window != null)
						ExtraForm.Window.Enabled = false;
					var npc = Context.NPC;
					for (; i < NPC.MAXNUMBER; i++)
						if (npc[i].Active)
							npc[i].AddBuff(0x99, 216000);
					if (ExtraForm.Window != null)
						ExtraForm.Window.Enabled = true;
					this.Enabled = true;
				}).Start();
			}, null, false);
			AddButton(buttonTabPage6, Lang.burnAllPlayer, 2, (Context) =>
			{
				int i = 0;
				Form p = new Form();
				ProgressBar pb = new ProgressBar();
				Label tip = new Label(), percent = new Label();
				tip.Text = "Burning NPCS...";
				tip.Location = new Point(0, 0);
				tip.Size = new Size(150, 30);
				tip.TextAlign = ContentAlignment.MiddleCenter;
				percent.Location = new Point(150, 0);
				percent.Size = new Size(50, 30);
				percent.TextAlign = ContentAlignment.MiddleCenter;
				System.Timers.Timer timer = new System.Timers.Timer(1);
				p.FormBorderStyle = FormBorderStyle.FixedSingle;
				p.ClientSize = new Size(300, 60);
				p.ControlBox = false;
				pb.Location = new Point(0, 30);
				pb.Size = new Size(300, 30);
				pb.Maximum = NPC.MAXNUMBER;
				pb.Minimum = 0;
				pb.Value = 0;
				p.Controls.Add(tip);
				p.Controls.Add(percent);
				p.Controls.Add(pb);
				timer.Elapsed += (sender, e) =>
				{
					pb.Value = i;
					percent.Text = pb.Value + "/" + pb.Maximum;
					if (i >= pb.Maximum) p.Dispose();
				};
				timer.Start();
				p.Show();
				p.Location = new Point(MainForm.mainWindow.Location.X + MainForm.mainWindow.Width / 2 - p.ClientSize.Width / 2, MainForm.mainWindow.Location.Y + MainForm.mainWindow.Height / 2 - p.ClientSize.Height / 2);
				new Thread(() =>
				{
					this.Enabled = false;
					if (ExtraForm.Window != null)
						ExtraForm.Window.Enabled = false;
					var player = Context.Players;
					for (; i < NPC.MAXNUMBER; i++)
						if (player[i].Active)
							player[i].AddBuff(44, 216000);
					if (ExtraForm.Window != null)
						ExtraForm.Window.Enabled = true;
					this.Enabled = true;
				}).Start();
			}, null, false);
			/*AddButton(buttonTabPage6, Lang.dropLava, 3, () =>
			{
				for (int i = 0; i < 50; i++)
				{
					if (!HackFunctions.getPlayerActive(i)) continue;
					if (i == HackFunctions.getMyPlayer()) continue;
					int X = (int)HackFunctions.getPlayerX(i) / 16;
					int Y = (int)HackFunctions.getPlayerY(i) / 16;
					HackFunctions.DropLiquid(X, Y, 32);
					if (HackFunctions.GetNetMode() == 1)
						HackFunctions.SendNetWater(X, Y);
				}
				return 1;
			}, null, false);*/
			unsafe
			{
				Button u = null;
				u = AddButton(buttonTabPage6, Lang.randomUUID, 4, (Context) =>
				  {
					  Context.UUID = Guid.NewGuid().ToString();
					  u.Text = Lang.randomUUID + ":" + Context.UUID;
				  }, null, false);
				u.Font = new Font("SimSun", 8);
			}




			AddButton(buttonTabPage3, Lang.toggleDay, 0, (Context) => Context.DayTime = !Context.DayTime, null, false);
			AddButton(buttonTabPage3, Lang.toggleSunDial, 1, (Context) => Context.FastForwardTime = !Context.FastForwardTime, null, false);
			AddButton(buttonTabPage3, Lang.toggleBloodMoon, 2, (Context) => Context.BloodMoon = !Context.BloodMoon, null, false);
			AddButton(buttonTabPage3, Lang.toggleEclipse, 3, (Context) => Context.Eclipse = !Context.Eclipse, null, false);
			AddButton(buttonTabPage3, Lang.snowMoon, 4, (Context) => Context.SnowMoon = !Context.SnowMoon, null, false);
			AddButton(buttonTabPage3, Lang.pumpkinMoon, 5, (Context) => Context.PumpkinMoon = !Context.PumpkinMoon, null, false);


			AddButton(buttonTabPage4, Lang.superRange, 0, Utils.SuperRange_E, Utils.SuperRange_D);
			AddButton(buttonTabPage4, Lang.fastTileSpeed, 1, Utils.FastTileSpeed_E, Utils.FastTileSpeed_D);
			AddButton(buttonTabPage4, Lang.rulerEffect, 2, Utils.RulerEffect_E, Utils.RulerEffect_D);
			AddButton(buttonTabPage4, Lang.machinicalRulerEffect, 3, Utils.MachinicalRulerEffect_E, Utils.MachinicalRulerEffect_D);
			AddButton(buttonTabPage4, Lang.showCircuit, 4, Utils.ShowCircuit_E, Utils.ShowCircuit_D);

			//AddButton(buttonTabPage5, Lang.infernoEffect, 0, HackFunctions.InfernoEffect, HackFunctions.De_InfernoEffect);
			AddButton(buttonTabPage5, Lang.shadowDodge, 1, Utils.ShadowDodge_E, Utils.ShadowDodge_D);

			LoadPlugins();
		}

		private void LoadPlugins()
		{
			if (!Directory.Exists(".\\plugins"))
			{
				Directory.CreateDirectory(".\\plugins");
			}
			Directory.EnumerateFiles(".\\plugins").ToList().ForEach(s =>
			{
				if (s.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
				{
					FileStream fs = File.Open(s, FileMode.Open);
					byte[] b = new byte[fs.Length];
					fs.Read(b, 0, (int)fs.Length);
					Assembly a = Assembly.Load(b);
					foreach (var type in a.GetTypes())
					{
						object[] ca = type.GetCustomAttributes(typeof(PluginInfo), false);
						if (ca.Length != 0)
						{
							var o = (Plugin)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
							Plugins.Add(o);
							o.Loaded();
							break;
						}
					}
				}
			});

		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		public Button AddButton(Panel p, string name, int rank, HackFunc hfunc, HackFunc cancel, bool Closeable = true)
		{
			int height = 30;
			Button b = new Button()
			{
				Text = name,
				Size = new Size(240 + (Closeable ? 0 : 50), height),
				Location = new Point(0, height * rank)
			};
			b.Click += delegate (object sender, EventArgs e)
			 {
				 if (CanHack)
				 {
					 hfunc(Context);
					 status.Text = Lang.sucToHack;
				 }
				 else
				 {
					 status.Text = Lang.faiToHack;
				 }
			 };
			p.Controls.Add(b);
			if (Closeable)
			{
				Button c = new Button();
				c.Text = Lang.off;
				c.Size = new Size(45, height);
				c.Location = new Point(245, height * rank);
				c.Click += delegate (object sender, EventArgs e)
				 {
					 if (CanHack)
					 {
						 cancel(Context);
						 status.Text = Lang.sucToCancel;
					 }
					 else
					 {
						 status.Text = Lang.faiToCancel;
					 }
				 };
				p.Controls.Add(c);
			}
			return b;
		}


		public MainForm()
		{

			BackColor = Color.LightGray;
			cross = (Image)resource.res.GetObject("cross");
			mainWindow = this;
			InitializeComponent();
			InitControls();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!flag)
				e.Graphics.DrawImage(cross, 20, 15, 25, 25);
			e.Graphics.DrawString(Lang.dragTip, new Font("Arial", 10), new SolidBrush(Color.Black), 20 + 25 + 20, 20);


		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.X >= 20 && e.Y >= 15 && e.X <= 20 + 25 && e.Y <= 15 + 25)
			{
				this.Cursor = Cursors.Cross;
			}
			else
			{
				if (!flag)
					this.Cursor = Cursors.Default;
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.X >= 20 && e.Y >= 15 && e.X <= 20 + 25 && e.Y <= 15 + 25)
			{
				flag = true;
				this.Refresh();
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (flag)
			{
				POINT p = new POINT();
				GetCursorPos(out p);
				IntPtr wnd;
				wnd = WindowFromPoint(p.X, p.Y);
				GetWindowThreadProcessId(wnd, out processID);
				hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, processID);
				Context = GameContext.OpenGame(processID);
				if (CanHack)
					status.Text = string.Format(Lang.baseaddr + ":{0:x8}", Context.MyPlayer.BaseAddress);
				else
					status.Text = Lang.faiToGetBase;
				flag = false;
				this.Refresh();
			}
		}
	}
}