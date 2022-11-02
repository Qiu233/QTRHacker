using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using QTRHacker.Scripts;
using QTRHacker.Views.PagePanels;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Reflection;
using QTRHacker.Localization;
using System.Security.Cryptography;
using System.Windows;
using QTRHacker.Core;

namespace QTRHacker.ViewModels.PagePanels;

public class DirectFunctionsPageViewModel : PagePanelViewModel
{
	public const string PATH_FUNCS = "./Content/Scripts";

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
			{
				HackGlobal.Logging.Log($"Loaded function category: [{fc.Category}] from file: [{file}]");
				return fc;
			}
		}
		catch (Exception e)
		{
			HackGlobal.Logging.Error($"Failed to load functions from file: [{file}]");
			HackGlobal.Logging.Exception(e);
		}
		return null;
	}

	private static IEnumerable<FunctionCategory> LoadBuiltInFunctions()
	{
		var types = Assembly.GetExecutingAssembly().DefinedTypes.Where(
			t => t.Namespace != null &&
				t.Namespace.StartsWith("QTRHacker.Scripts.Functions")).Where(t => t.IsSubclassOf(typeof(FunctionCategory)));
		foreach (var type in types)
		{
			var fc = Activator.CreateInstance(type) as FunctionCategory;
			HackGlobal.Logging.Log($"Loaded built-in function category: [{fc.Category}]");
			yield return fc;
		}
	}

	public void Load()
	{
		DateTime t0 = DateTime.Now;
		LoadAllFunctions();
		DateTime t1 = DateTime.Now;
		HackGlobal.Logging.Log("Time used to load scripts: " + (t1 - t0).TotalMilliseconds);
		HackGlobal.Logging.Log($"Loaded functions:\n{string.Join("\n", Functions.Select(t => $"{{{t?.Category}:\t{string.Join(", ", t.Select(s => $"[{s?.Name}]"))}}}"))}");
		t0 = DateTime.Now;
		Application.Current.Dispatcher.Invoke(new Action(() => UpdateUI()));
		t1 = DateTime.Now;
		HackGlobal.Logging.Log("Time used to update UI: " + (t1 - t0).TotalMilliseconds);
	}
	private void LoadAllFunctions()
	{
		HackGlobal.Logging.Enter("Initializing functions from scripts.");
		var builtin = LoadBuiltInFunctions();
		var fs = Directory.EnumerateFiles(PATH_FUNCS, "*.cs")
			.ToList()
			.Select(t => LoadFunctionsFromFile(t))
			.Where(t => t is not null);
		Functions.Clear();
		Functions.AddRange(builtin);
		Functions.AddRange(fs);
		HackGlobal.Logging.Exit();
	}

	private void UpdateUI()
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
			TabItems[0].IsSelected = true;
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
