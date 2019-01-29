using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Controls
{
	public class PopupProgressBar : Form
	{
		public Label TipLabel { get; }
		public Label PercentLabel { get; }
		public MProgressBar MainProgressBar { get; }
		public PopupProgressBar()
		{
			FormBorderStyle = FormBorderStyle.FixedSingle;
			ClientSize = new Size(300, 60);
			ControlBox = false;
			BackColor = Color.LightGray;
			TipLabel = new Label
			{
				BackColor = Color.Transparent,
				Text = "请稍等...",
				Location = new Point(0, 0),
				Size = new Size(150, 30),
				TextAlign = ContentAlignment.MiddleCenter
			};
			PercentLabel = new Label
			{
				BackColor = Color.Transparent,
				Location = new Point(150, 0),
				Size = new Size(50, 30),
				TextAlign = ContentAlignment.MiddleCenter
			};
			MainProgressBar = new MProgressBar
			{
				Location = new Point(0, 30),
				Size = new Size(300, 30),
			};
			this.Controls.Add(TipLabel);
			this.Controls.Add(PercentLabel);
			this.Controls.Add(MainProgressBar);
		}
	}
}
