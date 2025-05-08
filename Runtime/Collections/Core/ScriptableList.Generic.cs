#nullable enable

using System;
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
	///     A <see cref="ScriptableObject"/> that holds a <see cref="System.Collections.Generic.List{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
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

		/// <inheritdoc />
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

		/// <inheritdoc cref="ScriptableList.Count" />
		public sealed override int Count
		{
			get { return internalCount; }
			protected set
			{
				SetField(ref internalCount, value, countChangingArgs, countChangedArgs);
				Assert.AreEqual(internalCount, list.Count);
			}
		}

		/// <summary>
		///     Gets or sets the element at the specified index.
		/// </summary>
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

		/// <inheritdoc cref="ScriptableList.IsReadOnly" />
		public override bool IsReadOnly
		{
			get { return isReadOnly; }
			set { SetField(ref isReadOnly, value, isReadOnlyChangingArgs, isReadOnlyChangedArgs); }
		}

		/// <inheritdoc />
		public override bool SetEqualityCheck
		{
			get { return setEqualityCheck; }
			set { SetField(ref setEqualityCheck, value, setEqualityCheckChangingArgs, setEqualityCheckChangedArgs); }
		}

		/// <inheritdoc />
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
		///     Occurs when an item is added, removed, replaced, or the entire <see cref="ScriptableList{T}"/> is refreshed.
		/// </summary>
		public event CollectionChangedEventHandler<T>? OnCollectionChanged;

		private event NotifyCollectionChangedEventHandler? OnInternalCollectionChanged;

		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add { OnInternalCollectionChanged += value; }
			remove { OnInternalCollectionChanged -= value; }
		}

		/// <summary>
		///     Adds an item to the end of the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="item">The object to be added to the end of the <see cref="ScriptableList{T}" />. The value can be <c>null</c> for reference types.</param>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public void Add(T item)
		{
			AddInternal(item);
		}

		/// <summary>
		///     Adds an item to the end of the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="value">The object to be added to the end of the <see cref="ScriptableList{T}" />. The value can be <c>null</c> for reference types.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c> and <c>T</c> does not allow it.</exception>
		/// <exception cref="ArgumentException"><paramref name="value" /> is of a type that is not assignable to the list.</exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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

		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		///     Adds the elements of the specified collection to the end of the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="collection">The collection whose elements should be added to the end of the <see cref="ScriptableList{T}" />. The collection itself cannot be <c>null</c>, but it can contain elements that are <c>null</c>, if type <c>T</c> is a reference type.</param>
		/// <exception cref="ArgumentNullException"><paramref name="collection" /> is <c>null</c>.</exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
				InvokeCollectionChanged(in args);
			}

			AddStackTrace();
		}

		/// <summary>
		///     Searches the entire sorted <see cref="ScriptableList{T}" /> for an element using the default comparer and returns
		///     the zero-based index of the element.
		/// </summary>
		/// <param name="item">The object to locate. The value can be <c>null</c> for reference types.</param>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or <c>null</c> to use the default
		///     comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		/// <returns>
		///     The zero-based index of <paramref name="item" /> in the sorted <see cref="ScriptableList{T}" />, if <paramref name="item" /> is found;
		///     otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <paramref name="item" /> or,
		///     if there is no larger element, the bitwise complement of <see cref="Count" />.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///     <paramref name="comparer" /> is <c>null</c>, and the default comparer cannot find an implementation of the <see cref="IComparer{T}" /> generic interface or the <see cref="IComparable" /> interface for type <c>T</c>.
		/// </exception>
		public int BinarySearch(T item, IComparer<T>? comparer = null)
		{
			return BinarySearch(0, internalCount, item, comparer);
		}

		/// <summary>
		///     Searches a range of elements in the sorted <see cref="ScriptableList{T}" /> for an element using the specified
		///     comparer and returns the zero-based index of the element.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to search.</param>
		/// <param name="count">The length of the range to search.</param>
		/// <param name="item">The object to locate. The value can be <c>null</c> for reference types.</param>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or <c>null</c> to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		/// <returns>
		///     The zero-based index of <paramref name="item" /> in the sorted <see cref="ScriptableList{T}" />, if
		///     <paramref name="item" /> is found;
		///     otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than
		///     <paramref name="item" /> or,
		///     if there is no larger element, the bitwise complement of <paramref name="index" /> plus <paramref name="count" />.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than 0. -or- <paramref name="count" /> is less than 0.</exception>
		/// <exception cref="ArgumentException">
		///     <paramref name="index" /> and <paramref name="count" /> do not denote a valid range
		///     in the <see cref="ScriptableList{T}" />.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///     <paramref name="comparer" /> is <c>null</c>, and the default comparer cannot find an
		///     implementation of the <see cref="IComparer{T}" /> generic interface or the <see cref="IComparable" /> interface for
		///     type <typeparamref name="T" />.
		/// </exception>
		public int BinarySearch(int index, int count, T item, IComparer<T>? comparer = null)
		{
			return list.BinarySearch(index, count, item, comparer);
		}

		/// <summary>
		///     Removes all elements from the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		///     Determines whether an element is in the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="item">
		///     The object to locate in the <see cref="ScriptableList{T}" />. The value can be <c>null</c> for
		///     reference types.
		/// </param>
		/// <returns>
		///     <c>true</c> if <paramref name="item" /> is found in the <see cref="ScriptableList{T}" />; otherwise,
		///     <c>false</c>.
		/// </returns>
		public bool Contains(T item)
		{
			return list.Contains(item);
		}

		/// <summary>
		///     Determines whether an element is in the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="value">
		///     The object to locate in the <see cref="ScriptableList{T}" />. The value can be <c>null</c> for
		///     reference types.
		/// </param>
		/// <returns><c>true</c> if the item is found in the <see cref="ScriptableList{T}" />; otherwise, <c>false</c>.</returns>
		bool IList.Contains(object? value)
		{
			// Check if the value is the same type as the generic type and then call the Contains method.
			return EqualityHelper.IsSameType(value, out T? newValue) && Contains(newValue);
		}

		/// <summary>
		///     Converts the elements in the current <see cref="ScriptableList{T}" /> to another type, and returns a list
		///     containing the converted elements.
		/// </summary>
		/// <param name="converter">
		///     A <see cref="Converter{TInput,TOutput}" /> delegate that converts each element from one type to another type.
		/// </param>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
		/// <returns>
		///     A <see cref="List{TOutput}" /> of the target type containing the converted elements from the current
		///     <see cref="ScriptableList{T}" />.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="converter" /> is <c>null</c>.</exception>
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
		{
			return list.ConvertAll(converter);
		}

		/// <summary>
		///     Converts the elements in the current list to another type, and copies the converted elements to the specified list.
		/// </summary>
		/// <param name="destinationList">The destination list where the converted items will be copied to.</param>
		/// <param name="converter">
		///     A <see cref="Converter{TInput,TOutput}" /> delegate that converts each element from one type to another type.
		/// </param>
		/// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
		/// <exception cref="ArgumentNullException"><c>converter</c> is <c>null</c>.</exception>
		public void ConvertAll<TOutput>(IList<TOutput> destinationList, Converter<T, TOutput> converter)
		{
			ThrowHelper.ThrowIfNull(converter, nameof(converter));
			ThrowHelper.ThrowIfNull(destinationList, nameof(destinationList));

			destinationList.Clear();

			// Make sure the capacity is at least the same as the internal count.
			destinationList.TryEnsureCapacity(internalCount);

			for (int i = 0; i < internalCount; i++)
			{
				destinationList.Add(converter(list[i]));
			}
		}

		/// <summary>
		///     Copies the entire <see cref="ScriptableList{T}" /> to a compatible one-dimensional array, starting at the beginning
		///     of the target array.
		/// </summary>
		/// <param name="array">
		///     The one-dimensional array that is the destination of the elements copied from
		///     <see cref="ScriptableList{T}" />. The array must have zero-based indexing.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="array" /> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">
		///     The number of elements in the source <see cref="ScriptableList{T}" /> is greater
		///     than the number of elements that the destination array can contain.
		/// </exception>
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
		///     Determines whether the <see cref="ScriptableList{T}" /> contains elements that match the conditions defined by the
		///     specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		///     <c>true</c> if the <see cref="ScriptableList{T}" /> contains one or more elements that match the conditions defined by the specified predicate; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="match" /> is <c>null</c>.</exception>
		public bool Exists(Predicate<T> match)
		{
			return list.Exists(match);
		}

		/// <summary>
		///     Searches for an element that matches the conditions defined by the specified predicate, and returns the first
		///     occurrence within the entire <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the element to search for.</param>
		/// <returns>
		///     The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default
		///     value for type <typeparamref name="T" />.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="match" /> is <c>null</c>.</exception>
		public T? Find(Predicate<T> match)
		{
			return list.Find(match);
		}

		/// <summary>
		///     Retrieves all the elements that match the conditions defined by the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements to search for.</param>
		/// <returns>
		///     A <see cref="List{T}" /> containing all the elements that match the conditions defined by the specified predicate,
		///     if found; otherwise, an empty <see cref="List{T}" />
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
		///     The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>, if
		///     found; otherwise, -1.
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
		///     The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>, if
		///     found; otherwise, -1.
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
		///     The zero-based index of the first occurrence of an element that matches the conditions defined by <c>match</c>, if
		///     found; otherwise, -1.
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
		///     The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default
		///     value for type <c>T</c>.
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

		/// <summary>
		///     Creates a shallow copy of a range of elements in the source list.
		/// </summary>
		/// <param name="index">The zero-based list index at which the range starts.</param>
		/// <param name="count">The number of elements in the range.</param>
		/// <returns>A shallow copy of a range of elements in the source list.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0. Or <c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range of elements in the list.</exception>
		public List<T> GetRange(int index, int count)
		{
			return list.GetRange(index, count);
		}

		/// <summary>
		///     Copies a range of elements from the source list to the specified destination list.
		/// </summary>
		/// <param name="index">The zero-based list index at which the range starts.</param>
		/// <param name="count">The number of elements in the range.</param>
		/// <param name="destinationList">The list where the copied elements will be stored.</param>
		/// <exception cref="ArgumentNullException"><c>destinationList</c> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is less than 0. Or <c>count</c> is less than 0.</exception>
		/// <exception cref="ArgumentException"><c>index</c> and <c>count</c> do not denote a valid range of elements in the list.</exception>
		public void GetRange(int index, int count, IList<T> destinationList)
		{
			ThrowHelper.ThrowIfNull(destinationList, nameof(destinationList));
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			if (internalCount - index < count)
			{
				throw new ArgumentException(nameof(index) + " and " + nameof(index) + " do not denote a valid range of elements in the list.");
			}

			destinationList.Clear();

			// Make sure the capacity is at least the same as the internal count.
			destinationList.TryEnsureCapacity(count);

			for (int i = index; i < index + count; i++)
			{
				destinationList.Add(list[i]);
			}
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
		///     Searches for the specified object and returns the zero-based index of the first occurrence within the range of
		///     elements in the list that extends from the specified index to the last element.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
		/// <returns>
		///     The zero-based index of the first occurrence of <c>item</c> within the range of elements in the list that
		///     extends from <c>index</c> to the last element, if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the list.</exception>
		public int IndexOf(T item, int index)
		{
			return list.IndexOf(item, index);
		}

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the first occurrence within the range of
		///     elements in the list that starts at the specified index and contains the specified number of elements.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>
		///     The zero-based index of the first occurrence of <c>item</c> within the range of elements in the list that
		///     extends from <c>index</c> to the last element, if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <c>index</c> is outside the range of valid indexes for the list. Or
		///     <c>count</c> is less than 0. Or <c>index</c> and <c>count</c> do not specify a valid section in the list.
		/// </exception>
		public int IndexOf(T item, int index, int count)
		{
			return list.IndexOf(item, index, count);
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
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public void Insert(int index, T item)
		{
			InsertInternal(index, item);
		}

		/// <summary>
		///     Inserts an element into the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="value">The item to insert.</param>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the last occurrence within the entire list.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <returns>
		///     The zero-based index of the last occurrence of <c>item</c> within the entire the list, if found; otherwise, -1.
		/// </returns>
		public int LastIndexOf(T item)
		{
			return list.LastIndexOf(item);
		}

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the last occurrence within the range of
		///     elements in the list that extends from the first element to the specified index.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <param name="index">The zero-based starting index of the backward search.</param>
		/// <returns>
		///     The zero-based index of the last occurrence of <c>item</c> within the range of elements in the list that
		///     extends from the first element to <c>index</c>, if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>index</c> is outside the range of valid indexes for the list.</exception>
		public int LastIndexOf(T item, int index)
		{
			return list.LastIndexOf(item, index);
		}

		/// <summary>
		///     Searches for the specified object and returns the zero-based index of the last occurrence within the range of
		///     elements in the list that contains the specified number of elements and ends at the specified index.
		/// </summary>
		/// <param name="item">The object to locate in the list.</param>
		/// <param name="index">The zero-based starting index of the backward search.</param>
		/// <param name="count">The number of elements in the section to search.</param>
		/// <returns>
		///     The zero-based index of the last occurrence of <c>item</c> within the range of elements in the list that
		///     contains <c>count</c> number of elements and ends at <c>index</c>, if found; otherwise, -1.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <c>index</c> is outside the range of valid indexes for the list. Or
		///     <c>count</c> is less than 0. Or <c>index</c> and <c>count</c> do not specify a valid section in the list.
		/// </exception>
		public int LastIndexOf(T item, int index, int count)
		{
			return list.LastIndexOf(item, index, count);
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="item">
		///     The object to remove from the <see cref="ScriptableList{T}" />. The value can be <c>null</c> for
		///     reference types.
		/// </param>
		/// <returns>
		///     <c>true</c> if item was successfully removed from the <see cref="ScriptableList{T}" />;
		///     otherwise, <c>false</c>. This method also returns <c>false</c> if item is not found in the original
		///     <see cref="ScriptableList{T}" />.
		/// </returns>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public bool Remove(T item)
		{
			return RemoveInternal(item);
		}

		/// <summary>
		///     Removes the first occurrence of a specific object from the list.
		/// </summary>
		/// <param name="value">The object to remove from the list.</param>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions of the elements to remove.</param>
		/// <returns>The number of elements removed from the <see cref="ScriptableList{T}" />.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="match" /> is <c>null</c>.</exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public int RemoveAll(Predicate<T> match)
		{
			ThrowHelper.ThrowIfNull(match, nameof(match));

			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			using SpanOwner<T> removed = SpanOwner<T>.Allocate(list.Count);
			int removeLength = RemoveAllWithPredicate(removed.Span, out int firstIndex, match);

			if (removeLength > 0)
			{
				int removeCount = list.RemoveAll(match);
				InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(removed.Span.Slice(0, removeLength), firstIndex));

				Assert.AreEqual(removeLength, removeCount, "The expected count of removed items is not the same as the actually removed items count.");

				UpdateCounts();
				return removeCount;
			}

			return 0;
		}

		private int RemoveAllWithPredicate(Span<T> removedBuffer, out int firstIndex, Predicate<T> match)
		{
			// Keeps track of the number of items removed.
			int removeLength = 0;
			// Keeps track of the first index of the item that was removed.
			firstIndex = -1;
			for (int i = 0; i < list.Count; i++)
			{
				if (match(list[i]))
				{
					// If the item was removed, and we haven't set the first index yet, set it.
					if (firstIndex == -1)
					{
						firstIndex = i;
					}

					removedBuffer[removeLength] = list[i];
					removeLength++;
				}
			}

			return removeLength;
		}

		/// <summary>
		///     Removes the element at the specified index of the <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///     <paramref name="index" /> is less than 0.
		///     -or-
		///     <paramref name="index" /> is equal to or greater than <see cref="Count" />.
		/// </exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
				using SpanOwner<T> removed = SpanOwner<T>.Allocate(countToRemove);
				Span<T> removedSpan = removed.Span;

				for (int i = 0; i < countToRemove; i++)
				{
					removedSpan[i] = list[index + i];
				}

				list.RemoveRange(index, count);

				UpdateCounts();
				InvokeCollectionChanged(CollectionChangedArgs<T>.Remove(removedSpan, index));
			}

			AddStackTrace();
		}

		/// <summary>
		///     Reverses the order of the elements in the entire <see cref="ScriptableList{T}" />.
		/// </summary>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public void Reverse()
		{
			ReverseInternal(0, list.Count);
		}

		/// <summary>
		///     Reverses the order of the elements in the specified range.
		/// </summary>
		/// <param name="index">The starting index of the range to reverse.</param>
		/// <param name="count">The number of elements in the range to reverse.</param>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public void Reverse(int index, int count)
		{
			ReverseInternal(index, count);
		}

		/// <summary>
		///     Internal reverse method that skips a frame in the stack traces.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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

		/// <inheritdoc cref="GetRange(int,int)" />
		public List<T> Slice(int index, int count)
		{
			return GetRange(index, count);
		}

		/// <inheritdoc cref="GetRange(int,int,IList{T})" />
		public void Slice(int index, int count, IList<T> destinationList)
		{
			GetRange(index, count, destinationList);
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
		///     Sorts the elements in the entire <see cref="ScriptableList{T}" /> using the specified comparer.
		/// </summary>
		/// <param name="comparer">
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or <c>null</c> to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		/// <exception cref="InvalidOperationException">
		///     <paramref name="comparer" /> is <c>null</c>, and the default comparer cannot find an
		///     implementation of the <see cref="IComparer{T}" /> generic interface or the <see cref="IComparable" /> interface for
		///     type <typeparamref name="T" />.
		/// </exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		///     The <see cref="IComparer{T}" /> implementation to use when comparing elements, or <c>null</c> to use
		///     the default comparer <see cref="Comparer{T}.Default" />.
		/// </param>
		/// <exception cref="InvalidOperationException">
		///     <paramref name="comparer" /> is <c>null</c>, and the default comparer cannot find an
		///     implementation of the <see cref="IComparer{T}" /> generic interface or the <see cref="IComparable" /> interface for
		///     type <typeparamref name="T" />.
		/// </exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
		public void Sort(int index, int count, IComparer<T>? comparer = null)
		{
			SortInternal(index, count, comparer, null);
		}

		/// <summary>
		///     Sorts the elements in the entire list using the specified <see cref="Comparison{T}" />.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparison{T}" /> to use when comparing elements.</param>
		/// <exception cref="ArgumentNullException"><paramref name="comparison" /> is <c>null</c>.</exception>
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		/// <exception cref="System.Data.ReadOnlyException">If the object is read-only and the application is playing.</exception>
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
		///     Copies the elements of the <see cref="ScriptableList{T}" /> to a new array.
		/// </summary>
		/// <returns>An array containing copies of the elements of the <see cref="ScriptableList{T}" />.</returns>
		public T[] ToArray()
		{
			return list.ToArray();
		}

		/// <summary>
		///     Sets the capacity to the actual number of elements in the <see cref="ScriptableList{T}" />, if that number is less
		///     than a threshold value.
		/// </summary>
		public void TrimExcess()
		{
			list.TrimExcess();
			UpdateCounts();

			AddStackTrace();
		}

		/// <summary>
		///     Determines whether every element in the <see cref="ScriptableList{T}" /> matches the conditions defined by the
		///     specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the conditions to check against the elements.</param>
		/// <returns>
		///     <c>true</c> if every element in the <see cref="ScriptableList{T}" /> matches the conditions defined by the
		///     specified predicate; otherwise, <c>false</c>. If the list has no elements, the return value is <c>true</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="match" /> is <c>null</c>.</exception>
		public bool TrueForAll(Predicate<T> match)
		{
			return list.TrueForAll(match);
		}

		/// <summary>
		///     Tries to find an element in the list that matches the specified predicate.
		/// </summary>
		/// <param name="match">The <see cref="Predicate{T}" /> delegate that defines the condition to check against.</param>
		/// <param name="result">The first result that matches the delegate. If no element was found, this will be the default value.</param>
		/// <returns><c>true</c> if an element was found; otherwise, <c>false</c>.</returns>
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

		private void InvokeCollectionChanged(in CollectionChangedArgs<T> args)
		{
			OnCollectionChanged?.Invoke(args);
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
			ClearSubscribers();

			if (clearOnStart && !isReadOnly)
			{
				list.Clear();
			}

			internalCount = list.Count;
			internalCapacity = list.Capacity;
		}

		/// <summary>
		///     Warns if there are any left-over subscribers to the events.
		/// </summary>
		/// <remarks>This will only be called in the Unity editor and builds with the DEBUG flag.</remarks>
		protected override void WarnIfLeftOverSubscribers()
		{
			base.WarnIfLeftOverSubscribers();
			EventHelper.WarnIfLeftOverSubscribers(OnCollectionChanged, nameof(OnCollectionChanged), this);
			EventHelper.WarnIfLeftOverSubscribers(OnInternalCollectionChanged, "INotifyCollectionChanged.CollectionChanged", this);
		}

		/// <summary>
		///     Warns if there are any left-over objects in the dictionary.
		/// </summary>
		/// <remarks>This will only be called in the Unity editor and builds with the DEBUG flag.</remarks>
		[Conditional("DEBUG")]
		protected void WarnIfLeftOverObjects()
		{
			if (!isReadOnly && clearOnStart && list.Count > 0)
			{
				Debug.LogWarning($"There are left over objects in the scriptable list {name}. You should clear the list before leaving play mode.");
			}
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
				WarnIfLeftOverSubscribers();
			}
#endif
			OnCollectionChanged = null;
			OnInternalCollectionChanged = null;
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			WarnIfLeftOverObjects();
			WarnIfLeftOverSubscribers();

			list.TrimExcess();
		}
#endif

		#region Obsolete
#if UNITY_EDITOR // Don't include in build.
#pragma warning disable CS0067 // Event is never used
		/// <summary>
		///     Called when something was added. Gives you the newly added item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action<T>? OnAdded;
		/// <summary>
		///     Called when something was inserted. Gives you the index it was inserted at and the newly inserted item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action<int, T>? OnInserted;
		/// <summary>
		///     Called when something was added or inserted. Gives you the index it was added/inserted at and the newly
		///     added/inserted item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action<int, T>? OnAddedOrInserted;
		/// <summary>
		///     Called when something was set using the indexer. Gives you the index it was set at, the old value and the new
		///     value.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action<int, T, T>? OnSet;
		/// <summary>
		///     Called when something was removed. Gives you the index it was removed at and the removed item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action<int, T>? OnRemoved;
		/// <summary>
		///     Called when the list is cleared.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action? OnCleared;
		/// <summary>
		///     Called when the list is changed in any way.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' instead. This will be removed in build.", true)]
		public event Action<ListChangeType>? OnChanged;
#pragma warning restore CS0067 // Event is never used
#endif // UNITY_EDITOR
		#endregion
	}
}