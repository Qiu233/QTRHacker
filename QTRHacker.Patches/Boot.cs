using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace QTRHacker.Patches
{
	public class Boot
	{
		public static readonly string Version = "1.0.0.0";
		public static bool Initialized = false;
		public static event Action<SpriteBatch> OnGameDraw;
		static Boot()
		{
			if (Initialized)
				return;
			Initialized = true;
			LoadAll();

			HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("QTRHacker.Patches");
			harmony.PatchAll();

			List<GameInterfaceLayer> layers =
				typeof(Main).GetField("_gameInterfaceLayers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Main.instance) as List<GameInterfaceLayer>;
			int index = layers.FindIndex(t => t.Name == "Vanilla: Mouse Text");
			layers.Insert(index, new LegacyGameInterfaceLayer("QTRHacker: Game", delegate
			{
				OnGameDraw?.Invoke(Main.spriteBatch);
				return true;
			}, InterfaceScaleType.Game));
		}
		static void LoadAll()
		{
			var asm = System.Reflection.Assembly.GetExecutingAssembly();
			foreach (var type in asm.DefinedTypes)
				foreach (var method in type.DeclaredMethods)
					System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
		}
	}
}
