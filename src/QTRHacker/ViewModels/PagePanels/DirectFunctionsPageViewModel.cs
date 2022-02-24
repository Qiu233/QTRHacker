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
using QTRHacker.Functions;
using System.Security.Cryptography;
using System.Windows;
using System.ComponentModel;

namespace QTRHacker.ViewModels.PagePanels
{
	public class DirectFunctionsPageViewModel : PagePanelViewModel
	{
		public const string PATH_FUNCS = "./Content/Functions";

		//TODO: merge the two variables into one, using ItemTemplate
		public ObservableCollection<TabItem> TabItems { get; } = new();
		private readonly List<FunctionCategory> Functions = new();

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

		private FunctionCategory LoadFunctionsFromFile(string file)
		{
			try
			{
				object result = CSharpScript.EvaluateAsync(File.ReadAllText(file), ScriptOptions.Default.AddReferences(GetType().Assembly)).Result;
				if (result is FunctionCategory fc)
					return fc;
			}
			catch (Exception e)
			{
				HackGlobal.Logging.Error($"Failed to initialize from file: {file}");
				HackGlobal.Logging.Error(e.Message + "\n" + e.StackTrace);
			}
			return null;
		}

		public void LoadAllFunctions()
		{
			HackGlobal.Logging.Enter("Initializing functions from scripts.");
			var fs = Directory.EnumerateFiles(PATH_FUNCS, "*.cs")
				.ToList()
				.Select(t => LoadFunctionsFromFile(t))
				.Where(t => t is not null);
			Application.Current.Dispatcher.BeginInvoke(() =>
			{
				Functions.Clear();
				Functions.AddRange(fs);
			});
			HackGlobal.Logging.Exit();
		}

		public void UpdateUI()
		{
			HackGlobal.Logging.Enter("Updating UI from functions.");
			TabItems.Clear();
			foreach (FunctionCategory group in Functions)
			{
				var itemsControl = GetOrCreateTab(group[LocalizationManager.Instance.CultureName]).Content as FunctionsBox;
				var manager = RemoteDataManager<bool>.Create(HackGlobal.GameContext, SHA256.HashData(Encoding.UTF8.GetBytes(group.Category)));
				int index = 0;
				foreach (var func in group)
				{
					func.IsEnabled = manager[index];
					int id = index; // to capture these two local variables
					var sm = manager;
					func.PropertyChanged += (s, e) =>
					{
						if (e.PropertyName == nameof(BaseFunction.IsEnabled))
						{
							BaseFunction f = s as BaseFunction;
							sm[id] = f.IsEnabled;
						}
					};
					itemsControl.ViewModel.Functions.Add(func);
					index++;
				}
			}
			if (TabItems.Any())
			{
				TabItems[0].IsSelected = true;
			}
			DispatchOnLoaded();
			HackGlobal.Logging.Exit();
		}

		private void DispatchOnLoaded()
		{
			foreach (var tab in TabItems)
			{
				var box = tab.Content as FunctionsBox;
				foreach (var item in box.ViewModel.Functions)
					item.OnLoaded();
			}
		}

		public DirectFunctionsPageViewModel()
		{
			if (!Directory.Exists(PATH_FUNCS))
			{
				Directory.CreateDirectory(PATH_FUNCS);
			}
		}
	}
}
