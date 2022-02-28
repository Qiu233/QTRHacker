using QTRHacker.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki.NPC
{
	public class NPCCategoryFilter : ViewModelBase, ILocalizationProvider
	{
		private bool isChecked = true;
		private string hint;

		public bool IsSelected
		{
			get => isChecked;
			set
			{
				isChecked = value;
				OnPropertyChanged(nameof(IsSelected));
				SelectedChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		public event EventHandler SelectedChanged;
		public string Hint => hint;
		public NPCCategory Category { get; }

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			hint = LocalizationManager.Instance.GetValue($"UI.NPCCategories.{Category}");
			OnPropertyChanged(nameof(Hint));
		}

		public NPCCategoryFilter(NPCCategory category)
		{
			Category = category;
			LocalizationManager.RegisterLocalizationProvider(this);
		}
	}
}
