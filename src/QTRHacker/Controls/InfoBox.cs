using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QTRHacker.Controls;

public class InfoBox : ContentControl
{
	public Dock TipDock
	{
		get => (Dock)GetValue(TipDockProperty);
		set => SetValue(TipDockProperty, value);
	}
	public static readonly DependencyProperty TipDockProperty =
		DependencyProperty.Register(nameof(TipDock), typeof(Dock), typeof(InfoBox));

	public string Tip
	{
		get => (string)GetValue(TipProperty);
		set => SetValue(TipProperty, value);
	}
	public static readonly DependencyProperty TipProperty =
		DependencyProperty.Register(nameof(Tip), typeof(string), typeof(InfoBox));

	public Brush TipBackground
	{
		get => (Brush)GetValue(TipBackgroundProperty);
		set => SetValue(TipBackgroundProperty, value);
	}
	public static readonly DependencyProperty TipBackgroundProperty =
		DependencyProperty.Register(nameof(TipBackground), typeof(Brush), typeof(InfoBox));
	
	public string TipSharedSizeScope
	{
		get => (string)GetValue(TipSharedSizeScopeProperty);
		set => SetValue(TipSharedSizeScopeProperty, value);
	}
	public static readonly DependencyProperty TipSharedSizeScopeProperty =
		DependencyProperty.Register(nameof(TipSharedSizeScope), typeof(string), typeof(InfoBox));

	private TextBlock part_TipTextBlock;

	public TextBlock TipTextBlock => part_TipTextBlock;

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		part_TipTextBlock = GetTemplateChild("PART_TipTextBlock") as TextBlock;
	}

	static InfoBox()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoBox), new FrameworkPropertyMetadata(typeof(InfoBox)));
	}


}
