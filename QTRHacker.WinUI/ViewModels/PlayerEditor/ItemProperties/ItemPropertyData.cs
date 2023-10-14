using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using System.ComponentModel;
using System.Reflection;

namespace QTRHacker.ViewModels.PlayerEditor.ItemProperties;

public abstract class ItemPropertyData : ObservableObject
{
	private readonly LocalizationItem LocalizationItem;
	private object? _Value;

	public PropertyInfo ItemProperty { get; }
	public Type PropertyType => ItemProperty.PropertyType;

	public string Key { get; }
	public string Tip => LocalizationItem.Value;

	protected object? InternalValue
	{
		get => _Value;
		set
		{
			_Value = value;
			InternalValueChanged?.Invoke(this, new EventArgs());
		}
	}
	protected event EventHandler? InternalValueChanged;

	public virtual void UpdateFromItem(Item item)
	{
		InternalValue = ItemProperty.GetValue(item);
	}

	public virtual void UpdateToItem(Item item)
	{
		ItemProperty.SetValue(item, InternalValue);
	}

	public object? GetValue() => InternalValue;

	protected ItemPropertyData(string key)
	{
		Key = key;
		ItemProperty = typeof(Item).GetProperty(Key)!;
		if (ItemProperty == null)
			throw new Exception($"No such property: {Key}");//TODO: replace it with a user exception
		LocalizationItem = new LocalizationItem($"UI.ItemProperties.{Key}");
		LocalizationItem.PropertyChanged += LocalizationItem_PropertyChanged!;
	}
	private void LocalizationItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged(nameof(Tip));
	}
}
public abstract class ItemPropertyData<T> : ItemPropertyData
{
	public virtual T Value
	{
		get => (T)InternalValue!;
		set
		{
			InternalValue = value;
			OnPropertyChanged(nameof(Value));
		}
	}

	public ItemPropertyData(string key) : base(key)
	{
		InternalValue = default(T)!;
		if (ItemProperty.PropertyType != typeof(T))
			throw new Exception(
				$"Type doesn't match. " +
				$"Expected {typeof(T).Name}, got {ItemProperty.PropertyType.Name}. " +
				$"Key: {key}");//TODO: replace it with a user exception
		InternalValueChanged += ItemPropertyData_InternalValueChanged!;
	}

	private void ItemPropertyData_InternalValueChanged(object sender, EventArgs e)
	{
		OnPropertyChanged(nameof(Value));
	}
}
