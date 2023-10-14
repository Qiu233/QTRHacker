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
	private readonly Player Player;
	public ArmorPageViewModel(Func<int, ItemSlotViewModel> slotMaker, Player p)
	{
		Player = p;
		for (int i = 0; i < 30; i++)
			armor.Add(slotMaker(i));
		for (int i = 0; i < 10; i++)
			misc.Add(slotMaker(i));
	}
	private readonly ItemStack[] Buffer = new ItemStack[40];
	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			for (int i = 0; i < 30; i++)
				Buffer[i] = Player.Armor[i].Marshal();
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
}
