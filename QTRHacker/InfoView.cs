using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Terraria_Hacker
{
	public class InfoView : UserControl
	{
		public enum View
		{
			Image, Text, None
		}
		public string TipString { get { return Tip.Text; } set { Tip.Text = value; } }
		public string TextString { get { return ValueText.Text; } set { ValueText.Text = value; } }
		public Color TipColor { get { return Tip.BackColor; } set { Tip.BackColor = value; } }
		public Image Image
		{
			get { return Pic.Image; }
			set
			{
				Pic.Image = value;
			}
		}
		private Label Tip;
		private TextBox ValueText;
		private PictureBox Pic;
		private View view;
		public InfoView(int width, int height, View v, ContentAlignment ca)
		{
			Size = new Size(width, height);
			view = v;
			BorderStyle = BorderStyle.FixedSingle;
			Tip = new Label();
			Tip.Width = width;
			Tip.Height = 20;
			Tip.TextAlign = ContentAlignment.MiddleCenter;
			Tip.BorderStyle = BorderStyle.FixedSingle;
			this.Controls.Add(Tip);
			if (ca == ContentAlignment.TopCenter)
			{
				if (v == View.Text)
				{
					ValueText = new TextBox();
					ValueText.Multiline = true;
					ValueText.Location = new Point(0, 20);
					ValueText.Width = width;
					ValueText.Height = height - 20;
					ValueText.BorderStyle = BorderStyle.FixedSingle;
					this.Controls.Add(ValueText);
				}
				else if (v == View.Image)
				{
					Pic = new PictureBox();
					Pic.SizeMode = PictureBoxSizeMode.CenterImage;
					Pic.Location = new Point(0, 20);
					Pic.Width = width;
					Pic.Height = height - 20;
					Pic.BorderStyle = BorderStyle.FixedSingle;
					this.Controls.Add(Pic);
				}
				else
				{
					Tip.Height = this.Height;
				}
			}
			else if (ca == ContentAlignment.MiddleLeft)
			{
				Tip.Width = width / 2;
				Tip.Height = height;
				if (v == View.Text)
				{
					ValueText = new TextBox();
					ValueText.Multiline = true;
					ValueText.TextAlign = HorizontalAlignment.Center;
					ValueText.Location = new Point(Tip.Width, 0);
					ValueText.Width = width - Tip.Width;
					ValueText.Height = height;
					ValueText.BorderStyle = BorderStyle.FixedSingle;
					this.Controls.Add(ValueText);
				}
				else
				{
					Tip.Height = this.Height;
				}
			}
		}

	}
}
