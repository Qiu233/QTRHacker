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
		public TextEditor CodeBox;
		public FontPreviewView Preview;
		public Button Convert;
		public string LastRbLib = "";

		public MainForm()
		{
			if (!File.Exists("./LastRbLib"))
				File.Create("./LastRbLib").Close();

			if (!Directory.Exists("./Content"))
				Directory.CreateDirectory("./Content");

			if (!Directory.Exists("./Content/RainbowFonts"))
				Directory.CreateDirectory("./Content/RainbowFonts");
			LastRbLib = File.ReadAllText("./LastRbLib");

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
			Controls.Add(new ElementHost() { Bounds = new Rectangle(3, 28, 400, 240), Child = CodeBox });

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("RainbowFontsMaker.XSHD.XML-Mode.xshd"))
			{
				XmlTextReader xshd_reader = new XmlTextReader(s);
				CodeBox.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);
				xshd_reader.Close();
			}

			Preview = new FontPreviewView();
			Preview.Location = new Point(440, 27);

			Controls.Add(Preview);

			Convert = new Button();
			Convert.Text = "->";
			Convert.Bounds = new Rectangle(405, 100, 30, 30);
			Convert.FlatStyle = FlatStyle.Flat;
			Convert.Click += Convert_Click;
			Controls.Add(Convert);

			MenuStrip menu = new MenuStrip()
			{
				BackColor = Color.FromArgb(100, 100, 106),
				ForeColor = Color.White,
				Renderer = new MenuStripRender()
			};
			menu.Items.Add("保存").Click += (s, e) =>
			{
				Form form = new Form
				{
					StartPosition = FormStartPosition.CenterParent,
					ClientSize = new Size(400, 100),
					FormBorderStyle = FormBorderStyle.FixedSingle,
					MaximizeBox = false,
					MinimizeBox = false,
					Text = "保存"
				};
				Label tip = new Label
				{
					Text = "请输入要保存到的字库\n" +
				"如输入：A.B.C则是保存到/Content/RainbowFonts/A/B/C.rbfont",
					Bounds = new Rectangle(2, 1, 400, 40)
				};
				form.Controls.Add(tip);

				TextBox NameBox = new TextBox()
				{
					Text = LastRbLib,
					Bounds = new Rectangle(0, 43, 396, 20),
				};
				form.Controls.Add(NameBox);

				Label tip2 = new Label
				{
					Bounds = new Rectangle(10, 76, 45, 20),
					Text = "字符："
				};
				form.Controls.Add(tip2);

				TextBox CharBox = new TextBox()
				{
					Bounds = new Rectangle(60, 73, 40, 20)
				};
				form.Controls.Add(CharBox);

				Button SaveButton = new Button
				{
					Text = "保存",
					Bounds = new Rectangle(320, 68, 80, 30)
				};
				SaveButton.Click += (s1, e1) =>
				{
					string[] fs = NameBox.Text.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
					string b = "./Content/RainbowFonts/";
					for (int i = 0; i < fs.Length - 1; i++)
					{
						b += fs[i] + "/";
						if (!Directory.Exists(b))
							Directory.CreateDirectory(b);
					}
					b += fs.Last() + ".rbfont";
					if (!File.Exists(b))
					{
						File.Create(b).Close();
						XmlDocument doc = new XmlDocument();
						var dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
						var empdata = doc.CreateElement("data");
						doc.AppendChild(dec);
						doc.AppendChild(empdata);
						using (StreamWriter sw = new StreamWriter(File.Open(b, FileMode.Open), Encoding.UTF8))
							doc.Save(sw);
					}
					XmlDocument raw = new XmlDocument();
					try
					{
						raw.LoadXml(CodeBox.Text);
					}
					catch
					{
						MessageBox.Show("你的代码存在错误，请先修正错误");
						return;
					}

					XmlDocument doc2 = new XmlDocument();
					doc2.LoadXml(File.ReadAllText(b));
					var data = doc2["data"].ChildNodes.Cast<XmlElement>().Where(t =>
					{
						return t["type"].InnerText[0] == CharBox.Text[0];
					});
					if (data.Count() == 0)
					{
						var ch = doc2.CreateElement("char");
						var type = doc2.CreateElement("type");
						type.InnerText = CharBox.Text[0].ToString();
						ch.AppendChild(type);
						ch.AppendChild(doc2.ImportNode(raw["body"], true));
						doc2["data"].AppendChild(ch);
						using (StreamWriter sw = new StreamWriter(File.Open(b, FileMode.Open), Encoding.UTF8))
							doc2.Save(sw);
						MessageBox.Show("保存成功");
					}
					else
					{
						MessageBox.Show(form, "字符出现重复，请选择保留的字符，或关闭窗口以取消选择");

						Form sform = new Form
						{
							StartPosition = FormStartPosition.CenterParent,
							ClientSize = new Size(320, 280),
							FormBorderStyle = FormBorderStyle.FixedSingle,
							MaximizeBox = false,
							MinimizeBox = false,
							Text = "请选择"
						};

						Label exi = new Label
						{
							Bounds = new Rectangle(5, 7, 80, 20),
							Text = "已存在："
						};
						sform.Controls.Add(exi);

						Label now = new Label
						{
							Bounds = new Rectangle(165, 7, 80, 20),
							Text = "当前："
						};
						sform.Controls.Add(now);

						FontPreviewView exiView = new FontPreviewView
						{
							Location = new Point(2, 33)
						};
						exiView.Image = CharactersLoader.ParseBody(data.ElementAt(0)["body"]);
						exiView.Click += (s2, e2) =>
						{
							sform.Dispose();
						};
						sform.Controls.Add(exiView);

						FontPreviewView nowView = new FontPreviewView
						{
							Location = new Point(162, 33)
						};
						nowView.Click += (s2, e2) =>
						{
							var el = data.ElementAt(0);
							el.RemoveChild(el["body"]);
							el.AppendChild(doc2.ImportNode(raw["body"], true));
							using (StreamWriter sw = new StreamWriter(File.Open(b, FileMode.Open), Encoding.UTF8))
								doc2.Save(sw);
							sform.Dispose();
							MessageBox.Show("保存成功");
						};
						nowView.Image = CharactersLoader.ParseBody(raw["body"]);
						sform.Controls.Add(nowView);

						sform.ShowDialog(form);
					}

					LastRbLib = NameBox.Text;
					File.WriteAllText("./LastRbLib", LastRbLib);
					form.Dispose();
				};
				form.Controls.Add(SaveButton);

				form.ShowDialog(this);
			};
			this.Controls.Add(menu);
		}

		private void Convert_Click(object sender, EventArgs e)
		{
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(CodeBox.Text);
			Preview.Image = CharactersLoader.ParseBody(xml["body"]);
		}
	}
}
