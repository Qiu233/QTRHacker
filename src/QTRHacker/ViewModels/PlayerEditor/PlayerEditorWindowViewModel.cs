using QTRHacker.Functions.GameObjects.Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class PlayerEditorWindowViewModel
	{
		public Player Player { get; }
		public ItemSlotsEditorViewModel InventoryEditorViewModel { get; }
		public ItemSlotsEditorViewModel PiggyBankViewModel { get; }

		public PlayerEditorWindowViewModel(Player player)
		{
			Player = player;
			InventoryEditorViewModel = new ItemSlotsEditorViewModel(new InventoryLayout(), GetInventoryItem);
			PiggyBankViewModel = new ItemSlotsEditorViewModel(new BankLayout(), GetPiggyBankItem);
		}

		private Item GetInventoryItem(int index) => Player.Inventory[index];//TODO: cache Inventory
		private Item GetPiggyBankItem(int index) => Player.Bank.Item[index];//TODO: cache Bank.Item
	}
}
