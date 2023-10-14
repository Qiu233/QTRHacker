using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
using QTRHacker.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public class ChestPageViewModel : SlotsPageViewModel
{
	public override string Header { get; }
	private readonly List<ItemSlotViewModel> chest = new();
	public IReadOnlyList<ItemSlotViewModel> Chest => chest;
	public Chest Target { get; }
	private readonly ItemStack[] Buffer;
	public ChestPageViewModel(string header, Chest target, Func<int, ItemSlotViewModel> slotMaker)
	{
		Header = header;
		Target = target;
		int len = Target.Item.Length;
		for (int i = 0; i < len; i++)
			chest.Add(slotMaker(i));
		Buffer = new ItemStack[len];
	}
	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			var slots = Target.Item;
			for (int i = 0; i < Buffer.Length; i++)
				Buffer[i] = slots[i].Marshal();
		});
		foreach (var (item, b) in Chest.Zip(Buffer))
			UpdateItemStack(item, b.Type, b.Stack);
	}
}
