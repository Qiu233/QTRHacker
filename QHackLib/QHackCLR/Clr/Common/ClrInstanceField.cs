using QHackCLR.Clr.Builders.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public class ClrInstanceField : ClrField
	{
		public ClrInstanceField(ClrType decType, IFieldHelper helper, nuint handle) : base(decType, helper, handle)
		{
		}

		private unsafe nuint GetAddress(nuint offsetBase) => offsetBase + Offset;
		public unsafe nuint GetAddress(AddressableTypedEntity entity) => entity.OffsetBase + Offset;

		private T GetRawValue<T>(nuint offsetBase) where T : unmanaged => FieldHelper.DataAccess.Read<T>(GetAddress(offsetBase));
		public T GetRawValue<T>(AddressableTypedEntity entity) where T : unmanaged => FieldHelper.DataAccess.Read<T>(GetAddress(entity));

		private AddressableTypedEntity GetValue(nuint offsetBase)
		{
			if (Type.IsValueType)
				return new ClrValue(Type, GetAddress(offsetBase));
			return new ClrObject(Type.ClrObjectHelper, GetRawValue<nuint>(offsetBase));
		}
		public AddressableTypedEntity GetValue(AddressableTypedEntity entity)
		{
			if (Type.IsValueType)
				return new ClrValue(Type, GetAddress(entity));
			return new ClrObject(Type.ClrObjectHelper, GetRawValue<nuint>(entity));
		}
		private T GetValue<T>(nuint offsetBase) where T : AddressableTypedEntity => GetValue(offsetBase) as T;
		public T GetValue<T>(AddressableTypedEntity entity) where T : AddressableTypedEntity => GetValue(entity) as T;
	}
}
