using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableObjectPool<T> : ScriptablePool<T> where T : ScriptableObject
	{
		protected override T CreateObject()
		{
			return CreateInstance<T>();
		}
		
		protected override void DestroyObject(T item)
		{
			Destroy(item);
		}
	}
}