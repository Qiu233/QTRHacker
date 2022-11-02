using Microsoft.Win32;
using QTRHacker.Assets;
using QTRHacker.Commands;
using QTRHacker.Core.GameObjects.Terraria;
using QTRHacker.ViewModels.Common;
using QTRHacker.Views.Common;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace QTRHacker.ViewModels.PlayerEditor;

public class ItemSlotsEditorViewModel : ViewModelBase
{
	private record struct Clipboard(int Type, int Stack, byte Prefix);
	private bool updating;
	private readonly Player player;
	private Clipboard ItemClipboard = new(0, 0, 0);

	public delegate Item ItemFromIndexDelegate(int index);
	public ItemFromIndexDelegate ItemProvider { get; }

	public ItemPropertiesPanelViewModel ItemPropertiesPanelViewModel { get; }
	public ItemSlotsGridViewModel ItemSlotsGridViewModel { get; }

	public bool Updating
	{
		get => updating;
		set
		{
			updating = value;
			OnPropertyChanged(nameof(Updating));
		}
	}
	public RelayCommand ApplyItemDataCommand { get; }
	public RelayCommand InitItemDataCommand { get; }
	public RelayCommand SaveInvCommand { get; }
	public RelayCommand LoadInvCommand { get; }
	public RelayCommand ViewInWikiCommand { get; }
	public RelayCommand CopyCommand { get; }
	public RelayCommand PasteCommand { get; }
	public RelayCommand EditCommand { get; }

	private Item SelectedItem => ItemProvider(ItemSlotsGridViewModel.SelectedIndex);


	public ItemSlotsEditorViewModel(ISlotsLayout layout, Player player, ItemFromIndexDelegate itemProvider, DispatcherTimer updateTimer)
	{
		this.player = player;
		ItemProvider = itemProvider;
		ItemPropertiesPanelViewModel = new ItemPropertiesPanelViewModel();
		ItemSlotsGridViewModel = new(layout);
		ItemSlotsGridViewModel.SelectedIndexChanged += ItemSlotsGridViewModel_SelectedIndexChanged;
		ItemSlotsGridViewModel.SelectedIndex = 0;
		if (updateTimer != null)
			WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(updateTimer, nameof(DispatcherTimer.Tick), Timer_Tick);

		ApplyItemDataCommand = new RelayCommand(o => true, o =>
		{
			ItemPropertiesPanelViewModel.UpdatePropertiesToItem(SelectedItem);
		});
		InitItemDataCommand = new RelayCommand(o => true, o =>
		{
			InitItem();
			ItemPropertiesPanelViewModel.UpdatePropertiesFromItem(SelectedItem);
		});
		SaveInvCommand = new RelayCommand(o => true, o =>
		{
			SaveFileDialog dialog = new();
			dialog.Filter = "inv file|*.inv";
			dialog.InitialDirectory = Path.GetFullPath("./Content/Invs");
			if (dialog.ShowDialog() == true)
			{
				using var s = dialog.OpenFile();
				this.player.SaveInventory(s);
			}
		});
		LoadInvCommand = new RelayCommand(o => true, o =>
		{
			OpenFileDialog dialog = new();
			dialog.Filter = "inv file|*.inv";
			dialog.InitialDirectory = Path.GetFullPath("./Content/Invs");
			if (dialog.ShowDialog() == true)
			{
				using var s = dialog.OpenFile();
				this.player.LoadInventory(s);
			}
		});
		ViewInWikiCommand = new RelayCommand(o => true, o =>
		{
			Views.Wiki.WikiWindow window = new();
			var d = new Wiki.WikiWindowViewModel();
			d.ItemPageViewModel.SetSelectedItemType(SelectedItem.Type);
			window.DataContext = d;
			window.Show();
		});
		CopyCommand = new RelayCommand(o => true, o =>
		{
			ItemClipboard = new Clipboard(SelectedItem.Type, SelectedItem.Stack, SelectedItem.Prefix);
		});
		PasteCommand = new RelayCommand(o => true, o =>
		{
			if (ItemClipboard.Type == 0 || ItemClipboard.Stack == 0)
				return;
			SelectedItem.SetDefaultsAndPrefix(ItemClipboard.Type, ItemClipboard.Prefix);
			SelectedItem.Stack = ItemClipboard.Stack;
		});
		EditCommand = new RelayCommand(o => true, o =>
		{
			var vm = new PropertyEditorWindowViewModel();
			vm.Roots.Add(new Common.PropertyEditor.PropertyComplex(SelectedItem.InternalObject, "Item"));
			PropertyEditorWindow window = new();
			window.DataContext = vm;
			window.Show();
		});

		Update();
	}

	private void InitItem()
	{
		int type = (int)ItemPropertiesPanelViewModel.GetValue("Type");
		if (type == 0)
			return;
		int stack = (int)ItemPropertiesPanelViewModel.GetValue("Stack");
		stack = stack == 0 ? 1 : stack;
		byte prefix = (byte)ItemPropertiesPanelViewModel.GetValue("Prefix");
		var item = SelectedItem;
		item.SetDefaultsAndPrefix(type, prefix);
		item.Stack = stack;
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		if (!Updating)
			return;
		Update();
	}

	private void ItemSlotsGridViewModel_SelectedIndexChanged(object sender, EventArgs e)
	{
		ItemPropertiesPanelViewModel.UpdatePropertiesFromItem(ItemProvider(ItemSlotsGridViewModel.SelectedIndex));
	}

	public void Update()
	{
		foreach (var slot in ItemSlotsGridViewModel.Slots)
		{
			var item = ItemProvider(slot.Index);
			if (item == null)
				continue;
			slot.ItemImage = GameImages.GetItemImage(item.Type);
			slot.Stack = item.Stack;
		}
	}
}
