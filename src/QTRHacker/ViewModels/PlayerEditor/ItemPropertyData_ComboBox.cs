using System.Collections.ObjectModel;

namespace QTRHacker.ViewModels.PlayerEditor;

public class ItemPropertyData_ComboBox<T> : ItemPropertyData<T> where T:unmanaged
{
	public ObservableCollection<object> Source
	{
		get;
	} = new();

	public ItemPropertyData_ComboBox(string key) : base(key)
	{
	}
}
