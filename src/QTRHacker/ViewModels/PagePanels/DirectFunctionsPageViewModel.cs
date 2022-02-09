using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using QTRHacker.Controls;
using QTRHacker.Scripts;
using QTRHacker.Views.PagePanels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using QTRHacker.Localization;

namespace QTRHacker.ViewModels.PagePanels
{
	public class DirectFunctionsPageViewModel : ViewModelBase
	{
		public const string PATH_FUNCS = "./Content/Functions";
		public ObservableCollection<TabItem> TabItems { get; } = new();

		private TabItem GetOrCreateTab(string name)
		{
			var tab = TabItems.FirstOrDefault(t => t.Header is string v && v == name);
			if (tab == null)
			{
				tab = new TabItem() { Header = name };
				tab.Content = new FunctionsBox() { DataContext = new FunctionsBoxViewModel() };
				TabItems.Add(tab);
			}
			return tab;
		}

		public void AddFunction(BaseFunction func)
		{
			var itemsControl = GetOrCreateTab(func.Category).Content as FunctionsBox;
			itemsControl.ViewModel.Functions.Add(func);
		}

		public void UpdateFunctions()
		{
			lock (TabItems)
			{
				TabItems.Clear();
				foreach (var file in Directory.EnumerateFiles(PATH_FUNCS, "*.cs"))
				{
					try
					{
						object result = CSharpScript.EvaluateAsync(File.ReadAllText(file), ScriptOptions.Default.AddReferences(GetType().Assembly)).Result;
						if (result is BaseFunction func)
						{
							func.ApplyLocalization(LocalizationManager.Instance.CultureName);
							AddFunction(func);
							func.OnLoaded();
						}
					}
					catch //Currently we do nothing more than just skipping this function. TODO: show errors to user
					{

					}
				}
			}
		}

		public DirectFunctionsPageViewModel()
		{
			if (!Directory.Exists(PATH_FUNCS))
			{
				Directory.CreateDirectory(PATH_FUNCS);
			}
			UpdateFunctions();
		}
	}
}
