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
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using System.Text.RegularExpressions;

namespace QTRHacker.PagePanels
{
	internal sealed class CustomizedBrush : HighlightingBrush
	{
		private readonly System.Windows.Media.SolidColorBrush brush;
		public CustomizedBrush(System.Windows.Media.Color c)
		{
			var c2 = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
			brush = CreateFrozenBrush(c2);
		}

		public override string ToString()
		{
			return brush.ToString();
		}

		private static System.Windows.Media.SolidColorBrush CreateFrozenBrush(System.Windows.Media.Color color)
		{
			System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush(color);
			brush.Freeze();
			return brush;
		}

		public override System.Windows.Media.Brush GetBrush(ITextRunConstructionContext context)
		{
			return brush;
		}
	}
	public class PMHighlightingDefinition : IHighlightingDefinition
	{
		public string Name => "PMHighlightingDefinition";
		private string[] Keys;
		private Dictionary<string, string> propDict = new Dictionary<string, string>();
		public HighlightingRuleSet MainRuleSet
		{
			get
			{
				var n = new HighlightingRuleSet();
				string[] wordList = Keys;

				n.Rules.Add(new HighlightingRule()
				{
					Color = new HighlightingColor()
					{
						Foreground = new CustomizedBrush(System.Windows.Media.Colors.DarkGreen)
					},
					Regex = new Regex(@"#.*")
				});

				n.Rules.Add(new HighlightingRule()
				{
					Color = new HighlightingColor()
					{
						Foreground = new CustomizedBrush(System.Windows.Media.Colors.CadetBlue)
					},
					Regex = new Regex(string.Format(@"\b({0})\b", string.Join("|", wordList)))
				});

				n.Rules.Add(new HighlightingRule()
				{
					Color = new HighlightingColor()
					{
						Foreground = new CustomizedBrush(System.Windows.Media.Colors.LightGreen)
					},
					Regex = new Regex(@"\d+(\.[0-9]+)?")
				});
				return n;
			}
		}

		public PMHighlightingDefinition(IEnumerable<string> keys)
		{
			Keys = keys.ToArray();
		}

		public IEnumerable<HighlightingColor> NamedHighlightingColors
		{
			get
			{
				var n = new List<HighlightingColor>();
				return n;
			}
		}

		public IDictionary<string, string> Properties => propDict;

		public HighlightingColor GetNamedColor(string name)
		{
			throw new NotImplementedException();
		}

		public HighlightingRuleSet GetNamedRuleSet(string name)
		{
			throw new NotImplementedException();
		}
	}
	public partial class ProjectileCodeView : UserControl
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
		public ProjectileCodeView(IEnumerable<string> Keys)
		{
			InitializeComponent();


			CodeBox = new TextEditor()
			{
				Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 62, 62, 64)),
				Foreground = System.Windows.Media.Brushes.White,
				FontFamily = new System.Windows.Media.FontFamily("Consolas"),
				FontSize = 20,
				ShowLineNumbers = true,
				SyntaxHighlighting = new PMHighlightingDefinition(Keys),
				HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
				VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto
			};
			/*BracketRender = new BracketHighlightRenderer();
			CodeBox.TextArea.TextView.BackgroundRenderers.Add(BracketRender);
			CodeBox.TextArea.Caret.PositionChanged += BracketHighlighting;*/
			Controls.Add(new ElementHost() { Bounds = new Rectangle(5, 5, 780, 405), Child = CodeBox });

		}

		/*
		暂时放弃的括号匹配
		private int SearchMatchingRightBracket(string s, int j)
		{
			for (int i = j; i < s.Length; i++)
			{
				if (s[i] == '#')
					while (i < s.Length && s[i] != '\n') i++;
				else if (s[i] == '(')
					i = SearchMatchingRightBracket(s, i + 1);
				else if (s[i] == ')')
					return i;
			}
			return s.Length;
		}

		private int SearchMatchingLeftBracket(string s, int j)
		{
			for (int i = j; i >= 0; i--)//判断是否在注释内
			{
				if (s[i] == '\n') break;
				else if (s[i] == '#')
				{
					j = i - 1;
					break;
				}
			}
			for (int i = j; i >= 0; i--)
			{
				if (s[i] == '#')
					while (i < s.Length && s[i] != '\n') i++;
				else if (s[i] == ')')
					i = SearchMatchingLeftBracket(s, i - 1);
				else if (s[i] == '(')
					return i;
			}
			return -1;
		}

		private void BracketHighlighting(object sender, EventArgs e)
		{
			if (CodeBox == null) return;
			if (CodeBox.CaretOffset < CodeBox.Text.Length && CodeBox.Text[CodeBox.CaretOffset] == '(')
			{
				string s = CodeBox.Text;
				for (int i = CodeBox.CaretOffset; i >= 0; i--)//判断是否在注释内
				{
					if (s[i] == '\n') break;
					else if (s[i] == '#') return;
				}
				BracketRender.Enabled = true;
				BracketRender.Begin = CodeBox.CaretOffset;
				BracketRender.End = SearchMatchingRightBracket(s, CodeBox.CaretOffset + 1);
			}
			else if (CodeBox.CaretOffset < CodeBox.Text.Length && CodeBox.Text[CodeBox.CaretOffset] == ')')
			{
				string s = CodeBox.Text;
				for (int i = CodeBox.CaretOffset; i >= 0; i--)//判断是否在注释内
				{
					if (s[i] == '\n') break;
					else if (s[i] == '#') return;
				}
				BracketRender.Enabled = true;
				BracketRender.Begin = CodeBox.CaretOffset;
				BracketRender.End = SearchMatchingLeftBracket(s, CodeBox.CaretOffset - 1);
			}
			else
			{
				BracketRender.Enabled = false;
			}
			CodeBox.TextArea.TextView.InvalidateLayer(BracketRender.Layer);
		}*/
	}
}
