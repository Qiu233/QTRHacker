using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class MButton : Button
	{
		public MButton()
		{
			FlatStyle = FlatStyle.Flat;
			BackColor = Color.FromArgb(100, 150, 150, 150);
			ForeColor = Color.FromArgb(20, 20, 20);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
}
