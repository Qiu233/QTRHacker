using CommunityToolkit.Mvvm.ComponentModel;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QTRHacker.ViewModels.PlayerEditor.ItemProperties;

public abstract partial class ItemPropertyData : ObservableObject, INotifyDataErrorInfo
{
	private object value;
	public object Value
	{
		get => value;
		set
		{
			if (Validate(value) is object o)
			{
				HasErrors = false;
				SetProperty(ref this.value, o);
				if (!forwarding)
					_ = UpdateToItem();
			}
			else
			{
				HasErrors = true;
			}
		}
	}
	public abstract string Key { get; }
	private bool hasErrors;
	public bool HasErrors
	{
		get => hasErrors;
		protected set
		{
			SetProperty(ref hasErrors, value);
			OnErrorsChanged();
		}
	}
	private bool forwarding = false;

	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
	protected void OnErrorsChanged() => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(null));

	public IEnumerable GetErrors(string? propertyName) => new List<string>();

	protected abstract object? Validate(object? raw);

	public async Task UpdateFromItem()
	{
		forwarding = true;
		await UpdateFromItemOverride(SelectedItemHolder.SelectedItem);
		forwarding = false;
	}
	public async Task UpdateToItem()
	{
		await UpdateToItemOverride(SelectedItemHolder.SelectedItem);
	}
	protected abstract Task UpdateFromItemOverride(Item? item);
	protected abstract Task UpdateToItemOverride(Item? item);

	private SelectedItemHolder SelectedItemHolder { get; }

	protected ItemPropertyData(SelectedItemHolder holder, object defaultValue)
	{
		value = defaultValue;
		SelectedItemHolder = holder;
		holder.SelectedItemChanged += Holder_SelectedItemChanged;
		// TODO: review for delegate lifetime
		// should we use weak event here?
	}

	private async void Holder_SelectedItemChanged(object? sender, Item? e) => await UpdateFromItem();
}

public abstract partial class ItemPropertyData<T> : ItemPropertyData where T : unmanaged
{
	private readonly LocalizationItem LocalizationItem;
	public PropertyInfo ItemProperty { get; }
	public override string Key { get; }
	public string Tip => LocalizationItem.Value;

	protected override object? Validate(object? raw)
	{
		if (raw is null)
			return null;
		if (raw is T t)
			return t;
		try
		{
			if (Convert.ChangeType(raw, typeof(T)) is T t2)
				return t2;
		}
		catch
		{
		}
		return null;
	}

	private readonly Func<Item, T> Getter;
	private readonly Action<Item, T> Setter;

	public ItemPropertyData(string key, SelectedItemHolder holder) : base(holder, default(T)!)
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

	protected override async Task UpdateFromItemOverride(Item? item)
	{
		if (item is null)
			return;
		Value = await Task.Run(() => Getter(item));
	}

	protected override async Task UpdateToItemOverride(Item? item)
	{
		if (Value is not T v || item is null)
			return;
		await Task.Run(() => Setter(item, v));
	}
}
