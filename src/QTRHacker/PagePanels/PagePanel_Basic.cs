using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileImage.RainbowImage;
using QTRHacker.Configs;
using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PagePanels
{
	public class PagePanel_Basic : PagePanel
	{
		private readonly Panel ButtonsPanel, ContentPanel;
		private readonly Dictionary<Control, int> FunctionsNumber;
		private readonly Panel Page1, Page2, Page3, PageEvent, PageBuilder, PageMisc;
		private int FunctionsIdentity = 0;
		private int TabsNumber = 0;
		public PagePanel_Basic(int Width, int Height) : base(Width, Height)
		{

			FunctionsNumber = new Dictionary<Control, int>();
			ButtonsPanel = new Panel();
			ButtonsPanel.Bounds = new Rectangle(0, 0, 60, Height);
			ButtonsPanel.BackColor = Color.FromArgb(30, 255, 255, 255);
			Controls.Add(ButtonsPanel);

			ContentPanel = new Panel();
			ContentPanel.Bounds = new Rectangle(60, 2, Width - 60, Height - 6);
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

			AddFunctionButton(Page1, HackContext.CurrentLanguage["InfLife"], true, Utils.InfiniteLife_E, Utils.InfiniteLife_D);
			AddFunctionButton(Page1, HackContext.CurrentLanguage["InfOxygen"], true, Utils.InfiniteOxygen_E, Utils.InfiniteOxygen_D);
			AddFunctionButton(Page1, HackContext.CurrentLanguage["InfMinion"], true, Utils.InfiniteMinion_E, Utils.InfiniteMinion_D);
			AddFunctionButton(Page1, HackContext.CurrentLanguage["InfMana"], true, Utils.InfiniteMana_E, Utils.InfiniteMana_D);
			AddFunctionButton(Page1, HackContext.CurrentLanguage["InfItemAmmo"], true, Utils.InfiniteItemAmmo_E, Utils.InfiniteItemAmmo_D);
			AddFunctionButton(Page1, HackContext.CurrentLanguage["InfFly"], true, Utils.InfiniteFly_E, Utils.InfiniteFly_D);
			/*AddFunctionButton(Page1, HackContext.CurrentLanguage["ImmuneDebuff"], true, Utils.ImmuneDebuffs_E, Utils.ImmuneDebuffs_D);
			AddFunctionButton(Page1, HackContext.CurrentLanguage["HighLight"], true, Utils.HighLight_E, Utils.HighLight_D);*/
			AddFunctionButton(Page1, HackContext.CurrentLanguage["GhostMode"], true, Utils.GhostMode_E, Utils.GhostMode_D);

			/*AddFunctionButton(Page2, HackContext.CurrentLanguage["SlowFall"], true, Utils.LowGravity_E, Utils.LowGravity_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["FastSpeed"], true, Utils.FastSpeed_E, Utils.FastSpeed_D);*/
			//AddFunction(Page2, HackContext.CurrentLanguage["ProjectileIgnoreTiles"], true, Utils.ProjectileIgnoreTile_E, Utils.ProjectileIgnoreTile_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["GrabFarAway"], true, Utils.GrabItemFarAway_E, Utils.GrabItemFarAway_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["BonusTwoSlots"], true, Utils.BonusTwoSlots_E, Utils.BonusTwoSlots_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["GoldHoleDropBags"], true, Utils.GoldHoleDropsBag_E, Utils.GoldHoleDropsBag_D);
			//AddFunction(Page2, HackContext.CurrentLanguage["SlimeGunBurnNPCS"], true, Utils.SlimeGunBurn_E, Utils.SlimeGunBurn_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["FishOnlyCrates"], true, Utils.FishOnlyCrates_E, Utils.FishOnlyCrates_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["EnableAllRecipes"], true, Utils.EnableAllRecipes_E, Utils.EnableAllRecipes_D);
			AddFunctionButton(Page2, HackContext.CurrentLanguage["StengthenedVampireKnives"], true, Utils.StrengthenVampireKnives_E, Utils.StrengthenVampireKnives_D);
			//AddFunction(Page2, HackContext.CurrentLanguage["SwingIgnoringTiles"], true, Utils.SwingIgnoringTils_E, Utils.SwingIgnoringTils_D);
			//AddFunctionButton(Page2, HackContext.CurrentLanguage["SwingingAttacksAll"], true, Utils.SwingingAttacksAll_E, Utils.SwingingAttacksAll_D);
			
			AddFunctionButton(Page3, HackContext.CurrentLanguage["BurnAllNPCS"], false,
				g =>
				{
					var npc = HackContext.GameContext.NPC;
					ProgressPopupForm p = new ProgressPopupForm(MainForm.MainFormInstance.Width / 4 * 3, npc.Length, "Waiting...");
					p.Run(MainForm.MainFormInstance, (tick) =>
					{
						int i = 0;
						for (; i < npc.Length; i++)
						{
							if (npc[i].Active)
								npc[i].AddBuff(153, 216000);
							tick(i);
						}
					});
				}, null);
			AddFunctionButton(Page3, HackContext.CurrentLanguage["BurnAllPlayers"], false,
				g =>
				{
					var player = HackContext.GameContext.Players;
					ProgressPopupForm p = new ProgressPopupForm(MainForm.MainFormInstance.Width / 4 * 3, player.Length, "Waiting...");
					p.Run(MainForm.MainFormInstance, (tick) =>
					{
						int i = 0;
						for (; i < player.Length; i++)
						{
							if (player[i].Active)
								player[i].AddBuff(44, 216000);
							tick(i);
						}
					});
				}, null);
			/*AddFunctionButton(Page3, HackContext.CurrentLanguage["PourLavaOntoPlayers"], false, Utils.DropLavaOntoPlayers, null);
			FunctionButton _b = null;
			_b = AddFunctionButton(Page3, HackContext.CurrentLanguage["RandomUUID"], false,
				g =>
				{
					HackContext.GameContext.UUID = Guid.NewGuid().ToString();
					_b.Text = HackContext.GameContext.UUID;
				}, null);*/
			AddFunctionButton(Page3, HackContext.CurrentLanguage["ExploreWholeWorld"], false, Utils.RevealMap, null);
			/*AddFunctionButton(Page3, HackContext.CurrentLanguage["RightClickToTP"], false, Utils.RightClickToTP, null);*/
			AddFunctionButton(Page3, HackContext.CurrentLanguage["ProjectilePuzzle"], false,
				g =>
				{
					OpenFileDialog ofd = new OpenFileDialog();

					ofd.Filter = "png files (*.png)|*.png";
					if (ofd.ShowDialog(this) == DialogResult.OK)
					{
						var ctx = HackContext.GameContext;
						var config = (HackContext.Configs["CFG_ProjDrawer"] as CFG_ProjDrawer);
						ProjImage img = ProjImage.FromImage(ofd.FileName, config.ProjType, config.Resolution);
						Enabled = false;
						var pos = ctx.MyPlayer.Position;
						img.Emit(ctx, new MPointF(pos.X, pos.Y));
						Enabled = true;
					}
				}, null);
			AddFunctionButton(Page3, HackContext.CurrentLanguage["RainbowTexting"], false,
				g =>
				{
					MForm Form = new MForm
					{
						BackColor = Color.FromArgb(90, 90, 90),
						Text = HackContext.CurrentLanguage["RainbowTexting"],
						StartPosition = FormStartPosition.CenterParent,
						ClientSize = new Size(245, 72)
					};

					CheckBox ReloadAllFonts = new CheckBox()
					{
						Text = HackContext.CurrentLanguage["ReloadFonts"],
						Location = new Point(10, 0),
						Size = new Size(200, 20),
					};
					Form.MainPanel.Controls.Add(ReloadAllFonts);

					Label Tip = new Label()
					{
						Text = HackContext.CurrentLanguage["Text"] + "：",
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
					ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
					ConfirmButton.FlatStyle = FlatStyle.Flat;
					ConfirmButton.Size = new Size(65, 20);
					ConfirmButton.Location = new Point(180, 20);
					ConfirmButton.Click += (s1, e1) =>
					{
						if (HackContext.Characters == null || ReloadAllFonts.Checked)
						{
							HackContext.LoadRainbowFonts();
						}
						RainbowTextDrawer rtd = new RainbowTextDrawer(HackContext.Characters);
						rtd.DrawString(Box.Text, center: new MPointF());
						var ctx = HackContext.GameContext;
						var pos = ctx.MyPlayer.Position;
						rtd.Emit(ctx, new MPointF(pos.X, pos.Y));
						Form.Dispose();
					};
					Form.MainPanel.Controls.Add(ConfirmButton);
					Form.Activated += (s, e) =>
					{
						Box.Focus();
					};
					Form.ShowDialog(this);
				}, null);
			AddFunctionButton(Page3, HackContext.CurrentLanguage["CoronaVirus"], false,
				g =>
				{
					var player = g.MyPlayer;
					for (int i = 0; i < 3; i++)
						NPC.NewNPC(g, Convert.ToInt32(player.Position.X), Convert.ToInt32(player.Position.Y) - 2, 51);
					player.AddBuff(44, 600, true);
					player.AddBuff(153, 600, true);
					player.AddBuff(67, 600, true);
					player.AddBuff(24, 600, true);
				}, null);
			//AddFunction(Page3, HackContext.CurrentLanguage["HarpLeftClickTP"], "800DE5238B9475F09B2D49BAD8CF56D6", true, Utils.HarpToTP_E, Utils.HarpToTP_D);

			AddFunctionButton(PageEvent, HackContext.CurrentLanguage["ToggleDayNight"], false, g => g.DayTime = !g.DayTime, null);
			AddFunctionButton(PageEvent, HackContext.CurrentLanguage["ToggleSunDial"], false, g => g.FastForwardTime = !g.FastForwardTime, null);
			AddFunctionButton(PageEvent, HackContext.CurrentLanguage["ToggleBloodMoon"], false, g => g.BloodMoon = !g.BloodMoon, null);
			AddFunctionButton(PageEvent, HackContext.CurrentLanguage["ToggleEclipse"], false, g => g.Eclipse = !g.Eclipse, null);
			AddFunctionButton(PageEvent, HackContext.CurrentLanguage["ToggleSnowMoon"], false, g => g.SnowMoon = !g.SnowMoon, null);
			AddFunctionButton(PageEvent, HackContext.CurrentLanguage["TogglePumpkinMoon"], false, g => g.PumpkinMoon = !g.PumpkinMoon, null);

			/*AddFunctionButton(PageBuilder, HackContext.CurrentLanguage["SuperRange"], true, Utils.SuperRange_E, Utils.SuperRange_D);
			AddFunctionButton(PageBuilder, HackContext.CurrentLanguage["FastTilingAndWallingSpeed"], true, Utils.FastTileAndWallSpeed_E, Utils.FastTileAndWallSpeed_D);
			AddFunctionButton(PageBuilder, HackContext.CurrentLanguage["MechanicalRulerEffect"], true, Utils.MachinicalRulerEffect_E, Utils.MachinicalRulerEffect_D);
			AddFunctionButton(PageBuilder, HackContext.CurrentLanguage["MechanicalGlassesEffect"], true, Utils.ShowCircuit_E, Utils.ShowCircuit_D);

			AddFunctionButton(PageMisc, HackContext.CurrentLanguage["ShadowDodge"], true, Utils.ShadowDodge_E, Utils.ShadowDodge_D);
			AddFunctionButton(PageMisc, HackContext.CurrentLanguage["ShowInvisiblePlayers"], true, Utils.ShowInvisiblePlayers_E, Utils.ShowInvisiblePlayers_D);
			*/
			AddTab(HackContext.CurrentLanguage["Basic_1"], Page1).Selected = true;
			AddTab(HackContext.CurrentLanguage["Basic_2"], Page2);
			AddTab(HackContext.CurrentLanguage["Advanced"], Page3);
			AddTab(HackContext.CurrentLanguage["Event"], PageEvent);
			//AddTab(HackContext.CurrentLanguage["Builder"], PageBuilder);
			//AddTab(HackContext.CurrentLanguage["Miscs"], PageMisc);
		}
		public FunctionButton AddFunctionButton(Panel p, string Text, bool Closable, Action<GameContext> OnEnabled, Action<GameContext> OnDisabled)
		{
			if (!FunctionsNumber.ContainsKey(p))
				FunctionsNumber[p] = 0;

			FunctionButton b = new FunctionButton(FunctionsIdentity++,
				s => HackContext.GameContext.Signs[(s as FunctionButton).Identity] > 0, Closable);
			b.OnEnable += (s, e) =>
			{
#if DEBUG
#else
				if (HackContext.CurrentLanguage.Name == "zh-CN" && !(HackContext.Configs["CFG_QTRHacker"] as CFG_QTRHacker).OnlineMode && HackContext.GameContext.NetMode != 0)
				{
					MessageBox.Show("你无法在多人游戏中使用修改器的'基础功能'，因为在线模式已被关闭\n" +
	  "除非在配置文件./Content/Configs/CFG_QTRHacker.json中将OnlineMode项由false修改为true来开启在线模式\n" +
   "你将为你在多人游戏中使用本修改器做出的任何行为负全责\n" +
   "最后提醒：在多人游戏中使用修改器在大多数时候是不被允许的");
					e.Enabled = false;
					return;
				}
#endif
				var btn = (s as FunctionButton);
				OnEnabled?.Invoke(HackContext.GameContext);
				HackContext.GameContext.Signs[(s as FunctionButton).Identity] = 1;
			};
			b.OnDisable += (s, e) =>
			{
				OnDisabled?.Invoke(HackContext.GameContext);
				HackContext.GameContext.Signs[(s as FunctionButton).Identity] = 0;
			};
			b.Location = new Point(0, 20 * FunctionsNumber[p]);
			b.Text = Text;
			p.Controls.Add(b);
			FunctionsNumber[p]++;
			return b;
		}
		public TextButton AddTab(string Text, Control Content)
		{
			TextButton btn = new()
			{
				Location = new Point(0, 20 * (TabsNumber++)),
				Text = Text,
				ForeColor = Color.White
			};
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
