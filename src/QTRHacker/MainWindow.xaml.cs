using QTRHacker.Assets;
using QTRHacker.Controls;
using QTRHacker.Localization;
using QTRHacker.ViewModels;
using System;
using System.Reflection;

namespace QTRHacker;

public partial class MainWindow : MWindow
{
	public static MainWindow Instance { get; private set; }
	public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
	public const string GameVersion = "1.4.4.7";
	public MainWindow()
	{
		Instance = this;
		HackGlobal.LoadConfig();
		if (!HackGlobal.Config.ForceEnglish)
		{
#if DEBUG
#else
		LocalizationManager.Instance.SetCulture(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
#endif
		}
		else
			LocalizationManager.Instance.SetCulture("en");

		InitializeComponent();

		DataContext = new MainWindowViewModel();
		Title = $"QTRHacker-{Assembly.GetExecutingAssembly().GetName().Version} for {GameVersion}";

	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e.ExceptionObject is not Exception ex)
			return;
		string log = $"Unhandled exception from {sender}:\nIsTerminating: {e.IsTerminating}";
		HackGlobal.Logging.Error(log);
		HackGlobal.Logging.Exception(ex);
		HackGlobal.AlertExceptionOccured(ex);
		Environment.Exit(0);
	}

	static MainWindow()
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		GameImages.Touch();
	}
}
