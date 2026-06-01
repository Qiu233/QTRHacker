using Terraria;

namespace QTRHacker.Patches
{
	internal static class GameFocusHelper
	{
		public static bool HasFocus => Main.instance?.IsActive == true;
	}
}
