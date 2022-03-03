using QHackLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker.Core.GameObjects
{
	public interface IGameObjectArray<out T> : IEnumerable<T>
	{
		int Length { get; }
		T this[int index] { get; }
	}
	public class GameObjectArrayEnumerator<T> : IEnumerator<T>
	{
		private int Index = -1;
		public IGameObjectArray<T> Data { get; }
		public T Current => Data[Index];
		object IEnumerator.Current => Current;

		internal GameObjectArrayEnumerator(IGameObjectArray<T> data) => Data = data;
		public bool MoveNext() => ++Index < Data.Length;
		public void Reset() => Index = -1;
		public void Dispose() { GC.SuppressFinalize(this); }
	}
	/// <summary>
	/// For array of ref types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameObjectArray<T> : GameObject, IGameObjectArray<T> where T : GameObject
	{
		public int Length => TypedInternalObject.GetArrayLength();
		public T this[int index]
		{
			get => MakeGameObject<T>(InternalObject[index]);
			set => InternalObject[index] = value.InternalObject;
		}
		public GameObjectArray(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public IEnumerator<T> GetEnumerator() => new GameObjectArrayEnumerator<T>(this);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
	/// <summary>
	/// For fast access to unmanaged types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameObjectArrayV<T> : GameObject, IGameObjectArray<T> where T : unmanaged
	{
		public int Length => TypedInternalObject.GetArrayLength();
		public T this[int index]
		{
			get => (T)InternalObject[index];
			set => InternalObject[index] = value;
		}
		public GameObjectArrayV(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public T[] GetElements(int index, int length)
		{
			nuint addr = TypedInternalObject.Type.GetArrayElementAddress(TypedInternalObject.BaseAddress, new int[] { index });
			var array = new T[length];
			Context.HContext.DataAccess.Read(addr, array, (uint)length);
			return array;
		}
		public T[] GetAllElements()
		{
			nuint addr = TypedInternalObject.Type.GetElementsBase(TypedInternalObject.BaseAddress);
			var array = new T[Length];
			Context.HContext.DataAccess.Read(addr, array, (uint)array.Length);
			return array;
		}

		public IEnumerator<T> GetEnumerator() => new GameObjectArrayEnumerator<T>(this);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
	/// <summary>
	/// For when no GameObject wrapper is implemented.
	/// </summary>
	public class GameObjectArray : GameObject, IEnumerable
	{
		public int Length => TypedInternalObject.GetArrayLength();
		public dynamic this[int index]
		{
			get => InternalObject[index];
			set => InternalObject[index] = value;
		}
		public GameObjectArray(GameContext ctx, HackObject obj) : base(ctx, obj)
		{
		}

		public IEnumerator GetEnumerator() => new GameObjectArrayEnumerator(this);
		private sealed class GameObjectArrayEnumerator : IEnumerator
		{
			public object Current => Data[Index];
			public GameObjectArray Data { get; }
			private int Index = -1;
			public GameObjectArrayEnumerator(GameObjectArray data) => Data = data;
			public bool MoveNext() => ++Index < Data.Length;
			public void Reset() => Index = -1;
		}
	}
}
