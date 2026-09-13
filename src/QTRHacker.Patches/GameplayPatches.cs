using HarmonyLib;
using Microsoft.Xna.Framework;
using QTRHacker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Graphics.Light;
using Terraria.ID;

namespace QTRHacker.Patches
{
	public static class GameplayPatches
	{
		public static volatile int EnabledFeatures;
		public static bool LastChangeSucceeded;
		public static string LastError = "";
		public static int RequestedFeature;
		public static bool RequestedEnabled;
		public static volatile int RequestId;
		public static volatile int CompletedRequestId;
		private static bool installed;

		static GameplayPatches()
		{
			// Run patch installation and feature changes in a managed game frame.
			HooksDef.DoUpdateHook.Pre += ProcessRequest;
		}

		private static void ProcessRequest()
		{
			int request = RequestId;
			if (request == CompletedRequestId) return;
			SetEnabled(RequestedFeature, RequestedEnabled);
			CompletedRequestId = request;
		}

		public static bool IsEnabled(GameplayFeature feature)
		{
			return (EnabledFeatures & (1 << (int)feature)) != 0;
		}

		private static bool ForPlayer(Player player, GameplayFeature feature)
		{
			return IsEnabled(feature) && player.whoAmI == Main.myPlayer;
		}

		// Called by the managed update callback. Report failures to the controller
		// without letting patch installation exceptions escape into the game.
		public static void SetEnabled(int feature, bool enabled)
		{
			LastChangeSucceeded = false;
			try
			{
				if (!Enum.IsDefined(typeof(GameplayFeature), feature))
					throw new ArgumentOutOfRangeException(nameof(feature));
				if (enabled)
					Install();
				int bit = 1 << feature;
				EnabledFeatures = enabled ? EnabledFeatures | bit : EnabledFeatures & ~bit;
				LastError = "";
				LastChangeSucceeded = true;
			}
			catch (Exception ex)
			{
				LastError = ex.ToString();
			}
		}

		// No HarmonyPatch attributes: Boot.PatchAll must not install these until
		// requested. Install as one transaction, with semantic IL match checks.
		public static void Install()
		{
			if (installed)
				return;
			var harmony = new Harmony("QTRHacker.GameplayPatches");
			try
			{
				Patch(harmony, typeof(Player), "ResetEffects", postfix: nameof(ResetEffects));
				Patch(harmony, typeof(Player), "Hurt", prefix: nameof(Hurt));
				Patch(harmony, typeof(Player), "CheckMana", prefix: nameof(CheckMana));
				Patch(harmony, typeof(Player), "CheckDrowning", prefix: nameof(CheckDrowning));
				Patch(harmony, typeof(Player), "WingMovement", prefix: nameof(WingMovement), postfix: nameof(WingMovement));
				Patch(harmony, typeof(LightMap), "Blur", postfix: nameof(FullBright));
				Patch(harmony, typeof(CreativeUI), "Draw", transpiler: nameof(JourneyDifficulty));
				Patch(harmony, typeof(Player), "ToggleCreativeMenu", transpiler: nameof(JourneyDifficulty));
				Patch(harmony, typeof(Projectile), "FishingCheck_RollDropLevels", postfix: nameof(FishingDropLevels));
				Patch(harmony, typeof(Projectile), "AI", transpiler: nameof(CoinPortal));
				Patch(harmony, typeof(Player), "ItemCheck_Shoot", transpiler: nameof(VampireKnives));
				installed = true;
			}
			catch
			{
				harmony.UnpatchAll(harmony.Id);
				throw;
			}
		}

		private static void Patch(Harmony harmony, Type type, string name,
			string prefix = null, string postfix = null, string transpiler = null)
		{
			var method = AccessTools.Method(type, name);
			if (method == null)
				throw new MissingMethodException(type.FullName, name);
			harmony.Patch(method, Hook(prefix), Hook(postfix), Hook(transpiler));
		}

		private static HarmonyMethod Hook(string name)
		{
			return name == null ? null : new HarmonyMethod(typeof(GameplayPatches), name);
		}

		private static void ResetEffects(Player __instance)
		{
			if (__instance.whoAmI != Main.myPlayer)
				return;
			if (IsEnabled(GameplayFeature.SlowFall)) __instance.slowFall = true;
			if (IsEnabled(GameplayFeature.FastSpeed)) __instance.moveSpeed = 20f;
			if (IsEnabled(GameplayFeature.InfiniteMana)) __instance.manaCost = 0f;
			if (IsEnabled(GameplayFeature.InfiniteMinion))
			{
				__instance.maxMinions = 9999;
				__instance.maxTurrets = 9999;
			}
			if (IsEnabled(GameplayFeature.FastTileAndWallPlacingSpeed))
			{
				__instance.wallSpeed = 10f;
				__instance.tileSpeed = 10f;
			}
			if (IsEnabled(GameplayFeature.MachanicalRuler)) __instance.rulerGrid = true;
			if (IsEnabled(GameplayFeature.MachanicalLens)) __instance.InfoAccMechShowWires = true;
			if (IsEnabled(GameplayFeature.SuperRange))
			{
				Player.tileRangeX = 4096;
				Player.tileRangeY = 4096;
			}
		}

		private static bool Hurt(Player __instance, ref double __result)
		{
			if (!ForPlayer(__instance, GameplayFeature.InfiniteLife)) return true;
			__result = 0;
			return false;
		}

		private static bool CheckMana(Player __instance, ref bool __result)
		{
			if (!ForPlayer(__instance, GameplayFeature.InfiniteMana)) return true;
			__instance.slowMagicUse = false;
			__result = true;
			return false;
		}

		private static void CheckDrowning(Player __instance)
		{
			if (!ForPlayer(__instance, GameplayFeature.InfiniteOxygen)) return;
			__instance.breath = __instance.breathMax;
			__instance.breathCD = 0;
		}

		private static void WingMovement(Player __instance)
		{
			if (ForPlayer(__instance, GameplayFeature.InfiniteFlyTime))
				__instance.wingTime = __instance.wingTimeMax;
		}

		private static void FullBright(LightMap __instance, Vector3[] ____colors)
		{
			if (!IsEnabled(GameplayFeature.HighLight)) return;
			int length = __instance.Width * __instance.Height;
			for (int i = 0; i < length; i++)
				____colors[i] = Vector3.Max(____colors[i], Vector3.One);
		}

		private static byte MenuDifficulty(byte difficulty)
		{
			return IsEnabled(GameplayFeature.CreativeMenu) ? (byte)3 : difficulty;
		}

		private static IEnumerable<CodeInstruction> JourneyDifficulty(IEnumerable<CodeInstruction> instructions)
		{
			var field = AccessTools.Field(typeof(Player), nameof(Player.difficulty));
			int matches = 0;
			foreach (var instruction in instructions)
			{
				yield return instruction;
				if (instruction.LoadsField(field))
				{
					yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GameplayPatches), nameof(MenuDifficulty)));
					matches++;
				}
			}
			RequireMatches(matches, 1, "creative menu difficulty");
		}

		private static void FishingDropLevels(Projectile __instance, ref bool crate)
		{
			if (IsEnabled(GameplayFeature.FishCratesOnly) && __instance.owner == Main.myPlayer)
				crate = true;
		}

		private static int CoinDropType(int original)
		{
			return IsEnabled(GameplayFeature.CoinPortalDropsBags) ? ItemID.MoonLordBossBag : original;
		}

		private static IEnumerable<CodeInstruction> CoinPortal(IEnumerable<CodeInstruction> instructions)
		{
			var code = instructions.ToList();
			int matches = 0;
			for (int i = 0; i < code.Count; i++)
			{
				yield return code[i];
				// Identify the item-type argument of Item.NewItem, not an unrelated
				// constant or a particular x86 stack slot. The portal is its sole
				// GoldCoin call in Projectile.AI in 1.4.5.8.
				if (!code[i].LoadsConstant(ItemID.GoldCoin)) continue;
				var call = code.Skip(i + 1).Take(24).FirstOrDefault(c => c.opcode == OpCodes.Call || c.opcode == OpCodes.Callvirt);
				if (!(call?.operand is MethodInfo method) || method.DeclaringType != typeof(Item) || method.Name != "NewItem") continue;
				yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GameplayPatches), nameof(CoinDropType)));
				matches++;
			}
			RequireMatches(matches, 1, "coin portal item drop");
		}

		private static int KnifeCount(int original, Player player)
		{
			return ForPlayer(player, GameplayFeature.StrengthenVampireKnives) ? 100 : original;
		}

		private static IEnumerable<CodeInstruction> VampireKnives(IEnumerable<CodeInstruction> instructions)
		{
			var code = instructions.ToList();
			int matches = 0;
			for (int i = 0; i < code.Count; i++)
			{
				yield return code[i];
				if (i < 3 || !code[i].LoadsConstant(4)
					|| !code[i - 2].LoadsConstant(ItemID.VampireKnives)
					|| !code[i - 3].LoadsField(AccessTools.Field(typeof(Item), nameof(Item.type)))
					|| (code[i - 1].opcode != OpCodes.Bne_Un && code[i - 1].opcode != OpCodes.Bne_Un_S)) continue;
				yield return new CodeInstruction(OpCodes.Ldarg_0);
				yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GameplayPatches), nameof(KnifeCount)));
				matches++;
			}
			RequireMatches(matches, 1, "vampire knife count");
		}

		private static void RequireMatches(int actual, int expected, string operation)
		{
			if (actual != expected)
				throw new InvalidOperationException($"Unexpected {operation} IL: expected {expected} match, found {actual}.");
		}
	}
}
