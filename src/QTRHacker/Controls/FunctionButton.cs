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
			IsCheckedProperty.OverrideMetadata(typeof(FunctionButton), new FrameworkPropertyMetadata((s, e) => { }, (s, e) =>
			{
				if(s is FunctionButton fb)
				{
					if (fb.IsCheckable)
						return e;
					return false;
				}
				return false;
			}));
		}

		public static readonly DependencyProperty IsCheckableProperty =
			DependencyProperty.Register(nameof(IsCheckable), typeof(bool), typeof(FunctionButton));
		public bool IsCheckable
		{
			get => (bool)GetValue(IsCheckableProperty);
			set => SetValue(IsCheckableProperty, value);
		}
	}
}
