using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class NPCArray : GameObjectArray<NPC>
	{
		public NPCArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
	}
}
