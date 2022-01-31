using QTRHacker.Assets;
using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
		private static MainWindow instance;
		public static MainWindow Instance => instance;
		public MainWindow()
		{
			Application.Current.DispatcherUnhandledException += (s, e) =>
			{
				if(e.Exception is System.ComponentModel.Win32Exception)
					e.Handled = true;
			};
			if (instance != null)
				throw new InvalidOperationException();
			HackGlobal.LoadConfig();
			InitializeComponent();
			instance = this;
		}
		internal void EnableTabs()
		{
			if (MainTabControl.Items.Count == 0)
				return;
			foreach (TabItem item in MainTabControl.Items)
				item.IsEnabled = true;
			(MainTabControl.Items[0] as TabItem).IsSelected = true;
		}
		static MainWindow()
		{
			AssetsLoader.Touch();
		}
	}
}
