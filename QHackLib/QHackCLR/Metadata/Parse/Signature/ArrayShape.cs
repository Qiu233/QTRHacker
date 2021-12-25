using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHackCLR.Metadata.Parse.Signature
{
	public class ArrayShape : BaseSig
	{
		public readonly int Rank;
		public readonly int NumSizes;
		public readonly int NumLowerBounds;
		public readonly ImmutableArray<int> Sizes;
		public readonly ImmutableArray<int> LowerBounds;

		public ArrayShape(SigParser parser) : base(parser)
		{
			Parser.GetData(out Rank);

			Parser.GetData(out NumSizes);
			var builder = ImmutableArray.CreateBuilder<int>(NumSizes);
			for (int i = 0; i < NumSizes; i++)
			{
				Parser.GetData(out int size);
				builder.Add(size);
			}
			Sizes = builder.ToImmutable();

			Parser.GetData(out NumLowerBounds);
			builder = ImmutableArray.CreateBuilder<int>(NumLowerBounds);
			for (int i = 0; i < NumLowerBounds; i++)
			{
				Parser.GetData(out int lb);
				builder.Add(lb);
			}
			LowerBounds = builder.ToImmutable();
		}
	}
}
