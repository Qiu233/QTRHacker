using Newtonsoft.Json.Linq;
using QTRHacker.NewDimension.Controls;
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

namespace QTRHacker.NewDimension.Wiki
{
	public partial class WikiForm : Form
	{
		private MTabControl MainTab;
		private ItemsTabPage ItemsTabPage;
		public WikiForm()
		{
			InitializeComponent();
			

			MainTab = new MTabControl();
			MainTab.bColor = Color.DarkGray;
			MainTab.tColor = Color.Gray;

			ItemsTabPage = new ItemsTabPage() { Text = "Items" };


			MainTab.TabPages.Add(ItemsTabPage);
			MainTab.Size = ClientSize;
			Controls.Add(MainTab);
		}
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Task.Run((Action)ItemsTabPage.RefreshItems);
		}
	}
}
