using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Controls;
using QTRHacker.PlayerEditor;
using QTRHacker.Res;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PagePanels
{
	public class PagePanel_Player : PagePanel
	{
		private readonly MListView PlayerListView;
		private readonly System.Timers.Timer UpdatePlayerTimer;
		private readonly MButtonStrip ButtonStrip;
		private readonly MButton EditPlayerInfoButton, TpToPlayerButton,
			AddBuffButton, SetPetButton, SetMountButton;
		private readonly Panel PlayerAttributePanel;
		private readonly InfoView PlayerNameInfoView, PlayerLifeInfoView, PlayerManaInfoView,
			PlayerMaxLifeInfoView, PlayerMaxManaInfoView, PlayerXInfoView, PlayerYInfoView,
			PlayerInventoryBaseAddressInfoView;
		private int PlayerAttributeNumbers = 0;
		public PagePanel_Player(int Width, int Height) : base(Width, Height)
		{
			ButtonStrip = new MButtonStrip(80, 30);
			ButtonStrip.Bounds = new Rectangle(215, 2, 80, 210);
			ButtonStrip.Enabled = false;
			Controls.Add(ButtonStrip);

			EditPlayerInfoButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["EditPlayer"]);
			EditPlayerInfoButton.Click += (s, e) =>
			{
				int i = Convert.ToInt32(PlayerListView.SelectedItems[0].Text);
				PlayerEditorForm f = new PlayerEditorForm(HackContext.GameContext.Players[i], i == HackContext.GameContext.MyPlayerIndex);
				f.Show();
			};

			TpToPlayerButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["TpTo"]);
			TpToPlayerButton.Click += (s, e) =>
			{
				var p = HackContext.GameContext.Players[Convert.ToInt32(PlayerListView.SelectedItems[0].Text)];
				var mp = HackContext.GameContext.MyPlayer;
				mp.Position = p.Position;
			};

			AddBuffButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["AddBuff"]);
			AddBuffButton.Click += (s, e) =>
			{
				var ps = PlayerListView.SelectedIndices;
				if (ps.Count == 0) return;

				MForm AddBuffMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = HackContext.CurrentLanguage["AddBuff"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 72)
				};

				Label BuffTypeTip = new Label()
				{
					Text = HackContext.CurrentLanguage["BuffType"],
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
					Text = HackContext.CurrentLanguage["BuffTime"],
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
				ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
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

			SetPetButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["SetPet"]);
			SetPetButton.Click += (s, e) =>
			{
				var ps = PlayerListView.SelectedIndices;
				if (ps.Count == 0) return;

				MForm SetPetMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = HackContext.CurrentLanguage["SetPet"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label PetTypeTip = new Label()
				{
					Text = HackContext.CurrentLanguage["Pet"],
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
				ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
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

			SetMountButton = ButtonStrip.AddButton(HackContext.CurrentLanguage["SetMount"]);
			SetMountButton.Click += (s, e) =>
			{
				var ps = PlayerListView.SelectedIndices;
				if (ps.Count == 0) return;

				MForm SetMountMForm = new MForm
				{
					BackColor = Color.FromArgb(90, 90, 90),
					Text = HackContext.CurrentLanguage["SetMount"],
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(245, 52)
				};

				Label MountTypeTip = new Label()
				{
					Text = HackContext.CurrentLanguage["Mount"],
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
				ConfirmButton.Text = HackContext.CurrentLanguage["Confirm"];
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

			PlayerNameInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["PlayerName"], 120);
			PlayerLifeInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["Life"], 120);
			PlayerManaInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["Mana"], 120);
			PlayerMaxLifeInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["MaxLife"], 120);
			PlayerMaxManaInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["MaxMana"], 120);
			PlayerXInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["X_Coor"], 120);
			PlayerYInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["Y_Coor"], 120);
			PlayerInventoryBaseAddressInfoView = AddPlayerAttribute(HackContext.CurrentLanguage["PlayerInvAddress"], 120);


			PlayerListView = new MListView();
			PlayerListView.SelectedIndexChanged += (s, e) =>
			{
				var m = s as MListView;
				if (m.SelectedIndices.Count > 0)
				{
					ButtonStrip.Enabled = true;
					bool t = Convert.ToInt32(m.SelectedItems[0].SubItems[0].Text) == HackContext.GameContext.MyPlayerIndex;
					AddBuffButton.Enabled = t;
					SetPetButton.Enabled = t;
					SetMountButton.Enabled = t;
				}
				else
				{
					ButtonStrip.Enabled = false;
					ClearPlayerAttribute();
				}
			};
			PlayerListView.Bounds = new Rectangle(3, 3, 210, 200);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Index"], 40);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Name"], 70);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Life"], 50);
			PlayerListView.Columns.Add(HackContext.CurrentLanguage["Mana"], 50);
			Controls.Add(PlayerListView);


			UpdatePlayerTimer = new System.Timers.Timer(500);
			UpdatePlayerTimer.Elapsed += (s, e) =>
			{
				if (!Visible)
					return;
				if (PlayerListView.SelectedIndices.Count > 0)
					UpdatePlayerAttribute(HackContext.GameContext.Players[Convert.ToInt32(PlayerListView.SelectedItems[0].Text)]);
				UpdatePlayerList();
			};
			UpdatePlayerTimer.Start();
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				UpdatePlayerTimer.Stop();
				UpdatePlayerTimer.Dispose();
			}
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
			(PlayerNameInfoView.View as TextBox).Text = p.Name.GetString();
			(PlayerLifeInfoView.View as TextBox).Text = p.StatLife.ToString();
			(PlayerManaInfoView.View as TextBox).Text = p.StatMana.ToString();
			(PlayerMaxLifeInfoView.View as TextBox).Text = p.StatLifeMax.ToString();
			(PlayerMaxManaInfoView.View as TextBox).Text = p.StatManaMax.ToString();
			(PlayerXInfoView.View as TextBox).Text = p.Position.X.ToString();
			(PlayerYInfoView.View as TextBox).Text = p.Position.Y.ToString();
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
			int players = HackContext.GameContext.Players.Length;
			for (int i = 0; i < players; i++)
			{
				Player p = HackContext.GameContext.Players[i];
				var ps = PlayerListView.Items.Find(i.ToString(), false);
				if (p.Active)
				{
					if (ps.Length == 0)
					{
						var t = PlayerListView.Items.Add(i.ToString(), i.ToString(), 0);
						var name = t.SubItems.Add(p.Name.GetString());
						t.SubItems.Add(p.StatLife.ToString());
						t.SubItems.Add(p.StatMana.ToString());
						if (i == HackContext.GameContext.MyPlayerIndex)
							name.ForeColor = Color.DeepPink;
					}
					else
					{
						var t = ps[0];
						if (t.SubItems.Count == 4)
						{
							t.SubItems[1].Text = p.Name.GetString();
							t.SubItems[2].Text = p.StatLife.ToString();
							t.SubItems[3].Text = p.StatMana.ToString();
						}
					}
				}
				else
				{
					if (ps.Length > 0)
						ps[0].Remove();
				}
			}
		}

		private static int GetMountFromIndex(int id)
		{
			return Convert.ToInt32(GameResLoader.MountToID[GameResLoader.Mounts[id]]);
		}
		private static int GetPetFromIndex(int id)
		{
			return Convert.ToInt32(GameResLoader.PetToID[GameResLoader.Pets[id]]);
		}
	}
}
