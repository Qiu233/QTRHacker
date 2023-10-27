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
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;

namespace QTRHacker.Controls;

[ContentProperty(Name = nameof(Content))]
public sealed class InfoBox : Control
{
	public static readonly DependencyProperty TipDockProperty =
		DependencyProperty.Register(nameof(TipDock), typeof(Dock), typeof(InfoBox), 
			new PropertyMetadata(Dock.Left, (s, e) =>
			{
				((InfoBox)s).UpdateDock();
			}));
	public Dock TipDock
	{
		get => (Dock)GetValue(TipDockProperty);
		set => SetValue(TipDockProperty, value);
	}
	public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header), typeof(string), typeof(InfoBox), new PropertyMetadata(null));
	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
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

	private void UpdateDock()
	{
		switch (TipDock)
		{
			case Dock.Left:
				VisualStateManager.GoToState(this, "TipDockLeft", false);
				break;
			case Dock.Top:
				VisualStateManager.GoToState(this, "TipDockTop", false);
				break;
			case Dock.Right:
				VisualStateManager.GoToState(this, "TipDockRight", false);
				break;
			case Dock.Bottom:
				VisualStateManager.GoToState(this, "TipDockBottom", false);
				break;
		}
	}

	public InfoBox()
	{
		this.DefaultStyleKey = typeof(InfoBox);
	}
}
