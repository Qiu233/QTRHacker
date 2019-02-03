using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileMaker.Parse;
using QTRHacker.NewDimension.Controls;
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
using System.Windows.Forms.Integration;

namespace QTRHacker.NewDimension.PagePanels
{
	public partial class ProjMakerForm : MForm
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
		private TextEditor LogBox;
		private string FileName;

		private static string[] KEYS = { "MACRO", "DEF", "INSERT", "POINT", "RECT", "RECT_FILLED", "FIXED" };

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
			ClientSize = new Size(800, 590);
			FileName = $".\\Projs\\{file}.projimg";
			Text = "弹幕编辑器-名称：" + file;
			BackColor = sBlackColor;
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
			MainPanel.Controls.Add(MenuStrip);

			CodeView = new CodeView(KEYS)
			{
				Location = new Point(5, 30)
			};
			MainPanel.Controls.Add(CodeView);

			LogBox = new TextEditor()
			{
				Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(250, 92, 92, 94)),
				Foreground = System.Windows.Media.Brushes.White,
				FontFamily = new System.Windows.Media.FontFamily("Consolas"),
				FontSize = 14,
				HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
				VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
				WordWrap = true
			};
			MainPanel.Controls.Add(new ElementHost() { Bounds = new Rectangle(5, 450, 790, 105), Child = LogBox });

			Open();
		}

		public void OutputLog(string s)
		{
			LogBox.AppendText(s + "\n");
		}

		private void CompileMenuItem_Click(object sender, EventArgs e)
		{
			Save();
			Parser p = new Parser(CodeView.Text);
			var ctx = HackContext.GameContext;
			ProjImage img = null;
#if DEBUG
			img = p.Parse();
			img.Emit(ctx, ctx.MyPlayer.X, ctx.MyPlayer.Y);
#else
			try
			{
				LogBox.Text = "";
				OutputLog(DateTime.Now.ToString());
				img = p.Parse();
				OutputLog($"编译成功，生成了{img.Projs.Count}个弹幕");
				if (ctx != null)
				{
					img.Emit(ctx, ctx.MyPlayer.X, ctx.MyPlayer.Y);
					OutputLog($"已经发射到游戏");
				}
				else
				{
					OutputLog($"未锁定游戏进程，发射弹幕失败");
				}
			}
			catch (ParseException pe)
			{
				/*string[] s = pe.Message.Split(new string[] { "," }, StringSplitOptions.None);
				if (s[0] == "un")
					OutputLog($"编译失败，索引为{s[1]}开头的Token类型未知");
				else if (s[0] == "ex")
					OutputLog($"编译失败，索引为{s[1]}开头的Token超出预期");
				else if (s[0] == "ab")
					OutputLog($"编译失败，名称为{s[2]}的宏不存在");
				CodeView.CodeBox.Select(Convert.ToInt32(s[1]), 1);*/
				OutputLog(pe.Message);
				CodeView.CodeBox.Select(pe.Offset, 1);
			}
			OutputLog("\n\n");
#endif
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
