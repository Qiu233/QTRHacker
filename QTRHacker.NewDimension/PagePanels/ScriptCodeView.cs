using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit;
using System.Windows.Forms.Integration;
using System.Reflection;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Highlighting;

namespace QTRHacker.NewDimension.PagePanels
{
	public partial class ScriptCodeView : UserControl
	{
		public TextEditor CodeBox
		{
			get;
		}
		private BracketHighlightRenderer BracketRender
		{
			get;
		}
		public override string Text { get => CodeBox.Text; set => CodeBox.Text = value; }
		public ScriptCodeView()
		{
			InitializeComponent();


			CodeBox = new TextEditor()
			{
				Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 62, 62, 64)),
				Foreground = System.Windows.Media.Brushes.White,
				FontFamily = new System.Windows.Media.FontFamily("Consolas"),
				FontSize = 20,
				ShowLineNumbers = true,
				HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
				VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,

			};
			CodeBox.TextArea.TextEntering += TextArea_TextEntering;
			CodeBox.TextArea.TextEntered += TextArea_TextEntered;
			Controls.Add(new ElementHost() { Bounds = new Rectangle(5, 5, 680, 405), Child = CodeBox });


			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("QTRHacker.NewDimension.XSHD.Python.xshd"))
			{
				XmlTextReader xshd_reader = new XmlTextReader(s);
				CodeBox.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
				xshd_reader.Close();
			}

			
		}

		private void TextArea_TextEntered(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
		}

		private void TextArea_TextEntering(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
		}
	}
}
