using QHackCLR.Clr.Builders.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Clr
{
	public class ClrStaticField : ClrField
	{
		public ClrStaticField(ClrType decType, IFieldHelper helper, nuint handle) : base(decType, helper, handle)
		{
		}


		/// <summary>
		/// Gets address using default AppDomain.<br/>
		/// </summary>
		/// <returns></returns>
		public nuint GetAddress()
		{
			return FieldHelper.GetStaticFieldAddress(this);
		}


		public T GetRawValue<T>() where T : unmanaged
		{
			return FieldHelper.DataAccess.Read<T>(GetAddress());
		}

		/// <summary>
		/// Static fields except those of primitive types are always on the heap.
		/// </summary>
		/// <returns></returns>
		public AddressableTypedEntity GetValue()
		{
			if (Type.IsPrimitive)
				return new ClrValue(Type, GetAddress());
			return new ClrObject(Type.ClrObjectHelper, GetRawValue<nuint>());
		}
	}
}
