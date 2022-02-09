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

		public event EventHandler<FunctionStatusChangedEventArgs> FunctionEnabling;
		public event EventHandler<FunctionStatusChangedEventArgs> FunctionDisabling;

		protected override void OnClick()
		{
			FunctionStatusChangedEventArgs args = new();
			if (!IsCheckable)
			{
				FunctionEnabling?.Invoke(this, args);
				return;
			}
			if (!IsChecked.GetValueOrDefault())
			{
				FunctionEnabling?.Invoke(this, args);
				IsChecked = args.Success;
			}
			else
			{
				FunctionDisabling?.Invoke(this, args);
				IsChecked = !args.Success;
			}
		}
	}
	public sealed class FunctionStatusChangedEventArgs : EventArgs
	{
		public bool Success { get; set; } = true;
	}
}
