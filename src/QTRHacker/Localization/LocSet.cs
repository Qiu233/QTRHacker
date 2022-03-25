using Newtonsoft.Json.Linq;
using QTRHacker.AssetLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
			LocSet manager = new();
			try
			{
				using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/QTRHacker;component/Localization/Content/{culture}.json")).Stream;
				string json = new StreamReader(s, Encoding.UTF8).ReadToEnd();
				manager.Load(json);
			}
			catch
			{
				HackGlobal.Logging.Warn($"Failed to load localization file for {culture}, this should mean that the corresponding localization file is missing");
			}
			return manager;
		}

		public static LocSet LoadFromGame(string culture)
		{
			using var s = Application.GetResourceStream(new Uri($"pack://application:,,,/QTRHacker;component/Assets/Game/Localization.zip")).Stream;
			using ZipArchive z = new(s);
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
			if (Processed.TryGetValue(key, out string v))
				return v;
			return Processed[key] = Process(key);
		}

		public IEnumerable<string> Keys => RawValues.Keys;

		public string this[string key] => GetValue(key);
	}
}
