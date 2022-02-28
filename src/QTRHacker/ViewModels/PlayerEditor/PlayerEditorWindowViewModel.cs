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
		public ItemSlotsEditorViewModel SafeViewModel { get; }
		public ItemSlotsEditorViewModel ForgeViewModel { get; }
		public ItemSlotsEditorViewModel VoidVaultViewModel { get; }
		public DispatcherTimer UpdateTimer { get; }

		public PlayerEditorWindowViewModel(Player player)
		{
			Player = player;
			UpdateTimer = new();
			UpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.ItemUpdateInterval);

			InventoryEditorViewModel = new ItemSlotsEditorViewModel(new InventoryLayout(), player, GetInventoryItem, UpdateTimer);
			ArmorEditorViewModel = new ItemSlotsEditorViewModel(new ArmorLayout(), player, GetArmorItem, UpdateTimer);
			PiggyBankViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetPiggyBankItem, UpdateTimer);
			SafeViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetSafeItem, UpdateTimer);
			ForgeViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetForgeItem, UpdateTimer);
			VoidVaultViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetVoidVaultItem, UpdateTimer);

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
		private Item GetSafeItem(int index) => Player.Bank2.Item[index];//TODO: cache Bank.Item
		private Item GetForgeItem(int index) => Player.Bank3.Item[index];//TODO: cache Bank.Item
		private Item GetVoidVaultItem(int index) => Player.Bank4.Item[index];//TODO: cache Bank.Item
	}
}
