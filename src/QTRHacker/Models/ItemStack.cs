using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Models
{
	public readonly struct ItemStack
	{
		public readonly int Type;
		public readonly int Stack;
		public readonly byte Prefix;

		public ItemStack(int type, int stack, byte prefix)
		{
			Type = type;
			Stack = stack;
			Prefix = prefix;
		}
	}
}
