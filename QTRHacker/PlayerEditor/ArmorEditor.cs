using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Controls;
using QTRHacker.PlayerEditor.Controls;
using QTRHacker.Res;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public class ArmorEditor : ItemSlotsEditor<ArmorEditor.ArmorSlotsLayout>
	{
		public sealed class ArmorSlotsLayout : SlotsLayout
		{
			public Player Player
			{
				get;
				set;
			}
			public override Point Position(int index)
			{
				if (index >= 0 && index < Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT)
					return new Point(index % 10 * (SlotsWidth + SlotsGap), (int)Math.Floor((double)(index / 10)) * (SlotsWidth + SlotsGap));
				else if (index >= Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT
					&& index < Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT + Player.MISCDYE_MAX_COUNT)
					return new Point((index + 5) % 10 * (SlotsWidth + SlotsGap), (int)Math.Floor((double)((index + 5) / 10)) * (SlotsWidth + SlotsGap));
				return new Point(0, 0);
			}
			public ArmorSlotsLayout() : base()
			{
			}

			public override Item this[int index]
			{
				get
				{
					if (index < Player.ARMOR_MAX_COUNT)
						return Player.Armor[index];
					index -= Player.ARMOR_MAX_COUNT;

					if (index < Player.DYE_MAX_COUNT)
						return Player.Dye[index];
					index -= Player.DYE_MAX_COUNT;

					if (index < Player.MISC_MAX_COUNT)
						return Player.MiscEquips[index];
					index -= Player.MISC_MAX_COUNT;

					if (index < Player.MISCDYE_MAX_COUNT)
						return Player.MiscDyes[index];

					return Player.Armor[0];
				}
			}
		}
		public ArmorEditor(GameContext Context, Form ParentForm, Player TargetPlayer, bool Editable) :
			base(Context, ParentForm, TargetPlayer, HackContext.CurrentLanguage["Armor"], Editable,
				Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT + Player.MISCDYE_MAX_COUNT)
		{
			SlotsPanel.SlotsLayout.Player = TargetPlayer;
		}
	}
}
