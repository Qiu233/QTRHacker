using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PlayerEditor
{
	public abstract class FlowItemSlotsEditor : ItemSlotsEditor<FlowItemSlotsEditor.FlowSlotsLayout>
	{
		public sealed class FlowSlotsLayout : SlotsLayout
		{
			public ItemSlots SourceSlots
			{
				get;
				set;
			}
			public FlowSlotsLayout() : base()
			{
			}

			public override Item this[int index]
				=> SourceSlots[index];
			public override Point Position(int index)
				=> new Point(index % 10 * (SlotsWidth + SlotsGap), (int)Math.Floor((double)(index / 10)) * (SlotsWidth + SlotsGap));
		}
		public FlowItemSlotsEditor(GameContext ctx, Form parent, Player player, ItemSlots slots, string title, bool editable, int count) : base(ctx, parent, player, title, editable, count)
		{
			SlotsPanel.SlotsLayout.SourceSlots = slots;
		}
	}
}
