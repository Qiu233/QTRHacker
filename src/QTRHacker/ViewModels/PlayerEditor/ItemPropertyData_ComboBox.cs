using QTRHacker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor
{
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
}
