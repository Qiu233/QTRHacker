using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Controls;
using QTRHacker.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public partial class PlayerEditorForm : MForm
	{
		private readonly MTabControl Tabs;
		public PlayerEditorForm(Player TargetPlayer, bool Editable)
		{
			InitializeComponent();
			Text = TargetPlayer.Name.GetString() + (Editable ? "" : $" (Uneditable)");
			BackColor = Color.FromArgb(45, 45, 48);

			Tabs = new MTabControl
			{
				HeaderBackColor = Color.FromArgb(70, 70, 70),
				HeaderSelectedBackColor = Color.FromArgb(90, 90, 90),
				Bounds = new Rectangle(0, 1, 1005, 360),
				ForeColor = GlobalColors.TipForeColor
			};
			Tabs.Controls.Add(new PlayerEditor(TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			Tabs.Controls.Add(new InvEditor(HackContext.GameContext, TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			Tabs.Controls.Add(new ArmorEditor(HackContext.GameContext, TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			Tabs.Controls.Add(new PiggyBankEditor(HackContext.GameContext, TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			Tabs.Controls.Add(new SafeEditor(HackContext.GameContext, TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			Tabs.Controls.Add(new VoidVaultEditor(HackContext.GameContext, TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			Tabs.Controls.Add(new ForgeEditor(HackContext.GameContext, TargetPlayer, Editable) { BackColor = Tabs.HeaderBackColor });
			MainPanel.Controls.Add(Tabs);
		}
	}
}
