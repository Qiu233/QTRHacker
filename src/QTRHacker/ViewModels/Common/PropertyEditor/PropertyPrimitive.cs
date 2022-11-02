using QHackLib;

namespace QTRHacker.ViewModels.Common.PropertyEditor;

public abstract class PropertyPrimitive<T> : EditableProperty where T : unmanaged
{

	public PropertyPrimitive(HackEntity entity, string name) : base(entity, name)
	{
	}

	public T Value
	{
		get => Entity.Context.DataAccess.Read<T>(Entity.OffsetBase);
		set
		{
			Entity.Context.DataAccess.Write(Entity.OffsetBase, value);
			OnPropertyChanged(nameof(Value));
		}
	}

	public override string Text => $"{Name}: {GetUFName(Entity.Type.Name)} = ";
}
#region Signed
public class PropertyPInt64 : PropertyPrimitive<long>
{
	public PropertyPInt64(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPInt32 : PropertyPrimitive<int>
{
	public PropertyPInt32(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPInt16 : PropertyPrimitive<short>
{
	public PropertyPInt16(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPInt8 : PropertyPrimitive<sbyte>
{
	public PropertyPInt8(HackEntity entity, string name) : base(entity, name) { }
}
#endregion
#region Unsigned
public class PropertyPUInt64 : PropertyPrimitive<ulong>
{
	public PropertyPUInt64(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPUInt32 : PropertyPrimitive<uint>
{
	public PropertyPUInt32(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPUInt16 : PropertyPrimitive<ushort>
{
	public PropertyPUInt16(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPUInt8 : PropertyPrimitive<byte>
{
	public PropertyPUInt8(HackEntity entity, string name) : base(entity, name) { }
}
#endregion

public class PropertyPChar : PropertyPrimitive<char>
{
	public PropertyPChar(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPBool : PropertyPrimitive<bool>
{
	public PropertyPBool(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPFloat32 : PropertyPrimitive<float>
{
	public PropertyPFloat32(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPFloat64 : PropertyPrimitive<double>
{
	public PropertyPFloat64(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPIntPtr : PropertyPrimitive<IntPtr>
{
	public PropertyPIntPtr(HackEntity entity, string name) : base(entity, name) { }
}
public class PropertyPUIntPtr : PropertyPrimitive<UIntPtr>
{
	public PropertyPUIntPtr(HackEntity entity, string name) : base(entity, name) { }
}
