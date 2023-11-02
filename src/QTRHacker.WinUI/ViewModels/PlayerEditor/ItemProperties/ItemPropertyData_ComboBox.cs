using System.Collections.ObjectModel;

namespace QTRHacker.ViewModels.PlayerEditor.ItemProperties;

public class ItemPropertyData_ComboBox<T> : ItemPropertyData<T> where T : unmanaged
{
	public ObservableCollection<object> Source
	{
		get;
	} = new();

	public ItemPropertyData_ComboBox(string key, SelectedItemHolder holder) : base(key, holder)
	{
	}
}
