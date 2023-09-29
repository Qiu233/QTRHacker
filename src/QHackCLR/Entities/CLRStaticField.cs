using QHackCLR.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public class CLRStaticField : CLRField
{
	internal CLRStaticField(CLRType declType, IFieldHelper helper, nuint ClrHandle) : base(declType, helper, ClrHandle)
	{
	}

	public nuint Address => FieldHelper.GetStaticFieldAddress(this);

	public T GetRawValue<T>() where T : unmanaged => FieldHelper.DataAccess.ReadValue<T>(Address);
	public AddressableTypedEntity GetValue()
	{
		if (Type.IsPrimitive)
			return new CLRValue(Type, Address);
		return new CLRObject(Type, GetRawValue<nuint>());
	}
}
