using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.PlayerEditor;
using QTRHacker.NewDimension.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_Player : PagePanel
	{
		private readonly MListView PlayerListView;
		private readonly Timer UpdatePlayerTimer;
		private readonly Button EditPlayerInfoButton, TpToPlayerButton,
			AddBuffButton, SetPetButton, SetMountButton;
		private readonly Panel PlayerAttributePanel;
		private readonly InfoView PlayerNameInfoView, PlayerLifeInfoView, PlayerManaInfoView,
			PlayerMaxLifeInfoView, PlayerMaxManaInfoView, PlayerXInfoView, PlayerYInfoView,
			PlayerInventoryBaseAddressInfoView;
		private int PlayerAttributeNumbers = 0;
		public PagePanel_Player(int Width, int Height) : base(Width, Height)
		{
			EditPlayerInfoButton = new Button();
			EditPlayerInfoButton.Enabled = false;
			EditPlayerInfoButton.FlatStyle = FlatStyle.Flat;
			EditPlayerInfoButton.Text = "编辑玩家";
			EditPlayerInfoButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			EditPlayerInfoButton.Bounds = new Rectangle(215, 3, 80, 30);
			EditPlayerInfoButton.Click += (s, e) =>
			{
				int i = Convert.ToInt32(PlayerListView.SelectedItems[0].Text);
				PlayerEditorForm f = new PlayerEditorForm(HackContext.GameContext.Players[i], i == HackContext.GameContext.MyPlayerIndex);
				f.Show();
			};
			Controls.Add(EditPlayerInfoButton);

			TpToPlayerButton = new Button();
			TpToPlayerButton.Enabled = false;
			TpToPlayerButton.FlatStyle = FlatStyle.Flat;
			TpToPlayerButton.Text = "传送到";
			TpToPlayerButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			TpToPlayerButton.Bounds = new Rectangle(215, 33, 80, 30);
			TpToPlayerButton.Click += (s, e) =>
			{
				var p = HackContext.GameContext.Players[Convert.ToInt32(PlayerListView.SelectedItems[0].Text)];
				var mp = HackContext.GameContext.MyPlayer;
				mp.X = p.X;
				mp.Y = p.Y;
			};
			Controls.Add(TpToPlayerButton);

			AddBuffButton = new Button();
			AddBuffButton.Enabled = false;
			AddBuffButton.FlatStyle = FlatStyle.Flat;
			AddBuffButton.Text = "添加Buff";
			AddBuffButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			AddBuffButton.Bounds = new Rectangle(215, 63, 80, 30);
			Controls.Add(AddBuffButton);
			AddBuffButton.Click += (s, e) =>
			{
				var ps = PlayerListView.SelectedIndices;
				if (ps.Count == 0) return;

				MForm AddBuffMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = "添加Buff",
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 72)
				};

				Label BuffTypeTip = new Label()
				{
					Text = "Buff类型",
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				AddBuffMForm.MainPanel.Controls.Add(BuffTypeTip);

				TextBox BuffID = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "0",
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				BuffID.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				AddBuffMForm.MainPanel.Controls.Add(BuffID);


				Label BuffTimeTip = new Label()
				{
					Text = "持续时间",
					Location = new Point(0, 20),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				AddBuffMForm.MainPanel.Controls.Add(BuffTimeTip);

				TextBox BuffTime = new TextBox
				{
					BorderStyle = BorderStyle.FixedSingle,
					BackColor = Color.FromArgb(120, 120, 120),
					Text = "0",
					Location = new Point(85, 20),
					Size = new Size(95, 20)
				};
				BuffTime.KeyPress += (s1, e1) => e1.Handled = e1.Handled || (!Char.IsNumber(e1.KeyChar) && e1.KeyChar != 8 && e1.KeyChar != '-');
				AddBuffMForm.MainPanel.Controls.Add(BuffTime);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = "确定";
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 40);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					HackContext.GameContext.Players[ps[0]].AddBuff(Convert.ToInt32(BuffID.Text), Convert.ToInt32(BuffTime.Text), false);
					AddBuffMForm.Dispose();
				};
				AddBuffMForm.MainPanel.Controls.Add(ConfirmButton);
				AddBuffMForm.ShowDialog(this);
			};

			SetPetButton = new Button();
			SetPetButton.Enabled = false;
			SetPetButton.FlatStyle = FlatStyle.Flat;
			SetPetButton.Text = "设置宠物";
			SetPetButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			SetPetButton.Bounds = new Rectangle(215, 93, 80, 30);
			Controls.Add(SetPetButton);
			SetPetButton.Click += (s, e) =>
			{
				var ps = PlayerListView.SelectedIndices;
				if (ps.Count == 0) return;

				MForm SetPetMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = "设置宠物",
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label PetTypeTip = new Label()
				{
					Text = "宠物类型",
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				SetPetMForm.MainPanel.Controls.Add(PetTypeTip);

				ComboBox PetComboBox = new ComboBox()
				{
					DropDownStyle = ComboBoxStyle.DropDownList,
					DropDownHeight = 150,
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				foreach (var o in GameResLoader.Pets)
					PetComboBox.Items.Add(o);
				PetComboBox.SelectedIndex = 0;
				SetPetMForm.MainPanel.Controls.Add(PetComboBox);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = "确定";
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 20);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					HackContext.GameContext.Players[ps[0]].AddBuff(GetPetFromIndex(PetComboBox.SelectedIndex), 18000, false);
					SetPetMForm.Dispose();
				};
				SetPetMForm.MainPanel.Controls.Add(ConfirmButton);
				SetPetMForm.ShowDialog(this);
			};

			SetMountButton = new Button();
			SetMountButton.Enabled = false;
			SetMountButton.FlatStyle = FlatStyle.Flat;
			SetMountButton.Text = "设置坐骑";
			SetMountButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			SetMountButton.Bounds = new Rectangle(215, 123, 80, 30);
			Controls.Add(SetMountButton);
			SetMountButton.Click += (s, e) =>
			{
				var ps = PlayerListView.SelectedIndices;
				if (ps.Count == 0) return;

				MForm SetMountMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = "设置坐骑",
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label MountTypeTip = new Label()
				{
					Text = "坐骑类型",
					Location = new Point(0, 0),
					Size = new Size(80, 20),
					TextAlign = ContentAlignment.MiddleCenter
				};
				SetMountMForm.MainPanel.Controls.Add(MountTypeTip);

				ComboBox MountComboBox = new ComboBox()
				{
					DropDownStyle = ComboBoxStyle.DropDownList,
					DropDownHeight = 150,
					Location = new Point(85, 0),
					Size = new Size(95, 20)
				};
				foreach (var o in GameResLoader.Mounts)
					MountComboBox.Items.Add(o);
				MountComboBox.SelectedIndex = 0;
				SetMountMForm.MainPanel.Controls.Add(MountComboBox);

				Button ConfirmButton = new Button();
				ConfirmButton.Text = "确定";
				ConfirmButton.FlatStyle = FlatStyle.Flat;
				ConfirmButton.Size = new Size(65, 20);
				ConfirmButton.Location = new Point(180, 0);
				ConfirmButton.Click += (s1, e1) =>
				{
					HackContext.GameContext.Players[ps[0]].AddBuff(GetMountFromIndex(MountComboBox.SelectedIndex), 18000, false);
					SetMountMForm.Dispose();
				};
				SetMountMForm.MainPanel.Controls.Add(ConfirmButton);
				SetMountMForm.ShowDialog(this);
			};

			PlayerAttributePanel = new Panel();
			PlayerAttributePanel.Bounds = new Rectangle(3, 203, 210, 165);
			PlayerAttributePanel.BorderStyle = BorderStyle.FixedSingle;
			Controls.Add(PlayerAttributePanel);

			PlayerNameInfoView = AddPlayerAttribute("玩家名称", 120);
			PlayerLifeInfoView = AddPlayerAttribute("生命值", 120);
			PlayerManaInfoView = AddPlayerAttribute("魔法值", 120);
			PlayerMaxLifeInfoView = AddPlayerAttribute("最大生命值", 120);
			PlayerMaxManaInfoView = AddPlayerAttribute("最大魔法值", 120);
			PlayerXInfoView = AddPlayerAttribute("X坐标", 120);
			PlayerYInfoView = AddPlayerAttribute("Y坐标", 120);
			PlayerInventoryBaseAddressInfoView = AddPlayerAttribute("玩家背包基址", 120);


			PlayerListView = new MListView();
			PlayerListView.SelectedIndexChanged += (s, e) =>
			{
				var m = s as MListView;
				if (m.SelectedIndices.Count > 0)
				{
					EditPlayerInfoButton.Enabled = true;
					TpToPlayerButton.Enabled = true;
					AddBuffButton.Enabled = m.SelectedIndices[0] == HackContext.GameContext.MyPlayerIndex;
					SetPetButton.Enabled = m.SelectedIndices[0] == HackContext.GameContext.MyPlayerIndex;
					SetMountButton.Enabled = m.SelectedIndices[0] == HackContext.GameContext.MyPlayerIndex;
				}
				else
				{
					EditPlayerInfoButton.Enabled = false;
					TpToPlayerButton.Enabled = false;
					AddBuffButton.Enabled = false;
					SetPetButton.Enabled = false;
					SetMountButton.Enabled = false;
					ClearPlayerAttribute();
				}
			};
			PlayerListView.Bounds = new Rectangle(3, 3, 210, 200);
			PlayerListView.Columns.Add("序号", 40);
			PlayerListView.Columns.Add("名称", 70);
			PlayerListView.Columns.Add("生命值", 50);
			PlayerListView.Columns.Add("魔法值", 50);
			Controls.Add(PlayerListView);


			UpdatePlayerTimer = new Timer();
			UpdatePlayerTimer.Interval = 50;//每隔50ms进行一次玩家列表的检查和更新
			UpdatePlayerTimer.Tick += (s, e) =>
			{
				if (PlayerListView.SelectedIndices.Count > 0)
					UpdatePlayerAttribute(HackContext.GameContext.Players[Convert.ToInt32(PlayerListView.SelectedItems[0].Text)]);
				if (Visible)
					UpdatePlayerList();
			};
			UpdatePlayerTimer.Start();//开启更新线程
		}
		public void ClearPlayerAttribute()
		{
			(PlayerNameInfoView.View as TextBox).Text = "";
			(PlayerLifeInfoView.View as TextBox).Text = "";
			(PlayerManaInfoView.View as TextBox).Text = "";
			(PlayerMaxLifeInfoView.View as TextBox).Text = "";
			(PlayerMaxManaInfoView.View as TextBox).Text = "";
			(PlayerXInfoView.View as TextBox).Text = "";
			(PlayerYInfoView.View as TextBox).Text = "";
			(PlayerInventoryBaseAddressInfoView.View as TextBox).Text = "";
		}
		public void UpdatePlayerAttribute(Player p)
		{
			(PlayerNameInfoView.View as TextBox).Text = p.Name;
			(PlayerLifeInfoView.View as TextBox).Text = p.Life.ToString();
			(PlayerManaInfoView.View as TextBox).Text = p.Mana.ToString();
			(PlayerMaxLifeInfoView.View as TextBox).Text = p.MaxLife.ToString();
			(PlayerMaxManaInfoView.View as TextBox).Text = p.MaxMana.ToString();
			(PlayerXInfoView.View as TextBox).Text = p.X.ToString();
			(PlayerYInfoView.View as TextBox).Text = p.Y.ToString();
			(PlayerInventoryBaseAddressInfoView.View as TextBox).Text = p.Inventory.BaseAddress.ToString("X8");
		}
		private InfoView AddPlayerAttribute(string Text, int TipWidth)
		{
			InfoViewEx e = new InfoViewEx(TipWidth);
			e.Text = Text;
			e.Bounds = new Rectangle(0, 20 * (PlayerAttributeNumbers++), 210, 20);
			PlayerAttributePanel.Controls.Add(e);
			return e;
		}
		public void UpdatePlayerList()
		{
			if (HackContext.GameContext == null) return;
			for (int i = 0; i < Player.MAX_PLAYER; i++)
			{
				Player p = HackContext.GameContext.Players[i];
				var ps = PlayerListView.Items.Find(i.ToString(), false);
				if (p.Active)//玩家存在
				{
					if (ps.Length == 0)//添加到列表
					{
						var t = PlayerListView.Items.Add(i.ToString(), i.ToString(), 0);
						var name = t.SubItems.Add(p.Name);
						t.SubItems.Add(p.Life.ToString());
						t.SubItems.Add(p.Mana.ToString());
						if (i == HackContext.GameContext.MyPlayerIndex)
							name.ForeColor = Color.DeepPink;
					}
					else//更新数据
					{
						var t = ps[0];
						t.SubItems[1].Text = p.Name;
						t.SubItems[2].Text = p.Life.ToString();
						t.SubItems[3].Text = p.Mana.ToString();
					}
				}
				else//不存在
				{
					if (ps.Length > 0)//但是列表里存在
						ps[0].Remove();//移除
				}
			}
		}

		private int GetMountFromIndex(int id)
		{
			return Convert.ToInt32(GameResLoader.MountToID[GameResLoader.Mounts[id]]);
		}
		private int GetPetFromIndex(int id)
		{
			return Convert.ToInt32(GameResLoader.PetToID[GameResLoader.Pets[id]]);
		}
	}
}
