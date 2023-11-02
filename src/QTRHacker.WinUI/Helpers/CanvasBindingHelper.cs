using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Helpers;

public class CanvasBindingHelper
{
	public static readonly DependencyProperty TopBindingPathProperty =
		DependencyProperty.RegisterAttached(
			"TopBindingPath", typeof(string), typeof(CanvasBindingHelper),
			new PropertyMetadata(null, BindingPathPropertyChanged));

	public static readonly DependencyProperty LeftBindingPathProperty =
		DependencyProperty.RegisterAttached(
			"LeftBindingPath", typeof(string), typeof(CanvasBindingHelper),
			new PropertyMetadata(null, BindingPathPropertyChanged));

	public static string GetTopBindingPath(DependencyObject obj) => (string)obj.GetValue(TopBindingPathProperty);

	public static void SetTopBindingPath(DependencyObject obj, string value) => obj.SetValue(TopBindingPathProperty, value);

	public static string GetLeftBindingPath(DependencyObject obj) => (string)obj.GetValue(LeftBindingPathProperty);

	public static void SetLeftBindingPath(DependencyObject obj, string value) => obj.SetValue(LeftBindingPathProperty, value);

	private static void BindingPathPropertyChanged(
		DependencyObject obj, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not string propertyPath)
			return;
		var prop =
			e.Property == TopBindingPathProperty
			? Canvas.TopProperty
			: Canvas.LeftProperty;

		BindingOperations.SetBinding(
			obj,
			prop,
			new Binding { Path = new PropertyPath(propertyPath) });
	}
}
