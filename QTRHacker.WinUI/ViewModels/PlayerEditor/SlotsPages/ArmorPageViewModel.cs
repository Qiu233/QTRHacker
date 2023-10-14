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
	public ArmorPageViewModel(Func<int, ItemSlotViewModel> slotMaker)
	{
		for (int i = 0; i < 30; i++)
			armor.Add(slotMaker(i));
		for (int i = 0; i < 10; i++)
			misc.Add(slotMaker(i));
	}

	public override Task Update()
	{
		return Task.Run(() => { });
	}
}
