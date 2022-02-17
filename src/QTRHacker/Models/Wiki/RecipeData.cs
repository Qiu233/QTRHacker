using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Models.Wiki
{
	public class RecipeData
	{
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
