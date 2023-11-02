using QTRHacker.Core.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Models;

public record struct ItemStack(int Type, int Stack, short Prefix);
public static class ItemStackHelper
{
	public static ItemStack Marshal(this Item item)
	{
		return new ItemStack(item.Type, item.Stack, item.Prefix);
	}
}