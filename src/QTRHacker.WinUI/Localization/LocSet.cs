using Newtonsoft.Json.Linq;
using QTRHacker.Assets;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Windows.Globalization;

namespace QTRHacker.Localization;

public class LocSet
{
	private readonly Dictionary<string, string> RawValues = new();
	private readonly Dictionary<string, string> Processed = new();

	private void Load(string jsonText)
	{
		LoadOBJ(JObject.Parse(jsonText), string.Empty);
	}

	private void LoadOBJ(JObject obj, string prefix)
	{
		foreach (var section in obj)
		{
			if (section.Value is JObject subObj)
				LoadOBJ(subObj, prefix + section.Key + ".");
			else if (section.Value is JValue value && value.Type == JTokenType.String)
				RawValues[prefix + section.Key] = value.Value<string>()!;
		}
	}

	private LocSet()
	{

	}

	public static async Task<LocSet> LoadFromRes(string? culture = null)
	{
		LocSet set = new();
		try
		{
			string uri;
			if (culture is null)
				uri = $"ms-appx:///Localization/Content.json";
			else
				uri = $"ms-appx:///Localization/{culture}/Content.json";
			byte[] data = await AssetReader.ReadData(uri);
			string json = new StreamReader(new MemoryStream(data, false), Encoding.UTF8).ReadToEnd();
			set.Load(json);
		}
		catch
		{
			HackGlobal.Logging.Warn($"Failed to load localization file, this should mean that the corresponding localization file is missing");
		}
		return set;
	}

	public static async Task<LocSet> LoadFromGame(string culture)
	{
		byte[] data = await AssetReader.ReadData($"ms-appx:///Assets/Game/Localization.zip");
		using ZipArchive z = new(new MemoryStream(data, false));
		var es = z.Entries.Where(e => e.FullName.StartsWith($"Content.{culture}"));
		LocSet manager = new();
		foreach (var e in es)
		{
			using StreamReader sr = new(e.Open());
			manager.Load(sr.ReadToEnd());
		}
		return manager;
	}

	private string Process(string key)
	{
		if (!RawValues.ContainsKey(key))
			return key;
		return Regex.Replace(RawValues[key], "{\\$(\\w+\\.\\w+)}", new MatchEvaluator(m =>
		{
			return GetValue(m.Groups[1].Value);
		}));
	}

	public string GetValue(string key)
	{
		if (Processed.TryGetValue(key, out string? v))
			return v;
		return Processed[key] = Process(key);
	}

	public IEnumerable<string> Keys => RawValues.Keys;

	public string this[string key] => GetValue(key);
}
