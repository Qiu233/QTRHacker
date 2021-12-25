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

		/// <summary>
		/// Gets field address from ref_base.<br/>
		/// The formula is [nuint address = <paramref name="objRef"/> + <see cref="ClrField.Offset"/>
		/// </summary>
		/// <param name="objRef"></param>
		/// <returns></returns>
		public nuint GetAddress(nuint objRef) => objRef + Offset;

		public T GetRawValue<T>(nuint objRef) where T : unmanaged => FieldHelper.DataAccess.Read<T>(GetAddress(objRef));

		public AddressableTypedEntity GetValue(nuint objRef)
		{
			if (Type.IsValueType)
				return new ClrValue(Type, GetAddress(objRef));
			return new ClrObject(Type.ClrObjectHelper, GetRawValue<nuint>(objRef));
		}
		public T GetValue<T>(nuint objRef) where T : AddressableTypedEntity => GetValue(objRef) as T;
	}
}
