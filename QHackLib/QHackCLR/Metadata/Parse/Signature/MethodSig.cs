using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Metadata.Parse.Signature
{
	public class MethodSig : BaseSig
	{
		public readonly int CallingConvention = 0;
		public readonly int GenParamCount = 0;
		public readonly int ParamCount = 0;
		public readonly RetTypeSig RetType;
		public readonly ImmutableArray<ParamSig> Params;
		public readonly ImmutableArray<ParamSig> ExtraParams;
		public MethodSig(SigParser parser) : base(parser)
		{
			Parser.GetCallingConvInfo(out CallingConvention);
			if (Generic)
				Parser.GetData(out GenParamCount);
			Parser.GetData(out ParamCount);
			RetType = new RetTypeSig(Parser);
			var builder = ImmutableArray.CreateBuilder<ParamSig>(ParamCount);
			int i;
			for (i = 0; i < ParamCount; i++)
			{
				Parser.PeekElemType(out CorElementType type);
				if (type == CorElementType.SENTINEL)
					break;
				builder.Add(new ParamSig(Parser));
			}
			Params = builder.ToImmutable();
			if (i < ParamCount - 1)
			{
				Parser.GetElemType(out CorElementType _);
				builder = ImmutableArray.CreateBuilder<ParamSig>();
				for (; i < ParamCount; i++)
					builder.Add(new ParamSig(Parser));
				ExtraParams = builder.ToImmutable();
			}
		}

		public bool HasThis => (CallingConvention & SigParser.IMAGE_CEE_CS_CALLCONV_HASTHIS) != 0;
		public bool ExplicitThis => (CallingConvention & SigParser.IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS) != 0;
		public bool VarArg => (CallingConvention & SigParser.IMAGE_CEE_CS_CALLCONV_VARARG) != 0;
		public bool Generic => (CallingConvention & SigParser.IMAGE_CEE_CS_CALLCONV_GENERIC) != 0;
		public bool Default => (CallingConvention & SigParser.IMAGE_CEE_CS_CALLCONV_MASK) == 0;
	}
}
