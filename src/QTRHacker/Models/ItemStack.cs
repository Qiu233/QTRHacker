using QTRHacker.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Models
{
	public readonly struct ItemStack
	{
		public int Type { get; }
		public int Stack { get; }

		public ItemStack(int type, int stack)
		{
			Type = type;
			Stack = stack;
		}
	}
}
