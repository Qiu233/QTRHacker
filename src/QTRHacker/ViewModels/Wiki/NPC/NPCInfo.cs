using QTRHacker.Assets;
using QTRHacker.Localization;
using QTRHacker.Models.Wiki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki.NPC
{
	public class NPCInfo : ViewModelBase, ILocalizationProvider
	{
		private string name;
		private string category;
		public string Name => name;
		public string Category => category;

		public int Type { get; }
		public string Key { get; }
		public NPCData Data { get; }

		public NPCInfo(int type)
		{
			Type = type;
			Key = WikiResLoader.GetNPCKeyFromType(Type);
			Data = WikiResLoader.NPCDatum[Type];
			LocalizationManager.RegisterLocalizationProvider(this);
		}

		public NPCCategory GetNPCCategory()
		{
			var data = Data;
			NPCCategory result = NPCCategory.Others;
			if (data.Friendly) result |= NPCCategory.Friendly;
			if (data.TownNPC) result |= NPCCategory.Town;
			if (data.Boss) result |= NPCCategory.Boss;
			return result;
		}

		public void OnCultureChanged(object sender, CultureChangedEventArgs args)
		{
			name = LocalizationManager.Instance.GetValue($"NPCName.{Key}", LocalizationType.Game);
			OnPropertyChanged(nameof(Name));
			var c = GetNPCCategory();
			var values = Enum.GetValues<NPCCategory>()
				.Where(t => t != NPCCategory.Others && c.HasFlag(t))
				.Select(t => LocalizationManager.Instance.GetValue($"UI.NPCCategories.{t}"));
			category = string.Join(", ", values);
			if (!category.Any())
				category = LocalizationManager.Instance.GetValue("UI.NPCCategories.Others");
			OnPropertyChanged(nameof(Category));
		}
	}
}
