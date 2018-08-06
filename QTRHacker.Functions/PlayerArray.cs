using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class PlayerArray : GameObject
	{
		public Player this[int i]
		{
			get
			{
				ReadFromOffset(0x08 + 0x04 * i, out int v);
				return new Player(Context, v);
			}
		}
		public PlayerArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
	}
}
