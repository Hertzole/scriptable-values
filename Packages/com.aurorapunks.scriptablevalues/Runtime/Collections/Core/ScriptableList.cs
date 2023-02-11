using System;
using System.Collections;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     A scriptable object that holds a list.
	/// </summary>
	/// <typeparam name="T">The type in the list.</typeparam>
	public abstract class ScriptableList<T> : RuntimeScriptableObject, IList<T>, IReadOnlyList<T>, IList
	{
		[SerializeField]
		[Tooltip("If read only, the list cannot be changed at runtime and won't be cleared on start.")]
		private bool isReadOnly = false;
		[SerializeField]
		[Tooltip("If true, an equality check will be run before setting an item through the indexer to make sure the new object is not the same as the old one.")]
		private bool setEqualityCheck = true;
		[SerializeField]
		[Tooltip("If true, the list will be cleared on play mode start/game boot.")]
		private bool clearOnStart = true;
		[SerializeField]
		internal List<T> list = new List<T>();

		public T this[int index] { get { return list[index]; } set { SetValue(index, value); } }

		object IList.this[int index]
		{
			get { return list[index]; }
			set
			{
				if (EqualityHelper.IsSameType(value, out T newValue))
				{
					SetValue(index, newValue);
				}
			}
		}

		/// <summary>
		///     Gets the total number of elements the internal data structure can hold without resizing.
		/// </summary>
		public int Capacity { get { return list.Capacity; } }

		/// <summary>
		///     If true, an equality check will be run before setting an item through the indexer to make sure the new object is
		///     not the same as the old one.
		/// </summary>
		public bool SetEqualityCheck { get { return setEqualityCheck; } set { setEqualityCheck = value; } }
		/// <summary>
		///     If true, the list will be cleared on play mode start/game boot.
		/// </summary>
		public bool ClearOnStart { get { return clearOnStart; } set { clearOnStart = value; } }

		// Is this List synchronized (thread-safe)?
		bool ICollection.IsSynchronized { get { return false; } }
		// Synchronization root for this object.
		object ICollection.SyncRoot { get { return this; } }

		bool IList.IsFixedSize { get { return isReadOnly; } }
		/// <summary>
		///     If read only, the list cannot be changed at runtime and won't be cleared on start.
		/// </summary>
		public bool IsReadOnly { get { return isReadOnly; } set { isReadOnly = value; } }
		/// <summary>
		///     Gets the number of elements contained in the list.
		/// </summary>
		public int Count { get { return list.Count; } }
#if UNITY_EDITOR
		// Used in the CreateAssetMenu attribute.
		internal const int ORDER = ScriptableEvent.ORDER + 50;
#endif

		/// <summary>
		///     Called when something was added. Gives you the newly added item.
		/// </summary>
		public event Action<T> OnAdded;
		/// <summary>
		///     Called when something was inserted. Gives you the index it was inserted at and the newly inserted item.
		/// </summary>
		public event Action<int, T> OnInserted;
		/// <summary>
		///     Called when something was added or inserted. Gives you the index it was added/inserted at and the newly
		///     added/inserted item.
		/// </summary>
		public event Action<int, T> OnAddedOrInserted;
		/// <summary>
		///     Called when something was set using the indexer. Gives you the index it was set at, the old value and the new
		///     value.
		/// </summary>
		public event Action<int, T, T> OnSet;
		/// <summary>
		///     Called when something was removed. Gives you the index it was removed at and the removed item.
		/// </summary>
		public event Action<int, T> OnRemoved;
		/// <summary>
		///     Called when the list is cleared.
		/// </summary>
		public event Action OnCleared;

		/// <summary>
		///     Sets the value at the given index.
		/// </summary>
		/// <param name="index">The index where to set the item.</param>
		/// <param name="value">The new value to set.</param>
		private void SetValue(int index, T value)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be changed at runtime.");
				return;
			}

			// If the equality check is enabled, we don't want to set the value if it's the same as the current value.
			if (setEqualityCheck && EqualityHelper.Equals(list[index], value))
			{
				return;
			}

			T oldValue = list[index];
			list[index] = value;
			OnSet?.Invoke(index, oldValue, value);

			AddStackTrace();
		}

		/// <summary>
		///     Removes all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The predicate delegate that defines the conditions of the elements to remove.</param>
		/// <returns>The number of elements removed from the list.</returns>
		/// <exception cref="ArgumentNullException">match is null.</exception>
		public int RemoveAll(Predicate<T> match)
		{
#if DEBUG
			// Throw an exception if the match is null, but only in Debug builds.
			if (match == null)
			{
				throw new ArgumentNullException(nameof(match));
			}
#endif

			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be removed from at runtime.");
				return 0;
			}

			// Keep track of how many items were removed.
			int removeCount = 0;

			// Go in reverse as we are removing items.
			for (int i = list.Count - 1; i >= 0; i--)
			{
				// If the item matches the predicate, remove it.
				if (match(list[i]))
				{
					removeCount++;
					RemoveAt(i);
				}
			}

			return removeCount;
		}

		/// <summary>
		///     Reverses the order of the elements in the entire list.
		/// </summary>
		public void Reverse()
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be reversed at runtime.");
				return;
			}

			list.Reverse();

			AddStackTrace();
		}

		/// <summary>
		///     Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		public void Reverse(int index, int count)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be reversed at runtime.");
				return;
			}

			list.Reverse(index, count);

			AddStackTrace();
		}

		/// <summary>
		///     Sorts the elements in the entire list using the default comparer.
		/// </summary>
		public void Sort()
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be sorted at runtime.");
				return;
			}

			list.Sort();

			AddStackTrace();
		}

		/// <summary>
		///     Sorts the elements in the entire list using the specified comparer.
		/// </summary>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
		///     the default comparer Default.
		/// </param>
		public void Sort(IComparer<T> comparer)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be sorted at runtime.");
				return;
			}

			list.Sort(comparer);

			AddStackTrace();
		}

		/// <summary>
		///     Sorts the elements in a range of elements in list using the specified comparer.
		/// </summary>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="count">The number of elements in the range to sort.</param>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
		///     the default comparer Default.
		/// </param>
		public void Sort(int index, int count, IComparer<T> comparer)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be sorted at runtime.");
				return;
			}

			list.Sort(index, count, comparer);

			AddStackTrace();
		}

		/// <summary>
		///     Sorts the elements in the entire list using the specified <see cref="Comparison{T}" />.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparison{T}" /> to use when comparing elements.</param>
		public void Sort(Comparison<T> comparison)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be sorted at runtime.");
				return;
			}

			list.Sort(comparison);

			AddStackTrace();
		}

		/// <summary>
		///     Copies the elements of the list to a new array.
		/// </summary>
		/// <returns>An array containing copies of the elements of the list.</returns>
		public T[] ToArray()
		{
			return list.ToArray();
		}

		/// <summary>
		///     Sets the capacity to the actual number of elements in the list, if that number is less than a threshold value.
		/// </summary>
		public void TrimExcess()
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be trimmed at runtime.");
				return;
			}

			list.TrimExcess();

			AddStackTrace();
		}

		/// <summary>
		///     Determines whether every element in the list matches the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions to check against the elements.</param>
		/// <returns>
		///     True if every element matches the condition; otherwise, false. If the list has no elements, the return value
		///     is true.
		/// </returns>
		public bool TrueForAll(Predicate<T> match)
		{
			return list.TrueForAll(match);
		}

		/// <summary>
		///     Tries to find an element in the list that matches the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the condition to check against.</param>
		/// <param name="result">
		///     The first result that matches the delegate. If no element was found, this will be the default
		///     value.
		/// </param>
		/// <returns>True if an element was found; otherwise, false.</returns>
		public bool TryFind(Predicate<T> match, out T result)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				// Check if the element matches the predicate.
				if (match(list[i]))
				{
					// The element matches the predicate, so we can return it.
					result = list[i];
					return true;
				}
			}

			// There was no element that matched the predicate.
			result = default;
			return false;
		}

		/// <inheritdoc />
		public override void ResetValues()
		{
			ResetStackTraces();

			OnAdded = null;
			OnInserted = null;
			OnAddedOrInserted = null;
			OnSet = null;
			OnRemoved = null;
			OnCleared = null;

			if (clearOnStart && !isReadOnly)
			{
				list.Clear();
			}
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			if (!isReadOnly && clearOnStart && list.Count > 0)
			{
				Debug.LogWarning($"There are left over objects in the scriptable list {name}. You should clear the list before leaving play mode.");
			}

			EventHelper.WarnIfLeftOverSubscribers(OnAdded, nameof(OnAdded), this);
			EventHelper.WarnIfLeftOverSubscribers(OnInserted, nameof(OnInserted), this);
			EventHelper.WarnIfLeftOverSubscribers(OnAddedOrInserted, nameof(OnAddedOrInserted), this);
			EventHelper.WarnIfLeftOverSubscribers(OnSet, nameof(OnSet), this);
			EventHelper.WarnIfLeftOverSubscribers(OnRemoved, nameof(OnRemoved), this);
			EventHelper.WarnIfLeftOverSubscribers(OnCleared, nameof(OnCleared), this);

			list.TrimExcess();
		}
#endif

		/// <summary>
		///     Adds the elements of the specified collection to the end of the list.
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the list.</param>
		/// <exception cref="ArgumentNullException">If collection is null.</exception>
		public void AddRange(IEnumerable<T> collection)
		{
			ThrowHelper.ThrowIfNull(collection, nameof(collection));

			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be added to at runtime.");
				return;
			}

			// If it's a collection, we can get the count and add the capacity to the list to avoid resizing the list multiple times.
			// Otherwise, just add the items as normal.
			if (collection is ICollection<T> c)
			{
				int count = c.Count;
				if (count > 0)
				{
					// If the capacity is less than the current count + the number of items to add, increase the capacity.
					if (list.Capacity < list.Count + count)
					{
						list.Capacity = list.Count + count;
					}

					// Add the items. We use the enumerator directly to avoid allocations, instead of a foreach.
					using (IEnumerator<T> en = c.GetEnumerator())
					{
						while (en.MoveNext())
						{
							Add(en.Current);
						}
					}
				}
			}
			else
			{
				// Add the items. We use the enumerator directly to avoid allocations, instead of a foreach.
				using (IEnumerator<T> en = collection.GetEnumerator())
				{
					while (en.MoveNext())
					{
						Add(en.Current);
					}
				}
			}

			AddStackTrace();
		}

		/// <summary>
		///     Determines whether the list contains elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		///     True if the list contains one or more elements that match the conditions defined by the specified predicate;
		///     otherwise, false.
		/// </returns>
		public bool Exists(Predicate<T> match)
		{
			return list.Exists(match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the first
		///     occurrence within the entire list.
		/// </summary>
		/// <param name="match">The predicate delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The first element that matches the conditions defines by the specified predicate, if found; otherwise, the
		///     default value for type <see cref="T" />
		/// </returns>
		public T Find(Predicate<T> match)
		{
			return list.Find(match);
		}

		/// <summary>
		///     Adds an item to the list. May fail if the value is not the same type as the generic type.
		/// </summary>
		/// <param name="value">The item to add.</param>
		/// <returns>The new count of the list.</returns>
		int IList.Add(object value)
		{
			// Check if the value is the same type as the generic type.
			if (EqualityHelper.IsSameType(value, out T newValue))
			{
				Add(newValue);
				return Count - 1;
			}

			// It was not a valid type, return -1.
			return -1;
		}

		/// <summary>
		///     Determines whether an element is in the list.
		/// </summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>True if the item is found in the list; otherwise false.</returns>
		bool IList.Contains(object value)
		{
			// Check if the value is the same type as the generic type and then call the Contains method.
			return EqualityHelper.IsSameType(value, out T newValue) && Contains(newValue);
		}

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the first occurrence within the entire list.
		///     Returns -1 if the item is not found.
		/// </summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>The zero-based index of the first occurrence of item within the entire list, if found; otherwise, -1.</returns>
		int IList.IndexOf(object value)
		{
			// Check if the value is the same type as the generic type.
			if (EqualityHelper.IsSameType(value, out T newValue))
			{
				return IndexOf(newValue);
			}

			// It was not a valid type, return -1.
			return -1;
		}

		/// <summary>
		///     Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The object to insert.</param>
		void IList.Insert(int index, object value)
		{
			// Check if the value is the same type as the generic type.
			if (EqualityHelper.IsSameType(value, out T newValue))
			{
				Insert(index, newValue);
			}
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the list.
		/// </summary>
		/// <param name="value">The object to remove from the list.</param>
		void IList.Remove(object value)
		{
			// Check if the value is the same type as the generic type.
			if (EqualityHelper.IsSameType(value, out T newValue))
			{
				Remove(newValue);
			}
		}

		/// <summary>
		///     Copies the entire list to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The array destination.</param>
		/// <param name="index">The index in the destination array where the copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection) list).CopyTo(array, index);
		}

		/// <summary>
		///     Returns an enumerator that iterates through the list.
		/// </summary>
		/// <returns>A enumerator for the list.</returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return ((IEnumerable<T>) list).GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the list.
		/// </summary>
		/// <returns>A enumerator for the list.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) list).GetEnumerator();
		}

		/// <summary>
		///     Adds an item to the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void Add(T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be added to at runtime.");
				return;
			}

			int index = Count;
			list.Add(item);
			OnAdded?.Invoke(item);
			OnAddedOrInserted?.Invoke(index, item);

			AddStackTrace();
		}

		/// <summary>
		///     Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The index where the item should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		public void Insert(int index, T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be inserted to at runtime.");
				return;
			}

			list.Insert(index, item);
			OnInserted?.Invoke(index, item);
			OnAddedOrInserted?.Invoke(index, item);

			AddStackTrace();
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the list.
		/// </summary>
		/// <param name="item">The object to remove.</param>
		/// <returns>
		///     True if the item was successfully removed; otherwise false. The method also returns false if the item was not
		///     found in the list.
		/// </returns>
		public bool Remove(T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be removed from at runtime.");
				return false;
			}

			int index = list.IndexOf(item);
			if (index == -1)
			{
				return false;
			}

			list.RemoveAt(index);
			OnRemoved?.Invoke(index, item);
			AddStackTrace();

			return true;
		}

		/// <summary>
		///     Removes the element at the specified index of the list.
		/// </summary>
		/// <param name="index">The index of the element to remove.</param>
		public void RemoveAt(int index)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be removed from at runtime.");
				return;
			}

			T item = list[index];
			list.RemoveAt(index);
			OnRemoved?.Invoke(index, item);

			AddStackTrace();
		}

		/// <summary>
		///     Removes all elements from the list.
		/// </summary>
		public void Clear()
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be cleared at runtime.");
				return;
			}

			list.Clear();
			OnCleared?.Invoke();

			AddStackTrace();
		}

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the first occurrence within the entire list.
		///     Returns -1 if the item is not found.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <returns>The zero-based index of the first occurrence of item within the entire list, if found; otherwise, -1.</returns>
		public int IndexOf(T item)
		{
			return list.IndexOf(item);
		}

		/// <summary>
		///     Determines whether an element is in the list.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <returns>True if the item is found in the list; otherwise false.</returns>
		public bool Contains(T item)
		{
			return list.Contains(item);
		}

		/// <summary>
		///     Copies the entire list to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The array destination.</param>
		/// <param name="arrayIndex">The index in the destination array where the copying begins.</param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}
	}
}