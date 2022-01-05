using Newtonsoft.Json.Linq;
using QTRHacker.Controls;
using QTRHacker.Res;
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
	public partial class WikiForm : MForm
	{
		private readonly MTabControl MainTab;
		private readonly ItemsTabPage ItemsTabPage;
		private readonly NPCTabPage NPCTabPage;
		public WikiForm()
		{
			Size = new Size(740, 512);
			Text = "Wiki";
			using (Stream st = new MemoryStream(GameResLoader.ItemImageData["Item_3628"]))
				Icon = MainForm.ConvertToIcon(Image.FromStream(st));

			MainTab = new MTabControl();
			MainTab.HeaderBackColor = Color.FromArgb(80, 80, 80);
			MainTab.HeaderSelectedBackColor = Color.FromArgb(120, 120, 120);
			MainTab.ForeColor = Color.White;

			ItemsTabPage = new ItemsTabPage() { Text = "Items", BackColor = MainTab.HeaderBackColor };
			NPCTabPage = new NPCTabPage() { Text = "NPCs", BackColor = MainTab.HeaderBackColor };


			MainTab.TabPages.Add(ItemsTabPage);
			MainTab.TabPages.Add(NPCTabPage);
			MainTab.Size = ClientSize;
			MainPanel.Controls.Add(MainTab);
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
