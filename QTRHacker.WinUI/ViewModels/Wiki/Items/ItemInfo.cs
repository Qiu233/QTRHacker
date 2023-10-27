using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Assets;
using QTRHacker.Localization;
using QTRHacker.Models.Wiki;
using StrongInject;

namespace QTRHacker.ViewModels.Wiki.Items;

public partial class ItemInfo : ObservableObject, ILocalizationProvider
{
	private string? tooltip;
	private string? name;
	private string? category;
	public string? Name => name;
	public string? Category => category;
	public string? Tooltip => tooltip;

	public int Type
	{
		get;
	}
	public string Key
	{
		get;
	}
	public ItemData Data
	{
		get;
	}

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		name = LocalizationManager.Instance.GetValue($"ItemName.{Key}", LocalizationType.Game);
		OnPropertyChanged(nameof(Name));
		var c = GetItemCategory();
		var values = Enum.GetValues<ItemCategory>()
			.Where(t => t != ItemCategory.Others && c.HasFlag(t))
			.Select(t => LocalizationManager.Instance.GetValue($"Wiki.Items.ItemCategories.{t}"));
		category = string.Join(", ", values);
		if (!category.Any())
			category = LocalizationManager.Instance.GetValue("Wiki.Items.ItemCategories.Others");
		OnPropertyChanged(nameof(Category));
		string key = $"ItemTooltip.{Key}";
		tooltip = LocalizationManager.Instance.GetValue(key, LocalizationType.Game);
		if (tooltip == key)
			tooltip = string.Empty;
		OnPropertyChanged(nameof(Tooltip));
	}

	public ItemCategory GetItemCategory()
	{
		if (Data is null)
			return 0;
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

	public static async Task<ItemInfo> Create(int type)
	{
		string key = await WikiResLoader.GetItemKeyFromType(type);
		ItemData data = (await WikiResLoader.ItemDatum)[type];
		ItemInfo info = new(type, key, data);
		return info;
	}

	private ItemInfo(int type, string key, ItemData data)
	{
		Type = type;
		Key = key;
		Data = data;
		LocalizationManager.RegisterLocalizationProvider(this);
	}
}
