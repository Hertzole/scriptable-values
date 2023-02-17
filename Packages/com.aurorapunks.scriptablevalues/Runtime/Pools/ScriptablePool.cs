using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine.Assertions;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A scriptable object that holds a pool of values.
	/// </summary>
	/// <typeparam name="T">The type of to pool. Must be a class.</typeparam>
	public abstract class ScriptablePool<T> : RuntimeScriptableObject where T : class
	{
		private readonly List<T> activeObjects = new List<T>();
		private readonly Stack<T> pool = new Stack<T>();

		/// <summary>
		///     How many total objects that the pool is keeping track of.
		/// </summary>
		public int CountAll { get { return activeObjects.Count + pool.Count; } }
		/// <summary>
		///     How many objects that are currently active.
		/// </summary>
		public int CountActive { get { return activeObjects.Count; } }
		/// <summary>
		///     How many objects that are currently inactive.
		/// </summary>
		public int CountInactive { get { return pool.Count; } }
#if UNITY_EDITOR
		internal const int ORDER = ScriptableList<object>.ORDER + 50;
#endif

		/// <summary>
		///     Called when an object is created.
		/// </summary>
		public event Action<T> OnCreateObject;
		/// <summary>
		///     Called when an object is destroyed.
		/// </summary>
		public event Action<T> OnDestroyObject;
		/// <summary>
		///     Called when an object is retrieved.
		/// </summary>
		public event Action<T> OnGetObject;
		/// <summary>
		///     Called when an object is put back into the pool.
		/// </summary>
		public event Action<T> OnReturnObject;

		/// <summary>
		///     Returns an object from the pool.
		/// </summary>
		public T Get()
		{
			T item = null;
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
					OnCreateObject?.Invoke(item);
				}
			}

			activeObjects.Add(item);

			OnGetInternal(item);
			OnGetObject?.Invoke(item);

			AddStackTrace();

			return item;
		}

		/// <summary>
		///     Returns an object to the pool.
		/// </summary>
		/// <param name="item">The item to return.</param>
		public void Return(T item)
		{
			activeObjects.Remove(item);

			OnReturnInternal(item);
			OnReturnObject?.Invoke(item);

			pool.Push(item);

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

				OnDestroyObject?.Invoke(item);
				DestroyObject(item);
				activeObjects.RemoveAt(i);
			}

			// Destroy all inactive objects.
			while (pool.TryPop(out T item))
			{
				OnDestroyObject?.Invoke(item);
				DestroyObject(item);
			}

			// Make sure that the pool is empty.
			Assert.AreEqual(0, CountAll, $"CountAll should be 0 after clearing the pool but was {CountAll}.");

			AddStackTrace();
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

		protected override void OnStart()
		{
			// Remove any subscribers that are left over from play mode.
			// Don't warn if there are any subscribers left over because we already do that in OnExitPlayMode.
			ClearSubscribers();
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
				EventHelper.WarnIfLeftOverSubscribers(OnCreateObject, nameof(OnCreateObject), this);
				EventHelper.WarnIfLeftOverSubscribers(OnDestroyObject, nameof(OnDestroyObject), this);
				EventHelper.WarnIfLeftOverSubscribers(OnGetObject, nameof(OnGetObject), this);
				EventHelper.WarnIfLeftOverSubscribers(OnReturnObject, nameof(OnReturnObject), this);
			}
#endif

			OnCreateObject = null;
			OnDestroyObject = null;
			OnGetObject = null;
			OnReturnObject = null;
		}

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			EventHelper.WarnIfLeftOverSubscribers(OnCreateObject, nameof(OnCreateObject), this);
			EventHelper.WarnIfLeftOverSubscribers(OnDestroyObject, nameof(OnDestroyObject), this);
			EventHelper.WarnIfLeftOverSubscribers(OnGetObject, nameof(OnGetObject), this);
			EventHelper.WarnIfLeftOverSubscribers(OnReturnObject, nameof(OnReturnObject), this);

			Clear();
		}
#endif
	}
}