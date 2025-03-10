#nullable enable

using System;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	[Obsolete("Use DelegateHandlerList instead.")]
	internal sealed class EventHandlerList<TValue1, TValue2> : IDisposable, IEventList
	{
		private bool isDisposed = false;

		public int ListenersCount
		{
			get
			{
				ThrowHelper.ThrowIfDisposed(in isDisposed);

				return 0;
			}
		}

		public void Invoke(TValue1 value1, TValue2 value2)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
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
		}

		public ReadOnlySpan<Delegate> GetListeners()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);

			return ReadOnlySpan<Delegate>.Empty;
		}

		public void AddListener<TDelegate>(TDelegate action, object? context = null) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
		}

		public void RemoveListener<TDelegate>(TDelegate action) where TDelegate : Delegate
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
		}

		public void AddFrom(EventHandlerList<TValue1, TValue2> other)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
		}

		public void RemoveFrom(EventHandlerList<TValue1, TValue2> other)
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
		}

		public void ClearListeners()
		{
			ThrowHelper.ThrowIfDisposed(in isDisposed);
		}
	}
}