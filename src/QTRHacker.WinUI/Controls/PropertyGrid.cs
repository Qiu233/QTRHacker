using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Controls;
public enum PropertyEntryType
{
	Int, Float, Double, String,
}
public class PropertyEntry : DependencyObject
{
	public static readonly DependencyProperty NameProperty =
		DependencyProperty.Register(nameof(Name), typeof(string), typeof(PropertyEntry), new PropertyMetadata(null));
	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}
	public static readonly DependencyProperty TypeProperty =
		DependencyProperty.Register(nameof(Type), typeof(PropertyEntryType), typeof(PropertyEntry), new PropertyMetadata(PropertyEntryType.Int));
	public PropertyEntryType Type
	{
		get => (PropertyEntryType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}
}

[ContentProperty(Name = nameof(Entries))]
public sealed class PropertyGrid : Control
{
	public static readonly DependencyProperty ColumnsProperty =
		DependencyProperty.Register(nameof(Columns), typeof(int), typeof(PropertyGrid), new PropertyMetadata(1, OnGridChanged));
	public int Columns
	{
		get => (int)GetValue(ColumnsProperty);
		set => SetValue(ColumnsProperty, value);
	}
	public static readonly DependencyProperty EntriesProperty =
		DependencyProperty.Register(nameof(Entries), typeof(IList<PropertyEntry>), typeof(PropertyGrid), new PropertyMetadata(null, OnGridChanged));
	public IList<PropertyEntry> Entries
	{
		get => (IList<PropertyEntry>)GetValue(EntriesProperty);
	}
	private Grid MainGrid => (Grid)GetTemplateChild("MainGrid");
	private static void OnGridChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		if (o is not PropertyGrid grid)
			return;
		if (grid.MainGrid is null)
			return;
		grid.GenerateGrid(grid.Entries, grid.Columns);
	}
	protected override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		GenerateGrid(Entries, Columns);
	}
	private void GenerateGrid(IEnumerable<PropertyEntry>? entries, int columns)
	{
		if (entries is null)
			return;
		MainGrid.ColumnDefinitions.Clear();
		MainGrid.RowDefinitions.Clear();
		var el = entries.ToList();
		for (int i = 0; i < columns; i++)
		{
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
		}
		int num = el.Count;
		int rows = (num + columns - 1) / columns;
		for (int i = 0; i < rows; i++)
		{
			MainGrid.RowDefinitions.Add(new RowDefinition());
		}
		for (int i = 0; i < num; i++)
		{
			int x = i % columns;
			int y = i / columns;
			var t = new TextBlock();
			t.Text = el[i].Name;
			t.SetValue(Grid.ColumnProperty, x * 2);
			t.SetValue(Grid.RowProperty, y);
			MainGrid.Children.Add(t);
		}
	}
	public PropertyGrid()
	{
		this.DefaultStyleKey = typeof(PropertyGrid);
		this.SetValue(EntriesProperty, new List<PropertyEntry>());
	}
}
