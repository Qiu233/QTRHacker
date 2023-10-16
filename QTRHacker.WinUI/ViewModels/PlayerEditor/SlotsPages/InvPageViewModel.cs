using QTRHacker.Assets;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public class InvPageViewModel : SlotsPageViewModel
{
	public override string Header => "Inventory";

	private readonly List<ItemSlotViewModel> mainInv = new();
	public IReadOnlyList<ItemSlotViewModel> MainInv => mainInv;

	private readonly List<ItemSlotViewModel> subInv = new();
	public IReadOnlyList<ItemSlotViewModel> SubInv => subInv;

	public ItemSlotViewModel TrashItem { get; }
	public ItemSlotViewModel HoldItem { get; }

	public override IEnumerable<ItemSlotViewModel> Slots { get; }

	private readonly Player Player;

	public InvPageViewModel(Func<int, ItemSlotViewModel> slotMaker, Player player)
	{
		Player = player;
		for (int i = 0; i < 50; i++)
			mainInv.Add(slotMaker(i));
		for (int i = 0; i < 8; i++)
		{
			var t = slotMaker(50 + i);
			t.Size = 36;
			subInv.Add(t);
		}
		HoldItem = slotMaker(58);
		TrashItem = slotMaker(59);
		Slots = MainInv.Union(SubInv).Union(new ItemSlotViewModel[] { HoldItem, TrashItem }).ToList();
	}

	private readonly ItemStack[] Buffer = new ItemStack[60];

	// Design considerations:
	// we can put update-related code in every single slot, and then slots will do the heavy stuff by themselves
	// which is much more concise, but would be too bad in terms of performance
	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			for (int i = 0; i < Buffer.Length; i++)
				Buffer[i] = Player.Inventory[i].Marshal();
			Buffer[59] = Player.TrashItem.Marshal();
		});
		foreach (var item in MainInv)
		{
			var gi = Buffer[item.Index];
			UpdateItemStack(item, gi.Type, gi.Stack);
		}
		foreach (var item in SubInv)
		{
			var gi = Buffer[item.Index];
			UpdateItemStack(item, gi.Type, gi.Stack);
		}
		UpdateItemStack(HoldItem, Buffer[58].Type, Buffer[58].Stack);
		UpdateItemStack(TrashItem, Buffer[59].Type, Buffer[59].Stack);
	}

	public override async Task<Item> GetItem(int id)
	{
		if (id == 59)
			return await Task.Run(() => Player.TrashItem);
		return await Task.Run(() => Player.Inventory[id]);
	}
}
