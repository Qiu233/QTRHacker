using QHackCLR.Builders;
using QHackCLR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Entities;

public class CLRInstanceField : CLRField
{
	internal CLRInstanceField(CLRType declType, IFieldHelper helper, nuint ClrHandle) : base(declType, helper, ClrHandle)
	{
	}

	public nuint GetAddress(nuint offsetBase) => offsetBase + Offset;

	public T GetRawValue<T>(nuint offsetBase) where T : unmanaged => FieldHelper.DataAccess.ReadValue<T>(GetAddress(offsetBase));


	public AddressableTypedEntity GetValue(nuint offsetBase)
	{
		if (Type.IsValueType)
			return new CLRValue(Type, GetAddress(offsetBase));
		return new CLRObject(Type, GetRawValue<nuint>(offsetBase));
	}
	public nuint GetAddress(AddressableTypedEntity entity) => entity.OffsetBase + Offset;
	public T GetRawValue<T>(AddressableTypedEntity entity) where T : unmanaged => FieldHelper.DataAccess.ReadValue<T>(GetAddress(entity));
	public AddressableTypedEntity GetValue(AddressableTypedEntity entity)
	{
		if (Type.IsValueType)
			return new CLRValue(Type, GetAddress(entity));
		return new CLRObject(Type, GetRawValue<nuint>(entity));
	}

}
