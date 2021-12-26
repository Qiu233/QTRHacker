using Newtonsoft.Json.Linq;
using QTRHacker.Controls;
using QTRHacker.Wiki.Item;
using QTRHacker.Wiki.NPC;
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

namespace QTRHacker.Wiki
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
			MainTab.Enabled = false;
			await Task.Run(ItemsTabPage.RefreshItems);
			await Task.Run(NPCTabPage.RefreshNPCs);
			MainTab.Enabled = true;//only after everything is loaded
		}
	}
}
