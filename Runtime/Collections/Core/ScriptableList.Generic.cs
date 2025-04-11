#nullable enable

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
	public abstract partial class ScriptableList<T> : ScriptableList,
		IList<T>,
		IReadOnlyList<T>,
		IList,
		INotifyCollectionChanged,
		INotifyScriptableCollectionChanged<T>
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

		private int internalCapacity;
		private int internalCount;

		private readonly DelegateHandlerList<CollectionChangedEventHandler<T>, CollectionChangedArgs<T>> onCollectionChanged =
			new DelegateHandlerList<CollectionChangedEventHandler<T>, CollectionChangedArgs<T>>();

		/// <summary>
		///     Gets the total number of elements the internal data structure can hold without resizing.
		/// </summary>
		public sealed override int Capacity
		{
			get { return internalCapacity; }
			set
			{
				list.Capacity = value;
				SetField(ref internalCapacity, list.Capacity, capacityChangingArgs, capacityChangedArgs);
				Assert.AreEqual(internalCapacity, list.Capacity);
			}
		}

		/// <summary>
		///     Gets the number of elements contained in the list.
		/// </summary>
		public sealed override int Count
		{
			get { return internalCount; }
			protected set
			{
				SetField(ref internalCount, value, countChangingArgs, countChangedArgs);
				Assert.AreEqual(internalCount, list.Count);
			}
		}

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
		///     If read only, the list cannot be changed at runtime and won't be cleared on start.
		/// </summary>
		public override bool IsReadOnly
		{
			get { return isReadOnly; }
			set { SetField(ref isReadOnly, value, isReadOnlyChangingArgs, isReadOnlyChangedArgs); }
		}

		/// <summary>
		///     If true, an equality check will be run before setting an item through the indexer to make sure the new object is
		///     not the same as the old one.
		/// </summary>
		public override bool SetEqualityCheck
		{
			get { return setEqualityCheck; }
			set { SetField(ref setEqualityCheck, value, setEqualityCheckChangingArgs, setEqualityCheckChangedArgs); }
		}

		/// <summary>
		///     If true, the list will be cleared on play mode start/game boot.
		/// </summary>
		public override bool ClearOnStart
		{
			get { return clearOnStart; }
			set { SetField(ref clearOnStart, value, clearOnStartChangingArgs, clearOnStartChangedArgs); }
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

#if UNITY_EDITOR
		// Used in the CreateAssetMenu attribute.
		internal const int ORDER = ScriptableEvent.ORDER + 50;
#endif

		/// <summary>
		///     Occurs when an item is added, removed, replaced, or the entire list is refreshed.
		/// </summary>
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

		/// <summary>
		///     Adds an item to the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Add(T item)
		{
			AddInternal(item);
		}

		/// <summary>
		///     Adds an item to the list. May fail if the value is not the same type as the generic type.
		/// </summary>
		/// <param name="value">The item to add.</param>
		/// <returns>The new count of the list.</returns>
		/// <exception cref="ArgumentNullException"><c>item</c> is null and <c>T</c> does not allow it.</exception>
		/// <exception cref="ArgumentException"><c>item</c> is of a type that is not assignable to the list.</exception>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		int IList.Add(object? value)
		{
			// If the item is null and typeof(T) doesn't allow nulls, throw an exception.
			ThrowHelper.ThrowIfNullAndNullsAreIllegal<T>(value, nameof(value));

			try
			{
				AddInternal((T) value!);
			}
			catch (InvalidCastException)
			{
				// The item was not the correct type, throw an exception.
				ThrowHelper.ThrowWrongExpectedValueType<T>(value);
			}

			return Count - 1;
		}

		/// <summary>
		///     Internal method to add an item to the list. Skips one frame in the stack traces to give the impression that the
		///     calling method added the item.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		private void AddInternal(T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			int index = Count;
			list.Add(item);
			UpdateCounts();
			InvokeCollectionChanged(CollectionChangedArgs<T>.Add(item, index));

			AddStackTrace(1);
		}

		/// <summary>
		///     Adds the elements of the specified collection to the end of the list.
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the list.</param>
		/// <exception cref="ArgumentNullException">If <paramref name="collection" /> is null.</exception>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void AddRange(IEnumerable<T> collection)
		{
			ThrowHelper.ThrowIfNull(collection, nameof(collection));

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (CollectionScope<T> scope = new CollectionScope<T>(collection))
			{
				if (scope.Length == 0)
				{
					return;
				}

				int index = list.Count;

				using CollectionEnumerator<T> enumerator = scope.GetEnumerator();
				list.AddRange(enumerator);
				UpdateCounts();

				CollectionChangedArgs<T> args = CollectionChangedArgs<T>.Add(scope.Span, index);
				onCollectionChanged.Invoke(args);
				OnInternalCollectionChanged?.Invoke(this, args.ToNotifyCollectionChangedEventArgs());
			}

			AddStackTrace();
		}

		/// <summary>
		///     Searches the entire sorted list for an element using the specified comparer and returns the zero-based index of the
		///     element.
		/// </summary>
		/// <param name="item">The object to locate. The value can be <c>null</c> for reference types.</param>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements. Or <c>null</c> to
		///     use the default comparer.
		/// </param>
		/// <returns>
		///     The zero-based index of <c>item</c> in the sorted list, if <c>item</c> is found; otherwise, a negative number
		///     that is the bitwise complement of the index of the next element that is larger than <c>item</c> or, if there is no
		///     larger element, the bitwise complement of <see cref="Count" />
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///     <c>comparer</c> is <c>null</c>, and the default comparer cannot find an
		///     implementation of the <see cref="IComparer{T}" /> generic interface or the <see cref="IComparable" /> interface for
		///     type <c>T</c>
		/// </exception>
		public int BinarySearch(T item, IComparer<T>? comparer = null)
		{
			return BinarySearch(0, internalCount, item, comparer);
		}

		/// <summary>
		///     Searches a range of elements in the sorted list for an element using the specified comparer and returns the
		///     zero-based index of the element.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to search.</param>
		/// <param name="count">The length of the range to search.</param>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0. Or <c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range in the list.</exception>
		/// <inheritdoc cref="BinarySearch(T,System.Collections.Generic.IComparer{T}?)" path='/returns' />
		public int BinarySearch(int index, int count, T item, IComparer<T>? comparer = null)
		{
			return list.BinarySearch(index, count, item, comparer);
		}

		/// <summary>
		///     Removes all elements from the list.
		/// </summary>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="/returns" />
		public void Clear()
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			if (list.Count == 0)
			{
				return;
			}

			using CollectionScope<T> scope = new CollectionScope<T>(list);

			list.Clear();
			UpdateCounts();
			InvokeCollectionChanged(CollectionChangedArgs<T>.Clear(scope.Span));

			AddStackTrace();
		}

		/// <summary>
		///     Determines whether an element is in the list.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <returns><c>true</c> if <c>item</c> is found in the list; otherwise <c>false</c>.</returns>
		public bool Contains(T item)
		{
			return list.Contains(item);
		}

		/// <summary>
		///     Determines whether an element is in the list.
		/// </summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>True if the item is found in the list; otherwise false.</returns>
		bool IList.Contains(object? value)
		{
			// Check if the value is the same type as the generic type and then call the Contains method.
			return EqualityHelper.IsSameType(value, out T? newValue) && Contains(newValue);
		}

		/// <summary>
		///     Converts the elements in the current list to another type, and returns a list containing the converted elements.
		/// </summary>
		/// <param name="converter">
		///     A <see cref="Converter{TInput,TOutput}" /> delegate that converts each element from one type to
		///     another type.
		/// </param>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
		/// <returns>A <see cref="List{TOutput}" /> of the target type containing the converted elements of the current list.</returns>
		/// <exception cref="ArgumentNullException"><c>converter</c> is <c>null</c>.</exception>
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			return list.ConvertAll(converter);
		}

		/// <summary>
		///     Converts the elements in the current list to another type, and copies the converted elements to the specified list.
		/// </summary>
		/// <param name="destinationList">The destination list where the converted items will be copied to.</param>
		/// <param name="converter">
		///     A <see cref="Converter{TInput,TOutput}" /> delegate that converts each element from one type to
		///     another type.
		/// </param>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
		/// <exception cref="ArgumentNullException"><c>converter</c> is <c>null</c>.</exception>
		public void ConvertAll<TOutput>(IList<TOutput> destinationList, Converter<T, TOutput> converter)
		{
			ThrowHelper.ThrowIfNull(converter, nameof(converter));
			ThrowHelper.ThrowIfNull(destinationList, nameof(destinationList));

			destinationList.Clear();

			if (destinationList is List<TOutput> genericList)
			{
				// Make sure the capacity is at least the same as the internal count.
				if (genericList.Capacity < internalCount)
				{
					genericList.Capacity = internalCount;
				}
			}
			else if (destinationList is ScriptableList<TOutput> scriptableList)
			{
				// Make sure the capacity is at least the same as the internal count.
				scriptableList.EnsureCapacity(internalCount);
			}

			for (int i = 0; i < internalCount; i++)
			{
				destinationList.Add(converter(list[i]));
			}
		}

		/// <summary>
		///     Copies the entire list to a compatible one-dimensional array.
		/// </summary>
		/// <param name="array">The array destination.</param>
		public void CopyTo(T[] array)
		{
			list.CopyTo(array);
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
		///     Copies the entire list to a compatible one-dimensional array, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The array destination.</param>
		/// <param name="index">The index in the destination array where the copying begins.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection) list).CopyTo(array, index);
		}

		/// <summary>
		///     Ensures that the list has at least the specified capacity.
		/// </summary>
		/// <param name="capacity">The minimum capacity to ensure.</param>
		public void EnsureCapacity(int capacity)
		{
			if (list.Capacity < capacity)
			{
				Capacity = capacity;
			}
		}

		/// <summary>
		///     Determines whether the list contains elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The predicate delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		///     <c>true</c> if the list contains one or more elements that match the conditions defined by the specified predicate;
		///     otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
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
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		public T? Find(Predicate<T> match)
		{
			return list.Find(match);
		}

		/// <summary>
		///     Retrieves all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		///     A <see cref="List{T}" /> containing all the elements that match the conditions defined by the specified
		///     predicate, if found; otherwise, an empty <see cref="List{T}" />
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		public List<T> FindAll(Predicate<T> match)
		{
			ThrowHelper.ThrowIfNull(match, nameof(match));

			return list.FindAll(match);
		}

		/// <summary>
		///     Retrieves all the elements that match the conditions defined by the specified predicate and copies them to the
		///     specified list.
		/// </summary>
		/// <param name="destinationList"> The list where the found elements will be copied to.</param>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements to search for.</param>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		public void FindAll(IList<T> destinationList, Predicate<T> match)
		{
			ThrowHelper.ThrowIfNull(match, nameof(match));
			ThrowHelper.ThrowIfNull(destinationList, nameof(destinationList));

			destinationList.Clear();

			for (int i = 0; i < internalCount; i++)
			{
				if (match(list[i]))
				{
					destinationList.Add(list[i]);
				}
			}
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
		///     index of the first occurrence within the entire list.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>
		///     , if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		public int FindIndex(Predicate<T> match)
		{
			return list.FindIndex(match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
		///     index of the first occurrence within the range of elements in the list that extends from the specified index to the
		///     last element.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>
		///     , if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>startIndex</c> is outside the range of valid indexes for the list.</exception>
		public int FindIndex(int startIndex, Predicate<T> match)
		{
			return list.FindIndex(startIndex, match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
		///     index of the first occurrence within the range of elements in the list that starts at the specified index and
		///     contains the specified number of elements.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>
		///     , if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <c>startIndex</c> is outside the range of valid indexes for the list. Or
		///     <c>count</c> is less than 0. Or <c>startIndex</c> and <c>count</c> do not specify a valid section in the list.
		/// </exception>
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			return list.FindIndex(startIndex, count, match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the last
		///     occurrence within the entire list.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The last element that matches the conditions defined by the specified predicate, if found; otherwise, the
		///     default value for type <c>T</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		public T? FindLast(Predicate<T> match)
		{
			return list.FindLast(match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
		///     index of the last occurrence within the entire list.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The zero-based index of the last occurrence of an element that matches the conditions defined by <c>match</c>,
		///     if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		public int FindLastIndex(Predicate<T> match)
		{
			return list.FindLastIndex(match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
		///     index of the last occurrence within the range of elements in the list that extends from the first element to the
		///     specified index.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The zero-based index of the last occurrence of an element that matches the conditions defined by <c>match</c>,
		///     if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>startIndex</c> is outside the range of valid indexes for the list.</exception>
		public int FindLastIndex(int startIndex, Predicate<T> match)
		{
			return list.FindLastIndex(startIndex, match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based
		///     index of the last occurrence within the range of elements in the list that contains the specified number of
		///     elements and ends at the specified index.
		/// </summary>
		/// <param name="startIndex">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The zero-based index of the last occurrence of an element that matches the conditions defined by <c>match</c>,
		///     if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentNullException"><c>match</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <c>startIndex</c> is outside the range of valid indexes for the list. Or
		///     <c>count</c> is less than 0. Or <c>startIndex</c> and <c>count</c> do not specify a valid section in the list.
		/// </exception>
		public int FindLastIndex(int startIndex, int count, Predicate<T> match)
		{
			return list.FindLastIndex(startIndex, count, match);
		}

		/// <summary>
		///     Performs the specified action on each element of the list.
		/// </summary>
		/// <param name="action">The <see cref="Action{T}" /> delegate to perform on each element of the list.</param>
		/// <exception cref="ArgumentNullException"><c>action</c> is <c>null</c>.</exception>
		/// <exception cref="InvalidOperationException">An element in the collection has been modified.</exception>
		public void ForEach(Action<T> action)
		{
			list.ForEach(action);
		}

		/// <summary>
		///     Returns an enumerator that iterates through the list.
		/// </summary>
		/// <returns>A <see cref="List{T}.Enumerator" /> for the list.</returns>
		public List<T>.Enumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the list.
		/// </summary>
		/// <returns>A <see cref="List{T}.Enumerator" /> for the list.</returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return ((IEnumerable<T>) list).GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the list.
		/// </summary>
		/// <returns>A <see cref="List{T}.Enumerator" />  for the list.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) list).GetEnumerator();
		}

		//TODO: Implement GetRange

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

		//TODO: Implement IndexOf(T, int)
		//TODO: Implement IndexOf(T, int, int)

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the first occurrence within the entire list.
		///     Returns -1 if the item is not found.
		/// </summary>
		/// <param name="value">The object to locate in the list.</param>
		/// <returns>The zero-based index of the first occurrence of item within the entire list, if found; otherwise, -1.</returns>
		int IList.IndexOf(object value)
		{
			// Check if the value is the same type as the generic type.
			if (EqualityHelper.IsSameType(value, out T? newValue))
			{
				return IndexOf(newValue);
			}

			// It was not a valid type, return -1.
			return -1;
		}

		/// <summary>
		///     Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The index where the item should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Insert(int index, T item)
		{
			InsertInternal(index, item);
		}

		/// <summary>
		///     Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The item to insert.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		void IList.Insert(int index, object? value)
		{
			ThrowHelper.ThrowIfNullAndNullsAreIllegal<T>(value, nameof(value));

			try
			{
				InsertInternal(index, (T) value!);
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongExpectedValueType<T>(value);
			}
		}

		/// <summary>
		///     Internal method to insert an item into the list. Skips one frame in the stack traces to give the impression that
		///     the calling method inserted the item.
		/// </summary>
		/// <param name="index">The index where the item should be inserted.</param>
		/// <param name="item">The item to insert.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		/// <exception cref="ArgumentOutOfRangeException">If the index is out of bounds.</exception>
		private void InsertInternal(int index, T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);
			ThrowHelper.ThrowIfOutOfBounds(nameof(index), in index, 0, list.Count);

			list.Insert(index, item);
			UpdateCounts();
			InvokeCollectionChanged(CollectionChangedArgs<T>.Add(item, index));

			AddStackTrace(1);
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
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			ThrowHelper.ThrowIfNull(collection, nameof(collection));
			ThrowHelper.ThrowIfOutOfBounds(nameof(index), in index, 0, list.Count);

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using (CollectionScope<T> scope = new CollectionScope<T>(collection))
			{
				if (scope.Length == 0)
				{
					return;
				}

				using CollectionEnumerator<T> enumerator = scope.GetEnumerator();
				list.InsertRange(index, enumerator);
				UpdateCounts();
				CollectionChangedArgs<T> args = CollectionChangedArgs<T>.Add(scope.Span, index);
				InvokeCollectionChanged(args);
			}

			AddStackTrace();
		}

		//TODO: Implement LastIndexOf(T, int int)
		//TODO: Implement LastIndexOf(T, int)
		//TODO: Implement LastIndexOf(T)

		/// <summary>
		///     Removes the first occurrence of a specific object from the list.
		/// </summary>
		/// <param name="item">The object to remove.</param>
		/// <returns>
		///     True if the item was successfully removed; otherwise false. The method also returns false if the item was not
		///     found in the list.
		/// </returns>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public bool Remove(T item)
		{
			return RemoveInternal(item);
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the list.
		/// </summary>
		/// <param name="value">The object to remove from the list.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		void IList.Remove(object value)
		{
			// Check if the value is the same type as the generic type.
			if (EqualityHelper.IsSameType(value, out T? newValue))
			{
				Remove(newValue);
			}
		}

		/// <summary>
		///     Internal method to remove an item from the list. Skips one frame in the stack traces to give the impression that
		///     the calling method removed the item.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed; otherwise false.</returns>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		private bool RemoveInternal(T item)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			int index = list.IndexOf(item);
			if (index == -1)
			{
				return false;
			}

			T? itemToRemove = list[index];
			list.RemoveAt(index);
			UpdateCounts();
			InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(itemToRemove, index));

			AddStackTrace();

			return true;
		}

		/// <summary>
		///     Removes all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The predicate delegate that defines the conditions of the elements to remove.</param>
		/// <returns>The number of elements removed from the list.</returns>
		/// <exception cref="ArgumentNullException">match is null.</exception>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public int RemoveAll(Predicate<T> match)
		{
			ThrowHelper.ThrowIfNull(match, nameof(match));

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			T[]? removed = ArrayPool<T>.Shared.Rent(list.Count);
			try
			{
				int removeLength = 0;
				int firstIndex = -1;
				for (int i = 0; i < list.Count; i++)
				{
					if (match(list[i]))
					{
						if (firstIndex == -1)
						{
							firstIndex = i;
						}

						removed[removeLength] = list[i];
						removeLength++;
					}
				}

				if (removeLength > 0)
				{
					int removeCount = list.RemoveAll(match);
					InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(removed.AsSpan(0, removeLength), firstIndex));

					Assert.AreEqual(removeLength, removeCount, "The expected count of removed items is not the same as the actually removed items count.");

					UpdateCounts();
					return removeCount;
				}
			}
			finally
			{
				ArrayPool<T>.Shared.Return(removed, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
			}

			return 0;
		}

		/// <summary>
		///     Removes the element at the specified index of the list.
		/// </summary>
		/// <param name="index">The index of the element to remove.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void RemoveAt(int index)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			T? removedItem = list[index];
			list.RemoveAt(index);
			UpdateCounts();
			InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(removedItem, index));

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
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
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
				T[]? removed = ArrayPool<T>.Shared.Rent(countToRemove);
				try
				{
					for (int i = 0; i < countToRemove; i++)
					{
						removed[i] = list[index + i];
					}

					list.RemoveRange(index, count);

					Span<T> span = removed.AsSpan(0, countToRemove);

					UpdateCounts();
					InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(span, index));
				}
				finally
				{
					ArrayPool<T>.Shared.Return(removed, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
				}
			}

			AddStackTrace();
		}

		/// <summary>
		///     Reverses the order of the elements in the entire list.
		/// </summary>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Reverse()
		{
			ReverseInternal(0, list.Count);
		}

		/// <summary>
		///     Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Reverse(int index, int count)
		{
			ReverseInternal(index, count);
		}

		/// <summary>
		///     Internal reverse method that skips a frame in the stack traces.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
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

		//TODO: Implement Slice(int, int)

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
		///     Sorts the elements in the entire list using the specified comparer.
		/// </summary>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or null to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
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
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Sort(int index, int count, IComparer<T>? comparer = null)
		{
			SortInternal(index, count, comparer, null);
		}

		/// <summary>
		///     Sorts the elements in the entire list using the specified <see cref="Comparison{T}" />.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparison{T}" /> to use when comparing elements.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
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
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0 or <c>count</c> is less than 0.</exception>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
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
			list.TrimExcess();
			UpdateCounts();

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
		public bool TryFind(Predicate<T> match, out T? result)
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
		public void RegisterChangedListener(CollectionChangedEventHandler<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RegisterCallback(callback);
		}

		/// <inheritdoc />
		public void RegisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<T, TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(context, nameof(context));

			onCollectionChanged.RegisterCallback(callback, context);
		}

		/// <inheritdoc />
		public void UnregisterChangedListener(CollectionChangedEventHandler<T> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RemoveCallback(callback);
		}

		/// <inheritdoc />
		public void UnregisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<T, TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RemoveCallback(callback);
		}

		private void InvokeCollectionChanged(in CollectionChangedArgs<T> args)
		{
			onCollectionChanged.Invoke(args);
			OnInternalCollectionChanged?.Invoke(this, args.ToNotifyCollectionChangedEventArgs());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateCounts()
		{
			// Update the count and capacity properties.
			Count = list.Count;
			Capacity = list.Capacity;
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

			internalCount = list.Count;
			internalCapacity = list.Capacity;
		}

		[Conditional("DEBUG")]
		private void WarnLeftOverSubscribers()
		{
			EventHelper.WarnIfLeftOverSubscribers(onCollectionChanged, nameof(OnCollectionChanged), this);
			EventHelper.WarnIfLeftOverSubscribers(OnInternalCollectionChanged, "INotifyCollectionChanged.CollectionChanged", this);
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
			OnInternalCollectionChanged = null;
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

		#region Obsolete
#if UNITY_EDITOR // Don't include in build.
#pragma warning disable CS0067 // Event is never used
		/// <summary>
		///     Called when something was added. Gives you the newly added item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<T>? OnAdded;
		/// <summary>
		///     Called when something was inserted. Gives you the index it was inserted at and the newly inserted item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<int, T>? OnInserted;
		/// <summary>
		///     Called when something was added or inserted. Gives you the index it was added/inserted at and the newly
		///     added/inserted item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<int, T>? OnAddedOrInserted;
		/// <summary>
		///     Called when something was set using the indexer. Gives you the index it was set at, the old value and the new
		///     value.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<int, T, T>? OnSet;
		/// <summary>
		///     Called when something was removed. Gives you the index it was removed at and the removed item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<int, T>? OnRemoved;
		/// <summary>
		///     Called when the list is cleared.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action? OnCleared;
		/// <summary>
		///     Called when the list is changed in any way.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<ListChangeType>? OnChanged;
#pragma warning restore CS0067 // Event is never used
#endif // UNITY_EDITOR
		#endregion
	}
}