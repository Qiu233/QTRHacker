using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Map;

namespace QTRHacker.Patches
{
	public static class RuntimeActions
	{
		public static bool UnlockAllDuplicationsRequested;
		public static bool RevealTheWholeMapRequested;
		public static bool ToggleLanternNightRequested;
		public static int UnlockAllDuplicationsCompleted;
		public static int RevealTheWholeMapCompleted;
		public static int ToggleLanternNightCompleted;
		public static int LastErrorCode;

		public static void ApplyQueuedActions()
		{
			ReadSharedState();
			if (UnlockAllDuplicationsRequested)
			{
				UnlockAllDuplicationsRequested = false;
				unsafe { PatchState.Shared->UnlockAllDuplicationsRequested = 0; }
				Run(UnlockAllDuplications, ref UnlockAllDuplicationsCompleted);
			}
			if (RevealTheWholeMapRequested)
			{
				RevealTheWholeMapRequested = false;
				unsafe { PatchState.Shared->RevealTheWholeMapRequested = 0; }
				Run(RevealTheWholeMap, ref RevealTheWholeMapCompleted);
			}
			if (ToggleLanternNightRequested)
			{
				ToggleLanternNightRequested = false;
				unsafe { PatchState.Shared->ToggleLanternNightRequested = 0; }
				Run(ToggleLanternNight, ref ToggleLanternNightCompleted);
			}
		}

		private static unsafe void ReadSharedState()
		{
			PatchState.State* state = PatchState.Shared;
			UnlockAllDuplicationsRequested = PatchState.GetBool(state->UnlockAllDuplicationsRequested);
			RevealTheWholeMapRequested = PatchState.GetBool(state->RevealTheWholeMapRequested);
			ToggleLanternNightRequested = PatchState.GetBool(state->ToggleLanternNightRequested);
			UnlockAllDuplicationsCompleted = state->UnlockAllDuplicationsCompleted;
			RevealTheWholeMapCompleted = state->RevealTheWholeMapCompleted;
			ToggleLanternNightCompleted = state->ToggleLanternNightCompleted;
			LastErrorCode = state->LastErrorCode;
		}

		private static void UnlockAllDuplications()
		{
			if (Main.gameMenu || Main.LocalPlayer?.creativeTracker?.ItemSacrifices == null)
				return;

			var sacrifices = Main.LocalPlayer.creativeTracker.ItemSacrifices;
			for (int itemId = 0; itemId < ItemID.Count; itemId++)
				sacrifices.RegisterItemSacrifice(itemId, 9999);
		}

		private static void RevealTheWholeMap()
		{
			if (Main.gameMenu || Main.Map == null)
				return;

			int margin = WorldMap.BlackEdgeWidth;
			int maxX = Math.Max(margin, Main.maxTilesX - margin);
			int maxY = Math.Max(margin, Main.maxTilesY - margin);
			for (int x = margin; x < maxX; x++)
			{
				for (int y = margin; y < maxY; y++)
					Main.Map.UpdateLighting(x, y, byte.MaxValue);
			}
			Main.refreshMap = true;
		}

		private static void ToggleLanternNight()
		{
			if (Main.gameMenu)
				return;
			LanternNight.ToggleManualLanterns();
			LanternNight.UpdateTime();
		}

		private static void Run(Action action, ref int completed)
		{
			try
			{
				action();
				completed++;
				LastErrorCode = 0;
				WriteSharedCounters();
			}
			catch
			{
				LastErrorCode = 1;
				WriteSharedCounters();
			}
		}

		private static unsafe void WriteSharedCounters()
		{
			PatchState.Shared->UnlockAllDuplicationsCompleted = UnlockAllDuplicationsCompleted;
			PatchState.Shared->RevealTheWholeMapCompleted = RevealTheWholeMapCompleted;
			PatchState.Shared->ToggleLanternNightCompleted = ToggleLanternNightCompleted;
			PatchState.Shared->LastErrorCode = LastErrorCode;
		}
	}
}
