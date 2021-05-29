using Newtonsoft.Json.Linq;
using QTRHacker.NewDimension.Controls;
using QTRHacker.NewDimension.Res;
using QTRHacker.NewDimension.Wiki.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QTRHacker.NewDimension.Wiki.Item
{
	public class ItemInfoSubPage : TabPage
	{
		private readonly Color ItemsColor = Color.FromArgb(160, 160, 200);
		public InfoView ItemIconInfoView, ItemNameInfoView, ItemTypeInfoView, ItemRareInfoView, ItemDescriptionInfoView, ItemRecipeFromInfoView, ItemRecipeToInfoView, ItemValueInfoView;
		public ListBox RecipeToItems;
		public MTabControl RequireItems;

		public event Action<object, MouseEventArgs> OnRequireItemDoubleClick = (s, e) => { };
		public event Action<object, MouseEventArgs> OnRecipeToItemDoubleClick = (s, e) => { };

		public ItemInfoSubPage() : base(HackContext.CurrentLanguage["ItemInfo"])
		{
			ItemIconInfoView = new InfoView(new PictureBox() { SizeMode = PictureBoxSizeMode.CenterImage }, InfoView.TipDock.Top);
			ItemIconInfoView.Text = HackContext.CurrentLanguage["Icon"];
			ItemIconInfoView.Bounds = new Rectangle(5, 5, 80, 80);
			ItemIconInfoView.Tip.BackColor = ItemsColor;

			ItemNameInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.TipDock.Left, false);
			ItemNameInfoView.Text = HackContext.CurrentLanguage["Name"];
			ItemNameInfoView.Tip.BackColor = ItemsColor;
			ItemNameInfoView.Bounds = new Rectangle(0, 0, 170, 20);

			ItemTypeInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.TipDock.Left, false);
			ItemTypeInfoView.Text = HackContext.CurrentLanguage["Type"];
			ItemTypeInfoView.Tip.BackColor = ItemsColor;
			ItemTypeInfoView.Bounds = new Rectangle(0, 20, 170, 20);

			ItemRareInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Right }, InfoView.TipDock.Left, false);
			ItemRareInfoView.Text = HackContext.CurrentLanguage["Rare"];
			ItemRareInfoView.Tip.BackColor = ItemsColor;
			ItemRareInfoView.Bounds = new Rectangle(0, 40, 170, 20);

			InfoView ItemDetailInfoView = new InfoView(new Panel() { BorderStyle = BorderStyle.None }, InfoView.TipDock.Top);
			Panel ItemDetailInfoViewContent = (ItemDetailInfoView.View as Panel);
			ItemDetailInfoViewContent.Controls.Add(ItemNameInfoView);
			ItemDetailInfoViewContent.Controls.Add(ItemTypeInfoView);
			ItemDetailInfoViewContent.Controls.Add(ItemRareInfoView);
			ItemDetailInfoView.Text = HackContext.CurrentLanguage["Details"];
			ItemDetailInfoView.Tip.BackColor = ItemsColor;
			ItemDetailInfoView.Bounds = new Rectangle(90, 5, 170, 80);

			ItemDescriptionInfoView = new InfoView(new TextBox() { Multiline = true }, InfoView.TipDock.Left);
			ItemDescriptionInfoView.Text = HackContext.CurrentLanguage["Description"];
			ItemDescriptionInfoView.Tip.BackColor = ItemsColor;
			ItemDescriptionInfoView.Bounds = new Rectangle(5, 90, 255, 80);

			RequireItems = new MTabControl();
			RequireItems.TColor = ItemsColor;
			ItemRecipeFromInfoView = new InfoView(RequireItems, InfoView.TipDock.Top);
			ItemRecipeFromInfoView.Text = HackContext.CurrentLanguage["Recipe"] + "(From)";
			ItemRecipeFromInfoView.Tip.BackColor = ItemsColor;
			ItemRecipeFromInfoView.Bounds = new Rectangle(5, 175, 255, 100);

			RecipeToItems = new ListBox()
			{
				BorderStyle = BorderStyle.None
			};
			RecipeToItems.MouseDoubleClick += (s, e) =>
			{
				OnRecipeToItemDoubleClick(s, e);
			};
			ItemRecipeToInfoView = new InfoView(RecipeToItems, InfoView.TipDock.Top);
			ItemRecipeToInfoView.Text = HackContext.CurrentLanguage["Recipe"] + "(To)";
			ItemRecipeToInfoView.Tip.BackColor = ItemsColor;
			ItemRecipeToInfoView.Bounds = new Rectangle(5, 280, 255, 100);

			ItemValueInfoView = new InfoView(new TextBox() { TextAlign = HorizontalAlignment.Center }, InfoView.TipDock.Left);
			ItemValueInfoView.Text = HackContext.CurrentLanguage["Rare"];
			ItemValueInfoView.Tip.BackColor = ItemsColor;
			ItemValueInfoView.Bounds = new Rectangle(5, 385, 255, 20);



			Controls.Add(ItemIconInfoView);
			Controls.Add(ItemDetailInfoView);
			Controls.Add(ItemDescriptionInfoView);
			Controls.Add(ItemRecipeFromInfoView);
			Controls.Add(ItemRecipeToInfoView);
			Controls.Add(ItemValueInfoView);
		}

		public void ResetData()
		{
			(ItemIconInfoView.View as PictureBox).Image = null;
			(ItemNameInfoView.View as TextBox).Text = "";
			(ItemTypeInfoView.View as TextBox).Text = "";
			(ItemRareInfoView.View as TextBox).Text = "";
			string desc = "";
			(ItemDescriptionInfoView.View as TextBox).Text = desc;
			(ItemRecipeFromInfoView.View as TabControl).TabPages.Clear();
			(ItemRecipeToInfoView.View as ListBox).Items.Clear();

			(ItemValueInfoView.View as TextBox).Text = "";

		}

		public void SetData(int index)
		{
			Image img = GameResLoader.ItemImages.Images[index.ToString()];
			(ItemIconInfoView.View as PictureBox).Image = img;
			(ItemNameInfoView.View as TextBox).Text = ItemData.Data[index].Name.ToString();
			(ItemTypeInfoView.View as TextBox).Text = ItemData.Data[index].Type.ToString();
			(ItemRareInfoView.View as TextBox).Text = ItemData.Data[index].Rare.ToString();
			//string desc = string.Concat(ItemsTabPage.Items[index]["ToolTip"]["_tooltipLines"].ToList().Select(t => t.ToString() + "\n"));
			//(ItemDescriptionInfoView.View as TextBox).Text = desc;
			string desc = ItemsTabPage.ItemDescriptions[index].ToString();
			(ItemDescriptionInfoView.View as TextBox).Text = desc;
			(ItemRecipeFromInfoView.View as TabControl).TabPages.Clear();
			var pRe = RecipeData.Data.Where(t => t.TargetItem.Type == ItemData.Data[index].Type);
			if (pRe.Count() > 0)
			{
				int t = 0;
				foreach (var recipe in pRe)
				{
					t++;
					var ritems = recipe.RequiredItems;
					TabPage page = new TabPage(t.ToString());
					RequireItems.TabPages.Add(page);
					ListBox box = new ListBox()
					{
						BorderStyle = BorderStyle.None
					};
					box.MouseDoubleClick += (s, e) =>
					{
						OnRequireItemDoubleClick(s, e);
					};
					box.Dock = DockStyle.Fill;
					page.Controls.Add(box);
					foreach (var itm in ritems)
					{
						var itemType = itm.Type;
						if (itemType != 0)
							box.Items.Add("[" + itemType + "] " + ItemData.Data[itemType].Name + " [" + itm.Stack + "]");
					}
				}
			}
			(ItemRecipeToInfoView.View as ListBox).Items.Clear();
			pRe = RecipeData.Data.Where(
				t => t.RequiredItems.Where(
					y => index != 0 && y.Type == index).Count() > 0);
			foreach (var p in pRe)
			{
				var itm = p.TargetItem;
				(ItemRecipeToInfoView.View as ListBox).Items.Add("[" + itm.Type + "] " + ItemData.Data[Convert.ToInt32(itm.Type)].Name.ToString() + " [" + itm.Stack + "]");
			}

			(ItemValueInfoView.View as TextBox).Text = ItemsTabPage.GetValueString(Convert.ToInt32(ItemData.Data[index].Value.ToString()));

		}
	}
}
