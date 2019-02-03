using QTRHacker.NewDimension.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.PagePanels
{
	public class PagePanel_GameData : PagePanel
	{
		private Button GetDataButton;
		private MTreeView DataTreeView;
		public PagePanel_GameData(int Width, int Height) : base(Width, Height)
		{
			DataTreeView = new MTreeView();
			DataTreeView.Bounds = new Rectangle(3, 33, Width - 6, Height - 100);
			Controls.Add(DataTreeView);

			GetDataButton = new Button();
			GetDataButton.BackColor = Color.FromArgb(100, 150, 150, 150);
			GetDataButton.FlatStyle = FlatStyle.Flat;
			GetDataButton.Bounds = new Rectangle(3, 2, 120, 30);
			GetDataButton.Text = "获得游戏数据";
			GetDataButton.Click += GetDataButton_Click;
			Controls.Add(GetDataButton);
		}

		private void GetDataButton_Click(object sender, EventArgs e)
		{
			UpdateData();
		}

		public void UpdateData()
		{
		}
	}
}
