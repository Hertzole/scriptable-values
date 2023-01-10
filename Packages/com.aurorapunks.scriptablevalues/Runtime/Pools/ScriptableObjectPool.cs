using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A scriptable object that holds a pool of types inherited from <see cref="ScriptableObject" />.
	/// </summary>
	/// <typeparam name="T">The type of <see cref="ScriptableObject" />.</typeparam>
	public abstract class ScriptableObjectPool<T> : ScriptablePool<T> where T : ScriptableObject
	{
		protected override T CreateObject()
		{
			// Create a new instance of the scriptable object.
			return CreateInstance<T>();
		}

		protected override void DestroyObject(T item)
		{
			// Destroy the scriptable object.
			Destroy(item);
		}
	}
}