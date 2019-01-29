using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Controls
{
	public partial class TextButton : UserControl
	{
		public new string Text { get; set; }
		private Font SFont;
		private bool _Selected = false;
		public event Action<object, EventArgs> OnSelected = (s, e) => { };
		public bool Selected
		{
			get => _Selected;
			set
			{
				_Selected = value;
				if (_Selected)
					this.BackColor = SelectedColor;
				else
					this.BackColor = NormalColor;
				OnSelected(this, new EventArgs());
			}
		}
		public static Color NormalColor = Color.Transparent;
		public static Color HoverColor = Color.FromArgb(40, 40, 35);
		public static Color SelectedColor = Color.FromArgb(25, 25, 25);
		public TextButton()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			UpdateStyles();
			InitializeComponent();
			BackColor = Color.Transparent;
			SFont = new Font("Arial", 10);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using (Brush b = new SolidBrush(ForeColor))
				e.Graphics.DrawString(Text, SFont, b, 5, 4);
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			if (!Selected)
				this.BackColor = HoverColor;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!Selected)
				this.BackColor = NormalColor;
		}
	}
}
