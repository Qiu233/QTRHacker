using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.PlayerEditor
{
	public class ItemPropertyData_TextBox<T> : ItemPropertyData<T> where T : unmanaged
	{
		public ItemPropertyData_TextBox(string key) : base(key)
		{
		}
	}
}
