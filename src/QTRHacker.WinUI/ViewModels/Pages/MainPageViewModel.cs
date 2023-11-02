using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace QTRHacker.ViewModels.Pages;

public record ProcessItem(int Pid, string Name, bool UAC);
public partial class MainPageViewModel : PageViewModel
{
	[ObservableProperty]
	private string processSearchKey = "";
	public ObservableCollection<ProcessItem> Processes { get; } = new();
	public ObservableCollection<ProcessItem> FilteredProcesses { get; } = new();
	public MainPageViewModel()
	{
		this.PropertyChanged += MainPageViewModel_PropertyChanged;
	}

	private void MainPageViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ProcessSearchKey))
		{
			FilteredProcesses.Clear();
			var items = Processes.Where(t => t.Name.Contains(ProcessSearchKey, StringComparison.OrdinalIgnoreCase));
			foreach (var t in items)
			{
				FilteredProcesses.Add(t);
			}
		}
	}

	public ProcessItem? AttachedTo
	{
		get
		{
			if (HackGlobal.GameContext?.GameProcess is Process p && !p.HasExited)
				return new ProcessItem(p.Id, p.ProcessName, p.IsProcessOwnerAdmin());
			return null;
		}
	}

	public async Task LoadProcesses()
	{
		Processes.Clear();
		FilteredProcesses.Clear();
		List<int> prior = new();
		int i = 0;
		foreach (var t in Process.GetProcesses())
		{
			if (t.ProcessName.Contains("Terraria", StringComparison.OrdinalIgnoreCase))
				prior.Add(i);
			var uac = await Task.Run(() => t.IsProcessOwnerAdmin());
			var item = new ProcessItem(t.Id, t.ProcessName, uac);
			Processes.Add(item);
			await Task.Yield();
			i++;
		}
		for (int j = 0; j < prior.Count; j++)
			Processes.Move(prior[j], j);
		OnPropertyChanged(nameof(ProcessSearchKey));
	}

	public async Task<bool> AttachTo(int pid)
	{
		var p = Process.GetProcessById(pid);
		if (p.IsProcessOwnerAdmin() && !App.IsAdministrator())
		{
			ContentDialog dialog = new();
			dialog.XamlRoot = App.WindowXamlRoot;
			dialog.Content = "Attaching failed due to targeted process permission denied. \nPlease try running hack as Administrator.";
			dialog.Title = "Error";
			dialog.PrimaryButtonText = "OK";
			await dialog.ShowAsync();
			return false;
		}
		await HackGlobal.Initialize(pid);
		OnPropertyChanged(nameof(AttachedTo));
		return true;
	}
	[RelayCommand]
	public static async Task OpenWiki()
	{
		Containers.Wiki wiki = new();
		await wiki.Show();
	}
}
