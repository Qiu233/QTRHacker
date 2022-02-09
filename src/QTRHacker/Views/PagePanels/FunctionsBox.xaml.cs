using QTRHacker.Controls;
using QTRHacker.Scripts;
using QTRHacker.ViewModels.PagePanels;
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

		private void FunctionButton_FunctionEnabling(object sender, FunctionStatusChangedEventArgs e)
		{
			if (sender is not FunctionButton fb || fb.DataContext is not BaseFunction func)
				return;
			if (!HackGlobal.IsActive)
				return;
			e.Success = func.Enable(HackGlobal.GameContext) == FunctionResult.SUCCESS;
		}

		private void FunctionButton_FunctionDisabling(object sender, FunctionStatusChangedEventArgs e)
		{
			if (sender is not FunctionButton fb || fb.DataContext is not BaseFunction func)
				return;
			if (!HackGlobal.IsActive)
				return;
			e.Success = func.Disable(HackGlobal.GameContext) == FunctionResult.SUCCESS;
		}
	}
}
