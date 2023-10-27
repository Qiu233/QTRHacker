using Newtonsoft.Json;
using QTRHacker.Models.Wiki;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace QTRHacker.Assets;

public static class WikiResLoader
{
	private static readonly Dictionary<int, string> _ItemKeys = new();
	private static readonly Dictionary<string, int> _ItemTypes = new();
	private static readonly Dictionary<int, string> _NPCKeys = new();
	private static readonly Dictionary<string, int> _NPCTypes = new();
	private static readonly Dictionary<int, string> _BuffKeys = new();
	private static readonly Dictionary<string, int> _BuffTypes = new();
	private static readonly List<ItemData> _ItemDatum = new();
	private static readonly List<NPCData> _NPCDatum = new();
	private static readonly List<RecipeData> _RecipeDatum = new();

	private static bool loaded = false;

	private static Task? loadingTask = null;

	private static async Task LoadData()
	{
		var s = await AssetReader.ReadData("ms-appx:///Assets/Game/WikiRes.zip");
		using ZipArchive z = new(new MemoryStream(s));
		await Task.WhenAll(
			LoadItemData(z),
			LoadNPCData(z),
			LoadBuffData(z)
			);
		GC.Collect();
		loaded = true;
	}

	private static Task TryLoadIfNotLoaded()
	{
		if (loaded)
			return Task.CompletedTask;
		if (loadingTask is not null) // without yielding
			return loadingTask;
		return loadingTask = LoadData();
	}

	private static async Task LoadBuffData(ZipArchive z)
	{
		using var u = new StreamReader(z.GetEntry("ID/BuffID.json")!.Open());
		_BuffTypes.Clear();
		_BuffKeys.Clear();
		JsonConvert.DeserializeObject<Dictionary<string, int>>(await u.ReadToEndAsync())!.ToList().ForEach(t =>
		{
			_BuffTypes[t.Key] = t.Value;
			_BuffKeys[t.Value] = t.Key;
		});
	}
	private static async Task LoadNPCData(ZipArchive z)
	{
		using (var u = new StreamReader(z.GetEntry("ID/NPCID.json")!.Open()))
		{
			_NPCTypes.Clear();
			_NPCKeys.Clear();
			JsonConvert.DeserializeObject<Dictionary<string, int>>(await u.ReadToEndAsync())!.ToList().ForEach(t =>
			{
				_NPCTypes[t.Key] = t.Value;
				_NPCKeys[t.Value] = t.Key;
			});
		}
		_NPCDatum.Clear();
		using (var u = new StreamReader(z.GetEntry("NPCInfo.json")!.Open()))
			_NPCDatum.AddRange(JsonConvert.DeserializeObject<List<NPCData>>(await u.ReadToEndAsync())!);
	}

	private static async Task LoadItemData(ZipArchive z)
	{
		using (var u = new StreamReader(z.GetEntry("ID/ItemID.json")!.Open()))
		{
			_ItemTypes.Clear();
			_ItemKeys.Clear();
			JsonConvert.DeserializeObject<Dictionary<string, int>>(await u.ReadToEndAsync())!.ToList().ForEach(t =>
			{
				_ItemTypes[t.Key] = t.Value;
				_ItemKeys[t.Value] = t.Key;
			});
		}
		_ItemDatum.Clear();
		_RecipeDatum.Clear();
		using (var u = new StreamReader(z.GetEntry("ItemInfo.json")!.Open()))
			_ItemDatum.AddRange(JsonConvert.DeserializeObject<List<ItemData>>(await u.ReadToEndAsync())!);
		using (var u = new StreamReader(z.GetEntry("RecipeInfo.json")!.Open()))
			_RecipeDatum.AddRange(JsonConvert.DeserializeObject<List<RecipeData>>(await u.ReadToEndAsync())!);
	}

	public static async Task<string> GetItemKeyFromType(int type)
	{
		await TryLoadIfNotLoaded();
		if (_ItemKeys.TryGetValue(type, out string? v))
			return v;
		return "Unknown";
	}

	public static async Task<int> GetItemTypeFromKey(string key)
	{
		await TryLoadIfNotLoaded();
		if (_ItemTypes.TryGetValue(key, out int v))
			return v;
		return 0;
	}

	public static async Task<string> GetNPCKeyFromType(int type)
	{
		await TryLoadIfNotLoaded();
		if (_NPCKeys.TryGetValue(type, out string? v))
			return v;
		return "Unknown";
	}

	public static async Task<int> GetNPCTypeFromKey(string key)
	{
		await TryLoadIfNotLoaded();
		if (_NPCTypes.TryGetValue(key, out int v))
			return v;
		return 0;
	}
	public static async Task<string> GetBuffKeyFromType(int type)
	{
		await TryLoadIfNotLoaded();
		if (_BuffKeys.TryGetValue(type, out string? v))
			return v;
		return "Unknown";
	}

	public static async Task<int> GetBuffTypeFromKey(string key)
	{
		await TryLoadIfNotLoaded();
		if (_BuffTypes.TryGetValue(key, out int v))
			return v;
		return 0;
	}


	public static Task<List<ItemData>> ItemDatum => TryLoadIfNotLoaded().ContinueWith(t => _ItemDatum);
}
