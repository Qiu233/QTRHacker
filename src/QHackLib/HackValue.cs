using QHackCLR.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackLib
{
	public class HackValue : HackEntity
	{
		public unsafe override nuint OffsetBase => BaseAddress;
		public HackValue(QHackContext ctx, ClrType type, nuint baseAddress) : base(ctx, type, baseAddress)
		{
		}

		public object InternalConvert(Type type)
		{
			return Context.DataAccess.Read(type, BaseAddress);
		}

		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			result = InternalConvert(binder.Type);
			return true;
		}
	}
}
