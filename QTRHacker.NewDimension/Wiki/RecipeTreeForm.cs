using Newtonsoft.Json.Linq;
using QTRHacker.NewDimension.Res;
using QTRHacker.NewDimension.XNAControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeView = QTRHacker.NewDimension.XNAControls.TreeView;

namespace QTRHacker.NewDimension.Wiki
{
	public class RecipeTreeForm : Form
	{
		public static Dictionary<int, List<JToken>> RecipeTos = new Dictionary<int, List<JToken>>();
		public static Dictionary<int, List<JToken>> RecipeFroms = new Dictionary<int, List<JToken>>();

		public TreeView RecipeTreeView;

		private RecipeTreeForm()
		{
			MaximizeBox = false;
			MinimizeBox = false;
			FormBorderStyle = FormBorderStyle.FixedSingle;

			RecipeTreeView = new TreeView();
		}

		public static void ShowTree(int index)
		{
			RecipeTreeForm form = new RecipeTreeForm();
			form.VisitedItem.Clear();
			var node = form.ConstructTree(index);
			node.MoveTo(400, 100);
			form.RecipeTreeView.Roots.Add(node);
			form.RecipeTreeView.ArrangeTree();
			form.Show();
		}

		private static List<JToken> GetRecipeTo(int index)
		{
			if (RecipeTos.ContainsKey(index))
				return RecipeTos[index];
			var result = ItemsTabPage.Recipes.Where(t => t["item"]["type"].ToObject<int>() == index).ToList();
			RecipeTos[index] = result;
			return result;
		}

		private List<ItemTreeNode> VisitedItem = new List<ItemTreeNode>();

		/// <summary>
		/// Currently not support more than 1 recipes
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private ItemTreeNode ConstructTree(int index, int recipeFrom = 1)
		{
			var recipes = GetRecipeTo(index);
			var img = GameResLoader.ItemImages.Images[index.ToString()];
			if (recipes.Count == 0)
			{
				return new ItemTreeNode(RecipeTreeView, img, 1, recipeFrom);//1 for not showing
			}
			var recipe = recipes[0];
			int id = recipe["item"]["type"].ToObject<int>();
			int stack = recipe["item"]["stack"].ToObject<int>();
			ItemTreeNode itn = new ItemTreeNode(RecipeTreeView, img, stack, recipeFrom);
			var rItems = recipe["rItems"] as JArray;
			foreach (var item in rItems)
			{
				if (item["type"].ToObject<int>() == 0)
					continue;
				itn.SubNodes.Add(ConstructTree(item["type"].ToObject<int>(), item["stack"].ToObject<int>()));
			}
			return itn;
		}
	}
}
