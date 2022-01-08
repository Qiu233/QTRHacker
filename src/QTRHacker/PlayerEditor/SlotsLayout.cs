using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using QTRHacker.PlayerEditor.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.PlayerEditor
{
	public abstract class SlotsLayout
	{
		public const int SlotWidth = 50;
		public const int SlotGap = 5;
		public abstract Point GetPosition(int index);
		public abstract Item this[int index]
		{
			get;
		}

		public SlotsLayout()
		{
		}
	}
}
