#nullable enable

using System;
using System.Collections.Specialized;

namespace Hertzole.ScriptableValues.Tests
{
	public sealed class CollectionEventTracker<T> : BaseEventTracker
	{
		private readonly ScriptableList<T>? list = null;
		private readonly EventType eventType;
		private readonly InvokeCountContext context = new InvokeCountContext();
		private readonly INotifyCollectionChanged? notifyCollectionChanged = null;

		private NotifyCollectionChangedEventArgs? notifyCollectionChangedEventArgs = null;

		public NotifyCollectionChangedEventArgs NotifyCollectionChangedArgs
		{
			get
			{
				if (notifyCollectionChangedEventArgs == null)
				{
					throw new NotSupportedException("This event tracker is not tracking NotifyCollectionChangedEventArgs.");
				}

				return notifyCollectionChangedEventArgs;
			}
		}

		public CollectionChangedArgs<T> CollectionChangedArgs
		{
			get
			{
				if (!context.TryGetArg(ARGS_KEY, out CollectionChangedArgs<T> args) || args == default)
				{
					throw new NotSupportedException("This event tracker is not tracking CollectionChangedArgs.");
				}

				return args;
			}
		}

		public CollectionEventTracker(ScriptableList<T> list, EventType eventType)
		{
			this.list = list;
			this.eventType = eventType;
			context.AddArg("args", default(CollectionChangedArgs<int>));

			switch (eventType)
			{
				case EventType.Event:
					list.OnCollectionChanged += OnCollectionChanged;
					break;
				case EventType.Register:
					list.RegisterChangedListener(OnCollectionChanged);
					break;
				case EventType.RegisterWithContext:
					list.RegisterChangedListener(OnCollectionChangedWithContext, context);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
			}
		}

		public CollectionEventTracker(ScriptableList<T> list, EventType eventType, INotifyCollectionChanged collectionChanged) : this(list, eventType)
		{
			notifyCollectionChanged = collectionChanged;
			notifyCollectionChanged.CollectionChanged += OnCollectionChanged;
		}

		public CollectionEventTracker(INotifyCollectionChanged collectionChanged)
		{
			notifyCollectionChanged = collectionChanged;
			notifyCollectionChanged.CollectionChanged += OnCollectionChanged;
		}

		private const string ARGS_KEY = "args";

		private void OnCollectionChanged(CollectionChangedArgs<T> e)
		{
			context.invokeCount++;
			context.SetArg(ARGS_KEY, e);
		}

		private static void OnCollectionChangedWithContext(CollectionChangedArgs<T> e, InvokeCountContext context)
		{
			context.invokeCount++;
			context.SetArg(ARGS_KEY, e);
		}

		public override bool HasBeenInvoked()
		{
			if (notifyCollectionChangedEventArgs != null)
			{
				return true;
			}

			return context.invokeCount > 0;
		}

		public override void Dispose()
		{
			if (list != null)
			{
				switch (eventType)
				{
					case EventType.Event:
						list.OnCollectionChanged -= OnCollectionChanged;
						break;
					case EventType.Register:
						list.UnregisterChangedListener(OnCollectionChanged);
						break;
					case EventType.RegisterWithContext:
						list.UnregisterChangedListener<InvokeCountContext>(OnCollectionChangedWithContext);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null);
				}
			}

			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= OnCollectionChanged;
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			notifyCollectionChangedEventArgs = e;
		}
	}
}