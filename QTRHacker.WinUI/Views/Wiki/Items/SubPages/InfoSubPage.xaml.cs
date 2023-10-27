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
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Views.Wiki.Items.SubPages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class InfoSubPage : Page
{
    public InfoSubPage()
    {
        this.InitializeComponent();
	}
	private readonly List<ColumnDefinition> _columns = new();
	private double _colSize = 0.0;
	private void ColumnSizeChanged(object sender, SizeChangedEventArgs e)
	{
		var item = (FrameworkElement)sender;
		var grid = (Grid)item.Parent;
		var column = grid.ColumnDefinitions[0];
		if (!_columns.Contains(column))
		{
			_columns.Add(column);
		}
		var adjustments = new List<ColumnDefinition>();
		if (item.ActualWidth > _colSize)
		{
			_colSize = item.ActualWidth;
			adjustments.AddRange(_columns);
		}
		else
		{
			adjustments.Add(column);
		}
		foreach (var col in adjustments)
		{
			col.Width = new GridLength(_colSize);
		}
	}
}
