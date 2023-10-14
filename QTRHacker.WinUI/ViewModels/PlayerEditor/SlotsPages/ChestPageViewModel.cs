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
	public ChestPageViewModel(string header, Func<int, ItemSlotViewModel> slotMaker)
	{
		Header = header;
		for (int i = 0; i < 40; i++)
			chest.Add(slotMaker(i));
	}

	public override Task Update()
	{
		return Task.Run(() => { });
	}
}
