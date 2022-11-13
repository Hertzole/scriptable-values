using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableComponentPool<T> : ScriptablePool<T> where T : Component 
	{
		[SerializeField] 
		private T prefab = default;

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

		protected override void OnGet(T item)
		{
			item.gameObject.SetActive(true);
		}
		
		protected override void OnReturn(T item)
		{
			item.gameObject.SetActive(false);
		}
	}
}