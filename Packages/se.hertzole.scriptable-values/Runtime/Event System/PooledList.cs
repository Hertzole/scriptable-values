using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Hertzole.ScriptableValues
{
	internal sealed class PooledList<T> : IDisposable
	{
		private T[] items = Array.Empty<T>();

		public int Count { get; private set; }

		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= Count)
				{
					throw new IndexOutOfRangeException();
				}

				return items[index];
			}
		}

		public void Add(T item)
		{
			ResizeIfNeeded(Count + 1);

			items[Count++] = item;
		}

		public void AddFrom(PooledList<T> other)
		{
			if (other.Count == 0)
			{
				return;
			}

			ResizeIfNeeded(Count + other.Count);

			Array.Copy(other.items, 0, items, Count, other.Count);

			Count += other.Count;
		}

		public bool Remove(T item)
		{
			int index = IndexOf(item);

			if (index != -1)
			{
				RemoveAt(index);
			}

			return index != -1;
		}

		public void RemoveFrom(PooledList<T> other)
		{
			if (other.Count == 0 || Count == 0)
			{
				return;
			}

			for (int i = 0; i < other.Count; i++)
			{
				if (Count == 0)
				{
					break;
				}

				Remove(other[i]);
			}
		}

		public void Clear()
		{
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() && Count > 0)
			{
				Array.Clear(items, 0, Count);
			}

			Count = 0;
		}

		public int IndexOf(T item)
		{
			if (items.Length == 0)
			{
				return -1;
			}

			return Array.IndexOf(items, item, 0, Count);
		}

		private void RemoveAt(int index)
		{
			Count--;

			if (index < Count)
			{
				Array.Copy(items, index + 1, items, index, Count - index);
			}

			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				items[Count] = default!;
			}
		}

		public ReadOnlySpan<T> AsSpan()
		{
			return items.AsSpan(0, Count);
		}

		private void ResizeIfNeeded(int newCount)
		{
			if (items.Length < newCount)
			{
				T[] newItems = ArrayPool<T>.Shared.Rent(newCount);

				if (items.Length > 0)
				{
					items.CopyTo(newItems, 0);
					ArrayPool<T>.Shared.Return(items, true);
				}

				items = newItems;
			}
		}

		public void Dispose()
		{
			if (items.Length > 0)
			{
				ArrayPool<T>.Shared.Return(items, true);
			}
		}
	}
}