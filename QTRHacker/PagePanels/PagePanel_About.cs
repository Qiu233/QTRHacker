using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.PagePanels
{
    public class PagePanel_About : PagePanel
    {
        public PagePanel_About(int Width, int Height) : base(Width, Height)
        {
            AutoScroll = true;
			TextBox content = new()
			{
				BackColor = Color.FromArgb(255, 50, 50, 50),
				ForeColor = Color.White,
				BorderStyle = BorderStyle.None,
				Multiline = true,
				ScrollBars = ScrollBars.Vertical,
				ReadOnly = true,
				Size = new Size(Width, Height)
			};
			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Text.About.txt"))
            {
                var ss = new StreamReader(s);
                content.Text = ss.ReadToEnd();
            }
            Controls.Add(content);
        }
    }
}
