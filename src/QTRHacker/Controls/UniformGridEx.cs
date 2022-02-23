using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QTRHacker.Controls
{
	public class UniformGridEx : Panel
	{
		private int rows, columns;

		public int Columns
		{
			get => (int)GetValue(ColumnsProperty);
			set => SetValue(ColumnsProperty, value);
		}

		public static readonly DependencyProperty ColumnsProperty =
				DependencyProperty.Register(
						nameof(Columns),
						typeof(int),
						typeof(UniformGridEx),
						new FrameworkPropertyMetadata(
								0,
								FrameworkPropertyMetadataOptions.AffectsMeasure),
						new ValidateValueCallback(ValidateRowAndColumn));

		public int Rows
		{
			get => (int)GetValue(RowsProperty);
			set => SetValue(RowsProperty, value);
		}

		public static readonly DependencyProperty RowsProperty =
				DependencyProperty.Register(
						nameof(Rows),
						typeof(int),
						typeof(UniformGridEx),
						new FrameworkPropertyMetadata(
								(int)0,
								FrameworkPropertyMetadataOptions.AffectsMeasure),
						new ValidateValueCallback(ValidateRowAndColumn));

		public static int GetRow(UIElement target) =>
			(int)target.GetValue(RowProperty);
		public static void SetRow(UIElement target, int value) =>
			target.SetValue(RowProperty, value);

		public static readonly DependencyProperty RowProperty =
				DependencyProperty.RegisterAttached(
						"Row",
						typeof(int),
						typeof(UniformGridEx),
						new FrameworkPropertyMetadata(
								0,
								FrameworkPropertyMetadataOptions.AffectsMeasure),
						new ValidateValueCallback(ValidateRowAndColumn));

		public static int GetColumn(UIElement target) =>
			(int)target.GetValue(ColumnProperty);
		public static void SetColumn(UIElement target, int value) =>
			target.SetValue(ColumnProperty, value);

		public static readonly DependencyProperty ColumnProperty =
				DependencyProperty.RegisterAttached(
						"Column",
						typeof(int),
						typeof(UniformGridEx),
						new FrameworkPropertyMetadata(
								0,
								FrameworkPropertyMetadataOptions.AffectsMeasure),
						new ValidateValueCallback(ValidateRowAndColumn));

		private static bool ValidateRowAndColumn(object o) => (int)o >= 0;

		protected override Size MeasureOverride(Size availableSize)
		{
			GetRowsAndColumns();

			Size childConstraint = new(availableSize.Width / columns, availableSize.Height / rows);
			double maxChildDesiredWidth = 0.0;
			double maxChildDesiredHeight = 0.0;

			foreach (UIElement child in InternalChildren)
			{
				child.Measure(childConstraint);
				Size childDesiredSize = child.DesiredSize;
				if (maxChildDesiredWidth < childDesiredSize.Width)
					maxChildDesiredWidth = childDesiredSize.Width;

				if (maxChildDesiredHeight < childDesiredSize.Height)
					maxChildDesiredHeight = childDesiredSize.Height;
			}

			return new Size((maxChildDesiredWidth * columns), (maxChildDesiredHeight * rows));
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			double xStep = arrangeSize.Width / columns;
			double yStep = arrangeSize.Height / rows;

			foreach (UIElement child in InternalChildren)
			{
				int x = (int)child.GetValue(ColumnProperty);
				int y = (int)child.GetValue(RowProperty);
				child.Arrange(new Rect(x * xStep, y * yStep, xStep, yStep));
			}

			return arrangeSize;
		}

		private void GetRowsAndColumns()
		{
			rows = Rows;
			columns = Columns;
			if (rows == 0)
			{
				foreach (UIElement e in InternalChildren)
					rows = Math.Max(rows, (int)e.GetValue(RowProperty));
				rows++;
			}
			if (columns == 0)
			{
				foreach (UIElement e in InternalChildren)
					columns = Math.Max(columns, (int)e.GetValue(ColumnProperty));
				columns++;
			}
		}
	}
}
