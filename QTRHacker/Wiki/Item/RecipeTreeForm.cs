using Newtonsoft.Json.Linq;
using QTRHacker.Res;
using QTRHacker.Wiki.Data;
using QTRHacker.XNAControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeView = QTRHacker.XNAControls.TreeView;

namespace QTRHacker.Wiki.Item
{
	public class RecipeTreeForm : Form
	{
		public readonly static Dictionary<int, List<RecipeData>> RecipeTos = new Dictionary<int, List<RecipeData>>();
		public readonly static Dictionary<int, List<RecipeData>> RecipeFroms = new Dictionary<int, List<RecipeData>>();

		public TreeView RecipeTreeView;

		private RecipeTreeForm()
		{
			MaximizeBox = false;
			MinimizeBox = false;
			Text = HackContext.CurrentLanguage["ClickToItem"];
			FormBorderStyle = FormBorderStyle.FixedSingle;
			ClientSize = new System.Drawing.Size(600, 400);

			RecipeTreeView = new TreeView();
			RecipeTreeView.TAnchor = TreeView.TreeAnchor.Right;
			RecipeTreeView.Dock = DockStyle.Fill;
			Controls.Add(RecipeTreeView);
		}

		public static void ShowTree(int index)
		{
			RecipeTreeForm form = new RecipeTreeForm();
			form.ConstructTree(index);
			form.Show();
		}

		private static List<RecipeData> GetRecipeTo(int index)
		{
			if (RecipeTos.TryGetValue(index, out var value))
				return value;
			return RecipeTos[index] = ItemsTabPage.RecipeDatum.Where(t => t.TargetItem.Type == index).ToList();
		}
		private static List<RecipeData> GetRecipeFrom(int index)
		{
			if (RecipeFroms.TryGetValue(index, out var value))
				return value;
			return RecipeFroms[index] = ItemsTabPage.RecipeDatum.Where(
				t => t.RequiredItems.Where(
					y => index != 0 && y.Type == index).Any()).ToList();
		}


		private void ConstructTree(int index)
		{
			var img = GameResLoader.ItemImages.Images[index.ToString()];
			RecipeTreeView.Root = new ItemTreeNode(RecipeTreeView, img, 1, index);
			RecipeTreeView.Root.Location = new Microsoft.Xna.Framework.Vector2(100, 50);
			Random rand = new Random();
			var rTo = GetRecipeTo(index);
			var rFrom = GetRecipeFrom(index);
			foreach (var t in rTo)
			{
				var color = new Microsoft.Xna.Framework.Color(rand.Next() % 160 + 40, rand.Next() % 160 + 40, rand.Next() % 160 + 40);
				foreach (var ritem in t.RequiredItems)
				{
					int type = ritem.Type;
					if (type <= 0)
						continue;
					var node = new ItemTreeNode(RecipeTreeView,
						GameResLoader.ItemImages.Images[type.ToString()],
						ritem.Stack,
						type,
						color);
					node.OnClick += Node_OnClick;
					//node.Initialize();
					RecipeTreeView.NodesFrom.Add(node);
				}
			}
			foreach (var t in rFrom)
			{
				var item = t.TargetItem;
				int type = item.Type;
				var color = new Microsoft.Xna.Framework.Color(rand.Next() % 160 + 40, rand.Next() % 160 + 40, rand.Next() % 160 + 40);
				var node = new ItemTreeNode(RecipeTreeView,
						GameResLoader.ItemImages.Images[type.ToString()],
						item.Stack,
						type,
						color);
				node.OnClick += Node_OnClick;
				//node.Initialize();
				RecipeTreeView.NodesTo.Add(node);
			}

			RecipeTreeView.ArrangeTree();
		}

		private void Node_OnClick(object s, EventArgs e)
		{
			RecipeTreeView.NodesFrom.ForEach(t => t.Dispose());
			RecipeTreeView.NodesFrom.Clear();
			RecipeTreeView.NodesTo.ForEach(t => t.Dispose());
			RecipeTreeView.NodesTo.Clear(); ;
			ConstructTree((s as ItemTreeNode).Type);
			RecipeTreeView.OriginToWorld = new Microsoft.Xna.Framework.Point(0, 0);
		}
	}
}
