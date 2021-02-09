using QHackLib.Utilities;
using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileImage.RainbowImage;
using QTRHacker.NewDimension.Configs;
using QTRHacker.NewDimension.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_Basic : PagePanel
	{
		private readonly Panel ButtonsPanel, ContentPanel;
		private readonly Dictionary<Control, int> FunctionsNumber;
		private readonly Panel Page1, Page2, Page3, PageEvent, PageBuilder, PageMisc;
		//private readonly Dictionary<>
		private int ButtonsNumber = 0;
		public PagePanel_Basic(int Width, int Height) : base(Width, Height)
		{
			FunctionsNumber = new Dictionary<Control, int>();
			ButtonsPanel = new Panel();
			ButtonsPanel.Bounds = new Rectangle(0, 0, 60, Height);
			ButtonsPanel.BackColor = Color.FromArgb(30, 255, 255, 255);
			Controls.Add(ButtonsPanel);

			ContentPanel = new Panel();
			ContentPanel.Bounds = new Rectangle(60, 3, Width - 60, Height - 6);
			ContentPanel.BackColor = TextButton.SelectedColor;
			Controls.Add(ContentPanel);

			Page1 = new Panel();
			Page1.Bounds = new Rectangle(3, 0, Width - 60, Height);
			Page2 = new Panel();
			Page2.Bounds = new Rectangle(3, 0, Width - 60, Height);
			Page3 = new Panel();
			Page3.Bounds = new Rectangle(3, 0, Width - 60, Height);
			PageEvent = new Panel();
			PageEvent.Bounds = new Rectangle(3, 0, Width - 60, Height);
			PageBuilder = new Panel();
			PageBuilder.Bounds = new Rectangle(3, 0, Width - 60, Height);
			PageMisc = new Panel();
			PageMisc.Bounds = new Rectangle(3, 0, Width - 60, Height);

			AddFunction(Page1, MainForm.CurrentLanguage["InfLife"], "3BD0E7860C0441E2B95E63C3F04B4871", true, Utils.InfiniteLife_E, Utils.InfiniteLife_D);
			AddFunction(Page1, MainForm.CurrentLanguage["InfOxygen"], "37EF93BAB687481F87D4D0F95941C781", true, Utils.InfiniteOxygen_E, Utils.InfiniteOxygen_D);
			AddFunction(Page1, MainForm.CurrentLanguage["InfMinion"], "D1808E759CBC533C332968E2603376AD", true, Utils.InfiniteMinion_E, Utils.InfiniteMinion_D);
			AddFunction(Page1, MainForm.CurrentLanguage["InfMana"], "E8A50F6CC601D8B3977E8E9D4677F6DD", true, Utils.InfiniteMana_E, Utils.InfiniteMana_D);
			AddFunction(Page1, MainForm.CurrentLanguage["InfItemAmmo"], "830D900B05074CD0A8DADD1D2EB5F6BC", true, Utils.InfiniteItemAmmo_E, Utils.InfiniteItemAmmo_D);
			AddFunction(Page1, MainForm.CurrentLanguage["InfFly"], "8669ADD79BBDD647B240409BE2094DDB", true, Utils.InfiniteFly_E, Utils.InfiniteFly_D);
			AddFunction(Page1, MainForm.CurrentLanguage["ImmuneDebuff"], "A339FE530BB6693DCA0ABC020138A880", true, Utils.ImmuneDebuffs_E, Utils.ImmuneDebuffs_D);
			AddFunction(Page1, MainForm.CurrentLanguage["HighLight"], "800DE5238B9475F09B2D49BAD8CF56D6", true, Utils.HighLight_E, Utils.HighLight_D);
			AddFunction(Page1, MainForm.CurrentLanguage["GhostMode"], "BC5F7996B2B89012715CFBCDBF9434CB", true, Utils.GhostMode_E, Utils.GhostMode_D);

			AddFunction(Page2, MainForm.CurrentLanguage["SlowFall"], "84E55372621C1D6FD4389456C0D64C33", true, Utils.LowGravity_E, Utils.LowGravity_D);
			AddFunction(Page2, MainForm.CurrentLanguage["FastSpeed"], "5B9B9517D95F6F86CCE0D6F0CF16EDAA", true, Utils.FastSpeed_E, Utils.FastSpeed_D);
			//AddFunction(Page2, MainForm.CurrentLanguage["ProjectileIgnoreTiles"], "9E594D35FA0657AFA83604C40FF88C76", true, Utils.ProjectileIgnoreTile_E, Utils.ProjectileIgnoreTile_D);
			AddFunction(Page2, MainForm.CurrentLanguage["GrabFarAway"], "BDBFD8A007CA446B01FC3D5DA7BA8560", true, Utils.GrabItemFarAway_E, Utils.GrabItemFarAway_D);
			AddFunction(Page2, MainForm.CurrentLanguage["BonusTwoSlots"], "265E7EE043E026964283772D666DD914", true, Utils.BonusTwoSlots_E, Utils.BonusTwoSlots_D);
			AddFunction(Page2, MainForm.CurrentLanguage["GoldHoleDropBags"], "4FB4064F1FEB6E155313756DCBB82203", true, Utils.GoldHoleDropsBag_E, Utils.GoldHoleDropsBag_D);
			//AddFunction(Page2, MainForm.CurrentLanguage["SlimeGunBurnNPCS"], "23280EF2B335403BEF698C2F6AB9CB8A", true, Utils.SlimeGunBurn_E, Utils.SlimeGunBurn_D);
			AddFunction(Page2, MainForm.CurrentLanguage["FishOnlyCrates"], "77D322DE31DDE8C328C44A8A7E63AD04", true, Utils.FishOnlyCrates_E, Utils.FishOnlyCrates_D);
			AddFunction(Page2, MainForm.CurrentLanguage["EnableAllRecipes"], "DA840529BCE9704EF4F1BA5CB6C6ECD4", true, Utils.EnableAllRecipes_E, Utils.EnableAllRecipes_D);
			AddFunction(Page2, MainForm.CurrentLanguage["StengthenedVampireKnives"], "418C48524EA50ECC6717C5D629AF7B32", true, Utils.StrengthenVampireKnives_E, Utils.StrengthenVampireKnives_D);
			//AddFunction(Page2, MainForm.CurrentLanguage["SwingIgnoringTiles"], "4B246B44592B441C9DC23E9EBC077A98", true, Utils.SwingIgnoringTils_E, Utils.SwingIgnoringTils_D);
			AddFunction(Page2, MainForm.CurrentLanguage["SwingingAttacksAll"], "1D2F13AE9E084743898A11EE64D744D2", true, Utils.SwingingAttacksAll_E, Utils.SwingingAttacksAll_D);

			AddFunction(Page3, MainForm.CurrentLanguage["BurnAllNPCS"], "9A3F870D0DDB5B46F6E1B1266D6882AD", false,
				g =>
				{
					int i = 0;
					PopupProgressBar p = new PopupProgressBar();
					p.MainProgressBar.Maximum = NPC.MAXNUMBER;
					System.Timers.Timer timer = new System.Timers.Timer(1);
					timer.Elapsed += (sender, e) =>
					{
						var b = p.MainProgressBar;
						b.Value = i;
						b.Invalidate();
						b.Text = b.Value + "/" + b.Maximum;
						if (i >= b.Maximum) p.Dispose();
					};
					timer.Start();
					p.Show();
					p.Location = new Point(MainForm.MainFormInstance.Location.X + MainForm.MainFormInstance.Width / 2 - p.ClientSize.Width / 2, MainForm.MainFormInstance.Location.Y + MainForm.MainFormInstance.Height / 2 - p.ClientSize.Height / 2);
					new Thread(() =>
					{
						MainForm.MainFormInstance.Enabled = false;
						var npc = HackContext.GameContext.NPC;
						for (; i < NPC.MAXNUMBER; i++)
							if (npc[i].Active)
								npc[i].AddBuff(153, 216000);
						MainForm.MainFormInstance.Enabled = true;
					}).Start();
				}, null);
			AddFunction(Page3, MainForm.CurrentLanguage["BurnAllPlayers"], "37EF93BAB687481F87D4D0F95941C781", false,
				g =>
				{
					int i = 0;
					PopupProgressBar p = new PopupProgressBar();
					p.MainProgressBar.Maximum = Player.MAXNUMBER;
					System.Timers.Timer timer = new System.Timers.Timer(1);
					timer.Elapsed += (sender, e) =>
					{
						var b = p.MainProgressBar;
						b.Value = i;
						b.Invalidate();
						b.Text = b.Value + "/" + b.Maximum;
						if (i >= b.Maximum) p.Dispose();
					};
					timer.Start();
					p.Show();
					p.Location = new Point(MainForm.MainFormInstance.Location.X + MainForm.MainFormInstance.Width / 2 - p.ClientSize.Width / 2, MainForm.MainFormInstance.Location.Y + MainForm.MainFormInstance.Height / 2 - p.ClientSize.Height / 2);
					new Thread(() =>
					{
						MainForm.MainFormInstance.Enabled = false;
						var player = HackContext.GameContext.Players;
						for (; i < Player.MAX_PLAYER; i++)
							if (player[i].Active)
								player[i].AddBuff(44, 216000);
						MainForm.MainFormInstance.Enabled = true;
					}).Start();
				}, null);
			AddFunction(Page3, MainForm.CurrentLanguage["PourLavaOntoPlayers"], "D1808E759CBC533C332968E2603376AD", false, Utils.DropLavaOntoPlayers, null);
			FunctionButton _b = null;
			_b = AddFunction(Page3, MainForm.CurrentLanguage["RandomUUID"], "E8A50F6CC601D8B3977E8E9D4677F6DD", false,
				g =>
				{
					HackContext.GameContext.UUID = Guid.NewGuid().ToString();
					_b.Text = HackContext.GameContext.UUID;
				}, null);
			AddFunction(Page3, MainForm.CurrentLanguage["ExploreWholeWorld"], "830D900B05074CD0A8DADD1D2EB5F6BC", false, Utils.RevealMap, null);
			AddFunction(Page3, MainForm.CurrentLanguage["RightClickToTP"], "8669ADD79BBDD647B240409BE2094DDB", false, Utils.RightClickToTP, null);
			AddFunction(Page3, MainForm.CurrentLanguage["ProjectilePuzzle"], "A339FE530BB6693DCA0ABC020138A880", false,
				g =>
				{
					OpenFileDialog ofd = new OpenFileDialog();

					ofd.Filter = "png files (*.png)|*.png";
					if (ofd.ShowDialog(this) == DialogResult.OK)
					{
						var ctx = HackContext.GameContext;
						var config = (MainForm.Configs["CFG_ProjDrawer"] as CFG_ProjDrawer);
						ProjImage img = ProjImage.FromImage(ofd.FileName, config.ProjType, config.Resolution);
						this.Enabled = false;
						img.Emit(ctx, new MPointF(ctx.MyPlayer.X, ctx.MyPlayer.Y));
						this.Enabled = true;
					}
				}, null);
			AddFunction(Page3, MainForm.CurrentLanguage["RainbowTexting"], "89494BDF0C804010910932F71E5EC75E", false,
				g =>
				{
					MForm Form = new MForm
					{
						BackColor = Color.FromArgb(90, 90, 90),
						Text = MainForm.CurrentLanguage["RainbowTexting"],
						StartPosition = FormStartPosition.CenterParent,
						ClientSize = new Size(245, 72)
					};

					CheckBox ReloadAllFonts = new CheckBox()
					{
						Text = MainForm.CurrentLanguage["ReloadFonts"],
						Location = new Point(10, 0),
						Size = new Size(200, 20),
					};
					Form.MainPanel.Controls.Add(ReloadAllFonts);

					Label Tip = new Label()
					{
						Text = MainForm.CurrentLanguage["Text"] + "：",
						Location = new Point(0, 20),
						Size = new Size(80, 20),
						TextAlign = ContentAlignment.MiddleCenter
					};
					Form.MainPanel.Controls.Add(Tip);

					TextBox Box = new TextBox
					{
						BorderStyle = BorderStyle.FixedSingle,
						BackColor = Color.FromArgb(120, 120, 120),
						Text = "",
						Location = new Point(85, 20),
						Size = new Size(95, 20),
						ImeMode = ImeMode.On
					};
					Form.MainPanel.Controls.Add(Box);

					Button ConfirmButton = new Button();
					ConfirmButton.Text = MainForm.CurrentLanguage["Confirm"];
					ConfirmButton.FlatStyle = FlatStyle.Flat;
					ConfirmButton.Size = new Size(65, 20);
					ConfirmButton.Location = new Point(180, 20);
					ConfirmButton.Click += (s1, e1) =>
					{
						if (HackContext.Characters == null)
						{
							HackContext.Characters = new Dictionary<char, ProjImage>();
							HackContext.LoadRainbowFonts(HackContext.Characters);
						}
						if (ReloadAllFonts.Checked)
						{
							HackContext.Characters.Clear();
							HackContext.LoadRainbowFonts(HackContext.Characters);
						}
						RainbowTextDrawer rtd = new RainbowTextDrawer(HackContext.Characters);
						rtd.DrawString(Box.Text, center: new MPointF());
						var ctx = HackContext.GameContext;
						var player = ctx.MyPlayer;
						rtd.Emit(ctx, new MPointF(player.X, player.Y));
						Form.Dispose();
					};
					Form.MainPanel.Controls.Add(ConfirmButton);
					Form.Activated += (s, e) =>
					{
						Box.Focus();
					};
					Form.ShowDialog(this);
				}, null);
			AddFunction(Page3, MainForm.CurrentLanguage["CoronaVirus"], "7FF4CE10ED9B4924BA63F92E2443C79A", false,
				g =>
				{
					var player = g.MyPlayer;
					for (int i = 0; i < 3; i++)
						NPC.NewNPC(g, Convert.ToInt32(player.X), Convert.ToInt32(player.Y) - 2, 51);
					player.AddBuff(44, 600, true);
					player.AddBuff(153, 600, true);
					player.AddBuff(67, 600, true);
					player.AddBuff(24, 600, true);
				}, null);
			//AddFunction(Page3, MainForm.CurrentLanguage["HarpLeftClickTP"], "800DE5238B9475F09B2D49BAD8CF56D6", true, Utils.HarpToTP_E, Utils.HarpToTP_D);

			AddFunction(PageEvent, MainForm.CurrentLanguage["ToggleDayNight"], "E6F33D980A95291E6D0C0033F39E6629", false, g => g.DayTime = !g.DayTime, null);
			AddFunction(PageEvent, MainForm.CurrentLanguage["ToggleSunDial"], "6FFF093BA44EEFD41C9E3585FA18EBBA", false, g => g.FastForwardTime = !g.FastForwardTime, null);
			AddFunction(PageEvent, MainForm.CurrentLanguage["ToggleBloodMoon"], "C8DE8467590D278A312E9C5A0A63DF84", false, g => g.BloodMoon = !g.BloodMoon, null);
			AddFunction(PageEvent, MainForm.CurrentLanguage["ToggleEclipse"], "DA4EFFA8EAC423555CF33536D6851570", false, g => g.Eclipse = !g.Eclipse, null);
			AddFunction(PageEvent, MainForm.CurrentLanguage["ToggleSnowMoon"], "A74C10BB62FE043E1924AB2267570C42", false, g => g.SnowMoon = !g.SnowMoon, null);
			AddFunction(PageEvent, MainForm.CurrentLanguage["TogglePumpkinMoon"], "192BB3BD522421DC4AC18EC61800EE04", false, g => g.PumpkinMoon = !g.PumpkinMoon, null);

			AddFunction(PageBuilder, MainForm.CurrentLanguage["SuperRange"], "2045F3ED1E8276545D86390FFFA9B02E", true, Utils.SuperRange_E, Utils.SuperRange_D);
			AddFunction(PageBuilder, MainForm.CurrentLanguage["FastTilingAndWallingSpeed"], "B5892B95C1D1C38DA5E8E0499E34235D", true, Utils.FastTileAndWallSpeed_E, Utils.FastTileAndWallSpeed_D);
			AddFunction(PageBuilder, MainForm.CurrentLanguage["MechanicalRulerEffect"], "B84EB7D6788F944E81654735A22DB528", true, Utils.MachinicalRulerEffect_E, Utils.MachinicalRulerEffect_D);
			AddFunction(PageBuilder, MainForm.CurrentLanguage["MechanicalGlassesEffect"], "62DC9F0484D233B9FB54A6BF15521354", true, Utils.ShowCircuit_E, Utils.ShowCircuit_D);

			AddFunction(PageMisc, MainForm.CurrentLanguage["ShadowDodge"], "4C930E9DB193D5E61AFEDB24E8D5392E", true, Utils.ShadowDodge_E, Utils.ShadowDodge_D);
			AddFunction(PageMisc, MainForm.CurrentLanguage["ShowInvisiblePlayers"], "1EE572824476A3BBB1AA367E70BFF790", true, Utils.ShowInvisiblePlayers_E, Utils.ShowInvisiblePlayers_D);

			AddTab(MainForm.CurrentLanguage["Basic_1"], Page1).Selected = true;
			AddTab(MainForm.CurrentLanguage["Basic_2"], Page2);
			AddTab(MainForm.CurrentLanguage["Advanced"], Page3);
			AddTab(MainForm.CurrentLanguage["Event"], PageEvent);
			AddTab(MainForm.CurrentLanguage["Builder"], PageBuilder);
			AddTab(MainForm.CurrentLanguage["Miscs"], PageMisc);
		}
		public FunctionButton AddFunction(Panel p, string Text, string Iden, bool Closable, Action<GameContext> OnEnabled, Action<GameContext> OnDisabled)
		{
			if (!FunctionsNumber.ContainsKey(p))
				FunctionsNumber[p] = 0;

			FunctionButton b = new FunctionButton(Iden,
				s => HackContext.GetSign((s as FunctionButton).Identity) > 0, Closable);
			b.OnEnable += (s, e) =>
			{
#if DEBUG || ENG
#else
				if (!(MainForm.Configs["CFG_QTRHacker"] as CFG_QTRHacker).OnlineMode && HackContext.GameContext.NetMode != 0)
				{
					MessageBox.Show("你无法在多人游戏中使用修改器的'基础功能'\n" +
						"因为在线模式已被关闭\n" +
	  "除非在配置文件./configs/CFG_QTRHacker.json中将OnlineMode项由false修改为true来开启在线模式\n" +
   "而在线模式一旦开启，则代表你将为你使用本修改器做出的任何行为负全责\n" +
   "这并不代表在线模式处于关闭的情况下有除你之外的其他人需要为你使用修改器所做出的行为负责\n" +
   "最后，请记住，在多人游戏中使用修改器在大多数时候是不被允许的");
					e.Enabled = false;
					return;
				}
#endif
				var btn = (s as FunctionButton);
				OnEnabled?.Invoke(HackContext.GameContext);
				HackContext.SetSign((s as FunctionButton).Identity, 1);
			};
			b.OnDisable += (s, e) =>
			{
				OnDisabled?.Invoke(HackContext.GameContext);
				HackContext.SetSign((s as FunctionButton).Identity, 0);
			};
			b.Location = new Point(0, 20 * FunctionsNumber[p]);
			b.Text = Text;
			p.Controls.Add(b);
			FunctionsNumber[p]++;
			return b;
		}
		public TextButton AddTab(string Text, Control Content)
		{
			TextButton btn = new TextButton();
			btn.Location = new Point(0, 20 * (ButtonsNumber++));
			btn.Text = Text;
			btn.ForeColor = Color.White;
			btn.OnSelected += (s, e) =>
			{
				TextButton ts = (s as TextButton);
				if (!ts.Selected)
					return;
				foreach (var i in ButtonsPanel.Controls)
					if (i is TextButton && i != ts)
						(i as TextButton).Selected = false;
				if (Content == null)
					return;
				ContentPanel.Controls.Clear();
				ContentPanel.Controls.Add(Content);
			};
			btn.Click += (s, e) =>
			{
				TextButton ts = (s as TextButton);
				if (ts.Selected)
					return;
				ts.Selected = true;
			};
			ButtonsPanel.Controls.Add(btn);
			return btn;
		}
	}
}
