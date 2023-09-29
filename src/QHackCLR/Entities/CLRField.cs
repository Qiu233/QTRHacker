using QHackCLR.Builders;
using QHackCLR.Common;
using QHackCLR.DAC.DACP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public unsafe abstract class CLRField : CLREntity
{
	public CLRType DeclaringType { get; }
	public CLRType Type { get; }
	internal readonly IFieldHelper FieldHelper;
	internal readonly DacpFieldDescData Data;
	public string Name { get; }
	public FieldAttributes FieldAttributes { get; }
	internal CLRField(CLRType declType, IFieldHelper helper, nuint ClrHandle) : base(ClrHandle)
	{
		DeclaringType = declType;
		FieldHelper = helper;

		Data = new();
		fixed (DacpFieldDescData* ptr = &Data)
			helper.SOSDac.GetFieldDescData(NativeHandle, ptr);

		Type = helper.TypeFactory.GetCLRType(Data.MTOfType)!;

		helper.GetFieldProps(declType, Data.mb, out string? name, out FieldAttributes? attrs);
		if (name is null || attrs is null)
			throw new QHackCLRException("Failed to get field properties");
		Name = name;
		FieldAttributes = attrs.Value;
	}

	public CorElementType ElementType => Data.Type;
	public uint Offset => Data.dwOffset;
}
