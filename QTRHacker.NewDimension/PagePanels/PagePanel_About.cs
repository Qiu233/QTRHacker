using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_About : PagePanel
	{
		public PagePanel_About(int Width, int Height) : base(Width, Height)
		{
			Label content = new Label();
			content.BackColor = Color.FromArgb(20, 255, 255, 255);
			content.ForeColor = Color.White;
			content.Size = new Size(Width, Height);
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Text.About.txt"))
			{
				var ss = new StreamReader(s);
				content.Text = ss.ReadToEnd();
			}
			Controls.Add(content);
		}
	}
}
