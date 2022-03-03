using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects
{
	/// <summary>
	/// For 2D arrays.<br/>
	/// Use <see cref="GameObjectArrayMD{T}"/> when accessing arrays of higher rank.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameObjectArray2D<T> : GameObjectArrayMD<T> where T : GameObject
	{
		public T this[int i, int j]
		{
			get => GetValue(i, j);
			set => SetValue(value, i, j);
		}
		public GameObjectArray2D(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}
	}
	/// <summary>
	/// For 2D arrays.<br/>
	/// Use <see cref="GameObjectArrayMDV{T}"/> when accessing arrays of higher rank.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameObjectArray2DV<T> : GameObjectArrayMDV<T> where T : unmanaged
	{
		public T this[int i, int j]
		{
			get => GetValue(i, j);
			set => SetValue(value, i, j);
		}
		public GameObjectArray2DV(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}
	}
	/// <summary>
	/// For 2D arrays.<br/>
	/// Use <see cref="GameObjectArrayMD"/> when accessing arrays of higher rank.
	/// </summary>
	public class GameObjectArray2D : GameObjectArrayMD
	{
		public dynamic this[int i, int j]
		{
			get => GetValue(i, j);
			set => SetValue(value, i, j);
		}
		public GameObjectArray2D(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}
	}
}
