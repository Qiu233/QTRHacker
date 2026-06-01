using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
		public static event Action OnGameUpdate;
		public static event Action<SpriteBatch> OnGameDraw;
		private static bool GameLayerInstalled = false;
		private static System.Windows.Forms.Timer UpdateTimer = null;
		static Boot()
		{
			if (Initialized)
				return;
			try
			{
				RuntimeHelpers.RunClassConstructor(typeof(PatchState).TypeHandle);
				InitializePatchTypes();

				StartUpdateTimer();
				EnsureGameInterfaceLayer();
				Initialized = true;
			}
			catch (Exception e)
			{
				File.WriteAllText("./QTRHacker.Patches.boot.log", $"{e.GetType()}:{e.Message}\n{e.StackTrace}\n");
			}
		}
		private static void InitializePatchTypes()
		{
			RuntimeHelpers.RunClassConstructor(typeof(AimBot).TypeHandle);
			RuntimeHelpers.RunClassConstructor(typeof(AutoFishing).TypeHandle);
			RuntimeHelpers.RunClassConstructor(typeof(WorldPainter).TypeHandle);
		}
		private static void StartUpdateTimer()
		{
			if (UpdateTimer != null)
				return;

			UpdateTimer = new System.Windows.Forms.Timer
			{
				Interval = 15
			};
			UpdateTimer.Tick += delegate
			{
				try
				{
					EnsureGameInterfaceLayer();
					RunPatchUpdate();
				}
				catch (Exception e)
				{
					File.AppendAllText("./QTRHacker.Patches.Exceptions.log", $"{e.GetType()}:{e.Message}\n{e.StackTrace}\n");
				}
			};
			UpdateTimer.Start();
		}

		private static void EnsureGameInterfaceLayer()
		{
			if (GameLayerInstalled)
				return;

			try
			{
				if (Main.instance == null)
					return;

				FieldInfo field = typeof(Main).GetField("_gameInterfaceLayers", BindingFlags.NonPublic | BindingFlags.Instance);
				if (field == null)
					return;

				List<GameInterfaceLayer> layers = field.GetValue(Main.instance) as List<GameInterfaceLayer>;
				if (layers == null)
					return;

				layers.RemoveAll(t => t.Name == "QTRHacker: Game");
				int index = layers.FindIndex(t => t.Name == "Vanilla: Mouse Text");
				if (index < 0)
					index = layers.Count;
				layers.Insert(index, new LegacyGameInterfaceLayer("QTRHacker: Game", delegate
				{
					try
					{
						RunPatchUpdate();
						OnGameDraw?.Invoke(Main.spriteBatch);
					}
					catch (Exception e)
					{
						File.AppendAllText("./QTRHacker.Patches.Exceptions.log", $"{e.GetType()}:{e.Message}\n{e.StackTrace}\n");
					}
					return true;
				}, InterfaceScaleType.Game));
				GameLayerInstalled = true;
			}
			catch (Exception e)
			{
				File.AppendAllText("./QTRHacker.Patches.boot.log", $"{e.GetType()}:{e.Message}\n{e.StackTrace}\n");
			}
		}

		private static void RunPatchUpdate()
		{
			OnGameUpdate?.Invoke();
			PlayerToggles.Apply();
			RuntimeActions.ApplyQueuedActions();
		}
	}
}
