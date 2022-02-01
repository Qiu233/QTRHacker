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
		private static readonly string[] Properties = {
			"Type",         "Damage",
			"Stack",        "KnockBack",
			"Crit",         "Scale",
			"BuffType",     "BuffTime",
			"HealMana",     "HealLife",
			"UseTime",      "UseAnimation",
			"FishingPole",  "Bait",
			"Shoot",        "ShootSpeed",
			"Defense",      "Pick",
			"Axe",          "Hammer",
			"CreateTile",  "PlaceStyle",
			"CreateWall",
		};
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
				if (prop.Error == null)
					prop.UpdateToItem(item);
		}

		public ItemPropertiesPanelViewModel()
		{
			ItemPropertyDatum.CollectionChanged += ItemPropertyDatum_CollectionChanged;
			foreach (var prop in Properties)
				ItemPropertyDatum.Add(new ItemPropertyData(prop));
		}

		private void ItemPropertyDatum_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged(nameof(Rows));
		}
	}
}
