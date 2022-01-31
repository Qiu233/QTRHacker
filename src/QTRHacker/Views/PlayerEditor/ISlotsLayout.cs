using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Views.PlayerEditor
{
	public interface ISlotsLayout
	{
		int Slots { get; }
		(int Column, int Row) GetSlotLocation(int index);
	}
}
