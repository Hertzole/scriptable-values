#nullable enable

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hertzole.ScriptableValues.Helpers;

namespace Hertzole.ScriptableValues
{
	internal struct CollectionScope<T> : IDisposable, IEquatable<CollectionScope<T>>
	{
		private T[]? array;

		public int Length { get; private set; }

		public ReadOnlySpan<T> Span
		{
			get { return array == null ? ReadOnlySpan<T>.Empty : new ReadOnlySpan<T>(array, 0, Length); }
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
				Length = count;
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
				Length = i;
			}
		}

		/// <inheritdoc />
		public bool Equals(CollectionScope<T> other)
		{
			return Length == other.Length && ArrayHelpers.SequenceEquals(Span, other.Span);
		}

		/// <inheritdoc />
		public override bool Equals(object? obj)
		{
			return obj is CollectionScope<T> other && Equals(other);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			if (array == null)
			{
				return 0;
			}

			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			unchecked
			{
				int hash = 17;

				for (int i = 0; i < Length; i++)
				{
					hash = hash * 31 + comparer.GetHashCode(array[i]);
				}

				return hash;
			}
		}

		public CollectionEnumerator<T> GetEnumerator()
		{
			ThrowHelper.ThrowIfNull(array, nameof(array));

			CollectionEnumerator<T> enumerator = CollectionEnumerator<T>.enumeratorPool.Get();
			enumerator.SetArray(array!, Length);
			return enumerator;
		}

		public void Dispose()
		{
			if (array != null)
			{
				ArrayPool<T>.Shared.Return(array, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
				array = null;
				Length = 0;
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
				Length = count
			};
		}

		public static bool operator ==(CollectionScope<T> left, CollectionScope<T> right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(CollectionScope<T> left, CollectionScope<T> right)
		{
			return !left.Equals(right);
		}
	}
}