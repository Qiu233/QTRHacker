using QHackLib.Utilities;
using QTRHacker.Functions;
using QTRHacker.Functions.ProjectileImage;
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

			AddFunction(Page1, "无限生命", "3BD0E7860C0441E2B95E63C3F04B4871", true, Utils.InfiniteLife_E, Utils.InfiniteLife_D);
			AddFunction(Page1, "无限氧气", "37EF93BAB687481F87D4D0F95941C781", true, Utils.InfiniteOxygen_E, Utils.InfiniteOxygen_D);
			AddFunction(Page1, "无限召唤", "D1808E759CBC533C332968E2603376AD", true, Utils.InfiniteMinion_E, Utils.InfiniteMinion_D);
			AddFunction(Page1, "无限魔法", "E8A50F6CC601D8B3977E8E9D4677F6DD", true, Utils.InfiniteMana_E, Utils.InfiniteMana_D);
			AddFunction(Page1, "无限物品/弹药", "830D900B05074CD0A8DADD1D2EB5F6BC", true, Utils.InfiniteAmmo_E, Utils.InfiniteAmmo_D);
			AddFunction(Page1, "无限飞行", "8669ADD79BBDD647B240409BE2094DDB", true, Utils.InfiniteFly_E, Utils.InfiniteFly_D);
			AddFunction(Page1, "免疫负面Buff", "A339FE530BB6693DCA0ABC020138A880", true, Utils.ImmuneDebuffs_E, Utils.ImmuneDebuffs_D);
			AddFunction(Page1, "全地图高亮", "800DE5238B9475F09B2D49BAD8CF56D6", true, Utils.HighLight_E, Utils.HighLight_D);
			AddFunction(Page1, "幽灵模式", "BC5F7996B2B89012715CFBCDBF9434CB", true, Utils.GhostMode_E, Utils.GhostMode_D);

			AddFunction(Page2, "减慢下落速度", "84E55372621C1D6FD4389456C0D64C33", true, Utils.LowGravity_E, Utils.LowGravity_D);
			AddFunction(Page2, "加快移动速度", "5B9B9517D95F6F86CCE0D6F0CF16EDAA", true, Utils.FastSpeed_E, Utils.FastSpeed_D);
			AddFunction(Page2, "弹幕穿透方块", "9E594D35FA0657AFA83604C40FF88C76", true, Utils.ProjectileIgnoreTile_E, Utils.ProjectileIgnoreTile_D);
			AddFunction(Page2, "远距离拾取物品", "BDBFD8A007CA446B01FC3D5DA7BA8560", true, Utils.GrabItemFarAway_E, Utils.GrabItemFarAway_D);
			AddFunction(Page2, "额外两个饰品栏", "265E7EE043E026964283772D666DD914", true, Utils.BonusTwoSlots_E, Utils.BonusTwoSlots_D);
			AddFunction(Page2, "金洞掉落银财宝袋(金洞弹幕为518)", "4FB4064F1FEB6E155313756DCBB82203", true, Utils.GoldHoleDropsBag_E, Utils.GoldHoleDropsBag_D);
			AddFunction(Page2, "史莱姆枪燃烧NPC", "23280EF2B335403BEF698C2F6AB9CB8A", true, Utils.SlimeGunBurn_E, Utils.SlimeGunBurn_D);
			AddFunction(Page2, "只钓板条箱", "77D322DE31DDE8C328C44A8A7E63AD04", true, Utils.FishOnlyCrates_E, Utils.FishOnlyCrates_D);
			AddFunction(Page2, "全物品合成", "DA840529BCE9704EF4F1BA5CB6C6ECD4", true, Utils.EnableAllRecipes_E, Utils.EnableAllRecipes_D);
			AddFunction(Page2, "加强吸血飞刀", "418C48524EA50ECC6717C5D629AF7B32", true, Utils.StrengthenVampireKnives_E, Utils.StrengthenVampireKnives_D);

			AddFunction(Page3, "燃烧所有NPC", "9A3F870D0DDB5B46F6E1B1266D6882AD", false,
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
								npc[i].AddBuff(0x99, 216000);
						MainForm.MainFormInstance.Enabled = true;
					}).Start();
				}, null);
			AddFunction(Page3, "燃烧玩家", "37EF93BAB687481F87D4D0F95941C781", false,
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
						var player = HackContext.GameContext.Players;
						for (; i < Player.MAX_PLAYER; i++)
							if (player[i].Active)
								player[i].AddBuff(44, 216000);
						MainForm.MainFormInstance.Enabled = true;
					}).Start();
				}, null);
			AddFunction(Page3, "在所有玩家头上倒岩浆", "D1808E759CBC533C332968E2603376AD", false, Utils.DropLavaOntoPlayers, null);
			FunctionButton _b = null;
			_b = AddFunction(Page3, "随机UUID", "E8A50F6CC601D8B3977E8E9D4677F6DD", false,
				g =>
				{
					HackContext.GameContext.UUID = Guid.NewGuid().ToString();
					_b.Text = HackContext.GameContext.UUID;
				}, null);
			AddFunction(Page3, "探索全地图", "830D900B05074CD0A8DADD1D2EB5F6BC", false, Utils.RevealMap, null);
			AddFunction(Page3, "右键大地图传送", "8669ADD79BBDD647B240409BE2094DDB", false, Utils.RightClickToTP, null);
			AddFunction(Page3, "弹幕拼图", "A339FE530BB6693DCA0ABC020138A880", false,
				g =>
				{
					OpenFileDialog ofd = new OpenFileDialog();

					ofd.Filter = "png files (*.png)|*.png";
					if (ofd.ShowDialog(this) == DialogResult.OK)
					{
						var ctx = HackContext.GameContext;
						ProjImage img = ProjImage.FromImage(ofd.FileName, MainForm.Config_ProjDrawer.ProjType, MainForm.Config_ProjDrawer.Resolution);
						this.Enabled = false;
						img.Emit(ctx, ctx.MyPlayer.X, ctx.MyPlayer.Y);
						this.Enabled = true;
					}
				}, null);
			AddFunction(Page3, "竖琴左键传送", "800DE5238B9475F09B2D49BAD8CF56D6", true, Utils.HarpToTP_E, Utils.HarpToTP_D);

			AddFunction(PageEvent, "昼夜更替", "E6F33D980A95291E6D0C0033F39E6629", false, g => g.DayTime = !g.DayTime, null);
			AddFunction(PageEvent, "开/关 日晷", "6FFF093BA44EEFD41C9E3585FA18EBBA", false, g => g.FastForwardTime = !g.FastForwardTime, null);
			AddFunction(PageEvent, "开/关 血月", "C8DE8467590D278A312E9C5A0A63DF84", false, g => g.BloodMoon = !g.BloodMoon, null);
			AddFunction(PageEvent, "开/关 日食", "DA4EFFA8EAC423555CF33536D6851570", false, g => g.Eclipse = !g.Eclipse, null);
			AddFunction(PageEvent, "开/关 霜月", "A74C10BB62FE043E1924AB2267570C42", false, g => g.SnowMoon = !g.SnowMoon, null);
			AddFunction(PageEvent, "开/关 南瓜月", "192BB3BD522421DC4AC18EC61800EE04", false, g => g.PumpkinMoon = !g.PumpkinMoon, null);

			AddFunction(PageBuilder, "超远距离", "2045F3ED1E8276545D86390FFFA9B02E", true, Utils.SuperRange_E, Utils.SuperRange_D);
			AddFunction(PageBuilder, "极快的方块放置速度", "B5892B95C1D1C38DA5E8E0499E34235D", true, Utils.FastTileSpeed_E, Utils.FastTileSpeed_D);
			AddFunction(PageBuilder, "标尺：显示与玩家的相对坐标", "6FFA25DEFAFB8DD7EE02BC7179509859", true, Utils.RulerEffect_E, Utils.RulerEffect_D);
			AddFunction(PageBuilder, "机械尺：在屏幕上显示方块格子", "B84EB7D6788F944E81654735A22DB528", true, Utils.MachinicalRulerEffect_E, Utils.MachinicalRulerEffect_D);
			AddFunction(PageBuilder, "机械眼镜：在屏幕上显示电路", "62DC9F0484D233B9FB54A6BF15521354", true, Utils.ShowCircuit_E, Utils.ShowCircuit_D);

			AddFunction(PageMisc, "影分身", "4C930E9DB193D5E61AFEDB24E8D5392E", true, Utils.ShadowDodge_E, Utils.ShadowDodge_D);
			AddFunction(PageMisc, "显示隐身的玩家", "1EE572824476A3BBB1AA367E70BFF790", true, Utils.ShowInvisiblePlayers_E, Utils.ShowInvisiblePlayers_D);

			AddTab("基础1", Page1).Selected = true;
			AddTab("基础2", Page2);
			AddTab("高级", Page3);
			AddTab("事件", PageEvent);
			AddTab("建筑师", PageBuilder);
			AddTab("杂项", PageMisc);
		}
		public FunctionButton AddFunction(Panel p, string Text, string Iden, bool Closable, Action<GameContext> OnEnabled, Action<GameContext> OnDisabled)
		{
			if (!FunctionsNumber.ContainsKey(p))
				FunctionsNumber[p] = 0;

			FunctionButton b = new FunctionButton(Iden,
				s => HackContext.GetSign((s as FunctionButton).Identity) > 0, Closable);
			b.OnEnable += (s, e) =>
			{
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
