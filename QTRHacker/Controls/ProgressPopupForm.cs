using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.Controls
{
	public class ProgressPopupForm : Form
	{
		private readonly ProgressBar ProgressBar;
		private readonly Label Title, Percent;
		public ProgressPopupForm(int width, int maximum, string title)
		{
			ClientSize = new Size(width, 60);
			ControlBox = false;
			this.FormBorderStyle = FormBorderStyle.None;

			Percent = new Label
			{
				Bounds = new Rectangle(0, 0, width / 3, 30),
				TextAlign = ContentAlignment.MiddleCenter
			};
			Controls.Add(Percent);

			Title = new Label
			{
				Bounds = new Rectangle(width / 3, 0, width / 3 * 2, 30),
				TextAlign = ContentAlignment.MiddleLeft,
				Text = title
			};
			Controls.Add(Title);

			ProgressBar = new ProgressBar
			{
				Maximum = maximum,
				Minimum = 0,
				Bounds = new Rectangle(0, 30, width, 30),
			};
			Controls.Add(ProgressBar);
		}
		public void SetValue(int v)
		{
			ProgressBar.Value = v;
			Percent.Text = $"{ProgressBar.Value}/{ProgressBar.Maximum}";
		}
		public async void Run(Control back, Action<Action<int>> task, int timeout = 10000)
		{
			back.Enabled = false;
			ProgressBar.Value = 0;
			Show(back);
			Location = new Point(back.Location.X + back.Width / 2 - ClientSize.Width / 2,
				back.Location.Y + back.Height / 2 - ClientSize.Height / 2);
			await Task.WhenAny(
				Task.Run(() => task(SetValue)),
				Task.Run(() =>//timeout
				{
					System.Threading.Thread.Sleep(timeout);
					ProgressBar.Value = ProgressBar.Maximum;
					System.Threading.Thread.Sleep(200);
				}));

			Dispose();
			back.Enabled = true;
		}
	}
}
