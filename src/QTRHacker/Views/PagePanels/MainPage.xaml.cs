using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace QTRHacker.Views.PagePanels
{
	public partial class MainPage : UserControl
	{
		[DllImport("User32.dll")]
		private static extern nuint WindowFromPoint(int x, int y);
		[DllImport("User32.dll")]
		private static extern void GetWindowThreadProcessId(nuint hwnd, out int ID);
		public MainPage()
		{
			InitializeComponent();
		}

		public event EventHandler AttachedToGame;

		private void Cross_CrossReleased(object sender, CrossReleaseEventArgs e)
		{
			Point p = (sender as Control).PointToScreen(e.Point);
			nuint hwnd = WindowFromPoint((int)p.X, (int)p.Y);
			GetWindowThreadProcessId(hwnd, out int pid);
			if (pid == 0)
			{
				MessageBox.Show("Failed to fetch pid(got 0)", "Error");
				return;
			}
			else if (pid == Environment.ProcessId)
			{
				MessageBox.Show("Please drag the cross to Terraria's window.", "Error");
				return;
			}
			HackGlobal.Initialize(pid);
			AttachedToGame?.Invoke(this, new EventArgs());
		}
	}
}
