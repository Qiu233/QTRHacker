using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using System.ComponentModel;
using System.Reflection;

namespace QTRHacker.ViewModels.PlayerEditor.ItemProperties;

public abstract class ItemPropertyData : ObservableObject
{
	public abstract object Value { get; }
	public abstract string Key { get; }
	public abstract Task UpdateFromItem(Item item);
	public abstract Task UpdateToItem(Item item);
}

public abstract partial class ItemPropertyData<T> : ItemPropertyData
{
	private readonly LocalizationItem LocalizationItem;
	public PropertyInfo ItemProperty { get; }
	public override string Key { get; }
	public string Tip => LocalizationItem.Value;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Value))]
	private T internalValue = default!;
	public override object Value => InternalValue!;

	private readonly Func<Item, T> Getter;
	private readonly Action<Item, T> Setter;

	public ItemPropertyData(string key)
	{
		Key = key;
		ItemProperty = typeof(Item).GetProperty(Key)!;
		if (ItemProperty is null)
			throw new Exception($"No such property: {Key}");//TODO: replace it with a user exception
		LocalizationItem = new LocalizationItem($"UI.ItemProperties.{Key}");
		LocalizationItem.ValueChanged += (s, e) => OnPropertyChanged(nameof(Tip));
		if (ItemProperty.PropertyType != typeof(T))
			throw new Exception(
				$"Type doesn't match. " +
				$"Expected {typeof(T).Name}, got {ItemProperty.PropertyType.Name}. " +
				$"Key: {key}");//TODO: replace it with a user exception
		Getter = ItemProperty.GetGetMethod()!.CreateDelegate<Func<Item, T>>();
		Setter = ItemProperty.GetSetMethod()!.CreateDelegate<Action<Item, T>>();
	}
	public override async Task UpdateFromItem(Item item)
	{
		InternalValue = await Task.Run(() => Getter(item));
	}

	public override async Task UpdateToItem(Item item)
	{
		var v = InternalValue;
		await Task.Run(() => Setter(item, v));
	}
}
