using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MButtonStrip : Panel
	{
		private int Index
		{
			get;
			set;
		}
		private int ButtonHeight
		{
			get;
		}
		private int ButtonWidth
		{
			get;
		}
		public MButtonStrip(int bWidth, int bHeight)
		{
			ButtonWidth = bWidth;
			ButtonHeight = bHeight;
		}
		public MButton AddButton(string txt)
		{
			MButton btn = new MButton
			{
				Bounds = new System.Drawing.Rectangle(0, ButtonHeight * (Index++), ButtonWidth, ButtonHeight),
				Text = txt
			};
			Controls.Add(btn);
			return btn;
		}
	}
}
