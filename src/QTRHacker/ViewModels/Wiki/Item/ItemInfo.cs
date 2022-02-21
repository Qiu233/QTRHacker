using QTRHacker.Assets;
using QTRHacker.Localization;
using QTRHacker.Models.Wiki;

namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemInfo : ViewModelBase, ILocalizationProvider
{
	private string tooltip;
	private string name;
	private string category;
	public string Name => name;
	public string Category => category;

	public int Type { get; }
	public string Key => WikiResLoader.GetItemKeyFromType(Type);
	public ItemData Data => WikiResLoader.ItemDatum[Type];
	public string Tooltip => tooltip;

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		name = LocalizationManager.Instance.GetValue($"ItemName.{Key}");
		OnPropertyChanged(nameof(Name));
		var c = GetItemCategory();
		category = c.ToString();
		OnPropertyChanged(nameof(Category));
		string key = $"ItemTooltip.{Key}";
		tooltip = LocalizationManager.Instance.GetValue(key);
		if (tooltip == key)
			tooltip = string.Empty;
		OnPropertyChanged(nameof(Tooltip));
	}

	public ItemCategory GetItemCategory()
	{
		var data = Data;
		ItemCategory result = ItemCategory.Others;
		if (data.CreateTile != -1) result |= ItemCategory.Block;
		if (data.CreateWall != -1) result |= ItemCategory.Wall;
		if (data.QuestItem) result |= ItemCategory.Quest;
		if (data.HeadSlot != -1) result |= ItemCategory.Head;
		if (data.BodySlot != -1) result |= ItemCategory.Body;
		if (data.LegSlot != -1) result |= ItemCategory.Leg;
		if (data.Accessory) result |= ItemCategory.Accessory;
		if (data.Melee) result |= ItemCategory.Melee;
		if (data.Ranged) result |= ItemCategory.Ranged;
		if (data.Magic) result |= ItemCategory.Magic;
		if ((data.Summon || data.Sentry)) result |= ItemCategory.Summon;
		if (data.BuffType != 0) result |= ItemCategory.Buff;
		if (data.Consumable) result |= ItemCategory.Consumable;
		return result;
	}

	public ItemInfo(int type)
	{
		Type = type;
		LocalizationManager.RegisterLocalizationProvider(this);
	}
}
