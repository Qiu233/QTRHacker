using QTRHacker.Functions;
using QTRHacker.Functions.GameObjects;
using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PlayerEditor
{
	public abstract class FlowItemSlotsEditor : ItemSlotsEditor<FlowItemSlotsEditor.FlowSlotsLayout>
	{
		public sealed class FlowSlotsLayout : SlotsLayout
		{
			public GameObjectArray<Item> SourceSlots
			{
				get;
				set;
			}
			public FlowSlotsLayout() : base()
			{
			}

			public override Item this[int index]
				=> SourceSlots[index];
			public override Point GetPosition(int index)
				=> new Point(index % 10 * (SlotWidth + SlotGap), (int)Math.Floor((double)(index / 10)) * (SlotWidth + SlotGap));
		}
		public FlowItemSlotsEditor(GameContext ctx, Player player, GameObjectArray<Item> slots, string title, bool editable, int count) : base(ctx, player, title, editable, count)
		{
			SlotsPanel.SLayout.SourceSlots = slots;
		}
	}
}
