using HarmonyLib;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI;

namespace QTRHacker.Patches
{
	public static class RecipeUnlocks
	{
		public static bool Enabled;

		// Called on the game thread so both crafting interfaces refresh together.
		public static void SetEnabled(bool enabled)
		{
			Enabled = enabled;
			if (!Main.gameMenu)
			{
				Recipe.UpdateRecipeList();
				NewCraftingUI.RefreshGrid();
			}
		}

		[HarmonyPatch(typeof(Recipe), nameof(Recipe.PlayerMeetsEnvironmentConditions))]
		private static class EnvironmentConditions
		{
			private static bool Prefix(ref bool __result)
			{
				if (!Enabled)
					return true;
				__result = true;
				return false;
			}
		}

		[HarmonyPatch]
		private static class CraftableCount
		{
			// Keep the nested array type in IL: ILRepack 2.0.18 corrupts its
			// serialized type name when it is stored in a custom attribute.
			private static System.Reflection.MethodBase TargetMethod()
			{
				return AccessTools.Method(typeof(Recipe), nameof(Recipe.HowManyTimesCanRecipeBeCrafted), new[] { typeof(Recipe.RequiredItemEntry[]) });
			}

			private static bool Prefix(ref int __result)
			{
				if (!Enabled)
					return true;
				__result = Item.CommonMaxStack;
				return false;
			}
		}

		[HarmonyPatch(typeof(Recipe), nameof(Recipe.GetIngredientsForOneCraft))]
		private static class Ingredients
		{
			private static bool Prefix(List<Recipe.RequiredItemEntry> ingredients)
			{
				if (!Enabled)
					return true;
				// 1.4.5 also checks/consumes materials in CraftingRequests. An empty
				// list keeps unlocked crafts local and avoids requests for missing items.
				ingredients.Clear();
				return false;
			}
		}
	}
}
