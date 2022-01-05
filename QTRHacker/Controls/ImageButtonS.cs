using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public partial class ImageButtonS : ImageButton
	{
		private bool _Selected = false;
		public event Action<object, EventArgs> OnSelected;
		public bool Selected
		{
			get => _Selected;
			set
			{
				_Selected = value;
				if (_Selected)
				{
					BackColor = SelectedColor;
					OnSelected?.Invoke(this, new EventArgs());
				}
				else
					BackColor = NormalColor;
			}
		}
		public ImageButtonS(Image img) : base(img)
		{
			Size = new Size(100, 30);
			Selected = false;
		}
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (Selected)
				return;
			Selected = true;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!Selected)
				BackColor = MouseDownColor;
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!Selected)
				BackColor = HoverColor;
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			if (!Selected)
				BackColor = HoverColor;
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			if (!Selected)
				BackColor = NormalColor;
		}
	}
}
