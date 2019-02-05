using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public class GameObjectArray2D<T> : GameObject where T : GameObject
	{
		public virtual T this[int x, int y]
		{
			get
			{
				ReadFromOffset((x * D2 + y) * 4 + 0x18, out int vv);
				return (T)typeof(T).GetConstructor(new Type[] { typeof(GameContext), typeof(int) }).Invoke(new object[] { Context, vv });
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
		public GameObjectArray2D(GameContext context, int bAddr) : base(context, bAddr)
		{
		}
	}
}
