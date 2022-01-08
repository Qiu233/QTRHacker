using Newtonsoft.Json;
using QTRHacker.Res.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QTRHacker.Res
{
	public class GameLocLoader
	{
		public GameASMResLoader ResLoader { get; }
		public string Culture { get; }

		public GameLocLoader(GameASMResLoader loader, string cul)
		{
			ResLoader = loader;
			Culture = cul;

			/*string v = ResLoader.GetLocalizationText($"{Culture}", Encoding.UTF8);
			Content = JsonConvert.DeserializeObject<LocContent>(v);*/
			Game = JsonConvert.DeserializeObject<LocGame>(ResLoader.GetLocalizationText($"{Culture}.Game", Encoding.UTF8));
			Legacy = JsonConvert.DeserializeObject<LocLegacy>(ResLoader.GetLocalizationText($"{Culture}.Legacy", Encoding.UTF8));

			Items = JsonConvert.DeserializeObject<LocItems>(ResLoader.GetLocalizationText($"{Culture}.Items", Encoding.UTF8));
			NPCs = JsonConvert.DeserializeObject<LocNPCs>(ResLoader.GetLocalizationText($"{Culture}.NPCs", Encoding.UTF8));
			Projectiles = JsonConvert.DeserializeObject<LocProjectiles>(ResLoader.GetLocalizationText($"{Culture}.Projectiles", Encoding.UTF8));
			ResLoader.GetLocalizationText($"{Culture}.Town", Encoding.UTF8);

			//AddItems(Content);
			AddItems(Game);
			AddItems(Legacy);
			AddItems(Items);
			AddItems(NPCs);
			AddItems(Projectiles);
		}

		private void AddItems(object obj)
		{
			foreach (var v in obj.GetType().GetFields())
			{
				if (v.FieldType != typeof(Dictionary<string, string>))
					continue;
				var value = v.GetValue(obj) as Dictionary<string, string>;
				foreach (var item in value)
					RawValues[v.Name + "." + item.Key] = item.Value;
			}
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

		public string GetItemName(string name) => GetValue($"ItemName.{name}");
		public string GetNPCName(string name) => GetValue($"NPCName.{name}");
		public string GetItemTooltip(string name)
		{
			string key = $"ItemTooltip.{name}";
			if (!RawValues.ContainsKey(key))
				return "";
			return GetValue(key);
		}

		private LocItems Items { get; }
		//private LocContent Content { get; }
		private LocGame Game { get; }
		private LocLegacy Legacy { get; }
		private LocNPCs NPCs { get; }
		private LocProjectiles Projectiles { get; }
		private readonly Dictionary<string, string> RawValues = new();
		private readonly Dictionary<string, string> Processed = new();
	}
}
