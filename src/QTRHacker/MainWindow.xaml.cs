using QTRHacker.Assets;
using QTRHacker.Controls;
using QTRHacker.Localization;
using QTRHacker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QTRHacker
{
	public partial class MainWindow : MWindow
	{
		public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
		public MainWindow()
		{
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
			Title = "QTRHacker-" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.ExceptionObject is not Exception ex)
				return;
			string log = $"Unhandled exception from {sender}:\nIsTerminating: {e.IsTerminating}\n{ex.Message}\n{ex.StackTrace}";
			HackGlobal.Logging.Error(log);
			MessageBox.Show("Unhandled exception occured, please check the log for more information");
			Environment.Exit(0);
		}

		static MainWindow()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			GameImages.Touch();
		}
	}
}
