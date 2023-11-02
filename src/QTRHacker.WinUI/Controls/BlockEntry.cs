using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Controls;

public sealed class BlockEntry : ContentControl
{
	public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(BlockEntry), new PropertyMetadata(null));
	public object Header
	{
		get => GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}
	public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(BlockEntry), new PropertyMetadata(null));
	public DataTemplate HeaderTemplate
	{
		get => (DataTemplate)GetValue(HeaderTemplateProperty);
		set => SetValue(HeaderTemplateProperty, value);
	}

	public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(object), typeof(BlockEntry), new PropertyMetadata(null, OnDescriptionChanged));
	public object Description
	{
		get => GetValue(DescriptionProperty);
		set => SetValue(DescriptionProperty, value);
	}
	public static readonly DependencyProperty DescriptionTemplateProperty = DependencyProperty.Register(nameof(DescriptionTemplate), typeof(DataTemplate), typeof(BlockEntry), new PropertyMetadata(null));
	public DataTemplate DescriptionTemplate
	{
		get => (DataTemplate)GetValue(DescriptionTemplateProperty);
		set => SetValue(DescriptionTemplateProperty, value);
	}

	public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(IconElement), typeof(BlockEntry), new PropertyMetadata(null, OnIconChanged));
	public IconElement Icon
	{
		get => (IconElement)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	private static readonly DependencyProperty ComputedDescriptionVisibilityProperty = DependencyProperty.Register(nameof(ComputedDescriptionVisibility), typeof(Visibility), typeof(BlockEntry), new PropertyMetadata(Visibility.Collapsed));

	private Visibility ComputedDescriptionVisibility
	{
		get => (Visibility)GetValue(ComputedDescriptionVisibilityProperty);
		set => SetValue(ComputedDescriptionVisibilityProperty, value);
	}

	private static readonly DependencyProperty ComputedIconVisibilityProperty = DependencyProperty.Register(nameof(ComputedIconVisibility), typeof(Visibility), typeof(BlockEntry), new PropertyMetadata(Visibility.Collapsed));

	private Visibility ComputedIconVisibility
	{
		get => (Visibility)GetValue(ComputedIconVisibilityProperty);
		set => SetValue(ComputedIconVisibilityProperty, value);
	}

	private static void OnDescriptionChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		if (o is not BlockEntry be)
			return;
		if (args.NewValue is null)
			be.ComputedDescriptionVisibility = Visibility.Collapsed;
		else
			be.ComputedDescriptionVisibility = Visibility.Visible;
	}
	private static void OnIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		if (o is not BlockEntry be)
			return;
		if (args.NewValue is null)
			be.ComputedIconVisibility = Visibility.Collapsed;
		else
			be.ComputedIconVisibility = Visibility.Visible;
	}

	public BlockEntry()
	{
		this.DefaultStyleKey = typeof(BlockEntry);
	}
}
