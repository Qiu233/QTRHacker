using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.ViewModels.Wiki.NPC
{
	[Flags]
	public enum NPCCategory
	{
		Others = 0,
		Friendly = 1,
		Town = 2,
		Boss = 4
	}
}
