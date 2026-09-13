using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Terraria;

namespace QTRHacker.Patches
{
	[HarmonyPatch(typeof(Player), nameof(Player.PickAmmo))]
	public static class AmmoConsumption
	{
		public static bool Enabled;

		private static bool ShouldConsume(bool consumable, Player player)
		{
			return consumable && !(Enabled && player.whoAmI == Main.myPlayer);
		}

		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var consumable = AccessTools.Field(typeof(Item), nameof(Item.consumable));
			var shouldConsume = AccessTools.Method(typeof(AmmoConsumption), nameof(ShouldConsume));
			int matches = 0;
			foreach (var instruction in instructions)
			{
				yield return instruction;
				if (instruction.LoadsField(consumable))
				{
					// Keep PickAmmo's new ammo-cycling logic; dontConsume would skip it.
					yield return new CodeInstruction(OpCodes.Ldarg_0);
					yield return new CodeInstruction(OpCodes.Call, shouldConsume);
					matches++;
				}
			}
			if (matches != 1)
				throw new InvalidOperationException("Unexpected Player.PickAmmo consumption logic.");
		}
	}
}
