using QHackLib;
using QTRHacker.Core.GameObjects;

namespace QTRHacker.ViewModels.Common.PropertyEditor;

public class PropertyString : EditableProperty
{
	public PropertyString(HackEntity entity, string name) : base(entity, name)
	{
		IsEditable = false;
	}

	public string Value
	{
		get
		{
			if (Entity is not HackObject ho || ho.BaseAddress == 0)
				return "null";
			return GameString.GetString(ho);
		}
	}

	public override string Text => $"{Name}: string = ";
}
