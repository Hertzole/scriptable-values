using System.Collections.Generic;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptablePool<T> : RuntimeScriptableObject where T : class
	{
		private readonly List<T> activeObjects = new List<T>();
		private readonly Stack<T> pool = new Stack<T>();

		public int CountAll { get; private set; }
		public int CountActive { get { return CountAll - CountInactive; } }
		public int CountInactive { get { return pool.Count; } }

		public T Get()
		{
			AddStackTrace();

			T item = null;
			// Objects may be destroyed when switching scenes, so we need to check if they are null.
			// If the returned object is null, just keep going until we find one that isn't.
			// If it's still null, we'll create a new one.
			while (item == null)
			{
				item = pool.Count > 0 ? pool.Pop() : CreateObject();
			}

			activeObjects.Add(item);

			OnGet(item);

			return item;
		}

		public void Return(T item)
		{
			AddStackTrace();

			activeObjects.Remove(item);

			OnReturn(item);

			pool.Push(item);
		}

		public void Clear()
		{
			AddStackTrace();

			foreach (T activeObject in activeObjects)
			{
				DestroyObject(activeObject);
			}

			activeObjects.Clear();

			foreach (T inactiveObject in pool)
			{
				DestroyObject(inactiveObject);
			}

			pool.Clear();
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