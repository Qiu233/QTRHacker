using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Metadata.Parse.Signature
{
	public class CustomMod : BaseSig
	{
		public readonly int TypeDefOrRefOrSpecEncoded;
		public CustomMod(SigParser parser) : base(parser)
		{
			Parser.PeekElemType(out CorElementType type);
			if (type != CorElementType.CMOD_OPT &&
				type != CorElementType.CMOD_REQD)
			{
				throw new Exception("Unexpected custom mod");
			}
			Parser.GetElemType(out CorElementType _);
			Parser.GetToken(out TypeDefOrRefOrSpecEncoded);
		}

		public static ImmutableArray<CustomMod> ParseCustomeMods(SigParser parser)
		{
			var mods = ImmutableArray.CreateBuilder<CustomMod>();
			while (true)
			{
				parser.PeekElemType(out CorElementType t);
				if (t != CorElementType.CMOD_OPT && t != CorElementType.CMOD_REQD)
					break;
				mods.Add(new CustomMod(parser));
			}
			return mods.ToImmutable();
		}
	}
}
