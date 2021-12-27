using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Wiki.Data
{
	public struct RecipeData
	{
		public struct ItemStack
		{
			public int Type;
			public int Stack;
		}

		[JsonProperty("item")]
		public ItemStack TargetItem
		{
			get;
			set;
		}
		[JsonProperty("rItems")]
		public List<ItemStack> RequiredItems
		{
			get;
			set;
		}
		[JsonProperty("rTiles")]
		public List<int> RequiredTiles
		{
			get;
			set;
		}
	}
}
