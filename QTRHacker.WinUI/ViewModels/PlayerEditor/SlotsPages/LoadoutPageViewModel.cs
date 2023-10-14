using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public class LoadoutPageViewModel : SlotsPageViewModel
{
	public override string Header { get; }
	public EquipmentLoadout Target { get; }
	private readonly List<ItemSlotViewModel> loadout = new();
	public IReadOnlyList<ItemSlotViewModel> Loadout => loadout;
	private readonly ItemStack[] Buffer = new ItemStack[30];
	public LoadoutPageViewModel(string header, EquipmentLoadout target, Func<int, ItemSlotViewModel> slotMaker)
	{
		Header = header;
		Target = target;
		for (int i = 0; i < 30; i++)
			loadout.Add(slotMaker(i));
	}

	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			var slots = Target.Armor;
			for (int i = 0; i < Buffer.Length; i++)
				Buffer[i] = slots[i].Marshal();
		});
		foreach (var (item, b) in Loadout.Zip(Buffer))
			UpdateItemStack(item, b.Type, b.Stack);
	}
}
