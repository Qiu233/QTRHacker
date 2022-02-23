using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class PlayerEditorWindowViewModel : ViewModelBase
	{
		public Player Player { get; }
		public ItemSlotsEditorViewModel InventoryEditorViewModel { get; }
		public ItemSlotsEditorViewModel ArmorEditorViewModel { get; }
		public ItemSlotsEditorViewModel PiggyBankViewModel { get; }
		public DispatcherTimer UpdateTimer { get; }

		public PlayerEditorWindowViewModel(Player player)
		{
			Player = player;
			UpdateTimer = new();
			UpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.ItemUpdateInterval);

			InventoryEditorViewModel = new ItemSlotsEditorViewModel(new InventoryLayout(), GetInventoryItem, UpdateTimer);
			ArmorEditorViewModel = new ItemSlotsEditorViewModel(new ArmorLayout(), GetArmorItem, UpdateTimer);
			PiggyBankViewModel = new ItemSlotsEditorViewModel(new BankLayout(), GetPiggyBankItem, UpdateTimer);

			UpdateTimer.Start();
		}

		private Item GetArmorItem(int index)
		{
			if (index < Player.ARMOR_MAX_COUNT)
				return Player.Armor[index];
			index -= Player.ARMOR_MAX_COUNT;

			if (index < Player.DYE_MAX_COUNT)
				return Player.Dye[index];
			index -= Player.DYE_MAX_COUNT;

			if (index < Player.MISC_MAX_COUNT)
				return Player.MiscEquips[index];
			index -= Player.MISC_MAX_COUNT;

			if (index < Player.MISCDYE_MAX_COUNT)
				return Player.MiscDyes[index];
			return Player.Armor[0];
		}

		private Item GetInventoryItem(int index) => Player.Inventory[index];//TODO: cache Inventory
		private Item GetPiggyBankItem(int index) => Player.Bank.Item[index];//TODO: cache Bank.Item
	}
}
