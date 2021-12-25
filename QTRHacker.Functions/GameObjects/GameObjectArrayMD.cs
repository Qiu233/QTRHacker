using QHackLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Functions.GameObjects
{
	public interface IGameObjectArrayMD<T>
	{
		int Rank { get; }
		int GetLength(int dimension);
		T GetValue(params int[] indexes);
		void SetValue(T value, params int[] indexes);
	}
	/// <summary>
	/// For multidimensional arrays.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameObjectArrayMD<T> : GameObject, IGameObjectArrayMD<T> where T : GameObject
	{
		public int Rank =>
			TypedInternalObject.GetArrayRank();
		public int GetLength(int dimension) =>
			TypedInternalObject.GetArrayLength(dimension);
		public T GetValue(params int[] indexes) =>
			MakeGameObject<T>(TypedInternalObject.InternalGetIndex(indexes.Select(t => (object)t).ToArray()));
		public void SetValue(T value, params int[] indexes) =>
			TypedInternalObject.InternalSetIndex(indexes.Select(t => (object)t).ToArray(), value.InternalObject);

		public GameObjectArrayMD(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}
	}
	/// <summary>
	/// For fast access to unmanaged types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameObjectArrayMDV<T> : GameObject, IGameObjectArrayMD<T> where T : unmanaged
	{
		public int Rank =>
			TypedInternalObject.GetArrayRank();
		public int GetLength(int dimension) =>
			TypedInternalObject.GetArrayLength(dimension);
		public T GetValue(params int[] indexes) =>
			(T)(dynamic)TypedInternalObject.InternalGetIndex(indexes.Select(t => (object)t).ToArray());
		public void SetValue(T value, params int[] indexes) =>
			TypedInternalObject.InternalSetIndex(indexes.Select(t => (object)t).ToArray(), value);
		public GameObjectArrayMDV(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}
	}
	public class GameObjectArrayMD : GameObject
	{
		public int Rank =>
			TypedInternalObject.GetArrayRank();
		public int GetLength(int dimension) =>
			TypedInternalObject.GetArrayLength(dimension);
		public dynamic GetValue(params int[] indexes) =>
			TypedInternalObject.InternalGetIndex(indexes.Select(t => (object)t).ToArray());
		public void SetValue(dynamic value, params int[] indexes) =>
			TypedInternalObject.InternalSetIndex(indexes.Select(t => (object)t).ToArray(), value);

		public GameObjectArrayMD(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}
	}
}
