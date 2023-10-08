﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTRHacker;

public sealed class WeakEventListener<TInstance, TSource, TEventArgs>
		where TInstance : class
{
	private readonly WeakReference _weakInstance;
	public WeakEventListener(TInstance instance)
	{
		if (instance == null)
		{
			throw new ArgumentNullException(nameof(instance));
		}

		_weakInstance = new WeakReference(instance);
	}

	public Action<TInstance, TSource?, TEventArgs>? OnEventAction { get; set; }
	public Action<WeakEventListener<TInstance, TSource, TEventArgs>>? OnDetachAction { get; set; }

	public void OnEvent(TSource? source, TEventArgs eventArgs)
	{
		var target = (TInstance)_weakInstance.Target!;
		if (target != null)
		{
			OnEventAction?.Invoke(target, source, eventArgs);
		}
		else
		{
			Detach();
		}
	}
	public void Detach()
	{
		OnDetachAction?.Invoke(this);
		OnDetachAction = null;
	}
}