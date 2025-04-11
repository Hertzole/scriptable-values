#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
	///     Base class for a scriptable object holds a dictionary.
	/// </summary>
	public abstract class ScriptableDictionary : RuntimeScriptableObject, ICanBeReadOnly
	{
		public static readonly PropertyChangedEventArgs isReadOnlyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsReadOnly));
		public static readonly PropertyChangingEventArgs isReadOnlyChangingEventArgs = new PropertyChangingEventArgs(nameof(IsReadOnly));

		public static readonly PropertyChangedEventArgs setEqualityCheckChangedEventArgs = new PropertyChangedEventArgs(nameof(SetEqualityCheck));
		public static readonly PropertyChangingEventArgs setEqualityCheckChangingEventArgs = new PropertyChangingEventArgs(nameof(SetEqualityCheck));

		public static readonly PropertyChangedEventArgs clearOnStartChangedEventArgs = new PropertyChangedEventArgs(nameof(ClearOnStart));
		public static readonly PropertyChangingEventArgs clearOnStartChangingEventArgs = new PropertyChangingEventArgs(nameof(ClearOnStart));

		public static readonly PropertyChangedEventArgs countChangedEventArgs = new PropertyChangedEventArgs(nameof(Count));
		public static readonly PropertyChangingEventArgs countChangingEventArgs = new PropertyChangingEventArgs(nameof(Count));

#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool SetEqualityCheck { get; set; }
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool ClearOnStart { get; set; }
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract int Count { get; protected set; }

		/// <inheritdoc />
#if SCRIPTABLE_VALUES_PROPERTIES
		[CreateProperty]
#endif
		public abstract bool IsReadOnly { get; set; }

		internal virtual bool IsValid()
		{
			return false;
		}

		internal virtual bool IsIndexUnique(int index)
		{
			return false;
		}
	}

	/// <summary>
	///     A scriptable object that holds a dictionary.
	/// </summary>
	/// <typeparam name="TKey">The key of the dictionary.</typeparam>
	/// <typeparam name="TValue">The value of the dictionary.</typeparam>
	public abstract partial class ScriptableDictionary<TKey, TValue> : ScriptableDictionary,
		ISerializationCallbackReceiver,
		IDictionary<TKey, TValue>,
		IReadOnlyDictionary<TKey, TValue>,
		INotifyCollectionChanged,
		INotifyScriptableCollectionChanged<KeyValuePair<TKey, TValue>>,
		IDictionary where TKey : notnull
	{
		[SerializeField]
		[EditorTooltip("If read only, the dictionary cannot be changed at runtime and won't be cleared on start.")]
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
		[EditorTooltip("If true, the dictionary will be cleared on play mode start/game boot.")]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal bool clearOnStart = true;

		[SerializeField]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal List<TKey> keys = new List<TKey>();
		[SerializeField]
#if SCRIPTABLE_VALUES_PROPERTIES
		[DontCreateProperty]
#endif
		internal List<TValue> values = new List<TValue>();

		private int count = 0;

		private readonly DelegateHandlerList<CollectionChangedEventHandler<KeyValuePair<TKey, TValue>>, CollectionChangedArgs<KeyValuePair<TKey, TValue>>>
			onCollectionChanged =
				new DelegateHandlerList<CollectionChangedEventHandler<KeyValuePair<TKey, TValue>>, CollectionChangedArgs<KeyValuePair<TKey, TValue>>>();

		internal Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

		object? IDictionary.this[object key]
		{
			get { return EqualityHelper.IsSameType(key, out TKey? newKey) ? dictionary[newKey] : null; }
			set
			{
				ThrowHelper.ThrowIfNull(key, nameof(key));
				ThrowHelper.ThrowIfNullAndNullsAreIllegal<TValue>(value, nameof(value));

				try
				{
					TKey tempKey = (TKey) key;
					try
					{
						SetValue(tempKey, (TValue) value!);
					}
					catch (InvalidCastException)
					{
						ThrowHelper.ThrowWrongExpectedValueType<TValue>(value);
					}
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongExpectedValueType<TKey>(key);
				}
			}
		}

		public TValue this[TKey key]
		{
			get { return dictionary[key]; }
			set { SetValue(key, value); }
		}

		/// <summary>
		///     Gets or sets the <see cref="IEqualityComparer{T}" /> that is used to determine equality of keys for the dictionary.
		/// </summary>
		public IEqualityComparer<TKey> Comparer
		{
			get { return dictionary.Comparer; }
			set
			{
				// If you update the comparer, we need to re-create the whole dictionary.
				// Make sure it's a new comparer than the old one.
				if (!EqualityComparer<IEqualityComparer<TKey>>.Default.Equals(dictionary.Comparer, value))
				{
					Dictionary<TKey, TValue> newDictionary =
						value == null ? new Dictionary<TKey, TValue>(dictionary) : new Dictionary<TKey, TValue>(dictionary, value);

					dictionary = newDictionary;
				}
			}
		}

		/// <summary>
		///     Gets the number of key/value pairs contained in the dictionary.
		/// </summary>
		public sealed override int Count
		{
			get
			{
				Assert.AreEqual(dictionary.Count, count, "Dictionary count is not the same as the internal count.");
				return count;
			}
			protected set
			{
				SetField(ref count, value, countChangingEventArgs, countChangedEventArgs);
				Assert.AreEqual(dictionary.Count, count);
			}
		}

		/// <summary>
		///     Gets a collection containing the keys in the dictionary.
		/// </summary>
		public Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get { return dictionary.Keys; }
		}

		/// <summary>
		///     Gets a collection containing the values in the dictionary.
		/// </summary>
		public Dictionary<TKey, TValue>.ValueCollection Values
		{
			get { return dictionary.Values; }
		}

		/// <summary>
		///     If read only, the dictionary cannot be changed at runtime and won't be cleared on start.
		/// </summary>
		public override bool IsReadOnly
		{
			get { return isReadOnly; }
			set { SetField(ref isReadOnly, value, isReadOnlyChangingEventArgs, isReadOnlyChangedEventArgs); }
		}

		/// <summary>
		///     If true, an equality check will be run before setting an item through the indexer to make sure the new object is
		///     not the same as the old one.
		/// </summary>
		public override bool SetEqualityCheck
		{
			get { return setEqualityCheck; }
			set { SetField(ref setEqualityCheck, value, setEqualityCheckChangingEventArgs, setEqualityCheckChangedEventArgs); }
		}

		/// <summary>
		///     If true, the dictionary will be cleared on play mode start/game boot.
		/// </summary>
		public override bool ClearOnStart
		{
			get { return clearOnStart; }
			set { SetField(ref clearOnStart, value, clearOnStartChangingEventArgs, clearOnStartChangedEventArgs); }
		}

		bool IDictionary.IsFixedSize
		{
			get { return isReadOnly; }
		}
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}
		object ICollection.SyncRoot
		{
			get { return this; }
		}
		ICollection IDictionary.Values
		{
			get { return dictionary.Values; }
		}
		ICollection IDictionary.Keys
		{
			get { return dictionary.Keys; }
		}
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get { return dictionary.Keys; }
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get { return dictionary.Values; }
		}

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get { return dictionary.Keys; }
		}
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get { return dictionary.Values; }
		}

		// Internal event for the INotifyCollectionChanged interface as we don't want to expose that event directly.
		private event NotifyCollectionChangedEventHandler? OnInternalCollectionChanged;

		/// <inheritdoc />
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add { OnInternalCollectionChanged += value; }
			remove { OnInternalCollectionChanged -= value; }
		}

		/// <summary>
		///     Occurs when an item is added, removed, replaced, or the entire dictionary is refreshed.
		/// </summary>
		public event CollectionChangedEventHandler<KeyValuePair<TKey, TValue>> OnCollectionChanged
		{
			add { RegisterChangedListener(value); }
			remove { UnregisterChangedListener(value); }
		}

		/// <summary>
		///     Checks if the dictionary is a valid dictionary by checking the keys and values.
		/// </summary>
		/// <returns>True if the dictionary is valid; otherwise, false.</returns>
		internal override bool IsValid()
		{
			// If the keys and values are not the same length, the dictionary is invalid.
			if (keys.Count != values.Count)
			{
				return false;
			}

			// Check for duplicate keys.
			for (int i = 0; i < keys.Count; i++)
			{
				for (int j = i; j < keys.Count; j++)
				{
					// Ignore the same index.
					if (i == j)
					{
						continue;
					}

					// There's a duplicate key, the dictionary is invalid.
					if (Comparer.Equals(keys[i], keys[j]))
					{
						return false;
					}
				}
			}

			// The dictionary was valid.
			return true;
		}

		/// <summary>
		///     Checks if a key at a specific index is unique.
		/// </summary>
		/// <param name="index">The index to check.</param>
		/// <returns>True if the index is unique; otherwise, false.</returns>
		internal override bool IsIndexUnique(int index)
		{
			// Check for duplicate keys with the key at index.
			for (int i = 0; i < keys.Count; i++)
			{
				// Ignore the same index.
				if (i == index)
				{
					continue;
				}

				// A duplicate key was found, the index is invalid.
				if (Comparer.Equals(keys[i], keys[index]))
				{
					return false;
				}
			}

			// The index was valid.
			return true;
		}

		/// <summary>
		///     Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Add(TKey key, TValue value)
		{
			AddInternal(key, value);
		}

		/// <summary>
		///     Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		void IDictionary.Add(object key, object? value)
		{
			ThrowHelper.ThrowIfNullAndNullsAreIllegal<TKey>(key, nameof(key));
			ThrowHelper.ThrowIfNullAndNullsAreIllegal<TValue>(value, nameof(value));

			try
			{
				TKey tempKey = (TKey) key;

				try
				{
					AddInternal(tempKey, (TValue) value!);
				}
				catch (InvalidCastException)
				{
					ThrowHelper.ThrowWrongExpectedValueType<TValue>(value);
				}
			}
			catch (InvalidCastException)
			{
				ThrowHelper.ThrowWrongExpectedValueType<TKey>(key);
			}
		}

		/// <summary>
		///     Adds a key/value pair to the dictionary.
		/// </summary>
		/// <param name="item">The key/value pair to add.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			AddInternal(item.Key, item.Value);
		}

		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		private void AddInternal(TKey key, TValue value)
		{
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			dictionary.Add(key, value);

			keys.Add(key);
			values.Add(value);

			Count = dictionary.Count;

			Assert.AreEqual(dictionary.Keys.Count, keys.Count);
			Assert.AreEqual(dictionary.Values.Count, values.Count);

			InvokeCollectionChanged(CollectionChangedArgs<KeyValuePair<TKey, TValue>>.Add(new KeyValuePair<TKey, TValue>(key, value), -1));

			AddStackTrace(1);
		}

		/// <summary>
		///     Removes all elements from the dictionary.
		/// </summary>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public void Clear()
		{
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			if (Count == 0)
			{
				return;
			}

			using (CollectionScope<KeyValuePair<TKey, TValue>> scope = new CollectionScope<KeyValuePair<TKey, TValue>>(dictionary))
			{
				dictionary.Clear();

				keys.Clear();
				values.Clear();

				Count = 0;

				InvokeCollectionChanged(CollectionChangedArgs<KeyValuePair<TKey, TValue>>.Clear(scope.Span, -1, -1));

				AddStackTrace();
			}
		}

		/// <summary>
		///     Determines whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the dictionary contains the key; otherwise, false.</returns>
		public bool ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

		/// <summary>
		///     Determines whether the dictionary contains a specific value.
		/// </summary>
		/// <param name="value">The value to locate in the dictionary.</param>
		/// <returns>True if the dictionary contains the value; otherwise, false.</returns>
		public bool ContainsValue(TValue value)
		{
			return dictionary.ContainsValue(value);
		}

		/// <summary>
		///     Determines whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the key type is the same as the generic type and the dictionary contains the key; otherwise, false.</returns>
		bool IDictionary.Contains(object key)
		{
			return EqualityHelper.IsSameType(key, out TKey? newKey) && ContainsKey(newKey);
		}

		/// <summary>
		///     Determines if the dictionary contains the specified key/value pair.
		/// </summary>
		/// <param name="item">The key value pair to check.</param>
		/// <returns>True if the key and value both exist in the dictionary; otherwise, false.</returns>
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return dictionary.ContainsKey(item.Key) && dictionary.ContainsValue(item.Value);
		}

		/// <summary>
		///     Copies the elements of the dictionary to an <see cref="Array" />, starting at a particular array index.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="index">The index at which copying beings.</param>
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection) dictionary).CopyTo(array, index);
		}

		/// <summary>
		///     Copies the elements of the dictionary to an <see cref="Array" />, starting at a particular array index.
		/// </summary>
		/// <param name="array">The destination array.</param>
		/// <param name="arrayIndex">The index at which copying beings.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>) dictionary).CopyTo(array, arrayIndex);
		}

		/// <summary>
		///     Ensures that the dictionary can hold up to a specified number of entries without any further expansion of its
		///     backing storage.
		/// </summary>
		/// <param name="capacity">The number of entries.</param>
		/// <returns>The current capacity of the dictionary.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><c>capacity</c> is less than 0.</exception>
		public int EnsureCapacity(int capacity)
		{
			int result = dictionary.EnsureCapacity(capacity);
			if (keys.Capacity < capacity)
			{
				keys.Capacity = capacity;
			}

			if (values.Capacity < capacity)
			{
				values.Capacity = capacity;
			}

			return result;
		}

		/// <summary>
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator" /> for the dictionary.</returns>
		public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return dictionary.GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator" /> for the dictionary.</returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary) dictionary).GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator" /> for the dictionary.</returns>
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) dictionary).GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>A <see cref="Dictionary{TKey, TValue}.Enumerator" /> for the dictionary.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) dictionary).GetEnumerator();
		}

		/// <summary>
		///     Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		///     True if the element was found and removed; otherwise, false. This method returns false if the key is not found
		///     in the dictionary.
		/// </returns>
		/// <inheritdoc cref="RemoveInternal" path="exception" />
		public bool Remove(TKey key)
		{
			return RemoveInternal(key, out _);
		}

		/// <summary>
		///     Removes the value with the specified key from the dictionary, and copies the element to the <c>value</c> parameter.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <param name="value">The removed element.</param>
		/// <returns><c>true</c> if the element is successfully found and removed; otherwise, <c>false</c>.</returns>
		/// <inheritdoc cref="RemoveInternal" path="exception" />
		public bool Remove(TKey key, out TValue value)
		{
			return RemoveInternal(key, out value);
		}

		/// <summary>
		///     Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		void IDictionary.Remove(object key)
		{
			if (EqualityHelper.IsSameType(key, out TKey? newKey))
			{
				RemoveInternal(newKey, out _);
			}
		}

		/// <summary>
		///     Removes a key/value pair from the dictionary.
		/// </summary>
		/// <param name="item">The key/value pair to remove.</param>
		/// <returns>True if the key/value pair was found and removed; otherwise, false.</returns>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			if (dictionary.TryGetValue(item.Key, out TValue value) && EqualityHelper.Equals(item.Value, value))
			{
				return RemoveInternal(item.Key, out _);
			}

			return false;
		}

		/// <exception cref="ArgumentNullException"><c>key</c> is <c>null</c>.</exception>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		private bool RemoveInternal(TKey key, out TValue value)
		{
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			bool removed = dictionary.Remove(key, out value);
			if (removed)
			{
				keys.Remove(key);
				values.Remove(value);

				Count = dictionary.Count;

				Assert.AreEqual(dictionary.Keys.Count, keys.Count);
				Assert.AreEqual(dictionary.Values.Count, values.Count);

				InvokeCollectionChanged(CollectionChangedArgs<KeyValuePair<TKey, TValue>>.Remove(new KeyValuePair<TKey, TValue>(key, value), -1));

				AddStackTrace(1);
			}

			return removed;
		}

		/// <summary>
		///     Sets or adds a value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to set.</param>
		/// <param name="value">The new value to set.</param>
		private void SetValue(TKey key, TValue value)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			// Try to add it first. If it fails, we update the value.
			if (!TryAddInternal(key, value))
			{
				// Get the old value from the dictionary.
				TValue? oldValue = dictionary[key];

				// If the equality check is enabled, we don't want to set the value if it's the same as the current value.
				if (setEqualityCheck && EqualityHelper.Equals(oldValue, value))
				{
					return;
				}

				// Get the index of the value.
				int valueIndex = values.IndexOf(oldValue);
				Assert.AreNotEqual(-1, valueIndex, "The value was not found in the values list.");

				// Update the value in the values list.
				values[valueIndex] = value;

				// Update the value in the dictionary.
				dictionary[key] = value;

				KeyValuePair<TKey, TValue> oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
				KeyValuePair<TKey, TValue> newItem = new KeyValuePair<TKey, TValue>(key, value);

				InvokeCollectionChanged(CollectionChangedArgs<KeyValuePair<TKey, TValue>>.Replace(oldItem, newItem, -1));

				AddStackTrace();
			}
		}

		/// <summary>
		///     Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its
		///     entries.
		/// </summary>
		public void TrimExcess()
		{
			dictionary.TrimExcess();

			keys.TrimExcess();
			values.TrimExcess();

			AddStackTrace();
		}

		/// <summary>
		///     Sets the capacity of this dictionary to hold up a specified number of entries without any further expansion of its
		///     backing storage.
		/// </summary>
		/// <param name="capacity">The new capacity.</param>
		public void TrimExcess(int capacity)
		{
			dictionary.TrimExcess(capacity);

			keys.TrimExcess();
			values.TrimExcess();

			AddStackTrace();
		}

		/// <summary>
		///     Attempts to add the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add-</param>
		/// <param name="value">The value of the element to add.</param>
		/// <returns>True if the key/value pair was added to the dictionary; otherwise, false.</returns>
		/// <inheritdoc cref="ThrowHelper.ThrowIfIsReadOnly" path="exception" />
		public bool TryAdd(TKey key, TValue value)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			ThrowHelper.ThrowIfIsReadOnly(in isReadOnly, this);

			return TryAddInternal(key, value);
		}

		private bool TryAddInternal(TKey key, TValue value)
		{
			bool result = dictionary.TryAdd(key, value);

			if (result)
			{
				// We must update the lists too so they are in sync with the dictionary.
				keys.Add(key);
				values.Add(value);

				Count = dictionary.Count;

				Assert.AreEqual(dictionary.Keys.Count, keys.Count);
				Assert.AreEqual(dictionary.Values.Count, values.Count);

				InvokeCollectionChanged(CollectionChangedArgs<KeyValuePair<TKey, TValue>>.Add(new KeyValuePair<TKey, TValue>(key, value), -1));

				AddStackTrace(1);
			}

			return result;
		}

		/// <summary>
		///     Tries to find a key in the dictionary.
		/// </summary>
		/// <param name="predicate">The <see cref="Predicate{T}" /> to check against.</param>
		/// <param name="key">The key if it was found. Will be the default value if it wasn't found.</param>
		/// <returns>True if the key was found; otherwise, false.</returns>
		public bool TryFindKey(Predicate<TKey> predicate, [NotNullWhen(true)] out TKey? key)
		{
			ThrowHelper.ThrowIfNull(predicate, nameof(predicate));

			foreach (TKey dictionaryKey in dictionary.Keys)
			{
				if (predicate(dictionaryKey))
				{
					key = dictionaryKey;
					return true;
				}
			}

			key = default;
			return false;
		}

		/// <summary>
		///     Tries to find a value in the dictionary.
		/// </summary>
		/// <param name="predicate">The <see cref="Predicate{T}" /> to check against.</param>
		/// <param name="value">The value if it was found. Will be the default value if it wasn't found.</param>
		/// <returns>True if the key was found; otherwise, false.</returns>
		public bool TryFindValue(Predicate<TValue> predicate, out TValue? value)
		{
			ThrowHelper.ThrowIfNull(predicate, nameof(predicate));

			foreach (TValue dictionaryValue in dictionary.Values)
			{
				if (predicate(dictionaryValue))
				{
					value = dictionaryValue;
					return true;
				}
			}

			value = default;
			return false;
		}

		/// <summary>
		///     Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the element to get.</param>
		/// <param name="value">The value if it was found. Will be the default value if it wasn't found.</param>
		/// <returns>True if the dictionary contains an element with the specified key; otherwise, false.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		/// <inheritdoc />
		public void RegisterChangedListener(CollectionChangedEventHandler<KeyValuePair<TKey, TValue>> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RegisterCallback(callback);
		}

		/// <inheritdoc />
		public void RegisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<KeyValuePair<TKey, TValue>, TContext> callback, TContext context)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));
			ThrowHelper.ThrowIfNull(context, nameof(context));

			onCollectionChanged.RegisterCallback(callback, context);
		}

		/// <inheritdoc />
		public void UnregisterChangedListener(CollectionChangedEventHandler<KeyValuePair<TKey, TValue>> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RemoveCallback(callback);
		}

		/// <inheritdoc />
		public void UnregisterChangedListener<TContext>(CollectionChangedWithContextEventHandler<KeyValuePair<TKey, TValue>, TContext> callback)
		{
			ThrowHelper.ThrowIfNull(callback, nameof(callback));

			onCollectionChanged.RemoveCallback(callback);
		}

		private void InvokeCollectionChanged(CollectionChangedArgs<KeyValuePair<TKey, TValue>> args)
		{
			onCollectionChanged.Invoke(args);
			OnInternalCollectionChanged?.Invoke(this, args.ToNotifyCollectionChangedEventArgs());
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			// Does nothing.
		}

		/// <summary>
		///     Called after Unity has deserialized this type.
		/// </summary>
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
#if DEBUG
			// If the dictionary is invalid, stop here.
			if (!IsValid())
			{
				return;
			}
#endif

			// Update the dictionary with the serialized keys and values.
			dictionary.Clear();
			dictionary.EnsureCapacity(keys.Count);

			for (int i = 0; i < keys.Count; i++)
			{
				dictionary.Add(keys[i], values[i]);
			}

			Count = dictionary.Count;
		}

		/// <inheritdoc />
		protected override void OnStart()
		{
#if UNITY_EDITOR
			ResetStackTraces();
#endif

			ClearSubscribers();

			if (!isReadOnly && clearOnStart)
			{
				dictionary.Clear();
				keys.Clear();
				values.Clear();
				Count = 0;
			}
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
			if (!isReadOnly && clearOnStart && dictionary.Count > 0)
			{
				Debug.LogWarning($"There are left over objects in the scriptable dictionary {name}. You should clear the dictionary before leaving play mode.");
			}

			WarnLeftOverSubscribers();

			dictionary.TrimExcess();
		}
#endif

		#region Obsolete
#if UNITY_EDITOR // Don't include in builds.
		/// <summary>
		///     Called when an item was added. Gives you the key and value of the newly added item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
#pragma warning disable CS0067 // Event is never used
		public event Action<TKey, TValue>? OnAdded;
		/// <summary>
		///     Called when an item was set. Gives you the key, the old value, and the new value of the item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<TKey, TValue, TValue>? OnSet;
		/// <summary>
		///     Called when an item was removed. Gives you the key and value of the removed item.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<TKey, TValue>? OnRemoved;
		/// <summary>
		///     Called when the dictionary is cleared.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action? OnCleared;
		/// <summary>
		///     Called when the dictionary is changed in any way.
		/// </summary>
		[Obsolete("Use 'OnCollectionChanged' or RegisterChangedListener instead. This will be removed in build.", true)]
		public event Action<DictionaryChangeType>? OnChanged;
#pragma warning restore CS0067 // Event is never used
#endif // UNITY_EDITOR
		#endregion
	}
}