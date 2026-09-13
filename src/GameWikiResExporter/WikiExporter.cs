using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using Main = Terraria.Main;

namespace GameWikiResExporter;

internal static class WikiExporter
{
	private static readonly DateTimeOffset ArchiveTime = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

	// The assembly resolver must be installed before the JIT resolves Terraria/Newtonsoft types.
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Export(string outputDirectory, string imagesDirectory)
	{
		InitializeGameData();
		Console.WriteLine($"Items: {ItemID.Count}; NPCs: {NPCID.Count}; buffs: {BuffID.Count}; projectiles: {ProjectileID.Count}; recipes: {Recipe.numRecipes}.");
		string staging = Path.Combine(outputDirectory, ".wiki-export-" + Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(staging);
		try
		{
			ExportWiki(Path.Combine(staging, "WikiRes.zip"));
			ExportLocalization(Path.Combine(staging, "Localization.zip"));
			ExportConstants(Path.Combine(staging, "GameConstants.cs"));
			if (imagesDirectory != null)
				ImageExporter.Export(imagesDirectory, staging);
			foreach (string file in Directory.GetFiles(staging).OrderBy(Path.GetFileName, StringComparer.Ordinal))
			{
				string destination = Path.Combine(outputDirectory, Path.GetFileName(file));
				if (File.Exists(destination))
					File.Replace(file, destination, null);
				else
					File.Move(file, destination);
				Console.WriteLine($"Written: {destination}");
			}
		}
		finally
		{
			Directory.Delete(staging, true);
		}
	}

	private static void InitializeGameData()
	{
		Main.dedServ = true;
		Main.netMode = 2;
		Main.rand = new UnifiedRandom(0);
		Main.GameMode = 0;
		LanguageManager.Instance.SetLanguage("en-US");
		Lang.InitializeLegacyLocalization();
		Main.player[Main.myPlayer] = new Player();
		ContentSamples.Initialize();
		// Only initialize data tables: no game loop, world, save files, Steam or network server.
		foreach (string method in new[] { "Initialize_TileAndNPCData1", "Initialize_TileAndNPCData2", "Initialize_Items" })
			typeof(Main).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
		// Item.SetDefaults uses these tables to finalize grappling-hook use times.
		for (int id = 1; id < ProjectileID.Count; id++)
		{
			var projectile = ContentSamples.ProjectilesByType[id];
			Main.projHostile[id] = projectile.hostile;
			Main.projHook[id] = projectile.aiStyle == 7;
		}
		Recipe.SetupRecipeGroups();
		for (int i = 0; i < Recipe.maxRecipes; i++)
			Main.recipe[i] = new Recipe();
		Recipe.SetupRecipes();
		ContentSamples.FixItemsAfterRecipesAreAdded();
	}

	private static void ExportWiki(string path)
	{
		using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
		foreach (Type type in new[] { typeof(BuffID), typeof(ItemID), typeof(NPCID), typeof(ProjectileID) })
			WriteJson(archive, "ID/" + type.Name + ".json", GetIds(type));
		WriteJson(archive, "ItemInfo.json", Enumerable.Range(0, ItemID.Count).Select(id => ItemInfo(ContentSamples.ItemsByType[id])));
		WriteJson(archive, "NPCInfo.json", Enumerable.Range(0, NPCID.Count).Select(id =>
		{
			var npc = new NPC();
			npc.SetDefaults(id);
			return new
			{
				Type = id, Name = npc.FullName, AiStyle = npc.aiStyle, Width = npc.width, Height = npc.height,
				Color = new { npc.color.R, npc.color.G, npc.color.B, npc.color.A }, Value = npc.value,
				TownNPC = npc.townNPC, Friendly = npc.friendly, Boss = npc.boss,
				DefDamage = npc.defDamage, DefDefense = npc.defDefense, LifeMax = npc.lifeMax,
				KnockBackResist = npc.knockBackResist
			};
		}));
		if (Recipe.numRecipes <= 0)
			throw new InvalidDataException("No recipes were initialized.");
		WriteJson(archive, "RecipeInfo.json", Main.recipe.Take(Recipe.numRecipes).Select(recipe => new
		{
			item = new { type = recipe.createItem.type, stack = recipe.createItem.stack },
			rItems = recipe.requiredItem.Where(item => item.type != 0).Select(item => new { type = item.type, stack = item.stack }),
			// Terraria 1.4.5 uses a scalar; QTRHacker's existing schema expects a list.
			rTiles = recipe.requiredTile >= 0 ? new[] { recipe.requiredTile } : Array.Empty<int>()
		}));
	}

	private static Dictionary<string, int> GetIds(Type type)
	{
		return type.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(field => field.FieldType == typeof(int) || field.FieldType == typeof(short) || field.FieldType == typeof(ushort) || field.FieldType == typeof(byte) || field.FieldType == typeof(sbyte))
			.Select(field => new { field.Name, Value = Convert.ToInt32(field.GetValue(null)), field.MetadataToken })
			.OrderBy(field => field.Value).ThenBy(field => field.MetadataToken)
			.ToDictionary(field => field.Name, field => field.Value);
	}

	private static void ExportConstants(string path)
	{
		if (Main.npcFrameCount.Length != NPCID.Count || Main.npcFrameCount.Any(count => count <= 0))
			throw new InvalidDataException("NPC frame counts are incomplete.");
		using var writer = new StreamWriter(path, false, new UTF8Encoding(false));
		writer.WriteLine("// Generated by GameWikiResExporter from the selected Terraria assembly.");
		writer.WriteLine("namespace QTRHacker.Core;");
		writer.WriteLine();
		writer.WriteLine("public static class GameConstants");
		writer.WriteLine("{");
		writer.WriteLine($"\tpublic const string GameVersion = \"{typeof(Main).Assembly.GetName().Version}\";");
		writer.WriteLine($"\tpublic const int MaxItemTypes = {ItemID.Count};");
		writer.WriteLine($"\tpublic const int MaxNPCTypes = {NPCID.Count};");
		writer.WriteLine($"\tpublic const int MaxPlayerBuffs = {Player.maxBuffs};");
		writer.WriteLine($"\tpublic static int[] NPCFrameCount = new int[{Main.npcFrameCount.Length}]");
		writer.WriteLine("\t{");
		for (int i = 0; i < Main.npcFrameCount.Length; i += 10)
		{
			string values = string.Join(", ", Main.npcFrameCount.Skip(i).Take(10));
			writer.WriteLine("\t\t" + values + (i + 10 < Main.npcFrameCount.Length ? "," : ""));
		}
		writer.WriteLine("\t};");
		writer.WriteLine("}");
	}

	private static object ItemInfo(Item item) => new
	{
		Type = item.type, Rare = item.rare, Value = item.value, MaxStack = item.maxStack,
		HeadSlot = item.headSlot, BodySlot = item.bodySlot, LegSlot = item.legSlot,
		Accessory = item.accessory, Melee = item.melee, Ranged = item.ranged, Magic = item.magic,
		Summon = item.summon, Sentry = item.sentry, Consumable = item.consumable,
		Pick = item.pick, Axe = item.axe, Hammer = item.hammer, Damage = item.damage,
		Defense = item.defense, Crit = item.crit, Shoot = item.shoot, KnockBack = item.knockBack,
		ShootSpeed = item.shootSpeed, UseTime = item.useTime, UseAnimation = item.useAnimation,
		HealLife = item.healLife, HealMana = item.healMana, CreateTile = item.createTile,
		CreateWall = item.createWall, PlaceStyle = item.placeStyle, TileBoost = item.tileBoost,
		BuffType = item.buffType, BuffTime = item.buffTime, Mana = item.mana, Bait = item.bait,
		QuestItem = item.questItem
	};

	private static void WriteJson(ZipArchive archive, string name, object data)
	{
		using var stream = CreateEntry(archive, name).Open();
		using var text = new StreamWriter(stream, new UTF8Encoding(false));
		using var writer = new JsonTextWriter(text);
		JsonSerializer.CreateDefault().Serialize(writer, data);
	}

	private static ZipArchiveEntry CreateEntry(ZipArchive archive, string name)
	{
		var entry = archive.CreateEntry(name, CompressionLevel.Optimal);
		entry.LastWriteTime = ArchiveTime;
		return entry;
	}

	private static void ExportLocalization(string path)
	{
		const string prefix = "Terraria.Localization.";
		var assembly = typeof(Main).Assembly;
		using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
		foreach (string culture in new[] { "en-US", "zh-Hans" })
		{
			string[] resources = assembly.GetManifestResourceNames()
				.Where(name => name.StartsWith(prefix + "Content." + culture + ".", StringComparison.Ordinal) && name.EndsWith(".json", StringComparison.Ordinal))
				.OrderBy(name => name, StringComparer.Ordinal).ToArray();
			if (resources.Length == 0)
				throw new InvalidDataException($"Missing embedded localization for {culture}.");
			foreach (string resource in resources)
			{
				using var source = assembly.GetManifestResourceStream(resource);
				using var destination = CreateEntry(archive, resource.Substring(prefix.Length)).Open();
				source.CopyTo(destination);
			}
			Console.WriteLine($"Localization {culture}: {resources.Length} original game JSON files.");
		}
	}
}
