using Microsoft.CSharp;
using QHackLib;
using System.CodeDom;

namespace QTRHacker.ViewModels.Common.PropertyEditor;

public abstract class PropertyBase : ViewModelBase, ICloneable
{
	private bool isSelected;
	private bool isExpanded;
	private HackEntity entity;

	public bool IsSelected
	{
		get => isSelected;
		set
		{
			isSelected = value;
			OnPropertyChanged(nameof(IsSelected));
		}
	}

	public bool IsExpanded
	{
		get => isExpanded;
		set
		{
			isExpanded = value;
			OnPropertyChanged(nameof(IsExpanded));
			IsExpandedChanged?.Invoke(this, new EventArgs());
		}
	}
	public HackEntity Entity
	{
		get => entity;
		set
		{
			entity = value;
			OnPropertyChanged(nameof(Entity));
		}
	}

	public string Name
	{
		get;
	}

	public abstract string Text { get; }

	public event EventHandler IsExpandedChanged;


	public PropertyBase(HackEntity entity, string name)
	{
		Entity = entity;
		Name = name;
	}


	protected static string GetUFName(string type)
	{
		string name;
		using CSharpCodeProvider provider = new();
		name = provider.GetTypeOutput(new CodeTypeReference(type));
		return name;
	}

	public static PropertyBase New(HackEntity entity, string prefix)
	{
		if (entity.Type.IsPrimitive)
		{
			string name = entity.Type.Name;
			switch (name)
			{
				case "System.SByte":
					return new PropertyPInt8(entity, prefix);
				case "System.Byte":
					return new PropertyPUInt8(entity, prefix);
				case "System.Int16":
					return new PropertyPInt16(entity, prefix);
				case "System.UInt16":
					return new PropertyPUInt16(entity, prefix);
				case "System.Int32":
					return new PropertyPInt32(entity, prefix);
				case "System.UInt32":
					return new PropertyPUInt32(entity, prefix);
				case "System.Int64":
					return new PropertyPInt64(entity, prefix);
				case "System.UInt64":
					return new PropertyPUInt64(entity, prefix);

				case "System.Single":
					return new PropertyPFloat32(entity, prefix);
				case "System.Double":
					return new PropertyPFloat64(entity, prefix);

				case "System.Boolean":
					return new PropertyPBool(entity, prefix);
				case "System.Char":
					return new PropertyPChar(entity, prefix);

				case "System.IntPtr":
					return new PropertyPIntPtr(entity, prefix);
				case "System.UIntPtr":
					return new PropertyPUIntPtr(entity, prefix);
			}
		}
		else if (entity.Type.Name == "System.String")
			return new PropertyString(entity, prefix);
		else if (entity.Type.IsArray)
			return new PropertyArray(entity, prefix);
		return new PropertyComplex(entity, prefix);
	}

	public object Clone() => New(Entity, Name);
}
