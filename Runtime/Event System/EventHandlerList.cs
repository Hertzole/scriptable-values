﻿using System;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	internal sealed class EventHandlerList<T> : IDisposable, IEventList
	{
		private bool isDisposed = false;

		private readonly PooledList<EventClosure<T>> events = new PooledList<EventClosure<T>>();

		public int ListenersCount
		{
			get { return events.Count; }
		}

		public void Invoke(in object sender, in T args)
		{
			ReadOnlySpan<EventClosure<T>> span = events.AsSpan();
			for (int i = 0; i < span.Length; i++)
			{
				span[i].Invoke(sender, args);
			}
		}

		~EventHandlerList()
		{
			Dispose();
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);

			ThrowHelper.ThrowIfDisposed(in isDisposed);

			isDisposed = true;

			events.Dispose();
		}

		public ReadOnlySpan<Delegate> GetListeners()
		{
			return EventHelper.GetListeners(events);
		}

		public void AddListener<TDelegate>(TDelegate action, object context = null) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Add(new EventClosure<T>(action, context));
		}

		public void RemoveListener<TDelegate>(TDelegate action) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Remove(new EventClosure<T>(action, null));
		}

		public void ClearListeners()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Clear();
		}
	}
}