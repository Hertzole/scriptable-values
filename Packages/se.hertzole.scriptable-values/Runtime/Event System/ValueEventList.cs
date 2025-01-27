#nullable enable

using System;
using System.Buffers;

namespace Hertzole.ScriptableValues
{
	internal sealed class ValueEventList<TValue> : IDisposable, IEventList
	{
		private bool isDisposed = false;

		private readonly PooledList<ValueClosure<TValue>> events = new PooledList<ValueClosure<TValue>>();

		public int ListenersCount
		{
			get { return events.Count; }
		}

		public ReadOnlySpan<Delegate> GetListeners()
		{
			Delegate[]? listeners = ArrayPool<Delegate>.Shared.Rent(events.Count);

			try
			{
				for (int i = 0; i < events.Count; i++)
				{
					listeners[i] = events[i].action;
				}

				return new ReadOnlySpan<Delegate>(listeners, 0, events.Count);
			}
			finally
			{
				ArrayPool<Delegate>.Shared.Return(listeners);
			}
		}

		public void AddListener<TDelegate>(TDelegate action, object? context = null) where TDelegate : Delegate
		{
			ThrowIfDisposed();

			events.Add(new ValueClosure<TValue>(action, context));
		}

		public void RemoveListener<TDelegate>(TDelegate action) where TDelegate : Delegate
		{
			ThrowIfDisposed();

			events.Remove(new ValueClosure<TValue>(action, null));
		}

		public void ClearListeners()
		{
			ThrowIfDisposed();

			events.Clear();
		}

		public void Invoke(TValue oldValue, TValue newValue)
		{
			ThrowIfDisposed();

			ReadOnlySpan<ValueClosure<TValue>> span = events.AsSpan();
			for (int i = 0; i < span.Length; i++)
			{
				span[i].Invoke(oldValue, newValue);
			}
		}

		public void AddFrom(ValueEventList<TValue> other)
		{
			ThrowIfDisposed();

			other.events.AddFrom(events);
		}

		public void RemoveFrom(ValueEventList<TValue> other)
		{
			ThrowIfDisposed();

			events.RemoveFrom(other.events);
		}

		public void Dispose()
		{
			ThrowIfDisposed();

			events.Dispose();

			isDisposed = true;
		}

		private void ThrowIfDisposed()
		{
			if (isDisposed)
			{
				throw new ObjectDisposedException("Event list has been disposed.");
			}
		}
	}
}