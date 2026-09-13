using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using GameWikiResExporter;
using QTRHacker.Core;
using QTRHacker.Patches;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Light;
using GameMain = Terraria.Main;

namespace QTRHacker.Patches.Test;

internal static class Program
{
	static void Main(string[] args)
	{
		using var runtime = new GameRuntime(Path.GetFullPath(args.Length > 0 ? args[0] : "gamerefs/Terraria.exe"));
		runtime.Assembly.GetType("Terraria.Program").GetField("SavePath").SetValue(null, AppDomain.CurrentDomain.BaseDirectory);
		Run();
	}
	[MethodImpl(MethodImplOptions.NoInlining)]
	static void Run()
	{
		Assembly.Load("GameWikiResExporter").GetType("GameWikiResExporter.WikiExporter").GetMethod("InitializeGameData", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
		GameMain.netMode = 0;
		GameMain.gameMenu = false;
		var player = GameMain.LocalPlayer;
		player.whoAmI = GameMain.myPlayer;
		// Inspect the shipped, ILRepack-merged assembly too. Nested array type
		// arguments in Harmony attributes used to break Boot.PatchAll at runtime.
		foreach (var type in typeof(GameplayPatches).Assembly.GetTypes())
		{
			if (!type.GetCustomAttributes(false).Any(a => a.GetType().Name == "HarmonyPatch")) continue;
			var target = type.GetMethod("TargetMethod", BindingFlags.Static | BindingFlags.NonPublic);
			if (target != null) Require(target.Invoke(null, null) is MethodBase, "merged Harmony target: " + type.Name);
		}
		// Exercise the actual mailbox callback, including the first lazy install.
		using (new HighAddressChecks())
			SendRequest(GameplayFeature.MachanicalRuler, true);
		HighAddressChecks.VerifyJump();
		Require(GameplayPatches.EnabledFeatures == (1 << (int)GameplayFeature.MachanicalRuler), "request applied");
		SendRequest(GameplayFeature.MachanicalRuler, false);
		Require(GameplayPatches.EnabledFeatures == 0, "request disabled");
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		Console.WriteLine("PASS: all managed hooks and semantic IL match counts installed");
		player.ResetEffects();
		var baseline = Snapshot(player);
		GameMain.maxTilesX = GameMain.maxTilesY = 80;
		GameMain.tile = new Tile[80, 80];
		for (int x = 0; x < 80; x++) for (int y = 0; y < 80; y++) GameMain.tile[x,y] = new Tile();
		for (int i = 0; i < GameMain.item.Length; i++) GameMain.item[i] = new WorldItem();
		for (int i = 0; i < GameMain.projectile.Length; i++) GameMain.projectile[i] = new Projectile();
		player.position = new Vector2(400, 400);
		Lighting.Mode = LightMode.Color;
		foreach (GameplayFeature feature in Enum.GetValues(typeof(GameplayFeature))) {
			GameplayPatches.SetEnabled((int)feature, true);
			Require(GameplayPatches.LastChangeSucceeded, feature + ": " + GameplayPatches.LastError);
		}
		player.ResetEffects();
		Require(player.slowFall && player.moveSpeed == 20 && player.maxMinions == 9999 && player.maxTurrets == 9999, "player movement and minion limits");
		Require(player.wallSpeed == 10 && player.tileSpeed == 10 && player.rulerGrid && player.InfoAccMechShowWires, "builder effects");
		Require(Player.tileRangeX == 4096 && Player.tileRangeY == 4096, "build range");
		player.statMana = 0;
		player.slowMagicUse = true;
		Require(player.CheckMana(100, true, true) && player.statMana == 0, "mana payment bypass at zero mana");
		Require(!player.slowMagicUse, "stale low-mana casting slowdown cleared");
		var other = new Player { whoAmI = 123 };
		other.ResetEffects();
		Require(!other.slowFall && other.moveSpeed == 1 && other.maxMinions == 1 && !other.rulerGrid, "other player unaffected");
		int life = player.statLife;
		Require(player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByOther(0), 10, 0) == 0 && player.statLife == life, "damage blocked");
		player.wingsLogic = 1; player.wingTimeMax = 100; player.wingTime = 0;
		player.WingMovement();
		Require(player.wingTime == 100, "wing time maintained from empty");
		player.breath = 0; player.CheckDrowning();
		Require(player.breath == player.breathMax, "oxygen refilled");
		var creative = new Terraria.GameContent.Creative.CreativeUI();
		typeof(Terraria.GameContent.Creative.CreativeUI).GetField("_initialized", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(creative, true);
		var menuEnabled = typeof(Terraria.GameContent.Creative.CreativeUI).GetProperty("Enabled");
		menuEnabled.SetValue(creative, true);
		player.difficulty = 0; typeof(Player).GetProperty("talkNPC").SetValue(player, 0);
		creative.Draw(null);
		Require(creative.Enabled && player.difficulty == 0, "journey guard bypass does not change character difficulty");
		typeof(Player).GetProperty("talkNPC").SetValue(player, -1);
		var portal = new Projectile(); portal.SetDefaults(518); portal.position = player.position; portal.ai[0] = 61; portal.ai[1] = 1;
		portal.AI();
		Require(GameMain.item.Any(item => item.active && item.type == Terraria.ID.ItemID.MoonLordBossBag), "coin portal actually spawns boss bag");
		ShootKnives(player);
		Require(GameMain.projectile.Count(p => p.active && p.type == Terraria.ID.ProjectileID.VampireKnife) >= 100, "vampire knives actually spawn 100+ projectiles");
		var light = new LightMap();
		light.SetSize(10, 10);
		light.NonVisiblePadding = 0;
		light.Blur();
		Require(light[4, 4] == Vector3.One, "full brightness");
		var bobber = new Projectile { owner = player.whoAmI };
		var fishing = typeof(Projectile).GetMethod("FishingCheck_RollDropLevels", BindingFlags.Instance | BindingFlags.NonPublic);
		for (int i = 0; i < 30; i++) {
			object[] values = { 50, false, false, false, false, false, false };
			fishing.Invoke(bobber, values);
			Require((bool)values[6], "crate roll");
		}
		foreach (GameplayFeature feature in Enum.GetValues(typeof(GameplayFeature))) GameplayPatches.SetEnabled((int)feature, false);
		player.ResetEffects();
		Require(Snapshot(player) == baseline, "all reset effects restored to vanilla");
		player.WingMovement();
		Require(player.wingTime < 100, "wing consumption restored");
		typeof(Player).GetProperty("talkNPC").SetValue(player, 0); menuEnabled.SetValue(creative, true); creative.Draw(null);
		Require(!creative.Enabled && player.difficulty == 0, "journey guard restored");
		typeof(Player).GetProperty("talkNPC").SetValue(player, -1);
		portal.ai[0] = 61; portal.AI();
		Require(GameMain.item.Any(item => item.active && item.type == Terraria.ID.ItemID.GoldCoin), "coin drop restored");
		foreach (var p in GameMain.projectile) p.active = false;
		ShootKnives(player);
		int knifeCount = GameMain.projectile.Count(p => p.active && p.type == Terraria.ID.ProjectileID.VampireKnife);
		Require(knifeCount >= 4 && knifeCount <= 8, "normal vampire knife count restored");
		player.statMana = 200;
		Require(player.CheckMana(50, true, true) && player.statMana == 150, "mana payment restored");
		light.Clear(); light.Blur();
		Require(light[4, 4] == Vector3.Zero, "normal lighting restored");
		bool normalRoll = false;
		for (int i = 0; i < 30; i++) { object[] values = { 50, false, false, false, false, false, false }; fishing.Invoke(bobber, values); normalRoll |= !(bool)values[6]; }
		Require(normalRoll, "normal fishing rolls restored");
		GameplayPatches.SetEnabled(99, true);
		Require(!GameplayPatches.LastChangeSucceeded && GameplayPatches.EnabledFeatures == 0, "invalid feature reports failure without changing flags");
		Console.WriteLine("PASS: enable/disable behavior, player isolation, restoration and invalid input");
	}
	static string Snapshot(Player p) => string.Join(",", p.slowFall,p.moveSpeed,p.maxMinions,p.maxTurrets,p.manaCost,p.wallSpeed,p.tileSpeed,p.rulerGrid,p.InfoAccMechShowWires,Player.tileRangeX,Player.tileRangeY);
	static void SendRequest(GameplayFeature feature, bool enabled)
	{
		GameplayPatches.RequestedFeature = (int)feature;
		GameplayPatches.RequestedEnabled = enabled;
		int request = ++GameplayPatches.RequestId;
		var callback = (Action)typeof(QTRHacker.Patches.HooksDef.DoUpdateHook).GetField("Pre", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		callback();
		Require(GameplayPatches.CompletedRequestId == request && GameplayPatches.LastChangeSucceeded, "managed request completion: " + GameplayPatches.LastError);
	}
	static void ShootKnives(Player player) {
		var item = new Item(); item.SetDefaults(Terraria.ID.ItemID.VampireKnives);
		player.inventory[0] = item;
		GameMain.mouseX = 600; GameMain.mouseY = 400;
		typeof(Player).GetMethod("ItemCheck_Shoot", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(player, new object[]{ player.whoAmI, item, 10, false });
	}
	static void Require(bool condition, string label) { if (!condition) throw new Exception(label); }
}
