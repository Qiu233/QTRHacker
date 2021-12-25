using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Metadata.Parse.Signature
{
	public class ParamSig : BaseSig
	{
		public readonly ImmutableArray<CustomMod> CustomModifiers;
		public readonly bool ByRef;
		public readonly TypeSig Type;
		public ParamSig(SigParser parser) : base(parser)
		{
			CustomModifiers = CustomMod.ParseCustomeMods(Parser);
			Parser.PeekElemType(out CorElementType type);
			if (type == CorElementType.TypedByRef)
			{
				Parser.GetElemType(out CorElementType _);
				return;
			}
			if (type == CorElementType.ByRef)
			{
				ByRef = true;
				Parser.GetElemType(out CorElementType _);
			}
			Type = new TypeSig(Parser);
		}
	}
}
