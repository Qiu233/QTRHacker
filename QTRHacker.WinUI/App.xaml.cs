using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using QTRHacker.Assets;
using QTRHacker.Containers;
using QTRHacker.Localization;
using QTRHacker.ViewModels;
using QTRHacker.ViewModels.Settings;
using QTRHacker.Views.Settings;
using StrongInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		InitializeComponent();
	}

	private Containers.QTRHacker? Hack;

	/// <summary>
	/// Invoked when the application is launched.
	/// </summary>
	/// <param name="args">Details about the launch request and process.</param>
	protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
	{
		Hack = new Containers.QTRHacker();
		await Hack.Show();
		m_window = Hack.MainWindow;
		InitSettings();
	}

	private async void InitSettings()
	{
		ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
		var firstStartup = localSettings.Values["FirstStartup"];
		if (firstStartup is not false) // on first startup, this is null, thus is not false
		{
			using var ownedVM = Hack!.Resolve<LanguageSelectionViewModel>();
			SelectLanguageDialog dialog = new(ownedVM.Value)
			{
				XamlRoot = WindowXamlRoot
			};
			await dialog.ShowAsync();
			localSettings.Values["FirstStartup"] = false;
		}
	}

	public static bool IsAdministrator()
	{
		var identity = WindowsIdentity.GetCurrent();
		var principal = new WindowsPrincipal(identity);
		return principal.IsInRole(WindowsBuiltInRole.Administrator);
	}

	private MainWindow? m_window;

	public MainWindow Window => m_window!;

	public static MainWindow MainWindow => Instance.Window;

	public static App Instance => ((App)Application.Current);
	public static XamlRoot WindowXamlRoot => Instance.Window.Content.XamlRoot;

	public static string Version
	{
		get
		{
			Package package = Package.Current;
			PackageId packageId = package.Id;
			PackageVersion version = packageId.Version;

			return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
		}
	}

	public static string GameVersion => "1.4.4.9";
}
