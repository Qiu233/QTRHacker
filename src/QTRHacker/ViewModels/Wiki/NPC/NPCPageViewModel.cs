using QTRHacker.Assets;
using QTRHacker.Commands;
using System.Collections.ObjectModel;

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
			selectedNPCInfo = value;
			OnPropertyChanged(nameof(SelectedNPCInfo));
			SelectedNPCInfoChanged?.Invoke(this, EventArgs.Empty);
		}
	}
	public NPCInfoPagesViewModel NPCInfoPagesViewModel { get; }
	public event EventHandler SelectedNPCInfoChanged;

	private readonly RelayCommand addOneCommand;
	public RelayCommand AddOneCommand => addOneCommand;

	public NPCPageViewModel()
	{
		addOneCommand = new RelayCommand(o => HackGlobal.IsActive, o => AddSelectedNPCToGame());

		NPCInfoPagesViewModel = new NPCInfoPagesViewModel();
		NPCInfoPagesViewModel.FilterResumed += (s, e) => UpdateFilter();
		NPCInfoPagesViewModel.CategoryFilters.CollectionChanged += (s, e) => UpdateFilter();
		NPCInfoPagesViewModel.KeywordChanged += (s, e) => UpdateFilter();
		NPCInfoPagesViewModel.NPCCategoryFilterSelectedChanged += (s, e) => UpdateFilter();
		SelectedNPCInfoChanged += NPCPageViewModel_SelectedNPCInfoChanged; ;

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
			var flags = NPCInfoPagesViewModel.CategoryFilters
				.Where(t => t.IsSelected)
				.Select(t => (t.Category != NPCCategory.Others && cate.HasFlag(t.Category)) ||
							(t.Category == NPCCategory.Others && cate == NPCCategory.Others))
				.ToList();
			if (!flags.Any(t => t))
				continue;
			Items.Add(item);
		}
	}
}
