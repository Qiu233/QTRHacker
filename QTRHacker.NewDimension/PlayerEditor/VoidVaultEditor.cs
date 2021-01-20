using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PlayerEditor
{
	public class VoidVaultEditor : ItemSlotsEditor
	{
		public VoidVaultEditor(GameContext Context, Form ParentForm, Player TargetPlayer, bool Editable)
			: base(Context, ParentForm, TargetPlayer, TargetPlayer.Bank4.Item, Editable, TargetPlayer.Bank4.Item.Length)
		{
			Text = MainForm.CurrentLanguage["VoidVault"];
			SlotsPanel.Location = new Point(0, 30);


			Button OK = new Button();
			OK.Enabled = Editable;
			OK.Click += (sender, e) =>
			{
				ApplyItemData(Selected);
				InitItemData(Selected);
				RefreshSelected();
			};
			OK.FlatStyle = FlatStyle.Flat;
			OK.Text = MainForm.CurrentLanguage["Confirm"];
			OK.Size = new Size(80, 30);
			OK.Location = new Point(260, 0);
			ItemPropertiesPanel.Controls.Add(OK);


			Button Refresh = new Button();
			Refresh.Click += (sender, e) =>
			{
				InitItemData(Selected);
				SlotsPanel.Refresh();
			};
			Refresh.FlatStyle = FlatStyle.Flat;
			Refresh.Text = MainForm.CurrentLanguage["Refresh"];
			Refresh.Size = new Size(80, 30);
			Refresh.Location = new Point(260, 30);
			ItemPropertiesPanel.Controls.Add(Refresh);


			Button InitItem = new Button();
			InitItem.Enabled = Editable;
			InitItem.Click += (sender, e) =>
			{
				Item item = TargetItemSlots[Selected];
				item.SetDefaults(Convert.ToInt32(((TextBox)ItemPropertiesPanel.Hack["Type"]).Text));
				item.SetPrefix(GetPrefixFromIndex(ItemPropertiesPanel.SelectedPrefix));
				int stack = Convert.ToInt32(((TextBox)ItemPropertiesPanel.Hack["Stack"]).Text);
				item.Stack = stack == 0 ? 1 : stack;
				RefreshSelected();
				InitItemData(Selected);
			};
			InitItem.FlatStyle = FlatStyle.Flat;
			InitItem.Text = MainForm.CurrentLanguage["Init"];
			InitItem.Size = new Size(80, 30);
			InitItem.Location = new Point(260, 60);
			ItemPropertiesPanel.Controls.Add(InitItem);

		}
	}
}
