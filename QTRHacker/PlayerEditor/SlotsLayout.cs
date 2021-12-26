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
		public const int SlotsWidth = 50;
		public const int SlotsGap = 5;
		public abstract Point Position(int index);
		public abstract Item this[int index]
		{
			get;
		}

		public SlotsLayout()
		{
		}
	}
}
