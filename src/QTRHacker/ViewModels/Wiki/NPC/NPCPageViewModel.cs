using System.Collections.ObjectModel;
using QTRHacker.Assets;
using QTRHacker.Commands;

namespace QTRHacker.ViewModels.Wiki.NPC;

public class NPCPageViewModel : ViewModelBase
{
	private NPCInfo selectedNPCInfo;

	public ObservableCollection<NPCInfo> Items { get; } = new();

	public NPCInfo SelectedNPCInfo
	{
		get => selectedNPCInfo;
		set
		{
			if (SetProperty(ref selectedNPCInfo, value))
				SelectedNPCInfoChanged?.Invoke(this, EventArgs.Empty);
		}
	}
	public NPCInfoPagesViewModel NPCInfoPagesViewModel { get; }
	public event EventHandler SelectedNPCInfoChanged;

	public RelayCommand AddOneCommand { get; }

	public NPCPageViewModel()
	{
		AddOneCommand = new HackCommand(o => AddSelectedNPCToGame());

		NPCInfoPagesViewModel = new NPCInfoPagesViewModel();
		NPCInfoPagesViewModel.FilterChanged += (s, e) => UpdateFilter();
		SelectedNPCInfoChanged += NPCPageViewModel_SelectedNPCInfoChanged;

		for (int i = 1; i < WikiResLoader.NPCDatum.Count; i++)
			Items.Add(new NPCInfo(i));
	}


	public void AddSelectedNPCToGame()
	{
		if (!HackGlobal.IsActive)
			return;
		var ctx = HackGlobal.GameContext;
		if (SelectedNPCInfo == null)
			return;
		int id = SelectedNPCInfo.Type;
		var pos = ctx.MyPlayer.Position;
		Core.GameObjects.Terraria.NPC.NewNPC(ctx, (int)pos.X, (int)pos.Y, id);
	}

	private void NPCPageViewModel_SelectedNPCInfoChanged(object sender, EventArgs e)
	{
		NPCInfoPagesViewModel.NPCInfo = SelectedNPCInfo;
	}
	private void UpdateFilter()
	{
		if (NPCInfoPagesViewModel.IsFilterSuspended)
			return;
		Items.Clear();
		var kw = NPCInfoPagesViewModel.Keyword;
		for (int i = 1; i < WikiResLoader.NPCDatum.Count; i++)
		{
			var item = new NPCInfo(i);
			if (!item.Name.Contains(kw, StringComparison.OrdinalIgnoreCase))
				continue;
			var cate = item.GetNPCCategory();
			bool matchesCategory = NPCInfoPagesViewModel.CategoryFilters.Any(filter =>
				filter.IsSelected && (filter.Category == NPCCategory.Others
					? cate == NPCCategory.Others
					: cate.HasFlag(filter.Category)));
			if (!matchesCategory)
				continue;
			Items.Add(item);
		}
	}
}
