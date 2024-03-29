﻿using CommunityToolkit.Mvvm.ComponentModel;
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

	public async Task InvokeUpdate()
	{
		foreach (var prop in ItemPropertyDatum)
		{
			await prop.UpdateFromItem();
		}
	}

	public object? this[string key] => ItemPropertyDatum.FirstOrDefault(t => t.Key == key)?.Value;

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
