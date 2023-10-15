using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public class ArmorPageViewModel : SlotsPageViewModel
{
	public override string Header => "Armor";

	private readonly List<ItemSlotViewModel> armor = new();
	public IReadOnlyList<ItemSlotViewModel> Armor => armor;

	private readonly List<ItemSlotViewModel> misc = new();
	public IReadOnlyList<ItemSlotViewModel> Misc => misc;

	public override IEnumerable<ItemSlotViewModel> Slots { get; }

	private readonly Player Player;
	public ArmorPageViewModel(Func<int, ItemSlotViewModel> slotMaker, Player p)
	{
		Player = p;
		for (int i = 0; i < 30; i++)
			armor.Add(slotMaker(i));
		for (int i = 0; i < 10; i++)
			misc.Add(slotMaker(30 + i));
		Slots = Armor.Union(Misc).ToList();
	}
	private readonly ItemStack[] Buffer = new ItemStack[40];
	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			for (int i = 0; i < 20; i++)
				Buffer[i] = Player.Armor[i].Marshal();
			for (int i = 0; i < 10; i++)
				Buffer[i + 20] = Player.Dye[i].Marshal();
			for (int i = 0; i < 5; i++)
				Buffer[i + 30] = Player.MiscEquips[i].Marshal();
			for (int i = 0; i < 5; i++)
				Buffer[i + 35] = Player.MiscDyes[i].Marshal();
		});
		for (int i = 0; i < 30; i++)
		{
			var gi = Buffer[i];
			UpdateItemStack(Armor[i], gi.Type, gi.Stack);
		}
		for (int i = 0; i < 10; i++)
		{
			var gi = Buffer[i + 30];
			UpdateItemStack(Misc[i], gi.Type, gi.Stack);
		}
	}

	public override async Task<Item> GetItem(int id)
	{
		if (id >= 0 && id < 20)
			return await Task.Run(() => Player.Armor[id]);
		else if (id >= 20 && id < 30)
			return await Task.Run(() => Player.Dye[id - 20]);
		else if (id >= 30 && id < 35)
			return await Task.Run(() => Player.MiscEquips[id - 30]);
		else if (id >= 35 && id < 40)
			return await Task.Run(() => Player.MiscDyes[id - 35]);
		throw new ArgumentOutOfRangeException(nameof(id));
	}
}
