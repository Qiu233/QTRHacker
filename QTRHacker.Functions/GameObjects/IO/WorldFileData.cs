using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects.IO
{
	[GameFieldOffsetTypeName("Terraria.IO.WorldFileData")]
	public class WorldFileData : FileData
	{
		public WorldFileData(GameContext context, int bAddr) : base(context, bAddr)
		{
		}
	}
}
