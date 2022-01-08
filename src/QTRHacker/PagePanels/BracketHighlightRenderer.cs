using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace QTRHacker.PagePanels
{
	public class BracketHighlightRenderer : IBackgroundRenderer
	{
		private Pen borderPen;
		private Brush backgroundBrush;
		public int Begin
		{
			get; set;
		}
		public int End
		{
			get; set;
		}
		public bool Enabled
		{
			get; set;
		}


		public static readonly Color DefaultBackground = Color.FromArgb(22, 0, 0, 255);
		public static readonly Color DefaultBorder = Color.FromArgb(52, 0, 0, 255);


		public BracketHighlightRenderer()
		{

			this.borderPen = new Pen(new SolidColorBrush(DefaultBackground), 1);
			this.borderPen.Freeze();

			this.backgroundBrush = new SolidColorBrush(DefaultBorder);
			this.backgroundBrush.Freeze();

		}


		public KnownLayer Layer
		{
			get
			{
				return KnownLayer.Selection;
			}
		}

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			if (!Enabled)
				return;
			BackgroundGeometryBuilder builder = new BackgroundGeometryBuilder();

			builder.CornerRadius = 1;
			builder.AlignToWholePixels = true;

			builder.AddSegment(textView, new TextSegment() { StartOffset = Begin, Length = 1 });
			builder.CloseFigure();
			builder.AddSegment(textView, new TextSegment() { StartOffset = End, Length = 1 });

			Geometry geometry = builder.CreateGeometry();
			if (geometry != null)
			{
				drawingContext.DrawGeometry(backgroundBrush, borderPen, geometry);
			}
		}
	}
}