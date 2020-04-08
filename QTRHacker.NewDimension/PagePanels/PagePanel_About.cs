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
            this.AutoScroll = true;
            TextBox content = new TextBox();
            content.BackColor = Color.FromArgb(255, 50, 50, 50);
            content.ForeColor = Color.White;
            content.BorderStyle = BorderStyle.None;
            content.Multiline = true;
            content.ScrollBars = ScrollBars.Vertical;
            content.ReadOnly = true;
            content.Size = new Size(Width, Height);
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Text.About.txt"))
            {
                var ss = new StreamReader(s);
                content.Text = ss.ReadToEnd();
            }
            content.Text += "\r\n\r\n";
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.Res.Text.Donators.txt"))
            {
                var ss = new StreamReader(s);
                content.Text += ss.ReadToEnd();
            }
            Controls.Add(content);
        }
    }
}
