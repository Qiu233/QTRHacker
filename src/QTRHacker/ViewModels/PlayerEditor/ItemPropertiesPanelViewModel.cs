using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemPropertiesPanelViewModel : ViewModelBase
	{
		public ObservableCollection<ItemPropertyData> ItemPropertyDatum
		{
			get;
		} = new();
		private int _Columns = 2;

		public int Rows => (ItemPropertyDatum.Count + Columns - 1) / Columns;

		public int Columns
		{
			get => _Columns;
			set
			{
				_Columns = value;
				OnPropertyChanged(nameof(Columns));
				OnPropertyChanged(nameof(Rows));
			}
		}
		public void UpdatePropertiesFromItem(Item item)
		{
			foreach (var prop in ItemPropertyDatum)
				prop.UpdateFromItem(item);
		}

		public void UpdatePropertiesToItem(Item item)
		{
			foreach (var prop in ItemPropertyDatum)
				prop.UpdateToItem(item);
		}

		public object GetValue(string key) => ItemPropertyDatum.First(t => t.Key == key).GetValue();

		public ItemPropertiesPanelViewModel()
		{
			ItemPropertyDatum.CollectionChanged += ItemPropertyDatum_CollectionChanged;

			ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Type"));
			ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Damage"));
			ItemPropertyDatum.Add(new ItemPropertyData_TextBox<int>("Stack"));
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

		private void ItemPropertyDatum_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged(nameof(Rows));
		}

		public readonly struct Prefix
		{
			public string Name { get; }
			public byte Value { get; }

			public Prefix(string name, byte value)
			{
				Name = name;
				Value = value;
			}
		}
	}
}
