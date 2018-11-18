using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.ProjMaker
{
	public partial class CodeView : UserControl
	{
		private TextBox CodeBox;
		private const int LinesPerPage = 17;
		private const int UnitsPerPage = 10;
		public override string Text { get => CodeBox.Text; set => CodeBox.Text = value; }
		public CodeView()
		{
			InitializeComponent();

			CodeBox = new TextBox()
			{
				BackColor = Color.FromArgb(62, 62, 64),
				BorderStyle = BorderStyle.None,
				Font = new Font("Consolas", 18F),
				ForeColor = Color.White,
				Location = new Point(5, 5),
				Multiline = true,
				Size = new Size(780, 405),
				ScrollBars = ScrollBars.Vertical
			};
			Controls.Add(CodeBox);

		}

	}
}
