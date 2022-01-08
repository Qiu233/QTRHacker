using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.PlayerEditor.Controls;
using QTRHacker.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public class PiggyBankEditor : FlowItemSlotsEditor
	{
		public PiggyBankEditor(GameContext Context, Player TargetPlayer, bool Editable)
			: base(Context, TargetPlayer, TargetPlayer.Bank.Item, HackContext.CurrentLanguage["PiggyBank"], Editable, TargetPlayer.Bank.Item.Length)
		{
			SlotsPanel.Location = new Point(0, 30);
		}
	}
}
