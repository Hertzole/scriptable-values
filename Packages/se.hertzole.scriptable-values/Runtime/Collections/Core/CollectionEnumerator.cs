#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine.Pool;

namespace Hertzole.ScriptableValues
{
    /// <summary>
    ///     Internal reusable collection enumerator for collections.
    /// </summary>
    /// <typeparam name="T">The type of hte items in the collection.</typeparam>
    internal sealed class CollectionEnumerator<T> : ICollection<T>, IDisposable
    {
        private T[]? currentArray;

        internal static readonly ObjectPool<CollectionEnumerator<T>> enumeratorPool =
            new ObjectPool<CollectionEnumerator<T>>(static () => new CollectionEnumerator<T>());

        public int Count { get; private set; }
        public bool IsReadOnly
        {
            get { return true; }
        }

        public void SetArray(T[] newArray, int newCount)
        {
            currentArray = newArray;
            Count = newCount;
        }

        public void Dispose()
        {
            currentArray = null;
            Count = 0;
            enumeratorPool.Release(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (currentArray == null || Count == 0)
            {
                yield break;
            }

            for (int i = 0; i < Count; i++)
            {
                yield return currentArray[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(T item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ThrowHelper.ThrowIfNull(currentArray, nameof(currentArray));

            Array.Copy(currentArray, 0, array, arrayIndex, Count);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
    }
}