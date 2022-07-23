using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.Controls
{
	public class MvvmTextEditor : TextEditor
	{
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register(nameof(Text), typeof(string), typeof(MvvmTextEditor),
			new PropertyMetadata((obj, args) =>
			{
				TextEditor target = (MvvmTextEditor)obj;
				if (target.Text != (string)args.NewValue)
					target.Text = (string)args.NewValue;
			})
		);

		public new string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (Text != base.Text)
				Text = base.Text;
			base.OnTextChanged(e);
		}
	}
}
