using QTRHacker.Assets;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
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
		TrashItem = slotMaker(58);
	}

	private readonly ItemStack[] Buffer = new ItemStack[59];

	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			for (int i = 0; i < Buffer.Length; i++)
			{
				var item = Player.Inventory[i];
				Buffer[i] = new(item.Type, item.Stack, item.Prefix);
			}
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
		UpdateItemStack(TrashItem, Buffer[58].Type, Buffer[58].Stack);
	}
}
