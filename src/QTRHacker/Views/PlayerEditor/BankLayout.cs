using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Views.PlayerEditor
{
	public sealed class BankLayout : ISlotsLayout
	{
		public int Slots => 40;

		public (int Column, int Row) GetSlotLocation(int index)
		{
			return new(index % 10, index / 10);
		}
	}
}
