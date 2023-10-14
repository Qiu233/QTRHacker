using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QTRHacker.ViewModels.PlayerEditor;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Views.PlayerEditor;

public sealed partial class ItemPropertiesPanel : UserControl
{
	public ItemPropertiesPanelViewModel ViewModel => (ItemPropertiesPanelViewModel)DataContext;

	public ItemPropertiesPanel()
	{
		this.InitializeComponent();
	}

	private void TextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
	{
		if (sender is not TextBox)
			return;
		if (e.Key == Windows.System.VirtualKey.Enter)
		{
			e.Handled = true;
			this.Focus(FocusState.Programmatic);
		}
	}
}
