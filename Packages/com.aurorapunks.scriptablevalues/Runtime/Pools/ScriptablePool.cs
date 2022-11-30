using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine.Assertions;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptablePool<T> : RuntimeScriptableObject where T : class
	{
		private readonly List<T> activeObjects = new List<T>();
		private readonly Stack<T> pool = new Stack<T>();

		public int CountAll { get; private set; }
		public int CountActive { get { return activeObjects.Count; } }
		public int CountInactive { get { return pool.Count; } }

		public event Action<T> OnCreateObject;
		public event Action<T> OnDestroyObject;
		public event Action<T> OnGetObject;
		public event Action<T> OnReturnObject;

		public T Get()
		{
			AddStackTrace();

			T item = null;
			// Objects may be destroyed when switching scenes, so we need to check if they are null.
			// If the returned object is null, just keep going until we find one that isn't.
			// If it's still null, we'll create a new one.
			while (EqualityHelper.IsNull(item))
			{
				if (pool.Count > 0)
				{
					item = pool.Pop();
					if (EqualityHelper.IsNull(item))
					{
						// The item was null and we need to keep track of all of the objects we've created.
						// Remove one as it's no longer alive.
						CountAll--;
					}
				}
				else
				{
					item = CreateObject();
					CountAll++;
					OnCreateObject?.Invoke(item);
				}
			}

			activeObjects.Add(item);

			OnGetInternal(item);
			OnGetObject?.Invoke(item);

			return item;
		}

		public void Return(T item)
		{
			AddStackTrace();

			activeObjects.Remove(item);

			OnReturnInternal(item);
			OnReturnObject?.Invoke(item);

			pool.Push(item);
		}

		public void Clear()
		{
			AddStackTrace();

			foreach (T activeObject in activeObjects)
			{
				OnDestroyObject?.Invoke(activeObject);
				DestroyObject(activeObject);
				CountAll--;
			}

			activeObjects.Clear();

			foreach (T inactiveObject in pool)
			{
				OnDestroyObject?.Invoke(inactiveObject);
				DestroyObject(inactiveObject);
				CountAll--;
			}

			pool.Clear();
			Assert.AreEqual(0, CountAll, $"CountAll should be 0 after clearing the pool but was {CountAll}.");
		}

		internal virtual void OnGetInternal(T item)
		{
			if (item is IPoolable poolable)
			{
				poolable.OnUnpooled();
			}

			OnGet(item);
		}

		internal virtual void OnReturnInternal(T item)
		{
			if (item is IPoolable poolable)
			{
				poolable.OnPooled();
			}

			OnReturn(item);
		}

		protected abstract T CreateObject();

		protected abstract void DestroyObject(T item);

		protected virtual void OnGet(T item) { }

		protected virtual void OnReturn(T item) { }

		public override void ResetValues()
		{
			CountAll = 0;
		}

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			Clear();
			CountAll = 0;
		}
#endif
	}
}