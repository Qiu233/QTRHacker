using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Wiki.Data
{
	public class RecipeData
	{
		public struct ItemStack
		{
			public int Type;
			public int Stack;
		}
		public ItemStack TargetItem;
		public List<ItemStack> RequiredItems
		{
			get;
			private set;
		}
		public List<int> RequiredTiles
		{
			get;
			private set;
		}

		private RecipeData()
		{
			RequiredItems = new List<ItemStack>();
			RequiredTiles = new List<int>();
		}

		public static bool Initialized
		{
			get;
			private set;
		}
		public static List<RecipeData> Data
		{
			get;
			private set;
		}

		public static void InitializeFromJson(JArray RecipeInfo)
		{
			if (Initialized)
				return;
			Data = new List<RecipeData>();
			foreach (var recipe in RecipeInfo)
			{
				RecipeData rcp_data = new();
				Data.Add(rcp_data);
				var jtgt = recipe["item"];
				var jritms = recipe["rItems"];
				var jrtls = recipe["rTiles"];
				rcp_data.TargetItem.Type = jtgt["type"].Value<int>();
				rcp_data.TargetItem.Stack = jtgt["stack"].Value<int>();
				for (int i = 0; i < 15; i++)
				{
					var ritm = jritms[i];
					var rtl = jrtls[i];
					rcp_data.RequiredItems.Add(new ItemStack() { Type = ritm["type"].Value<int>(), Stack = ritm["stack"].Value<int>() });
					rcp_data.RequiredTiles.Add(rtl.Value<int>());
				}
			}
			Initialized = true;
		}
	}
}
