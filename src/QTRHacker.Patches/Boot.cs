using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace QTRHacker.Patches
{
	public class Boot
	{
		public static readonly string Version = "1.1.0.0";
		public static bool Initialized = false;
		public static event Action<SpriteBatch> OnGameDraw;
		static Boot()
		{
			if (Initialized)
				return;
			try
			{
				LoadAll();
				Initialized = true;

				HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("QTRHacker.Patches");
				harmony.PatchAll();

				List<GameInterfaceLayer> layers =
					typeof(Main).GetField("_gameInterfaceLayers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Main.instance) as List<GameInterfaceLayer>;
				int index = layers.FindIndex(t => t.Name == "Vanilla: Mouse Text");
				layers.Insert(index, new LegacyGameInterfaceLayer("QTRHacker: Game", delegate
				{
					try
					{
						OnGameDraw?.Invoke(Main.spriteBatch);
					}
					catch (Exception e)
					{
						File.AppendAllText("./QTRHacker.Patches.Exceptions.log", $"{e.GetType()}:{e.Message}\n{e.StackTrace}\n");
					}
					return true;
				}, InterfaceScaleType.Game));
			}
			catch (Exception e)
			{
				File.WriteAllText("./QTRHacker.Patches.boot.log", $"{e.GetType()}:{e.Message}\n{e.StackTrace}\n");
			}
		}
		static void LoadAll()
		{
			var asm = System.Reflection.Assembly.GetExecutingAssembly();
			foreach (var type in asm.DefinedTypes)
				System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
		}
	}
}
