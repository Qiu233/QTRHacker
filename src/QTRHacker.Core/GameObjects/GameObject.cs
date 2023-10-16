using Newtonsoft.Json;
using QHackLib;
using System;

namespace QTRHacker.Core.GameObjects;

/// <summary>
/// Represents a ref object on heap including boxed values.
/// </summary>
public class GameObject : IEquatable<GameObject>
{
	[JsonIgnore]
	public dynamic InternalObject { get; }

	[JsonIgnore]
	public HackObject TypedInternalObject => InternalObject;

	[JsonIgnore]
	public GameContext Context { get; }

	[JsonIgnore]
	public nuint BaseAddress => InternalObject.BaseAddress;


	public GameObject(GameContext context, HackObject obj)
	{
		Context = context;
		InternalObject = obj;
	}

	public T MakeGameObject<T>(HackObject obj) where T : GameObject
	{
		if (obj is null)
			return null;
		return typeof(T).GetConstructor(new Type[] { typeof(GameContext), typeof(HackObject) })
			   .Invoke(new object[] { Context, obj }) as T;
	}

	public static bool operator ==(GameObject a, GameObject b)
	{
		if (a is null)
			return b is null;
		return a.BaseAddress == b.BaseAddress;
	}
	public static bool operator !=(GameObject a, GameObject b)
	{
		return !(a == b);
	}

	bool IEquatable<GameObject>.Equals(GameObject other)
	{
		return this == other;
	}

	public override bool Equals(object obj)
	{
		if (obj is not GameObject o)
			return false;
		return o == this;
	}

	public override int GetHashCode()
	{
		return BaseAddress.GetHashCode();
	}
}
