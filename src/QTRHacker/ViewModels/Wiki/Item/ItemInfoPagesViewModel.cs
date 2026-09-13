using System.Collections.ObjectModel;
using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Core;
using QTRHacker.Localization;

namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemInfoPagesViewModel : WikiFilterViewModel<ItemCategoryFilter>, ILocalizationProvider
{
	private string value;
	private int selectedRecipeFrom;
	private ItemStackInfo selectedRecipeTo;
	private ItemInfo itemInfo;

	public string Value => value;
	public ObservableCollection<RecipeFromInfo> RecipeFroms { get; } = new();
	public ObservableCollection<ItemStackInfo> RecipeTos { get; } = new();
	public int SelectedRecipeFrom
	{
		get => selectedRecipeFrom;
		set => SetProperty(ref selectedRecipeFrom, value);
	}
	public ItemStackInfo SelectedRecipeTo
	{
		get => selectedRecipeTo;
		set => SetProperty(ref selectedRecipeTo, value);
	}
	public ItemInfo ItemInfo
	{
		get => itemInfo;
		set
		{
			if (SetProperty(ref itemInfo, value))
				InitData();
		}
	}

	public RelayCommand GetItemStackCommand { get; }
	public RelayCommand JumpToCommand { get; }

	public event EventHandler<JumpToItemEventArgs> JumpToItem;

	public void InitData()
	{
		RecipeFroms.Clear();
		RecipeTos.Clear();
		if (ItemInfo == null)
			return;
		int type = itemInfo.Type;
		var pRe = WikiResLoader.RecipeDatum.Where(t => t.TargetItem.Type == type).ToList();
		if (pRe.Any())
			for (int i = 0; i < pRe.Count; i++)
				RecipeFroms.Add(new RecipeFromInfo(i.ToString(), pRe[i]));
		pRe = WikiResLoader.RecipeDatum.Where(
			t => t.RequiredItems.Where(
				y => type != 0 && y.Type == type).Any()).ToList();
		pRe.Sort((a, b) => a.TargetItem.Type - b.TargetItem.Type);
		if (pRe.Any())
			foreach (var item in pRe)
				RecipeTos.Add(new ItemStackInfo(item.TargetItem));
		ApplyLocalization();
	}

	private void ApplyLocalization()
	{
		if (ItemInfo == null)
			return;

		string P = LocalizationManager.Instance.GetValue("UI.ItemValue.Platinum");
		string G = LocalizationManager.Instance.GetValue("UI.ItemValue.Gold");
		string S = LocalizationManager.Instance.GetValue("UI.ItemValue.Silver");
		string C = LocalizationManager.Instance.GetValue("UI.ItemValue.Copper");
		int p = ItemInfo.Data.Value / 1000000;
		int t = ItemInfo.Data.Value % 1000000;
		int g = t / 10000;
		t %= 10000;
		int s = t / 100;
		t %= 100;
		int c = t;
		value = $"{p}{P} {g}{G} {s}{S} {c}{C}";
		OnPropertyChanged(nameof(Value));
	}

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		ApplyLocalization();
	}

	public ItemInfoPagesViewModel()
		: base(Enum.GetValues<ItemCategory>().Select(category => new ItemCategoryFilter(category)))
	{
		GetItemStackCommand = new HackCommand(o =>
		{
			if (o is not ItemStackInfo stack)
				return;
			HackGlobal.GameContext.AddItemStackToInv(stack.ItemInfo.Type, stack.Stack);
		});
		JumpToCommand = new RelayCommand(o =>
		{
			if (o is not ItemStackInfo stack)
				return;
			JumpToItem?.Invoke(this, new JumpToItemEventArgs(stack.ItemInfo));
		});

		LocalizationManager.RegisterLocalizationProvider(this);
	}
}
