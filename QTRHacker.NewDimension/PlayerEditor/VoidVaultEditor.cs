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
		}
	}
}
