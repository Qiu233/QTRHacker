using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class Tile2DArray : GameObject
	{
		public Tile this[int x, int y]
		{
			get
			{
				ReadFromOffset((x * D2 + y) * 4 + 0x18, out int vv);
				return new Tile(Context, vv);
			}
		}

		public int D1
		{
			get
			{
				ReadFromOffset(0x8, out int v);
				return v;
			}
		}
		public int D2
		{
			get
			{
				ReadFromOffset(0xc, out int v);
				return v;
			}
		}
		public Tile2DArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
	}
}
