using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ScheMaker
{
	public class Schematic
	{
		public int Width
		{
			get;
		}
		public int Height
		{
			get;
		}
		RawTile[,] Tiles
		{
			get;
		}
		public Schematic(int width, int height)
		{
			Width = width;
			Height = height;

			Tiles = new RawTile[width, height];
		}

	}
}
