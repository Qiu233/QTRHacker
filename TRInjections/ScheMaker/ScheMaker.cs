using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace TRInjections.ScheMaker
{
	public class ScheMaker
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct CTile
		{
			public ushort Type;
			public byte Wall;
			public byte Liquid;
			public byte BTileHeader;
			public byte BTileHeader2;
			public byte BTileHeader3;
			public short FrameX;
			public short FrameY;
			public short STileHeader;
			public static CTile FromTile(Tile t)
			{
				return new CTile()
				{
					Type = t.type,
					Wall = t.wall,
					Liquid = t.liquid,
					BTileHeader = t.bTileHeader,
					BTileHeader2 = t.bTileHeader2,
					BTileHeader3 = t.bTileHeader3,
					FrameX = t.frameX,
					FrameY = t.frameY,
					STileHeader = t.sTileHeader,
				};
			}
			public Tile ToTile()
			{
				return new Tile()
				{
					type = Type,
					wall = Wall,
					liquid = Liquid,
					bTileHeader = BTileHeader,
					bTileHeader2 = BTileHeader2,
					bTileHeader3 = BTileHeader3,
					frameX = FrameX,
					frameY = FrameY,
					sTileHeader = STileHeader,
				};
			}
		}

		public static Tile[,] Tiles;

		public static void LoadTiles(int ptr)
		{
			int[] size = new int[2];
			int unitSize = Marshal.SizeOf(typeof(CTile));
			Marshal.Copy((IntPtr)ptr, size, 0, 2);
			int width = size[0], height = size[1];
			Tiles = new Tile[width, height];

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Tiles[i, j] = ((CTile)Marshal.PtrToStructure((IntPtr)(ptr + 8 + (i * height + j) * unitSize), typeof(CTile))).ToTile();
				}
			}

		}

		public ScheMaker()
		{

		}
	}
}
