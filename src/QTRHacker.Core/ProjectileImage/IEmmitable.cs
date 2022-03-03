using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.ProjectileImage
{
	public interface IEmmitable
	{
		void Emit(GameContext Context, MPointF Location);
	}
}
