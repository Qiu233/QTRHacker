using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Models;
using QTRHacker.ViewModels.Common;

namespace QTRHacker.ViewModels.PlayerEditor.SlotsPages;

public class LoadoutPageViewModel : SlotsPageViewModel
{
	public override string Header { get; }
	public EquipmentLoadout Target { get; }
	private readonly List<ItemSlotViewModel> loadout = new();
	public IReadOnlyList<ItemSlotViewModel> Loadout => loadout;

	public override IEnumerable<ItemSlotViewModel> Slots { get; }

	private readonly ItemStack[] Buffer = new ItemStack[30];
	public LoadoutPageViewModel(string header, EquipmentLoadout target, Func<int, ItemSlotViewModel> slotMaker)
	{
		Header = header;
		Target = target;
		for (int i = 0; i < 30; i++)
			loadout.Add(slotMaker(i));
		Slots = loadout.ToList();
	}

	public override async Task Update()
	{
		// TODO: reduce update by unchanged
		await Task.Run(() =>
		{
			var armor = Target.Armor;
			var dye = Target.Dye;
			for (int i = 0; i < 20; i++)
				Buffer[i] = armor[i].Marshal();
			for (int i = 0; i < 10; i++)
				Buffer[i + 20] = dye[i].Marshal();
		});
		foreach (var (item, b) in Loadout.Zip(Buffer))
			UpdateItemStack(item, b.Type, b.Stack);
	}

	public override async Task<Item> GetItem(int id)
	{
		if (id >= 0 && id < 20)
			return await Task.Run(() => Target.Armor[id]);
		else if (id >= 20 && id < 30)
			return await Task.Run(() => Target.Dye[id - 20]);
		throw new ArgumentOutOfRangeException(nameof(id));
	}
}
