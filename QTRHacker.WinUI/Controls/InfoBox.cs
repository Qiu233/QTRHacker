using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QTRHacker.Controls;

public sealed class InfoBox : Control
{
	public static readonly DependencyProperty ContentDockProperty = DependencyProperty.Register(nameof(ContentDock), typeof(Dock), typeof(InfoBox), new PropertyMetadata(Dock.Right));
	public Dock ContentDock
	{
		get => (Dock)GetValue(ContentDockProperty);
		set => SetValue(ContentDockProperty, value);
	}
	public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(object), typeof(InfoBox), new PropertyMetadata(null));
	public object Header
	{
		get => GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}
	public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(InfoBox), new PropertyMetadata(null));
	public DataTemplate HeaderTemplate
	{
		get => (DataTemplate)GetValue(HeaderTemplateProperty);
		set => SetValue(HeaderTemplateProperty, value);
	}
	public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(InfoBox), new PropertyMetadata(null));
	public object Content
	{
		get => GetValue(ContentProperty);
		set => SetValue(ContentProperty, value);
	}
	public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(InfoBox), new PropertyMetadata(null));
	public DataTemplate ContentTemplate
	{
		get => (DataTemplate)GetValue(ContentTemplateProperty);
		set => SetValue(ContentTemplateProperty, value);
	}

	public InfoBox()
	{
		this.DefaultStyleKey = typeof(InfoBox);
	}
}
