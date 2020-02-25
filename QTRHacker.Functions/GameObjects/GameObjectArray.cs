using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public abstract class GameObjectArray<T> : GameObject where T : GameObject
	{
		public virtual T this[int index]
		{
			get
			{
				ReadFromOffset(0x8 + 4 * index, out int v);
				return (T)typeof(T).GetConstructor(new Type[] { typeof(GameContext), typeof(int) }).Invoke(new object[] { Context, v });
			}
		}
		public virtual int Length
		{
			get
			{
				ReadFromOffset(0x4, out int v);
				return v;
			}
		}
		public GameObjectArray(GameContext Context, int bAddr) : base(Context, bAddr)
		{
		}
	}
}
