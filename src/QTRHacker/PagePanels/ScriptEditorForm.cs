using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using QTRHacker.Functions.ProjectileImage;
using QTRHacker.Functions.ProjectileMaker.Parse;
using QTRHacker.Controls;
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

namespace QTRHacker.PagePanels
{
	public class ScriptEditorForm : MForm
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
		private class TextEditorWriter : TextWriter
		{
			private TextEditor tb;
			public TextEditorWriter(TextEditor tb)
			{
				this.tb = tb;
			}
			public override void Write(string v)
			{
				tb.AppendText(v);
			}
			public override void WriteLine(string v)
			{
				tb.AppendText(v);
				tb.AppendText(NewLine);
			}
			public override Encoding Encoding
			{
				get { return Encoding.Unicode; }
			}
		}
		private class OnWrittenEventArgs<T> : EventArgs
		{
			public T Value
			{
				get;
				private set;
			}
			public OnWrittenEventArgs(T value)
			{
				this.Value = value;
			}
		}


		private class EventRaisingStreamWriter : StreamWriter
		{
			#region Event
			public event EventHandler<OnWrittenEventArgs<string>> StringWritten;
			#endregion

			#region CTOR
			public EventRaisingStreamWriter(Stream s) : base(s)
			{ }
			#endregion

			#region Private Methods
			private void LaunchEvent(string txtWritten)
			{
				if (StringWritten != null)
				{
					StringWritten(this, new OnWrittenEventArgs<string>(txtWritten));
				}
			}
			#endregion


			#region Overrides

			public override void Write(string value)
			{
				base.Write(value);
				LaunchEvent(value);
			}

			// here override all writing methods...

			#endregion
		}
		private class MenuStripRender : ToolStripProfessionalRenderer
		{
			public MenuStripRender() : base(new MenuColorTable())
			{
			}
		}
		private MenuStrip MenuStrip;
		private ScriptCodeView CodeView;
		private TextEditor LogBox;
		private TextEditorWriter LogBoxWriter;
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
		public ScriptEditorForm(string file)
		{
			ClientSize = new Size(700, 650);
			FileName = Path.Combine(HackContext.PATH_SCRIPTS, $"{file}.qhscript");
			Text = "ScriptEditor-Name：" + file;
			BackColor = sBlackColor;
			MenuStrip = new MenuStrip()
			{
				BackColor = Color.FromArgb(37, 37, 38),
				ForeColor = Color.White,
				Renderer = new MenuStripRender()
			};
			ToolStripMenuItem FileMenuItem = new ToolStripMenuItem(HackContext.CurrentLanguage["File"])
			{
				ForeColor = Color.White,
			};
			AddMenuItem(FileMenuItem, HackContext.CurrentLanguage["Save"], (s, e) => Save());
			AddMenuItem(FileMenuItem, HackContext.CurrentLanguage["Reopen"], (s, e) => Open());
			MenuStrip.Items.Add(FileMenuItem);

			ToolStripMenuItem CompileMenuItem = new ToolStripMenuItem(HackContext.CurrentLanguage["Execute"])
			{
				ForeColor = Color.White,
			};
			CompileMenuItem.Click += CompileMenuItem_Click;
			MenuStrip.Items.Add(CompileMenuItem);
			MainPanel.Controls.Add(MenuStrip);

			CodeView = new ScriptCodeView()
			{
				Location = new Point(5, 30)
			};
			CodeView.CodeBox.KeyDown += (s, e) =>
			{
				if (e.Key == System.Windows.Input.Key.F5)
					Execute();
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
				WordWrap = false
			};
			MainPanel.Controls.Add(new ElementHost() { Bounds = new Rectangle(5, 450, 690, 160), Child = LogBox });
			LogBoxWriter = new TextEditorWriter(LogBox);

			Open();
		}

		private void Execute()
		{
			Save();

			LogBox.Clear();
			LogBox.AppendText(DateTime.Now.ToString() + "\n");
			MemoryStream ms = new MemoryStream();
			EventRaisingStreamWriter outputWr = new EventRaisingStreamWriter(ms);
			outputWr.StringWritten += sWr_StringWritten;

			var o = HackContext.QHScriptEngine.Runtime.IO.OutputStream;
			HackContext.QHScriptEngine.Runtime.IO.SetOutput(ms, outputWr);
			var scope = HackContext.CreateScriptScope(HackContext.QHScriptEngine);
			HackContext.QHScriptEngine.Execute(CodeView.Text, scope);
			HackContext.QHScriptEngine.Runtime.IO.SetOutput(o, Encoding.UTF8);

			void sWr_StringWritten(object sd, OnWrittenEventArgs<string> ev)
			{
				LogBox.AppendText(ev.Value);
			}
		}

		private void CompileMenuItem_Click(object sender, EventArgs e)
		{
			Execute();
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
