using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions
{
	public abstract class GameObjectArray<T> : GameObject where T : GameObject
	{
		public T this[int i]
		{
			get
			{
				ReadFromOffset(0x08 + 0x04 * i, out int v);
				return (T)typeof(T).GetConstructor(new Type[] { typeof(GameContext), typeof(int) }).Invoke(new object[] { Context, v });
			}
		}
		public GameObjectArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{
		}
	}
}
