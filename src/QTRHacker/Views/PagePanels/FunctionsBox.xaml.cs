using QTRHacker.Controls;
using QTRHacker.Scripts;
using QTRHacker.ViewModels.PagePanels;
using System;
using System.Collections.Generic;
using System.Linq;
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
	/// <summary>
	/// FunctionsBox.xaml 的交互逻辑
	/// </summary>
	public partial class FunctionsBox : UserControl
	{
		public FunctionsBoxViewModel ViewModel => DataContext as FunctionsBoxViewModel;
		public FunctionsBox()
		{
			InitializeComponent();
		}

		private void FunctionButton_FunctionEnabling(object sender, EventArgs e)
		{
			if (sender is not FunctionButton fb || fb.DataContext is not BaseFunction func)
				return;
			if (!HackGlobal.IsActive)
				return;
			if (func.IsProgressing)
				return;
			func.IsProgressing = true;
			Task.Run(() =>
			{
				try
				{
					func.Enable(HackGlobal.GameContext);
				}
				catch (Exception ex)
				{
					HackGlobal.Logging.Log($"Exception occured when running a function named {func.Name}({func.GetType().FullName}): \n{ex.Message}\n{ex.StackTrace}");
					MessageBox.Show("Exception occured, please check the log file for more information");
				}
			}).ContinueWith(t =>
			{
				func.IsProgressing = false;
				func.Progress = 0;
			});
		}

		private void FunctionButton_FunctionDisabling(object sender, EventArgs e)
		{
			if (sender is not FunctionButton fb || fb.DataContext is not BaseFunction func)
				return;
			if (!HackGlobal.IsActive)
				return;
			if (func.IsProgressing)
				return;
			func.IsProgressing = true;
			Task.Run(() =>
			{
				func.Disable(HackGlobal.GameContext);
			}).ContinueWith(t =>
			{
				func.IsProgressing = false;
				func.Progress = 0;
			});
		}

		private void PART_TooltipButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
	}
}
