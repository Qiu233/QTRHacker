using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class PlayerArray : GameObjectArray<Player>
	{
		public PlayerArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{

		}
	}
}
