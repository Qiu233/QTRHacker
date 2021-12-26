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
	public class SafeEditor : FlowItemSlotsEditor
	{
		public SafeEditor(GameContext Context, Form ParentForm, Player TargetPlayer, bool Editable)
			: base(Context, ParentForm, TargetPlayer, TargetPlayer.Bank2.Item, HackContext.CurrentLanguage["Safe"], Editable, TargetPlayer.Bank2.Item.Length)
		{
			SlotsPanel.Location = new Point(0, 30);
		}
	}
}
