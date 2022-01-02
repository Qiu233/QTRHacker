using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public class VoidVaultEditor : FlowItemSlotsEditor
	{
		public VoidVaultEditor(GameContext Context, Player TargetPlayer, bool Editable)
			: base(Context, TargetPlayer, TargetPlayer.Bank4.Item, HackContext.CurrentLanguage["VoidVault"], Editable, TargetPlayer.Bank4.Item.Length)
		{
			SlotsPanel.Location = new Point(0, 30);
		}
	}
}
