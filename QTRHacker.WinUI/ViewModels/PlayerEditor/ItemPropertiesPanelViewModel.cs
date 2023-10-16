using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.Localization;
using QTRHacker.ViewModels.PlayerEditor.ItemProperties;
using System.Collections.ObjectModel;
using Windows.Foundation.Collections;

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

	private void UpdateItem()
	{
		async Task UpdateInner(CancellationToken ct)
		{
			foreach (var prop in ItemPropertyDatum)
			{
				ct.ThrowIfCancellationRequested();
				await prop.UpdateFromItem();
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


	public ItemPropertiesPanelViewModel(
		Func<string, ItemPropertyData_TextBox<int>> intMaker,
		Func<string, ItemPropertyData_TextBox<float>> floatMaker,
		Func<string, ItemPropertyData_ComboBox<byte>> byteComboMaker,
		Func<string, ItemPropertyData_CheckBox> checkBoxMaker)
	{
		ItemPropertyDatum.CollectionChanged += (s, e) => OnPropertyChanged(nameof(Rows));

		ItemPropertyDatum.Add(intMaker("Type"));
		ItemPropertyDatum.Add(intMaker("Stack"));
		ItemPropertyDatum.Add(intMaker("Damage"));
		ItemPropertyDatum.Add(floatMaker("KnockBack"));
		ItemPropertyDatum.Add(intMaker("Crit"));
		ItemPropertyDatum.Add(floatMaker("Scale"));

		ItemPropertyDatum.Add(intMaker("BuffType"));
		ItemPropertyDatum.Add(intMaker("BuffTime"));

		ItemPropertyDatum.Add(intMaker("HealLife"));
		ItemPropertyDatum.Add(intMaker("HealMana"));

		ItemPropertyDatum.Add(intMaker("UseTime"));
		ItemPropertyDatum.Add(intMaker("UseAnimation"));

		ItemPropertyDatum.Add(intMaker("FishingPole"));
		ItemPropertyDatum.Add(intMaker("Bait"));

		ItemPropertyDatum.Add(intMaker("Shoot"));
		ItemPropertyDatum.Add(floatMaker("ShootSpeed"));

		ItemPropertyDatum.Add(intMaker("Defense"));

		ItemPropertyDatum.Add(intMaker("Pick"));
		ItemPropertyDatum.Add(intMaker("Axe"));
		ItemPropertyDatum.Add(intMaker("Hammer"));

		ItemPropertyDatum.Add(intMaker("CreateTile"));
		ItemPropertyDatum.Add(intMaker("PlaceStyle"));
		ItemPropertyDatum.Add(intMaker("CreateWall"));

		var prefix = byteComboMaker("Prefix");
		Enum.GetValues<Models.UsefulPrefixes>().ToList().ForEach(
			t => prefix.Source.Add(new Prefix(t.ToString(), (byte)t)));
		ItemPropertyDatum.Add(prefix);
		ItemPropertyDatum.Add(checkBoxMaker("AutoReuse"));
		ItemPropertyDatum.Add(checkBoxMaker("Accessory"));
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
