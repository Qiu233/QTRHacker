using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension
{
	public abstract class PagePanel : Panel
	{
		public PagePanel(int Width, int Height)
		{
			Size = new System.Drawing.Size(Width, Height);
		}
	}
}
