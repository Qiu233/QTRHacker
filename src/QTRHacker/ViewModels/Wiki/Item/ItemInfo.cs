using QTRHacker.Assets;
using QTRHacker.Localization;
using QTRHacker.Models.Wiki;

namespace QTRHacker.ViewModels.Wiki.Item;

public class ItemInfo : ViewModelBase, ILocalizationProvider
{
	private string name;
	private string category;
	public string Name => name;
	public string Category => category;

	public int Type { get; }
	public string Key => WikiResLoader.GetItemKeyFromType(Type);
	public ItemData Data => WikiResLoader.ItemDatum[Type];

	public void OnCultureChanged(object sender, CultureChangedEventArgs args)
	{
		name = LocalizationManager.Instance.GetValue("ItemName." + WikiResLoader.GetItemKeyFromType(Type));
		OnPropertyChanged(nameof(Name));
		var c = GetItemCategory();
		category = c.ToString();
		OnPropertyChanged(nameof(Category));
	}

	private ItemCategory GetItemCategory()
	{
		var data = Data;
		if (data.CreateTile != -1) return ItemCategory.Block;
		if (data.CreateWall != -1) return ItemCategory.Wall;
		if (data.QuestItem) return ItemCategory.Quest;
		if (data.HeadSlot != -1) return ItemCategory.Head;
		if (data.BodySlot != -1) return ItemCategory.Body;
		if (data.LegSlot != -1) return ItemCategory.Leg;
		if (data.Accessory) return ItemCategory.Accessory;
		if (data.Melee) return ItemCategory.Melee;
		if (data.Ranged) return ItemCategory.Ranged;
		if (data.Magic) return ItemCategory.Magic;
		if ((data.Summon || data.Sentry)) return ItemCategory.Summon;
		if (data.BuffType != 0) return ItemCategory.Buff;
		if (data.Consumable) return ItemCategory.Consumable;
		return ItemCategory.None;
	}

	public ItemInfo(int type)
	{
		Type = type;
		LocalizationManager.RegisterLocalizationProvider(this);
	}
}
