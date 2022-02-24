using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace QTRHacker.Controls
{
	/// <summary>
	/// The fucking ToggleButton cannot cancel Checked/Unchecked events.
	/// That's why I need this.
	/// </summary>
	public class FunctionButton : ToggleButton
	{
		static FunctionButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FunctionButton), new FrameworkPropertyMetadata(typeof(FunctionButton)));
		}

		public static readonly DependencyProperty IsCheckableProperty =
			DependencyProperty.Register(nameof(IsCheckable), typeof(bool), typeof(FunctionButton));
		public bool IsCheckable
		{
			get => (bool)GetValue(IsCheckableProperty);
			set => SetValue(IsCheckableProperty, value);
		}

		public static readonly DependencyProperty HasProgressProperty =
			DependencyProperty.Register(nameof(HasProgress), typeof(bool), typeof(FunctionButton), 
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
		public bool HasProgress
		{
			get => (bool)GetValue(HasProgressProperty);
			set => SetValue(HasProgressProperty, value);
		}

		public static readonly DependencyProperty ProgressMaximumProperty =
			DependencyProperty.Register(nameof(ProgressMaximum), typeof(double), typeof(FunctionButton),
				new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
		public double ProgressMaximum
		{
			get => (double)GetValue(ProgressMaximumProperty);
			set => SetValue(ProgressMaximumProperty, value);
		}

		public static readonly DependencyProperty ProgressValueProperty =
			DependencyProperty.Register(nameof(ProgressValue), typeof(double), typeof(FunctionButton),
				new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
		public double ProgressValue
		{
			get => (double)GetValue(ProgressValueProperty);
			set => SetValue(ProgressValueProperty, value);
		}

		public static readonly DependencyProperty IsProgressingProperty =
			DependencyProperty.Register(nameof(IsProgressing), typeof(bool), typeof(FunctionButton), 
				new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
		public bool IsProgressing
		{
			get => (bool)GetValue(IsProgressingProperty);
			set => SetValue(IsProgressingProperty, value);
		}

		public event EventHandler FunctionEnabling;
		public event EventHandler FunctionDisabling;

		protected override void OnClick()
		{
			if (!IsCheckable)
				FunctionEnabling?.Invoke(this, EventArgs.Empty);
			if (!IsChecked.GetValueOrDefault())
				FunctionEnabling?.Invoke(this, EventArgs.Empty);
			else
				FunctionDisabling?.Invoke(this, EventArgs.Empty);
		}
	}
}
