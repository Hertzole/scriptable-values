#nullable enable

using System;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	internal sealed class EventHandlerList<TValue1, TValue2> : IDisposable, IEventList
	{
		private bool isDisposed = false;

		private readonly PooledList<StructClosure<TValue1, TValue2>> events = new PooledList<StructClosure<TValue1, TValue2>>();

		public int ListenersCount
		{
			get
			{
				ThrowHelper.ThrowIfDisposed(in isDisposed);

				return events.Count;
			}
		}

		public void Invoke(TValue1 value1, TValue2 value2)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			ReadOnlySpan<StructClosure<TValue1, TValue2>> span = events.AsSpan();
			for (int i = 0; i < span.Length; i++)
			{
				span[i].Invoke(value1, value2);
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
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			return EventHelper.GetListeners(events);
		}

		public void AddListener<TDelegate>(TDelegate action, object? context = null) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Add(new StructClosure<TValue1, TValue2>(action, context));
		}

		public void RemoveListener<TDelegate>(TDelegate action) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Remove(new StructClosure<TValue1, TValue2>(action, null));
		}

		public void AddFrom(EventHandlerList<TValue1, TValue2> other)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			other.events.AddFrom(events);
		}

		public void RemoveFrom(EventHandlerList<TValue1, TValue2> other)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.RemoveFrom(other.events);
		}

		public void ClearListeners()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Clear();
		}
	}
}