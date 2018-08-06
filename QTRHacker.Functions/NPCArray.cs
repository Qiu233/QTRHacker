using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public class NPCArray : GameObject
	{
		public NPC this[int i]
		{
			get
			{
				ReadFromOffset(0x08 + 0x04 * i, out int v);
				return new NPC(Context, v);
			}
		}
		public NPCArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
	}
}
