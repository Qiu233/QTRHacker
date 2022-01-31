using Newtonsoft.Json.Linq;
using QTRHacker.AssetLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace QTRHacker.Localization
{
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
					RawValues[prefix + section.Key] = value.Value<string>();
			}
		}

		private LocSet()
		{

		}

		public static LocSet LoadFromRes(string culture)
		{
			using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/QTRHacker;component/Localization/Content/{culture}.json")).Stream;
			string json = new StreamReader(s, Encoding.UTF8).ReadToEnd();
			LocSet manager = new();
			manager.Load(json);
			return manager;
		}

		public static LocSet LoadFromGame(GameASMResLoader loader, string culture)
		{
			var locs = loader.Cache
				.Where(t => t.Key.StartsWith($"Terraria.Localization.Content.{culture}") && t.Key.EndsWith(".json"))
				.Select(t => Encoding.UTF8.GetString(t.Value));
			LocSet manager = new();
			foreach (var json in locs)
				manager.Load(json);
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
			if (Processed.TryGetValue(key, out string v))
				return v;
			return Processed[key] = Process(key);
		}

		public IEnumerable<string> Keys => RawValues.Keys;

		public string this[string key] => GetValue(key);
	}
}
