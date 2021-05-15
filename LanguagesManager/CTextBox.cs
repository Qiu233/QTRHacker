using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LanguagesManager
{
	public class CTextBox : TextBox
	{
		public CTextBox()
		{
			BackColor = Color.FromArgb(105, 110, 105);
			ForeColor = Color.White;
			BorderStyle = BorderStyle.Fixed3D;
			TextAlign = HorizontalAlignment.Center;
			Font = new Font(SystemFonts.DefaultFont.FontFamily, 12);
		}
	}
}
