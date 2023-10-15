using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using QTRHacker.ViewModels.PlayerEditor.ItemProperties;
using System.Collections.ObjectModel;

namespace QTRHacker.ViewModels.PlayerEditor;

public partial class ItemPropertiesPanelViewModel : ObservableObject
{
	public ObservableCollection<ItemPropertyData> ItemPropertyDatum
	{
		get;
	} = new();
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Rows))]
	private int columns = 3;

	public int Rows => (ItemPropertyDatum.Count + Columns - 1) / Columns;

	//public void UpdatePropertiesFromItem(Item item)
	//{
	//	foreach (var prop in ItemPropertyDatum)
	//		prop.UpdateFromItem(item);
	//}

	//public void UpdatePropertiesToItem(Item item)
	//{
	//	foreach (var prop in ItemPropertyDatum)
	//		prop.UpdateToItem(item);
	//}
	private CancellationTokenSource? lastUpdateTaskCTS;
	private Task? lastUpdateTask;

	private Item? targetItem;
	public Item? TargetItem
	{
		get => targetItem;
		set
		{
			SetProperty(ref targetItem, value);
			UpdateProperties();
		}
	}

	private void UpdateProperties()
	{
		async Task UpdateInner(CancellationToken ct)
		{
			if (TargetItem is null)
				return;
			foreach (var prop in ItemPropertyDatum)
			{
				ct.ThrowIfCancellationRequested();
				await prop.UpdateFromItem(TargetItem);
			}
		}
		// TODO: review for CTS lifetime
		// as per https://stackoverflow.com/a/51220106, disposal is only needed when not cancelled
		if (lastUpdateTask is not null && !lastUpdateTask.IsCompleted &&
			lastUpdateTaskCTS is not null && !lastUpdateTaskCTS.IsCancellationRequested)
			lastUpdateTaskCTS.Cancel();
		var cts = lastUpdateTaskCTS = new CancellationTokenSource();
		lastUpdateTask = UpdateInner(cts.Token).ContinueWith(t => cts.Dispose(), TaskContinuationOptions.OnlyOnRanToCompletion);
	}

	public object GetValue(string key) => ItemPropertyDatum.First(t => t.Key == key).Value!;

	public ItemPropertiesPanelViewModel()
	{
		ItemPropertyDatum.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Rows));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Type"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Stack"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Damage"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<float>("KnockBack"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Crit"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<float>("Scale"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("BuffType"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("BuffTime"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("HealLife"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("HealMana"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("UseTime"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("UseAnimation"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("FishingPole"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Bait"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Shoot"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<float>("ShootSpeed"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Defense"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Pick"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Axe"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Hammer"));

		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("CreateTile"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("PlaceStyle"));
		ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("CreateWall"));

		var prefix = new ItemPropertyData_ComboBox<byte>("Prefix");
		Enum.GetValues<Models.UsefulPrefixes>().ToList().ForEach(
			t => prefix.Source.Add(new Prefix(t.ToString(), (byte)t)));
		ItemPropertyDatum.Add(prefix);
		ItemPropertyDatum.Add(new ItemPropertyData_CheckBox("AutoReuse"));
		ItemPropertyDatum.Add(new ItemPropertyData_CheckBox("Accessory"));
	}

	public sealed class Prefix : ObservableObject, ILocalizationProvider
	{
		public string Key { get; }
		public byte Value { get; }

		public string Name
		{
			get
			{
				if (Key == "None")
					return LocalizationManager.Instance.GetValue($"UI.None");
				return LocalizationManager.Instance.GetValue($"Prefix.{Key}", LocalizationType.Game);
			}
		}

		public Prefix(string key, byte value)
		{
			Key = key;
			Value = value;
			LocalizationManager.RegisterLocalizationProvider(this);
		}

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			OnPropertyChanged(nameof(Name));
		}
	}
}
