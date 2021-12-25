using QHackCLR.Dac.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Metadata.Parse.Signature
{
	public class TypeSig : BaseSig
	{
		public readonly CorElementType Type;
		public readonly int ContentTypeToken;

		public readonly TypeSig ArrayElementType;
		public readonly ArrayShape ArrayShape;

		public readonly TypeSig PtrType;
		public readonly CorElementType PtrTypeVoid;
		public readonly ImmutableArray<CustomMod> CustomMods;

		public readonly MethodSig FnPtrMethod;

		public readonly ImmutableArray<TypeSig> GenTypes;
		public readonly int VARNumber;

		public TypeSig(SigParser parser) : base(parser)
		{
			Parser.GetElemType(out Type);
			if (Type.IsPrimitive() ||
				Type == CorElementType.Object ||
				Type == CorElementType.String)
			{
			}
			else if (Type == CorElementType.Class || Type == CorElementType.Struct)
			{
				Parser.GetToken(out ContentTypeToken);
			}
			else if (Type == CorElementType.Array)
			{
				ArrayElementType = new TypeSig(Parser);
				ArrayShape = new ArrayShape(Parser);
			}
			else if (Type == CorElementType.FunctionPointer)
			{
				FnPtrMethod = new MethodSig(Parser);
			}
			else if (Type == CorElementType.GenericInstantiation)
			{
				Parser.GetElemType(out CorElementType _);
				Parser.GetToken(out ContentTypeToken);
				Parser.GetData(out int count);
				GenTypes = ParseTypes(Parser, count);
			}
			else if (Type == CorElementType.MVar || Type == CorElementType.Var)
			{
				Parser.GetData(out VARNumber);
			}
			else if (Type == CorElementType.Pointer)
			{
				CustomMods = CustomMod.ParseCustomeMods(Parser);
				Parser.PeekElemType(out CorElementType type);
				if (type == CorElementType.Void)
					Parser.GetElemType(out PtrTypeVoid);
				else
					PtrType = new TypeSig(Parser);
			}
			else if (Type == CorElementType.SZArray)
			{
				CustomMods = CustomMod.ParseCustomeMods(Parser);
				ArrayElementType = new TypeSig(Parser);
			}
		}

		public static ImmutableArray<TypeSig> ParseTypes(SigParser parser, int count)
		{
			var mods = ImmutableArray.CreateBuilder<TypeSig>();
			for (int i = 0; i < count; i++)
				mods.Add(new TypeSig(parser));
			return mods.ToImmutable();
		}

	}
}
