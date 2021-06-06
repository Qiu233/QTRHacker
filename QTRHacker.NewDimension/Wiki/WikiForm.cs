using Newtonsoft.Json.Linq;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Wiki.Item;
using QTRHacker.NewDimension.Wiki.NPC;
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
		private NPCTabPage NPCTabPage;
		public WikiForm()
		{
			InitializeComponent();
			

			MainTab = new MTabControl();
			MainTab.BColor = Color.DarkGray;
			MainTab.TColor = Color.Gray;

			ItemsTabPage = new ItemsTabPage() { Text = "Items" };
			NPCTabPage = new NPCTabPage() { Text = "NPCs" };


			MainTab.TabPages.Add(ItemsTabPage);
			MainTab.TabPages.Add(NPCTabPage);
			MainTab.Size = ClientSize;
			Controls.Add(MainTab);
		}
		protected override async void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Enabled = false;
			await Task.Run(ItemsTabPage.RefreshItems);
			await Task.Run(NPCTabPage.RefreshNPCs);
			Enabled = true;//only after everything is loaded
		}
	}
}
