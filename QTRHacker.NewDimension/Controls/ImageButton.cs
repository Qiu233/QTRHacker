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
	public partial class ImageButton : UserControl
	{
		public new string Text { get; set; }
		public Image Image { get; set; }
		private Font TextFont;
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
		//private Color NormalColor = Color.FromArgb(43, 43, 42);
		public static Color NormalColor = Color.Transparent;
		public static Color HoverColor = Color.FromArgb(50, 50, 45);
		public static Color SelectedColor = Color.FromArgb(30, 30, 30);
		public static Color MouseDownColor = Color.FromArgb(40, 40, 37);
		public ImageButton()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			UpdateStyles();
			InitializeComponent();
			BackColor = NormalColor;
			ForeColor = Color.White;
			Selected = false;
			TextFont = new Font("Arial", 10);
			//BorderStyle = BorderStyle.FixedSingle;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!Selected)
				this.BackColor = MouseDownColor;
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!Selected)
				this.BackColor = HoverColor;
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
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawImage(Image, new Rectangle(4, 4, 22, 22));
			using (Brush b = new SolidBrush(ForeColor))
				e.Graphics.DrawString(Text, TextFont, b, 30, 8);
		}
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!Enabled) ForeColor = Color.FromArgb(200, 200, 200);
			else ForeColor = Color.White;
		}
	}
}
