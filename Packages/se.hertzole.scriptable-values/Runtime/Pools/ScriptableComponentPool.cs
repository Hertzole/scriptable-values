using System.Collections.Generic;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Pool;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     A <see cref="ScriptableObject" /> that holds a pool of <see cref="Component" />.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Component" />.</typeparam>
    public abstract class ScriptableComponentPool<T> : ScriptablePool<T> where T : Component
    {
        [SerializeField]
        private T prefab = null!;

        public T Prefab
        {
            get { return prefab; }
            set { prefab = value; }
        }

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
            Guard.IsNotNull(item, nameof(item));

            item.gameObject.SetActive(true);

            using PooledObject<List<IPoolable>> scope = ListPool<IPoolable>.Get(out List<IPoolable> poolableBuffer);
            item.GetComponentsInChildren(true, poolableBuffer);
            for (int i = 0; i < poolableBuffer.Count; i++)
            {
                poolableBuffer[i].OnUnpooled();
            }
        }

        protected override void OnReturn(T item)
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