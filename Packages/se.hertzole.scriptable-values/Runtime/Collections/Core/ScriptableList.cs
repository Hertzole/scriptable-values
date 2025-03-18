#nullable enable

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hertzole.ScriptableValues.Helpers;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;
#if SCRIPTABLE_VALUES_PROPERTIES
using Unity.Properties;
#endif

namespace Hertzole.ScriptableValues
{
	/// <summary>
	///     A scriptable object that holds a list.
	/// </summary>
	/// <typeparam name="T">The type in the list.</typeparam>
	public abstract partial class ScriptableList<T> : RuntimeScriptableObject, IList<T>, IReadOnlyList<T>, IList, INotifyCollectionChanged
	{
		[SerializeField]
		[EditorTooltip("If read only, the list cannot be changed at runtime and won't be cleared on start.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool isReadOnly = false;
		[SerializeField]
		[EditorTooltip(
			"If true, an equality check will be run before setting an item through the indexer to make sure the new object is not the same as the old one.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool setEqualityCheck = true;
		[SerializeField]
		[EditorTooltip("If true, the list will be cleared on play mode start/game boot.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool clearOnStart = true;
		[SerializeField]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal List<T> list = new List<T>();

		private readonly DelegateHandlerList<CollectionChangedEventHandler<T>, CollectionChangedArgs<T>> onCollectionChanged =
			new DelegateHandlerList<CollectionChangedEventHandler<T>, CollectionChangedArgs<T>>();

		public T this[int index]
		{
			get { return list[index]; }
			set { SetValue(index, value); }
		}

		object? IList.this[int index]
		{
			get { return list[index]; }
			set
			{
				ThrowHelper.ThrowIfNullAndNullsAreIllegal<T>(value, nameof(value));

				try
				{
					this[index] = (T) value!;
				}
				catch (InvalidCastException)
				{
					// The item was not the correct type, throw an exception.
					ThrowHelper.ThrowWrongExpectedValueType<T>(value);
				}
			}
		}

		/// <summary>
		///     Gets the total number of elements the internal data structure can hold without resizing.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public int Capacity
		{
			get { return list.Capacity; }
		}

		/// <summary>
		///     If true, an equality check will be run before setting an item through the indexer to make sure the new object is
		///     not the same as the old one.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public bool SetEqualityCheck
		{
			get { return setEqualityCheck; }
			set
			{
				//TODO: Cache changing/changed args
				SetField(ref setEqualityCheck, value);
			}
		}
		/// <summary>
		///     If true, the list will be cleared on play mode start/game boot.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public bool ClearOnStart
		{
			get { return clearOnStart; }
			set
			{
				// TODO: Cache changing/changed args
				SetField(ref clearOnStart, value);
			}
		}

		// Is this List synchronized (thread-safe)?
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		// Synchronization root for this object.
		object ICollection.SyncRoot
		{
			get { return this; }
		}

		bool IList.IsFixedSize
		{
			get { return isReadOnly; }
		}
		/// <summary>
		///     If read only, the list cannot be changed at runtime and won't be cleared on start.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public bool IsReadOnly
		{
			get { return isReadOnly; }
			set
			{
				//TODO: Cache changing/changed args
				SetField(ref isReadOnly, value);
			}
		}
		/// <summary>
		///     Gets the number of elements contained in the list.
		/// </summary>
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public int Count
		{
			get { return list.Count; }
		}
#if UNITY_EDITOR
		// Used in the CreateAssetMenu attribute.
		internal const int ORDER = ScriptableEvent.ORDER + 50;
#endif

		/// <summary>
		///     Called when something was added. Gives you the newly added item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action<T>? OnAdded;
		/// <summary>
		///     Called when something was inserted. Gives you the index it was inserted at and the newly inserted item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action<int, T>? OnInserted;
		/// <summary>
		///     Called when something was added or inserted. Gives you the index it was added/inserted at and the newly
		///     added/inserted item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action<int, T>? OnAddedOrInserted;
		/// <summary>
		///     Called when something was set using the indexer. Gives you the index it was set at, the old value and the new
		///     value.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action<int, T, T>? OnSet;
		/// <summary>
		///     Called when something was removed. Gives you the index it was removed at and the removed item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action<int, T>? OnRemoved;
		/// <summary>
		///     Called when the list is cleared.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action? OnCleared;
		/// <summary>
		///     Called when the list is changed in any way.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead.", true)]
		public event Action<ListChangeType>? OnChanged;

		public event CollectionChangedEventHandler<T> OnCollectionChanged
		{
			add { RegisterChangedListener(value); }
			remove { UnregisterChangedListener(value); }
		}

		private event NotifyCollectionChangedEventHandler? OnInternalCollectionChanged;

		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add { OnInternalCollectionChanged += value; }
			remove { OnInternalCollectionChanged -= value; }
		}

		private void OnDestroy()
		{
			onCollectionChanged.Dispose();
		}

		/// <summary>
		///     Sets the value at the given index.
		/// </summary>
		/// <param name="index">The index where to set the item.</param>
		/// <param name="value">The new value to set.</param>
		private void SetValue(int index, T value)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			// If the equality check is enabled, we don't want to set the value if it's the same as the current value.
			if (setEqualityCheck && EqualityHelper.Equals(list[index], value))
			{
				return;
			}

			T oldValue = list[index];
			list[index] = value;
			InvokeCollectionChanged(CollectionChangedArgs<T>.Replace(oldValue, value, index));

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
			ThrowHelper.ThrowIfNull(match, nameof(match));

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (new ChangeScope(this))
			{
				T[]? removed = ArrayPool<T>.Shared.Rent(list.Count);
				try
				{
					int count = 0;
					int firstIndex = -1;
					for (int i = 0; i < list.Count; i++)
					{
						if (match(list[i]))
						{
							if (firstIndex == -1)
							{
								firstIndex = i;
							}

							removed[count] = list[i];
							count++;
						}
					}

					if (count > 0)
					{
						int removeCount = list.RemoveAll(match);
						InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(removed.AsSpan(0, count), firstIndex));

						Assert.AreEqual(count, removeCount, "The expected count of removed items is not the same as the actually removed items count.");

						return removeCount;
					}
				}
				finally
				{
					ArrayPool<T>.Shared.Return(removed, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
				}

				return 0;
			}
		}

		/// <summary>
		///     Reverses the order of the elements in the entire list.
		/// </summary>
		public void Reverse()
		{
			ReverseInternal(0, list.Count);
		}

		/// <summary>
		///     Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		public void Reverse(int index, int count)
		{
			ReverseInternal(index, count);
		}

		/// <summary>
		///     Internal reverse method that skips a frame in the stack traces.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <exception cref="ReadOnlyException"></exception>
		private void ReverseInternal(int index, int count)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			// There are no items in the list, so we don't need to do anything.
			if (list.Count == 0)
			{
				AddStackTrace(1);
				return;
			}

			using CollectionScope<T> oldItemsScope = CollectionScope<T>.FromListSlice(list, index, count);
			list.Reverse(index, count);

			using CollectionScope<T> newItemsScope = CollectionScope<T>.FromListSlice(list, index, count);
			InvokeCollectionChanged(CollectionChangedArgs<T>.Replace(oldItemsScope.Span, newItemsScope.Span, index));

			AddStackTrace(1);
		}

		/// <summary>
		///     Sorts the elements in the entire list using the specified comparer.
		/// </summary>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		public void Sort(IComparer<T>? comparer = null)
		{
			SortInternal(0, list.Count, comparer, null);
		}

		/// <summary>
		///     Sorts the elements in a range of elements in list using the specified comparer.
		/// </summary>
		/// <param name="index">The starting index of the range to sort.</param>
		/// <param name="count">The number of elements in the range to sort.</param>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		public void Sort(int index, int count, IComparer<T>? comparer = null)
		{
			SortInternal(index, count, comparer, null);
		}

		/// <summary>
		///     Sorts the elements in the entire list using the specified <see cref="Comparison{T}" />.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparison{T}" /> to use when comparing elements.</param>
		public void Sort(Comparison<T> comparison)
		{
			ThrowHelper.ThrowIfNull(comparison, nameof(comparison));

			SortInternal(0, list.Count, null, comparison);
		}

		/// <summary>
		///     Internal sort method that skips a frame in the stack traces.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort.</param>
		/// <param name="count">The length of the range to sort.</param>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		/// <param name="comparison">The <see cref="Comparison{T}" /> to use when comparing elements.</param>
		/// <exception cref="ReadOnlyException">The list is marked as read-only.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0 or <c>count</c> is less than 0.</exception>
		private void SortInternal(int index, int count, IComparer<T>? comparer, Comparison<T>? comparison)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			if (list.Count == 0)
			{
				AddStackTrace(1);
				return;
			}

			using CollectionScope<T> oldItemsScope = CollectionScope<T>.FromListSlice(list, index, count);

			// If there's a comparison, we sort using that. Otherwise, we use the comparer, or the default comparer if the comparer is null.
			if (comparison != null)
			{
				list.Sort(comparison);
			}
			else
			{
				list.Sort(index, count, comparer);
			}

			using CollectionScope<T> newItemsScope = CollectionScope<T>.FromListSlice(list, index, count);
			InvokeCollectionChanged(CollectionChangedArgs<T>.Replace(oldItemsScope.Span, newItemsScope.Span, index));

			AddStackTrace(1);
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
			using (new ChangeScope(this))
			{
				list.TrimExcess();
			}

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

		/// <summary>
		///     Ensures that the list has at least the specified capacity.
		/// </summary>
		/// <param name="capacity">The minimum capacity to ensure.</param>
		public void EnsureCapacity(int capacity)
		{
			if (list.Capacity < capacity)
			{
				int originalCapacity = list.Capacity;
				list.Capacity = capacity;

				if (originalCapacity != list.Capacity)
				{
					NotifyPropertyChanged(nameof(Capacity));
				}
			}
		}

		/// <inheritdoc />
		protected override void OnStart()
		{
#if UNITY_EDITOR
			ResetStackTraces();
#endif

			ClearSubscribers();

			if (clearOnStart && !isReadOnly)
			{
				list.Clear();
			}
		}

		[Conditional("DEBUG")]
		private void WarnLeftOverSubscribers()
		{
			EventHelper.WarnIfLeftOverSubscribers(onCollectionChanged, nameof(OnCollectionChanged), this);
		}

		/// <summary>
		///     Removes any subscribers from the event.
		/// </summary>
		/// <param name="warnIfLeftOver">
		///     If true, a warning will be printed in the console if there are any subscribers.
		///     The warning will only be printed in the editor and debug builds.
		/// </param>
		public void ClearSubscribers(bool warnIfLeftOver = false)
		{
#if DEBUG
			if (warnIfLeftOver)
			{
				WarnLeftOverSubscribers();
			}
#endif
			onCollectionChanged.Reset();
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			if (!isReadOnly && clearOnStart && list.Count > 0)
			{
				Debug.LogWarning($"There are left over objects in the scriptable list {name}. You should clear the list before leaving play mode.");
			}

			WarnLeftOverSubscribers();

			list.TrimExcess();
		}
#endif

		/// <summary>
		///     Adds the elements of the specified collection to the end of the list.
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the list.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="collection" /> is null.</exception>
		/// <exception cref="ReadOnlyException">If the object is marked as read-only and the application is playing.</exception>
		public void AddRange(IEnumerable<T> collection)
		{
			ThrowHelper.ThrowIfNull(collection, nameof(collection));

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (new ChangeScope(this))
			{
				int index = list.Count;
				using (CollectionScope<T> scope = new CollectionScope<T>(collection))
				{
					using CollectionScope<T>.Enumerator enumerator = scope.GetEnumerator();
					list.AddRange(enumerator);
					CollectionChangedArgs<T> args = CollectionChangedArgs<T>.Add(scope.Span, index);
					onCollectionChanged.Invoke(args);
					OnInternalCollectionChanged?.Invoke(this, args);
				}
			}

			AddStackTrace();
		}

		/// <summary>
		///     Inserts the elements of a collection into the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the new elements should be inserted.</param>
		/// <param name="collection">The collection whose elements should be inserted into the list.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="collection" /> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     If <paramref name="index" /> is less than 0 or is greater than
		///     <see cref="Count" />.
		/// </exception>
		/// <exception cref="ReadOnlyException">If the object is marked as read-only and the application is playing.</exception>
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			ThrowHelper.ThrowIfNull(collection, nameof(collection));
			ThrowHelper.ThrowIfOutOfBounds(nameof(index), in index, 0, list.Count);

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (new ChangeScope(this))
			{
				using (CollectionScope<T> scope = new CollectionScope<T>(collection))
				{
					using CollectionScope<T>.Enumerator enumerator = scope.GetEnumerator();
					list.InsertRange(index, enumerator);
					CollectionChangedArgs<T> args = CollectionChangedArgs<T>.Add(scope.Span, index);
					InvokeCollectionChanged(args);
				}
			}

			AddStackTrace();
		}

		/// <summary>
		///     Removes a range of elements from the list.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///     If <paramref name="index" /> is less than 0 or greater than
		///     <see cref="Count" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     If <paramref name="index" /> is less than 0 or greater than
		///     <see cref="Count" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">If <paramref name="count" /> is less than 0.</exception>
		/// <exception cref="ArgumentException">
		///     If <paramref name="index" /> and <paramref name="count" /> do not denote a valid
		///     range of elements.
		/// </exception>
		public void RemoveRange(int index, int count)
		{
#if DEBUG
			ThrowHelper.ThrowIfOutOfBounds(nameof(index), in index, 0, list.Count);

			if (index + count > list.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be greater than the number of elements in the list.");
			}
#endif

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			// Calculate how many items to remove based on the count and index.
			int countToRemove = Math.Min(count, list.Count - index);

			if (countToRemove > 0)
			{
				using (new ChangeScope(this))
				{
					T[]? removed = ArrayPool<T>.Shared.Rent(countToRemove);
					try
					{
						for (int i = 0; i < countToRemove; i++)
						{
							removed[i] = list[index + i];
						}

						list.RemoveRange(index, count);

						Span<T> span = removed.AsSpan(0, countToRemove);

						InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(span, index));
					}
					finally
					{
						ArrayPool<T>.Shared.Return(removed, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
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
		///     Returns an enumerator that iterates through the list.
		/// </summary>
		/// <returns>A enumerator for the list.</returns>
		public List<T>.Enumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>
		///     Copies the entire list to a compatible one-dimensional array.
		/// </summary>
		/// <param name="array">The array destination.</param>
		public void CopyTo(T[] array)
		{
			list.CopyTo(array);
		}

		//TODO: Document
		public void RegisterChangedListener(CollectionChangedEventHandler<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RegisterCallback(callback);
		}

		//TODO: Document
		public void RegisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<T, TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(context, nameof(context));

			onCollectionChanged.RegisterCallback(callback, context);
		}

		//TODO: Document
		public void UnregisterChangedListener(CollectionChangedEventHandler<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RemoveCallback(callback);
		}

		//TODO: Document
		public void UnregisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<T, TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RemoveCallback(callback);
		}

		private void InvokeCollectionChanged(in CollectionChangedArgs<T> args)
		{
			onCollectionChanged.Invoke(args);
			OnInternalCollectionChanged?.Invoke(this, args);
		}

		/// <summary>
		///     Adds an item to the list. May fail if the value is not the same type as the generic type.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>The new count of the list.</returns>
		/// <exception cref="ArgumentNullException"><c>item</c> is null and <c>T</c> does not allow it.</exception>
		/// <exception cref="ArgumentException"><c>item</c> is of a type that is not assignable to the list.</exception>
		int IList.Add(object? item)
		{
			// If the item is null and typeof(T) doesn't allow nulls, throw an exception.
			ThrowHelper.ThrowIfNullAndNullsAreIllegal<T>(item, nameof(item));

			try
			{
				Add((T) item!);
			}
			catch (InvalidCastException)
			{
				// The item was not the correct type, throw an exception.
				ThrowHelper.ThrowWrongExpectedValueType<T>(item);
			}

			return Count - 1;
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
		/// <param name="item">The item to insert.</param>
		void IList.Insert(int index, object? item)
		{
			ThrowHelper.ThrowIfNullAndNullsAreIllegal<T>(item, nameof(item));

			try
			{
				Insert(index, (T) item!);
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongExpectedValueType<T>(item);
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
		///     Adds an item to the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <exception cref="ReadOnlyException">If the object is marked as read-only and the application is playing.</exception>
		public void Add(T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (new ChangeScope(this))
			{
				int index = Count;
				list.Add(item);
				InvokeCollectionChanged(CollectionChangedArgs<T>.Add(item, index));
			}

			AddStackTrace();
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
		///     Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The index where the item should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		public void Insert(int index, T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);
			ThrowHelper.ThrowIfOutOfBounds(nameof(index), in index, 0, list.Count);

			using (new ChangeScope(this))
			{
				list.Insert(index, item);
				InvokeCollectionChanged(CollectionChangedArgs<T>.Add(item, index));
			}

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
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			int index = list.IndexOf(item);
			if (index == -1)
			{
				return false;
			}

			using (new ChangeScope(this))
			{
				T? itemToRemove = list[index];
				list.RemoveAt(index);
				InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(itemToRemove, index));
			}

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
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (new ChangeScope(this))
			{
				T? removedItem = list[index];
				list.RemoveAt(index);
				InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(removedItem, index));
			}

			AddStackTrace();
		}

		/// <summary>
		///     Removes all elements from the list.
		/// </summary>
		public void Clear()
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			if (list.Count == 0)
			{
				return;
			}

			using (new ChangeScope(this))
			{
				using CollectionScope<T> scope = new CollectionScope<T>(list);

				list.Clear();
				InvokeCollectionChanged(CollectionChangedArgs<T>.Reset());
			}

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

		/// <summary>
		///     Helper scope for notifying if the count and/or capacity has changed between changes.
		/// </summary>
		private readonly ref struct ChangeScope
		{
			private readonly int originalCount;
			private readonly int originalCapacity;

			private readonly ScriptableList<T> list;

			public ChangeScope(ScriptableList<T> list)
			{
				this.list = list;
				originalCount = list.Count;
				originalCapacity = list.Capacity;
			}

			public void Dispose()
			{
				if (originalCount != list.Count)
				{
					//TODO: Cache count parameter
					list.NotifyPropertyChanged(nameof(Count));
				}

				if (originalCapacity != list.Capacity)
				{
					//TODO: Cache capacity parameter
					list.NotifyPropertyChanged(nameof(Capacity));
				}
			}
		}
	}
}