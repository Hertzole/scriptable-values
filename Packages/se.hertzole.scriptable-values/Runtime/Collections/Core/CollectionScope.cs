#nullable enable

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine.Pool;

namespace Hertzole.ScriptableValues
{
	internal struct CollectionScope<T> : IDisposable
	{
		private T[]? array;
		private int length;

		private static readonly ObjectPool<Enumerator> enumeratorPool =
			new ObjectPool<Enumerator>(static () => new Enumerator());

		public ReadOnlySpan<T> Span
		{
			get { return array == null ? ReadOnlySpan<T>.Empty : new ReadOnlySpan<T>(array, 0, length); }
		}

		public CollectionScope(T item)
		{
			array = ArrayPool<T>.Shared.Rent(1);
			array[0] = item;
			length = 1;
		}

		public CollectionScope(IEnumerable<T> items)
		{
			if (items.TryGetNonEnumeratedCount(out int count))
			{
				T[]? tempArray = ArrayPool<T>.Shared.Rent(count);

				if (items is ICollection<T> collection)
				{
					collection.CopyTo(tempArray, 0);
				}
				else
				{
					int i = 0;
					foreach (T item in items)
					{
						tempArray[i++] = item;
					}
				}

				array = tempArray;
				length = count;
			}
			else
			{
				T[]? tempArray = ArrayPool<T>.Shared.Rent(4);

				int i = 0;
				foreach (T item in items)
				{
					ArrayHelpers.EnsureCapacity(ref tempArray, i);
					tempArray[i++] = item;
				}

				array = tempArray;
				length = i;
			}
		}

		public Enumerator GetEnumerator()
		{
			ThrowHelper.ThrowIfNull(array, nameof(array));

			Enumerator enumerator = enumeratorPool.Get();
			enumerator.SetArray(array!, length);
			return enumerator;
		}

		public void Dispose()
		{
			if (array != null)
			{
				ArrayPool<T>.Shared.Return(array, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
				array = null;
				length = 0;
			}
		}

		public static CollectionScope<T> FromListSlice(IReadOnlyList<T> list, int index, int count)
		{
			T[]? array = ArrayPool<T>.Shared.Rent(count);
			for (int i = 0; i < count; i++)
			{
				array[i] = list[index + i];
			}

			return new CollectionScope<T>
			{
				array = array,
				length = count
			};
		}

		public class Enumerator : ICollection<T>, IDisposable
		{
			private T[]? array;

			public int Count { get; private set; }
			public bool IsReadOnly
			{
				get { return true; }
			}

			public void SetArray(T[] newArray, int newCount)
			{
				array = newArray;
				Count = newCount;
			}

			public void Dispose()
			{
				array = null;
				Count = 0;
				enumeratorPool.Release(this);
			}

			public IEnumerator<T> GetEnumerator()
			{
				if (array == null || Count == 0)
				{
					yield break;
				}

				for (int i = 0; i < Count; i++)
				{
					yield return array[i];
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

			public void CopyTo(T[] destination, int arrayIndex)
			{
				ThrowHelper.ThrowIfNull(array, nameof(array));

				Array.Copy(array, 0, destination, arrayIndex, Count);
			}

			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}
		}
	}
}