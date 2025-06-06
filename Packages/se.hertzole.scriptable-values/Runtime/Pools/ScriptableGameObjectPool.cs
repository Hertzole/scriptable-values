﻿using System.Collections.Generic;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Pool;

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A <see cref="ScriptableObject"/> that holds a pool of <see cref="GameObject" />.
	/// </summary>
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Game Object Pool", menuName = "Hertzole/Scriptable Values/Pools/Game Object Pool", order = ORDER)]
#endif
	public class ScriptableGameObjectPool : ScriptablePool<GameObject>
	{
		[SerializeField]
		private GameObject prefab = default;

		public GameObject Prefab
		{
			get { return prefab; }
			set { prefab = value; }
		}

		protected override GameObject CreateObject()
		{
			return Instantiate(prefab);
		}

		protected override void DestroyObject(GameObject item)
		{
			// The object may already have been destroyed. Check if it exists first.
			if (item != null)
			{
				Destroy(item.gameObject);
			}
		}

		internal override void OnGetInternal(GameObject item)
		{
			// Don't check for poolable here as it's done with components in OnGet.
			OnGet(item);
		}

		internal override void OnReturnInternal(GameObject item)
		{
			// Don't check for poolable here as it's done with components in OnReturn.
			OnReturn(item);
		}

		protected override void OnGet(GameObject item)
		{
			ThrowHelper.ThrowIfNull(item, nameof(item));

			item.gameObject.SetActive(true);

			using PooledObject<List<IPoolable>> scope = ListPool<IPoolable>.Get(out List<IPoolable> poolableBuffer);
			item.GetComponentsInChildren(true, poolableBuffer);
			for (int i = 0; i < poolableBuffer.Count; i++)
			{
				poolableBuffer[i].OnUnpooled();
			}
		}

		protected override void OnReturn(GameObject item)
		{
			using PooledObject<List<IPoolable>> scope = ListPool<IPoolable>.Get(out List<IPoolable> poolableBuffer);
			item.GetComponentsInChildren(true, poolableBuffer);

			for (int i = 0; i < poolableBuffer.Count; i++)
			{
				poolableBuffer[i].OnPooled();
			}

			item.gameObject.SetActive(false);
		}
	}
}