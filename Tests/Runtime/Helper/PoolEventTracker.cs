#nullable enable

using System;

namespace Hertzole.ScriptableValues.Tests
{
	public sealed class PoolEventTracker<T> : BaseEventTracker
	{
		private readonly IPoolCallbacks<T> pool;
		private readonly InvokeCountContext context = new InvokeCountContext();

		public PoolAction LastAction
		{
			get { return context.GetArg<PoolAction>("action"); }
		}

		public T LastItem
		{
			get { return context.GetArg<T>("item"); }
		}

		/// <inheritdoc />
		public PoolEventTracker(IPoolCallbacks<T> pool, Predicate<PoolAction>? actionPredicate = null)
		{
			this.pool = pool;

			if (actionPredicate != null)
			{
				context.AddArg("actionPredicate", actionPredicate);
			}

			pool.OnPoolChanged += OnPoolChanged;
		}

		private void OnPoolChanged(PoolAction action, T item)
		{
			if (context.TryGetArg<Predicate<PoolAction>>("actionPredicate", out Predicate<PoolAction>? actionPredicate) && !actionPredicate(action))
			{
				return;
			}

			context.invokeCount++;
			context.SetArg("action", action);
			context.SetArg("item", item);
		}

		/// <inheritdoc />
		public override bool HasBeenInvoked()
		{
			return context.invokeCount > 0;
		}

		public override void Dispose()
		{
			pool.OnPoolChanged -= OnPoolChanged;
		}
	}
}