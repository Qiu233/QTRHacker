using QHackLib;
using System;

namespace QTRHacker.Core.GameObjects;

public interface IGameObjectArrayMD<T>
{
	int Length { get; }
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
	public int Length =>
		TypedInternalObject.GetArrayLength();
	public int Rank =>
		TypedInternalObject.GetArrayRank();
	public int GetLength(int dimension) =>
		TypedInternalObject.GetArrayLength(dimension);
	public T GetValue(params int[] indexes) =>
		MakeGameObject<T>(TypedInternalObject.InternalGetIndex(indexes) as HackObject);
	public void SetValue(T value, params int[] indexes) =>
		TypedInternalObject.InternalSetIndex(indexes, value.InternalObject);

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
	public int Length =>
		TypedInternalObject.GetArrayLength();
	public int Rank =>
		TypedInternalObject.GetArrayRank();
	public int GetLength(int dimension) =>
		TypedInternalObject.GetArrayLength(dimension);
	public T GetValue(params int[] indexes) =>
		(T)(dynamic)TypedInternalObject.InternalGetIndex(indexes);
	public void SetValue(T value, params int[] indexes) =>
		TypedInternalObject.InternalSetIndex(indexes, value);
	public GameObjectArrayMDV(GameContext ctx, HackObject obj) : base(ctx, obj)
	{
	}
	public T[] GetElements(int[] indices, int length)
	{
		nuint addr = TypedInternalObject.Type.GetArrayElementAddress(TypedInternalObject.BaseAddress, indices);
		var array = new T[length];
		Context.HContext.DataAccess.Read(addr, array.AsSpan());
		return array;
	}
	public T[] GetAllElements()
	{
		if (TypedInternalObject.GetArrayLength() == 0)
			return Array.Empty<T>();
		nuint addr = TypedInternalObject.Type.GetElementsBase(TypedInternalObject.BaseAddress);
		var array = new T[Length];
		Context.HContext.DataAccess.Read(addr, array.AsSpan());
		return array;
	}
}
public class GameObjectArrayMD : GameObject
{
	public int Length =>
		TypedInternalObject.GetArrayLength();
	public int Rank =>
		TypedInternalObject.GetArrayRank();
	public int GetLength(int dimension) =>
		TypedInternalObject.GetArrayLength(dimension);
	public dynamic GetValue(params int[] indexes) =>
		TypedInternalObject.InternalGetIndex(indexes);
	public void SetValue(dynamic value, params int[] indexes) =>
		TypedInternalObject.InternalSetIndex(indexes, value);

	public GameObjectArrayMD(GameContext ctx, HackObject obj) : base(ctx, obj)
	{
	}
}
