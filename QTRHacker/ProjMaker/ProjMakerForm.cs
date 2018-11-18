using QTRHacker.ProjMaker.Parse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.ProjMaker
{
	public partial class ProjMakerForm : Form
	{
		public static Color bColor = Color.FromArgb(37, 37, 38);
		public static Color sColor = Color.FromArgb(62, 62, 64);
		public static Color sBlackColor = Color.FromArgb(27, 27, 28);
		private class MenuColorTable : ProfessionalColorTable
		{
			public MenuColorTable()
			{
				base.UseSystemColors = false;
			}
			public override Color MenuItemSelected => sColor;
			public override Color MenuBorder => sBlackColor;
			public override Color MenuItemSelectedGradientBegin => sColor;
			public override Color MenuItemSelectedGradientEnd => sColor;

			public override Color MenuItemPressedGradientBegin => sBlackColor;
			public override Color MenuItemPressedGradientMiddle => sBlackColor;
			public override Color MenuItemPressedGradientEnd => sBlackColor;

			public override Color MenuStripGradientBegin => sBlackColor;
			public override Color MenuStripGradientEnd => sBlackColor;

			public override Color ToolStripDropDownBackground => sBlackColor;
			public override Color MenuItemBorder => sColor;
			public override Color ImageMarginGradientBegin => sBlackColor;
			public override Color ImageMarginGradientEnd => sBlackColor;
			public override Color ImageMarginGradientMiddle => sBlackColor;

		}
		private class MenuStripRender : ToolStripProfessionalRenderer
		{
			public MenuStripRender() : base(new MenuColorTable())
			{
			}
		}
		private MenuStrip MenuStrip;
		private CodeView CodeView;

		private static void AddMenuItem(ToolStripMenuItem menu, string text, Action<object, EventArgs> click)
		{
			var item = new ToolStripMenuItem(text)
			{
				BackColor = sBlackColor,
				ForeColor = Color.White
			};
			item.Click += new EventHandler(click);
			menu.DropDownItems.Add(item);
		}
		public ProjMakerForm()
		{
			InitializeComponent();
			this.BackColor = sBlackColor;
			MenuStrip = new MenuStrip()
			{
				BackColor = Color.FromArgb(37, 37, 38),
				ForeColor = Color.White,
				Renderer = new MenuStripRender()
			};
			ToolStripMenuItem FileMenuItem = new ToolStripMenuItem("文件")
			{
				ForeColor = Color.White,
			};
			AddMenuItem(FileMenuItem, "打开", (s, e) => Open());
			AddMenuItem(FileMenuItem, "保存", (s, e) => Save());
			MenuStrip.Items.Add(FileMenuItem);

			Controls.Add(MenuStrip);

			CodeView = new CodeView()
			{
				Location = new Point(5, 30)
			};
			Controls.Add(CodeView);
		}

		private void Save()
		{
			Parser p = new Parser(CodeView.Text);
			var ps = p.Parse();
			MessageBox.Show(ps.Projs.Count.ToString());
		}

		private void Open()
		{
		}
	}
}
