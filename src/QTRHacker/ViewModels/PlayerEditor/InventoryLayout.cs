using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.Views.PlayerEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public sealed class InventoryLayout : ISlotsLayout
	{
		public int Slots => 50;

		public (int Column, int Row) GetSlotLocation(int index)
		{
			return new(index % 10, index / 10);
		}
	}
}
