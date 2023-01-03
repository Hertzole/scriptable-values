using System;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine.Assertions;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptablePool<T> : RuntimeScriptableObject where T : class
	{
#if UNITY_EDITOR
		internal const int ORDER = ScriptableList<object>.ORDER + 50;
#endif
		
		private readonly List<T> activeObjects = new List<T>();
		private readonly Stack<T> pool = new Stack<T>();

		public int CountAll { get { return activeObjects.Count + pool.Count; } }
		public int CountActive { get { return activeObjects.Count; } }
		public int CountInactive { get { return pool.Count; } }

		public event Action<T> OnCreateObject;
		public event Action<T> OnDestroyObject;
		public event Action<T> OnGetObject;
		public event Action<T> OnReturnObject;

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
					if (EqualityHelper.IsNull(item))
					{
						// The item was null and we should just move onto the next one.
						continue;
					}
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

		public void Return(T item)
		{
			activeObjects.Remove(item);

			OnReturnInternal(item);
			OnReturnObject?.Invoke(item);

			pool.Push(item);
		
			AddStackTrace();
		}

		public void Clear()
		{
			for (int i = activeObjects.Count - 1; i >= 0; i--)
			{
				T item = activeObjects[i];
				
				OnDestroyObject?.Invoke(item);
				DestroyObject(item);
				activeObjects.RemoveAt(i);
			}

			while (pool.TryPop(out T item))
			{
				OnDestroyObject?.Invoke(item);
				DestroyObject(item);
			}

			Assert.AreEqual(0, CountAll, $"CountAll should be 0 after clearing the pool but was {CountAll}.");
			
			AddStackTrace();
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

#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			Clear();
		}
#endif
	}
}