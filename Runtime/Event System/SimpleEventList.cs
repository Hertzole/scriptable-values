using System;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	internal sealed class SimpleEventList : IDisposable, IEventList
	{
		private bool isDisposed = false;

		private readonly PooledList<EventClosure<object>> events = new PooledList<EventClosure<object>>();

		public int ListenersCount
		{
			get { return events.Count; }
		}

		public void Invoke(in object sender)
		{
			var span = events.AsSpan();
			for (int i = 0; i < span.Length; i++)
			{
				span[i].Invoke(sender, null);
			}
		}

		public void Dispose()
		{
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

			events.Add(new EventClosure<object>(action, context));
		}

		public void RemoveListener<TDelegate>(TDelegate action) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Remove(new EventClosure<object>(action, null));
		}

		public void ClearListeners()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			events.Clear();
		}
	}
}