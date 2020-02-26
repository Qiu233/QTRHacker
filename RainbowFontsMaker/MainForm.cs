using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using QTRHacker.Functions.ProjectileImage.RainbowImage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Xml;

namespace RainbowFontsMaker
{
	public partial class MainForm : Form
	{
		public TextEditor CodeBox;
		public FontPreviewView Preview;
		public Button Convert;

		public MainForm()
		{
			InitializeComponent();

			BackColor = Color.FromArgb(120, 130, 130);
			CodeBox = new TextEditor()
			{
				Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 80, 80, 92)),
				Foreground = System.Windows.Media.Brushes.White,
				FontFamily = new System.Windows.Media.FontFamily("Consolas"),
				FontSize = 15,
				ShowLineNumbers = true,
				HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
				VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
			};
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RainbowFontsMaker.Res.Example.txt"))
				CodeBox.Text = new StreamReader(stream).ReadToEnd();
			Controls.Add(new ElementHost() { Bounds = new Rectangle(3, 3, 400, 240), Child = CodeBox });

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("RainbowFontsMaker.XSHD.XML-Mode.xshd"))
			{
				XmlTextReader xshd_reader = new XmlTextReader(s);
				CodeBox.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
				xshd_reader.Close();
			}

			Preview = new FontPreviewView();
			Preview.Bounds = new Rectangle(440, 2, 150, 240);

			Controls.Add(Preview);

			Convert = new Button();
			Convert.Text = "->";
			Convert.Bounds = new Rectangle(405, 100, 30, 30);
			Convert.FlatStyle = FlatStyle.Flat;
			Convert.Click += Convert_Click;
			Controls.Add(Convert);
		}

		private void Convert_Click(object sender, EventArgs e)
		{
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(CodeBox.Text);
			Preview.Image = CharactersLoader.ParseBody(xml["body"]);
		}
	}
}
