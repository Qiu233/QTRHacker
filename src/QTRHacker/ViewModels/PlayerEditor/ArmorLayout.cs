using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public sealed class ArmorLayout : ISlotsLayout
	{
		public int Slots => Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT + Player.MISCDYE_MAX_COUNT;

		public (int Column, int Row) GetSlotLocation(int index)
		{
			if (index >= 0 && index < Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT)
				return (index % 10, (int)Math.Floor((double)(index / 10)));
			else if (index >= Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT
				&& index < Player.ARMOR_MAX_COUNT + Player.DYE_MAX_COUNT + Player.MISC_MAX_COUNT + Player.MISCDYE_MAX_COUNT)
				return ((index + 5) % 10, (int)Math.Floor((double)((index + 5) / 10)));
			return (0, 0);
		}
	}
}
