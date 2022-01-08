using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public partial class ChatSnippetBox : UserControl
	{
		public static Color NormalColor = Color.FromArgb(20, 255, 255, 255);
		public TextBox ColorBox, ContentBox;
		public event Action<ChatSnippetBox> OnDeleteClicked = (s) => { };
		public ChatSnippetBox()
		{
			BackColor = NormalColor;
			InitializeComponent();

			ColorBox = new TextBox();
			ColorBox.Size = new Size(40, 20);
			ColorBox.BackColor = Color.FromArgb(120, 120, 120);
			ColorBox.BorderStyle = BorderStyle.FixedSingle;
			ColorBox.ForeColor = Color.White;
			ColorBox.TextAlign = HorizontalAlignment.Center;
			ColorBox.Text = "FFFFFF";
			ColorBox.KeyPress += (s, e) =>
			{
				if (ColorBox.Text.Length == 6 && e.KeyChar != 8)
					e.Handled = true;
			};
			ColorBox.TextChanged += (s, e) =>
			{
				if (ColorBox.Text.Length == 6)
				{
					int RGB = Convert.ToInt32(ColorBox.Text, 16);
					ColorBox.ForeColor = Color.FromArgb(RGB >> 16, (RGB & 0x00FF00) >> 8, RGB & 0x0000FF);
					ContentBox.ForeColor = ColorBox.ForeColor;
				}
				else
				{
					ColorBox.ForeColor = Color.White;
					ContentBox.ForeColor = Color.White;
				}
			};
			Controls.Add(ColorBox);

			ContentBox = new TextBox();
			ContentBox.Location = new Point(43, 0);
			ContentBox.Size = new Size(195, 20);
			ContentBox.BackColor = Color.FromArgb(120, 120, 120);
			ContentBox.BorderStyle = BorderStyle.FixedSingle;
			ContentBox.ForeColor = Color.White;
			ContentBox.ImeMode = ImeMode.OnHalf;
			Controls.Add(ContentBox);

			Button deleteButton = new Button();
			//deleteButton.BackColor = Color.Transparent;
			deleteButton.FlatStyle = FlatStyle.Flat;
			deleteButton.ForeColor = Color.White;
			deleteButton.Text = HackContext.CurrentLanguage["Delete"];
			deleteButton.Bounds = new Rectangle(240, 0, 50, 20);
			deleteButton.Font = new Font(deleteButton.Font.Name, 7);
			deleteButton.Click += (s, e) =>
			{
				OnDeleteClicked(this);
			};
			Controls.Add(deleteButton);
		}
	}
}
