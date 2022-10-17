using QTRHacker.Core.GameObjects.Terraria;
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
		public PlayerPropertiesEditorViewModel PlayerPropertiesEditorViewModel { get; }
		public ItemSlotsEditorViewModel InventoryEditorViewModel { get; }
		public ItemSlotsEditorViewModel ArmorEditorViewModel { get; }
		public ItemSlotsEditorViewModel PiggyBankViewModel { get; }
		public ItemSlotsEditorViewModel SafeViewModel { get; }
		public ItemSlotsEditorViewModel ForgeViewModel { get; }
		public ItemSlotsEditorViewModel VoidVaultViewModel { get; }
		public ItemSlotsEditorViewModel Loadout1ViewModel { get; }
		public ItemSlotsEditorViewModel Loadout2ViewModel { get; }
		public ItemSlotsEditorViewModel Loadout3ViewModel { get; }
		public DispatcherTimer UpdateTimer { get; }

		public PlayerEditorWindowViewModel(Player player)
		{
			Player = player;
			UpdateTimer = new();
			UpdateTimer.Interval = TimeSpan.FromMilliseconds(HackGlobal.Config.ItemUpdateInterval);

			PlayerPropertiesEditorViewModel = new PlayerPropertiesEditorViewModel(player);

			InventoryEditorViewModel = new ItemSlotsEditorViewModel(new InventoryLayout(), player, GetInventoryItem, UpdateTimer);
			ArmorEditorViewModel = new ItemSlotsEditorViewModel(new ArmorLayout(), player, GetArmorItem, UpdateTimer);
			PiggyBankViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetPiggyBankItem, UpdateTimer);
			SafeViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetSafeItem, UpdateTimer);
			ForgeViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetForgeItem, UpdateTimer);
			VoidVaultViewModel = new ItemSlotsEditorViewModel(new BankLayout(), player, GetVoidVaultItem, UpdateTimer);

			Loadout1ViewModel = new ItemSlotsEditorViewModel(new LoadoutLayout(), player, index => GetLoadoutItem(0, index), UpdateTimer);
			Loadout2ViewModel = new ItemSlotsEditorViewModel(new LoadoutLayout(), player, index => GetLoadoutItem(1, index), UpdateTimer);
			Loadout3ViewModel = new ItemSlotsEditorViewModel(new LoadoutLayout(), player, index => GetLoadoutItem(2, index), UpdateTimer);

			UpdateTimer.Start();
		}

		~PlayerEditorWindowViewModel()
		{
			UpdateTimer?.Stop();
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

		private Item GetLoadoutItem(int loadoutIndex, int index)
		{
			if (0 <= index && index < 20)
				return Player.Loadouts[loadoutIndex].Armor[index];//TODO: cache Inventory
			else if (20 <= index && index < 30)
				return Player.Loadouts[loadoutIndex].Dye[index];//TODO: cache Inventory
			else
				throw new IndexOutOfRangeException();
		}

		private Item GetInventoryItem(int index) => Player.Inventory[index];//TODO: cache Inventory
		private Item GetPiggyBankItem(int index) => Player.Bank.Item[index];//TODO: cache Bank.Item
		private Item GetSafeItem(int index) => Player.Bank2.Item[index];//TODO: cache Bank.Item
		private Item GetForgeItem(int index) => Player.Bank3.Item[index];//TODO: cache Bank.Item
		private Item GetVoidVaultItem(int index) => Player.Bank4.Item[index];//TODO: cache Bank.Item
	}
}
