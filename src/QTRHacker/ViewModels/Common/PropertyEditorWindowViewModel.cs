using QTRHacker.Commands;
using QTRHacker.ViewModels.Common.PropertyEditor;
using QTRHacker.Views.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.ViewModels.Common
{
	public class PropertyEditorWindowViewModel : ViewModelBase
	{
		private PropertyBase selectedProperty;

		public ObservableCollection<PropertyBase> Roots { get; } = new();
		public RelayCommand HelpCommand { get; }
		public RelayCommand ScopeToCommand { get; }
		public RelayCommand ScopeToInNewWindowCommand { get; }

		public PropertyBase SelectedProperty
		{
			get => selectedProperty;
			set
			{
				selectedProperty = value;
				OnPropertyChanged(nameof(SelectedProperty));
			}
		}

		public PropertyEditorWindowViewModel()
		{
			HelpCommand = new RelayCommand(o => true, o =>
			{
				string msg = Localization.LocalizationManager.Instance.GetValue("UI.PropertyEditor.Help");
				MessageBox.Show(msg, "Help");
			});
			ScopeToCommand = new RelayCommand(o => true, o =>
			{
				Roots.Clear();
				var clone = SelectedProperty.Clone() as PropertyBase;
				clone.IsExpanded = SelectedProperty.IsExpanded;
				Roots.Add(clone);
			});
			ScopeToInNewWindowCommand = new RelayCommand(o => true, o =>
			{
				PropertyEditorWindow window = new();
				window.DataContext = new PropertyEditorWindowViewModel();
				var clone = SelectedProperty.Clone() as PropertyBase;
				clone.IsExpanded = SelectedProperty.IsExpanded;
				window.ViewModel.Roots.Add(clone);
				window.Show();
			});
		}
	}
}
