using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace QTRHacker.Patches.HooksDef
{
	[HarmonyPatch(typeof(Terraria.Main), "DoUpdate")]
	public class DoUpdateHook
	{
		public static event Action Pre;
		public static event Action Post;
		private static void Prefix(ref GameTime gameTime)
		{
			Pre?.Invoke();
		}
		private static void Postfix(ref GameTime gameTime)
		{
			Post?.Invoke();
		}
	}
}
