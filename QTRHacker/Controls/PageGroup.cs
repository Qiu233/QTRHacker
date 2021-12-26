using QTRHacker.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class PageGroup : UserControl
	{
		private bool _expanded;
		private int ButtonsNumber = 0;
		public int ExpandedWidth = 100;
		public int NonExpandedWidth = 30;

		public bool Expanded
		{
			get => _expanded;
			set
			{
				_expanded = value;
				this.Width = _expanded ? ExpandedWidth : NonExpandedWidth;
			}
		}
		public PageGroup()
		{
			BackColor = Color.FromArgb(255, 74, 74, 74);
		}

		public ImageButton AddButton(string Text, Image Icon, Control Content, Action<object, EventArgs> OnSelected)
		{
			ImageButton b = new ImageButton();
			b.Location = new Point(0, 30 * (ButtonsNumber++));
			b.Image = Icon;
			b.Text = Text;
			this.Controls.Add(b);
			b.OnSelected += OnSelected;
			return b;
		}

	}
}
