using QTRHacker.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QTRHacker.ViewModels.Wiki.NPC
{
	public class NPCInfoPagesViewModel : ViewModelBase
	{
		private NPCInfo npcInfo;
		private string keyword = "";
		private string keywordInput = "";
		private bool isFilterSuspended;
		public NPCInfo NPCInfo
		{
			get => npcInfo;
			set
			{
				npcInfo = value;
				OnPropertyChanged(nameof(NPCInfo));
			}
		}
		public ObservableCollection<NPCCategoryFilter> CategoryFilters { get; } = new();


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
		public event EventHandler KeywordChanged;
		public event EventHandler NPCCategoryFilterSelectedChanged;
		public event EventHandler FilterSuspended;
		public event EventHandler FilterResumed;

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

		public NPCInfoPagesViewModel()
		{
			var values = Enum.GetValues<NPCCategory>();
			foreach (var cate in values)
			{
				var filter = new NPCCategoryFilter(cate);
				filter.SelectedChanged += (s, e) => NPCCategoryFilterSelectedChanged?.Invoke(s, e);
				CategoryFilters.Add(filter);
			}
		}
	}
}
