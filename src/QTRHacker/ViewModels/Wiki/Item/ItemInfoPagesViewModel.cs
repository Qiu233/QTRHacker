using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Core;
using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QTRHacker.ViewModels.Wiki.Item
{
	public class ItemInfoPagesViewModel : ViewModelBase, ILocalizationProvider
	{
		private string value;
		private int selectedRecipeFrom = 0;
		private ItemStackInfo selectedRecipeTo;
		private ItemInfo itemInfo;
		private string keyword = "";
		private string keywordInput = "";
		private bool isFilterSuspended;
		private readonly RelayCommand getItemStackCommand;
		private readonly RelayCommand jumpToCommand;

		public string Value => value;
		public ObservableCollection<RecipeFromInfo> RecipeFroms { get; } = new();
		public ObservableCollection<ItemStackInfo> RecipeTos { get; } = new();
		public ObservableCollection<ItemCategoryFilter> CategoryFilters { get; } = new();
		public int SelectedRecipeFrom
		{
			get => selectedRecipeFrom;
			set
			{
				selectedRecipeFrom = value;
				OnPropertyChanged(nameof(SelectedRecipeFrom));
			}
		}
		public ItemStackInfo SelectedRecipeTo
		{
			get => selectedRecipeTo;
			set
			{
				selectedRecipeTo = value;
				OnPropertyChanged(nameof(SelectedRecipeTo));
			}
		}
		public ItemInfo ItemInfo
		{
			get => itemInfo;
			set
			{
				itemInfo = value;
				OnPropertyChanged(nameof(ItemInfo));
				InitData();
			}
		}
		public string Keyword
		{
			get => keyword;
			set
			{
				keyword = value;
				OnPropertyChanged(nameof(Keyword));
				KeywordChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		public string KeywordInput
		{
			get => keywordInput;
			set
			{
				keywordInput = value;
				OnPropertyChanged(nameof(KeywordInput));
			}
		}
		public bool IsFilterSuspended
		{
			get => isFilterSuspended;
			set
			{
				isFilterSuspended = value;
				OnPropertyChanged(nameof(FilterSuspended));
				if (isFilterSuspended)
					FilterSuspended?.Invoke(this, EventArgs.Empty);
				else
					FilterResumed?.Invoke(this, EventArgs.Empty);
			}
		}

		//TODO: make these command internal field
		public ICommand ApplyKeyword => new RelayCommand(o => true, o =>
		{
			Keyword = KeywordInput;
		});
		public ICommand ReverseSelection => new RelayCommand(o => true, o =>
		{
			IsFilterSuspended = true;
			foreach (var filter in CategoryFilters)
				filter.IsSelected = !filter.IsSelected;
			IsFilterSuspended = false;
		});
		public ICommand ResetFilter => new RelayCommand(o => true, o =>
		{
			IsFilterSuspended = true;
			foreach (var filter in CategoryFilters)
				filter.IsSelected = true;
			keyword = "";
			IsFilterSuspended = false;
		});

		public RelayCommand GetItemStackCommand => getItemStackCommand;
		public RelayCommand JumpToCommand => jumpToCommand;

		public event EventHandler KeywordChanged;
		public event EventHandler ItemCategoryFilterSelectedChanged;
		public event EventHandler FilterSuspended;
		public event EventHandler FilterResumed;
		public event EventHandler<JumpToItemEventArgs> JumpToItem;

		public void InitData()
		{
			RecipeFroms.Clear();
			RecipeTos.Clear();
			if (ItemInfo == null)
				return;
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
			ApplyLocalization();
		}

		private void ApplyLocalization()
		{
			if (ItemInfo == null)
				return;

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

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			ApplyLocalization();
		}

		public ItemInfoPagesViewModel()
		{
			getItemStackCommand = new HackCommand(o =>
			{
				if (o is not ItemStackInfo stack)
					return;
				HackGlobal.GameContext.AddItemStackToInv(stack.ItemInfo.Type, stack.Stack);
			});
			jumpToCommand = new RelayCommand(o => true, o =>
			{
				if (o is not ItemStackInfo stack)
					return;
				JumpToItem?.Invoke(this, new JumpToItemEventArgs(stack.ItemInfo));
			});

			var values = Enum.GetValues<ItemCategory>();
			foreach (var cate in values)
			{
				var filter = new ItemCategoryFilter(cate);
				filter.SelectedChanged += (s, e) => ItemCategoryFilterSelectedChanged?.Invoke(s, e);
				CategoryFilters.Add(filter);
			}
			LocalizationManager.RegisterLocalizationProvider(this);
		}
	}
}
