//using Newtonsoft.Json;
//using QTRHacker.Models.Wiki;
//using System.IO;
//using System.IO.Compression;
//using System.Windows;

//namespace QTRHacker.Assets;

//public static class WikiResLoader
//{
//	public static readonly Dictionary<int, string> ItemKeys = new();
//	public static readonly Dictionary<string, int> ItemTypes = new();
//	public static readonly Dictionary<int, string> NPCKeys = new();
//	public static readonly Dictionary<string, int> NPCTypes = new();
//	public static readonly Dictionary<int, string> BuffKeys = new();
//	public static readonly Dictionary<string, int> BuffTypes = new();
//	public static readonly List<ItemData> ItemDatum = new();
//	public static readonly List<NPCData> NPCDatum = new();
//	public static readonly List<RecipeData> RecipeDatum = new();
//	static WikiResLoader()
//	{
//		using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/Assets/Game/WikiRes.zip", UriKind.Absolute)).Stream;
//		using ZipArchive z = new(s);
//		LoadItemDatum(z);
//		LoadNPCDatum(z);
//		LoadBuffDatum(z);
//		GC.Collect();
//	}

//	private static void LoadBuffDatum(ZipArchive z)
//	{
//		using var u = new StreamReader(z.GetEntry("ID/BuffID.json").Open());
//		BuffTypes.Clear();
//		BuffKeys.Clear();
//		JsonConvert.DeserializeObject<Dictionary<string, int>>(u.ReadToEnd()).ToList().ForEach(t =>
//		{
//			BuffTypes[t.Key] = t.Value;
//			BuffKeys[t.Value] = t.Key;
//		});
//	}
//	private static void LoadNPCDatum(ZipArchive z)
//	{
//		using (var u = new StreamReader(z.GetEntry("ID/NPCID.json").Open()))
//		{
//			NPCTypes.Clear();
//			NPCKeys.Clear();
//			JsonConvert.DeserializeObject<Dictionary<string, int>>(u.ReadToEnd()).ToList().ForEach(t =>
//			{
//				NPCTypes[t.Key] = t.Value;
//				NPCKeys[t.Value] = t.Key;
//			});
//		}
//		NPCDatum.Clear();
//		using (var u = new StreamReader(z.GetEntry("NPCInfo.json").Open()))
//			NPCDatum.AddRange(JsonConvert.DeserializeObject<List<NPCData>>(u.ReadToEnd()));
//	}

//	private static void LoadItemDatum(ZipArchive z)
//	{
//		using (var u = new StreamReader(z.GetEntry("ID/ItemID.json").Open()))
//		{
//			ItemTypes.Clear();
//			ItemKeys.Clear();
//			JsonConvert.DeserializeObject<Dictionary<string, int>>(u.ReadToEnd()).ToList().ForEach(t =>
//			{
//				ItemTypes[t.Key] = t.Value;
//				ItemKeys[t.Value] = t.Key;
//			});
//		}
//		ItemDatum.Clear();
//		RecipeDatum.Clear();
//		using (var u = new StreamReader(z.GetEntry("ItemInfo.json").Open()))
//			ItemDatum.AddRange(JsonConvert.DeserializeObject<List<ItemData>>(u.ReadToEnd()));
//		using (var u = new StreamReader(z.GetEntry("RecipeInfo.json").Open()))
//			RecipeDatum.AddRange(JsonConvert.DeserializeObject<List<RecipeData>>(u.ReadToEnd()));
//	}

//	public static string GetItemKeyFromType(int type)
//	{
//		if (ItemKeys.TryGetValue(type, out string v))
//			return v;
//		return "Unknown";
//	}

//	public static int GetItemTypeFromKey(string key)
//	{
//		if (ItemTypes.TryGetValue(key, out int v))
//			return v;
//		return 0;
//	}

//	public static string GetNPCKeyFromType(int type)
//	{
//		if (NPCKeys.TryGetValue(type, out string v))
//			return v;
//		return "Unknown";
//	}

//	public static int GetNPCTypeFromKey(string key)
//	{
//		if (NPCTypes.TryGetValue(key, out int v))
//			return v;
//		return 0;
//	}
//	public static string GetBuffKeyFromType(int type)
//	{
//		if (BuffKeys.TryGetValue(type, out string v))
//			return v;
//		return "Unknown";
//	}

//	public static int GetBuffTypeFromKey(string key)
//	{
//		if (BuffTypes.TryGetValue(key, out int v))
//			return v;
//		return 0;
//	}
//}
