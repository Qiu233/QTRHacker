using QTRHacker.Assets;
using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki.Item
{
	public class ItemInfoSubPageViewModel : ViewModelBase, ILocalizationProvider
	{
		private string tooltip;
		private string value;
		private int selectedRecipeFrom = 0;

		public ItemInfo ItemInfo { get; }
		public string Tooltip => tooltip;
		public string Value => value;
		public ObservableCollection<RecipeFromInfo> RecipeFroms { get; } = new();
		public ObservableCollection<ItemStackInfo> RecipeTos { get; } = new();
		public int SelectedRecipeFrom
		{
			get => selectedRecipeFrom;
			set
			{
				selectedRecipeFrom = value;
				OnPropertyChanged(nameof(SelectedRecipeFrom));
			}
		}

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			string key = $"ItemTooltip.{ItemInfo.Key}";
			tooltip = LocalizationManager.Instance.GetValue(key);
			if (tooltip == key)
				tooltip = string.Empty;
			OnPropertyChanged(nameof(Tooltip));

			string P = LocalizationManager.Instance.GetValue("UI.ItemValue.Platinum");
			string G = LocalizationManager.Instance.GetValue("UI.ItemValue.Gold");
			string S = LocalizationManager.Instance.GetValue("UI.ItemValue.Silver");
			string C = LocalizationManager.Instance.GetValue("UI.ItemValue.Copper");
			int p = ItemInfo.Data.Value / 1000000;
			int t = ItemInfo.Data.Value % 1000000;
			int g = t / 10000;
			t %= 10000;
			int s = t / 100;
			t %= 100;
			int c = t;
			value = $"{p}{P} {g}{G} {s}{S} {c}{C}";
			OnPropertyChanged(nameof(Value));
		}

		public ItemInfoSubPageViewModel(ItemInfo itemInfo)
		{
			ItemInfo = itemInfo;
			int type = itemInfo.Type;
			var pRe = WikiResLoader.RecipeDatum.Where(t => t.TargetItem.Type == type).ToList();
			if (pRe.Any())
				for (int i = 0; i < pRe.Count; i++)
					RecipeFroms.Add(new RecipeFromInfo(i.ToString(), pRe[i]));
			pRe = WikiResLoader.RecipeDatum.Where(
				t => t.RequiredItems.Where(
					y => type != 0 && y.Type == type).Any()).ToList();
			pRe.Sort((a, b) => a.TargetItem.Type - b.TargetItem.Type);
			if (pRe.Any())
				foreach (var item in pRe)
					RecipeTos.Add(new ItemStackInfo(item.TargetItem));
			LocalizationManager.RegisterLocalizationProvider(this);
		}
	}
}
