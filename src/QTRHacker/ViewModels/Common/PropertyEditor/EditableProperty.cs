using QHackCLR.Common;
using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Common.PropertyEditor
{
	public abstract class EditableProperty : PropertyBase
	{
		private bool isEditing;
		private bool isEditable = true;
		public bool IsEditing
		{
			get => isEditing;
			set
			{
				isEditing = value;
				OnPropertyChanged(nameof(IsEditing));
			}
		}
		public bool IsEditable
		{
			get => isEditable;
			set
			{
				isEditable = value;
				OnPropertyChanged(nameof(IsEditable));
			}
		}

		protected EditableProperty(HackEntity entity, string name) : base(entity, name)
		{
		}
	}
}
