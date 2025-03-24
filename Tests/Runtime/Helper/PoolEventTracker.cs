#nullable enable

using System;

namespace Hertzole.ScriptableValues.Tests
{
	public sealed class PoolEventTracker<T> : BaseEventTracker
	{
		private readonly IPoolCallbacks<T> pool;
		private readonly EventType eventType;
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
		public PoolEventTracker(IPoolCallbacks<T> pool, EventType eventType, Predicate<PoolAction>? actionPredicate = null)
		{
			this.pool = pool;
			this.eventType = eventType;

			if (actionPredicate != null)
			{
				context.AddArg("actionPredicate", actionPredicate);
			}

			switch (eventType)
			{
				case EventType.Event:
					pool.OnPoolChanged += OnPoolChanged;
					break;
				case EventType.Register:
					pool.RegisterChangedCallback(OnPoolChanged);
					break;
				case EventType.RegisterWithContext:
					pool.RegisterChangedCallback(OnPoolChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}
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

		private static void OnPoolChangedWithContext(PoolAction action, T item, InvokeCountContext context)
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
			switch (eventType)
			{
				case EventType.Event:
					pool.OnPoolChanged -= OnPoolChanged;
					break;
				case EventType.Register:
					pool.UnregisterChangedCallback(OnPoolChanged);
					break;
				case EventType.RegisterWithContext:
					pool.UnregisterChangedCallback<InvokeCountContext>(OnPoolChangedWithContext);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}
		}
	}
}