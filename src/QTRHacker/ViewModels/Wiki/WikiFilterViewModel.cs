using System.Windows.Input;
using QTRHacker.Commands;

namespace QTRHacker.ViewModels.Wiki;

public abstract class WikiFilterViewModel<TFilter> : ViewModelBase
	where TFilter : CategoryFilterViewModel
{
	private string keyword = string.Empty;
	private string keywordInput = string.Empty;
	private bool isFilterSuspended;

	public IReadOnlyList<TFilter> CategoryFilters { get; }

	public string Keyword
	{
		get => keyword;
		set
		{
			if (SetProperty(ref keyword, value ?? string.Empty))
				OnFilterChanged();
		}
	}

	public string KeywordInput
	{
		get => keywordInput;
		set => SetProperty(ref keywordInput, value ?? string.Empty);
	}

	public bool IsFilterSuspended
	{
		get => isFilterSuspended;
		private set => SetProperty(ref isFilterSuspended, value);
	}

	public ICommand ApplyKeyword { get; }
	public ICommand ReverseSelection { get; }
	public ICommand ResetFilter { get; }

	public event EventHandler FilterChanged;

	protected WikiFilterViewModel(IEnumerable<TFilter> categoryFilters)
	{
		CategoryFilters = categoryFilters.ToList().AsReadOnly();
		foreach (var filter in CategoryFilters)
			filter.SelectedChanged += (_, _) => OnFilterChanged();

		ApplyKeyword = new RelayCommand(_ => UpdateFilters(() => Keyword = KeywordInput));
		ReverseSelection = new RelayCommand(_ => UpdateFilters(() =>
		{
			foreach (var filter in CategoryFilters)
				filter.IsSelected = !filter.IsSelected;
		}));
		ResetFilter = new RelayCommand(_ => UpdateFilters(() =>
		{
			foreach (var filter in CategoryFilters)
				filter.IsSelected = true;
			KeywordInput = string.Empty;
			Keyword = string.Empty;
		}));
	}

	private void UpdateFilters(Action update)
	{
		bool wasSuspended = IsFilterSuspended;
		try
		{
			IsFilterSuspended = true;
			update();
		}
		finally
		{
			IsFilterSuspended = wasSuspended;
			OnFilterChanged();
		}
	}

	private void OnFilterChanged()
	{
		if (!IsFilterSuspended)
			FilterChanged?.Invoke(this, EventArgs.Empty);
	}
}
