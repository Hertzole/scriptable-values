using System.Collections.Generic;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableComponentPool<T> : ScriptablePool<T> where T : Component
	{
		[SerializeField]
		private T prefab = default;

		private readonly List<IPoolable> poolableBuffer = new List<IPoolable>(10);

		protected override T CreateObject()
		{
			return Instantiate(prefab);
		}

		protected override void DestroyObject(T item)
		{
			// The object may already have been destroyed. Check if it exists first.
			if (item != null)
			{
				Destroy(item.gameObject);
			}
		}

		internal override void OnGetInternal(T item)
		{
			// Don't check for poolable here and it's done with components in OnGet.
			OnGet(item);
		}
		
		internal override void OnReturnInternal(T item)
		{
			// Don't check for poolable here and it's done with components in OnReturn.
			OnReturn(item);
		}

		protected override void OnGet(T item)
		{
			item.gameObject.SetActive(true);

			poolableBuffer.Clear();
			item.GetComponentsInChildren(true, poolableBuffer);
			for (int i = 0; i < poolableBuffer.Count; i++)
			{
				poolableBuffer[i].OnUnpooled();
			}
		}

		protected override void OnReturn(T item)
		{
			poolableBuffer.Clear();
			item.GetComponentsInChildren(true, poolableBuffer);

			for (int i = 0; i < poolableBuffer.Count; i++)
			{
				poolableBuffer[i].OnPooled();
			}

			item.gameObject.SetActive(false);
		}
	}
}