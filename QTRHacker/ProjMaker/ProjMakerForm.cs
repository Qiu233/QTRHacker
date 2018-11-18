using QTRHacker.Functions.ProjectileImage;
using QTRHacker.ProjMaker.Parse;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
		private string FileName;

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
		public ProjMakerForm(string file)
		{
			FileName = $".\\Projs\\{file}.projimg";
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
			AddMenuItem(FileMenuItem, "保存", (s, e) => Save());
			AddMenuItem(FileMenuItem, "重新打开", (s, e) => Open());
			MenuStrip.Items.Add(FileMenuItem);

			ToolStripMenuItem CompileMenuItem = new ToolStripMenuItem("编译")
			{
				ForeColor = Color.White,
			};
			CompileMenuItem.Click += CompileMenuItem_Click;
			MenuStrip.Items.Add(CompileMenuItem);
			Controls.Add(MenuStrip);

			CodeView = new CodeView()
			{
				Location = new Point(5, 30)
			};
			Controls.Add(CodeView);
			Open();
		}

		private void CompileMenuItem_Click(object sender, EventArgs e)
		{
			Save();
			Parser p = new Parser(CodeView.Text);
			var ctx = MainForm.Context;
			ProjImage img = null;
			try
			{
				img = p.Parse();
				img.Emit(ctx, ctx.MyPlayer.X, ctx.MyPlayer.Y);
			}
			catch (ParseException pe)
			{
				string[] s = pe.Message.Split(new string[] { "," }, StringSplitOptions.None);
				if (s[0] == "un")
				{
					MessageBox.Show($"编译失败，索引为{s[1]}开头的Token类型未知");
				}
				else if (s[0] == "ex")
				{
					MessageBox.Show($"编译失败，索引为{s[1]}开头的Token超出预期");
				}
				CodeView.CodeBox.Select(Convert.ToInt32(s[1]), 1);
				CodeView.CodeBox.ScrollToCaret();
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);
		}

		private void Save()
		{
			using (var f = new StreamWriter(File.Open(FileName, FileMode.Create)))
			{
				f.Write(CodeView.Text);
				f.Flush();
			}
		}

		private void Open()
		{
			using (var f = new StreamReader(File.Open(FileName, FileMode.OpenOrCreate)))
			{
				CodeView.Text = f.ReadToEnd();
			}
		}

	}
}
