#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine.Assertions;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	public abstract class ScriptablePool : RuntimeScriptableObject
	{
		public static readonly PropertyChangingEventArgs countAllChangingEventArgs = new PropertyChangingEventArgs(nameof(CountAll));
		public static readonly PropertyChangedEventArgs countAllChangedEventArgs = new PropertyChangedEventArgs(nameof(CountAll));

		public static readonly PropertyChangingEventArgs countActiveChangingEventArgs = new PropertyChangingEventArgs(nameof(CountActive));
		public static readonly PropertyChangedEventArgs countActiveChangedEventArgs = new PropertyChangedEventArgs(nameof(CountActive));

		public static readonly PropertyChangingEventArgs countInactiveChangingEventArgs = new PropertyChangingEventArgs(nameof(CountInactive));
		public static readonly PropertyChangedEventArgs countInactiveChangedEventArgs = new PropertyChangedEventArgs(nameof(CountInactive));

		/// <summary>
		///     How many total objects that the pool is keeping track of.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int CountAll { get; protected set; }

		/// <summary>
		///     How many objects that are currently active.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int CountActive { get; protected set; }

		/// <summary>
		///     How many objects that are currently inactive.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int CountInactive { get; protected set; }
	}

	/// <summary>
	///     A scriptable object that holds a pool of values.
	/// </summary>
	/// <typeparam name="T">The type of to pool. Must be a class.</typeparam>
	public abstract partial class ScriptablePool<T> : ScriptablePool, IPoolCallbacks<T> where T : class
	{
		internal readonly List<T> activeObjects = new List<T>();
		internal readonly Stack<T> pool = new Stack<T>();

		private readonly DelegateHandlerList<PoolEventArgs<T>, PoolAction, T> onPoolChanged = new DelegateHandlerList<PoolEventArgs<T>, PoolAction, T>();

		private int countAll;
		private int countActive;
		private int countInactive;

		/// <inheritdoc />
		public sealed override int CountAll
		{
			get { return countAll; }
			protected set
			{
				Assert.AreEqual(activeObjects.Count + pool.Count, value,
					$"CountAll should be equal to the sum of active and inactive objects but was {value} instead.");

				SetField(ref countAll, value, countAllChangingEventArgs, countAllChangedEventArgs);
			}
		}

		/// <inheritdoc />
		public sealed override int CountActive
		{
			get { return countActive; }
			protected set
			{
				Assert.AreEqual(activeObjects.Count, value, $"CountActive should be equal to the number of active objects but was {value} instead.");
				SetField(ref countActive, value, countActiveChangingEventArgs, countActiveChangedEventArgs);
			}
		}

		/// <inheritdoc />
		public sealed override int CountInactive
		{
			get { return countInactive; }
			protected set
			{
				Assert.AreEqual(pool.Count, value, $"CountInactive should be equal to the number of inactive objects but was {value} instead.");
				SetField(ref countInactive, value, countInactiveChangingEventArgs, countInactiveChangedEventArgs);
			}
		}

#if UNITY_EDITOR
		internal const int ORDER = ScriptableList<object>.ORDER + 50;
#endif

		public event PoolEventArgs<T> OnPoolChanged
		{
			add { RegisterChangedCallback(value); }
			remove { UnregisterChangedCallback(value); }
		}

		/// <summary>
		///     Returns an object from the pool.
		/// </summary>
		public T Get()
		{
			T? item = null;
			// Objects may be destroyed when switching scenes, so we need to check if they are null.
			// If the returned object is null, just keep going until we find one that isn't.
			// If it's still null, we'll create a new one.
			while (EqualityHelper.IsNull(item))
			{
				if (pool.Count > 0)
				{
					item = pool.Pop();
				}
				else
				{
					item = CreateObject();
					onPoolChanged.Invoke(PoolAction.CreatedObject, item);
				}
			}

			Assert.IsNotNull(item);

			activeObjects.Add(item!);

			onPoolChanged.Invoke(PoolAction.RentedObject, item!);
			OnGetInternal(item!);

			UpdateCounts();

			AddStackTrace();

			return item!;
		}

		/// <summary>
		///     Returns an object to the pool.
		/// </summary>
		/// <param name="item">The item to return.</param>
		public void Release(T item)
		{
			ThrowHelper.ThrowIfNull(item, nameof(item));

			activeObjects.Remove(item);

			OnReturnInternal(item);
			onPoolChanged.Invoke(PoolAction.ReleasedObject, item);

			pool.Push(item);

			UpdateCounts();

			AddStackTrace();
		}

		/// <summary>
		///     Clears the pool.
		/// </summary>
		public void Clear()
		{
			// Destroy all active objects.
			// Go in reverse order so that we don't mess up the list.
			for (int i = activeObjects.Count - 1; i >= 0; i--)
			{
				T item = activeObjects[i];

				DestroyObjectInternal(item);
				activeObjects.RemoveAt(i);
			}

			// Destroy all inactive objects.
			while (pool.TryPop(out T item))
			{
				DestroyObjectInternal(item);
			}

			UpdateCounts();

			// Make sure that the pool is empty.
			Assert.AreEqual(0, CountAll, $"CountAll should be 0 after clearing the pool but was {CountAll}.");

			AddStackTrace();
		}

		private void UpdateCounts()
		{
			CountAll = activeObjects.Count + pool.Count;
			CountActive = activeObjects.Count;
			CountInactive = pool.Count;
		}

		/// <summary>
		///     Internal method for getting objects.
		/// </summary>
		/// <param name="item">The object that was just received from the pool.</param>
		internal virtual void OnGetInternal(T item)
		{
			// If the item implements IPoolable, call OnUnpooled.
			if (item is IPoolable poolable)
			{
				poolable.OnUnpooled();
			}

			// Call the OnGet method.
			OnGet(item);
		}

		internal virtual void OnReturnInternal(T item)
		{
			// If the item implements IPoolable, call OmPooled.
			if (item is IPoolable poolable)
			{
				poolable.OnPooled();
			}

			// Call the OnReturn method.
			OnReturn(item);
		}

		internal virtual void DestroyObjectInternal(T item)
		{
			onPoolChanged.Invoke(PoolAction.DestroyedObject, item);
			DestroyObject(item);
		}

		/// <summary>
		///     Called when a new object needs to be created.
		/// </summary>
		/// <returns></returns>
		protected abstract T CreateObject();

		/// <summary>
		///     Called when an object needs to be destroyed.
		/// </summary>
		/// <param name="item">The object to be destroyed.</param>
		protected abstract void DestroyObject(T item);

		/// <summary>
		///     Called when an object is retrieved from the pool.
		/// </summary>
		/// <param name="item">The object that was retrieved from the pool.</param>
		protected virtual void OnGet(T item) { }

		/// <summary>
		///     Called when an object is returned to the pool.
		/// </summary>
		/// <param name="item">The object that was returned to the pool.</param>
		protected virtual void OnReturn(T item) { }

		/// <inheritdoc />
		public void RegisterChangedCallback(PoolEventArgs<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onPoolChanged.RegisterCallback(callback);
		}

		/// <inheritdoc />
		public void RegisterChangedCallback<TContext>(PoolEventArgsWithContext<T, TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onPoolChanged.RegisterCallback(callback, context);
		}

		/// <inheritdoc />
		public void UnregisterChangedCallback(PoolEventArgs<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onPoolChanged.RemoveCallback(callback);
		}

		/// <inheritdoc />
		public void UnregisterChangedCallback<TContext>(PoolEventArgsWithContext<T, TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onPoolChanged.RemoveCallback(callback);
		}

		protected override void OnStart()
		{
			// Remove any subscribers that are left over from play mode.
			// Don't warn if there are any subscribers left over because we already do that in OnExitPlayMode.
			ClearSubscribers();
		}

		[Conditional("DEBUG")]
		protected void WarnLeftOverSubscribers()
		{
			EventHelper.WarnIfLeftOverSubscribers(onPoolChanged, nameof(OnPoolChanged), this);
		}

		/// <summary>
		///     Removes any subscribers from the event.
		/// </summary>
		/// <param name="warnIfLeftOver">
		///     If true, a warning will be printed in the console if there are any subscribers.
		///     The warning will only be printed in the editor and debug builds.
		/// </param>
		public void ClearSubscribers(bool warnIfLeftOver = false)
		{
#if DEBUG
			if (warnIfLeftOver)
			{
				WarnLeftOverSubscribers();
			}
#endif

			onPoolChanged.Reset();
		}

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			WarnLeftOverSubscribers();

			Clear();
		}

#endif // UNITY_EDITOR

		#region Obsolete
#if UNITY_EDITOR
		/// <summary>
		///     Called when an object is created.
		/// </summary>
		[Obsolete("Use 'OnPoolChanged' or 'RegisterChangedCallback' instead. This will be removed in build.", true)]
#pragma warning disable CS0067 // Event is never used
		public event Action<T>? OnCreateObject;
		/// <summary>
		///     Called when an object is destroyed.
		/// </summary>
		[Obsolete("Use 'OnPoolChanged' or 'RegisterChangedCallback' instead. This will be removed in build.", true)]
		public event Action<T>? OnDestroyObject;
		/// <summary>
		///     Called when an object is retrieved.
		/// </summary>
		[Obsolete("Use 'OnPoolChanged' or 'RegisterChangedCallback' instead. This will be removed in build.", true)]
		public event Action<T>? OnGetObject;
		/// <summary>
		///     Called when an object is put back into the pool.
		/// </summary>
		[Obsolete("Use 'OnPoolChanged' or 'RegisterChangedCallback' instead. This will be removed in build.", true)]
		public event Action<T>? OnReturnObject;
#pragma warning restore CS0067 // Event is never used

		[Obsolete("Use 'Release' instead. This will be removed in build.", true)]
		[ExcludeFromCodeCoverage]
		public void Return(T item)
		{
			throw new NotSupportedException("'Return' is obsolete. Use 'Release' instead.");
		}
#endif
		#endregion
	}
}