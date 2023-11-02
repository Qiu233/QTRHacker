using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
namespace QTRHacker.Controls;

public class CrossReleasedEventArgs : EventArgs
{
	internal PointerPoint Point { get; set; }
	public CrossReleasedEventArgs(PointerPoint point) { Point = point; }
}
public sealed class Cross : Control
{
	private bool Dragginng = false;
	public event EventHandler<CrossReleasedEventArgs>? CrossReleased;
	public Cross()
	{
		this.DefaultStyleKey = typeof(Cross);
	}

	protected override void OnPointerPressed(PointerRoutedEventArgs e)
	{
		base.OnPointerPressed(e);
		CapturePointer(e.Pointer);
		Dragginng = true;
		Opacity = 0;
	}
	protected override void OnPointerReleased(PointerRoutedEventArgs e)
	{
		base.OnPointerReleased(e);
		if (!Dragginng)
			return;
		ReleasePointerCapture(e.Pointer);
		Dragginng = false;
		Opacity = 1;
		CrossReleased?.Invoke(this, new CrossReleasedEventArgs(e.GetCurrentPoint(this)));
	}
	protected override void OnPointerEntered(PointerRoutedEventArgs e)
	{
		base.OnPointerEntered(e);
		ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Cross);
	}
	protected override void OnPointerExited(PointerRoutedEventArgs e)
	{
		base.OnPointerExited(e);
		if (!Dragginng)
			ProtectedCursor = null;
	}
}
