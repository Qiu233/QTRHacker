using QTRHacker.Functions.GameObjects;
using QTRHacker.Controls;
using QTRHacker.Res;
using QTRHacker.Wiki;
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
	public class PagePanel_Misc : PagePanel
	{
		public int ButtonsCount { get; set; }
		public PagePanel_Misc(int Width, int Height) : base(Width, Height)
		{
			ButtonsCount = 0;
			Image img_Wiki, img_Player;
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3628"]))
				img_Wiki = Image.FromStream(st);
			using (Stream st = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.Res.Image.player.png"))
				img_Player = Image.FromStream(st);
			AddButton(img_Wiki, "Wiki", () => { new WikiForm().Show(); });
		}
		public virtual ImageButton AddButton(Image img, string txt, Action onclick)
		{
			ImageButton btn = new ImageButton();
			btn.BorderStyle = BorderStyle.FixedSingle;
			btn.Image = img;
			btn.Text = txt;
			btn.Click += (s, e) => onclick();

			btn.Location = new Point(20 + ButtonsCount % 2 * 150, 10 + ButtonsCount / 2 * 30);
			ButtonsCount++;
			Controls.Add(btn);
			return btn;
		}
	}
}
