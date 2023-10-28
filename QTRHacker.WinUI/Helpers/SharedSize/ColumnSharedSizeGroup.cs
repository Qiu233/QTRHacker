using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Helpers.SharedSize;

public class ColumnSharedSizeGroup : DependencyObject
{
	private readonly List<ColumnDefinition> _columns = new();
	private double _colSize = 0.0;
	public void Update(FrameworkElement item)
	{
		var grid = (Grid)item.Parent;
		var column = grid.ColumnDefinitions[(int)item.GetValue(Grid.ColumnProperty)];
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
