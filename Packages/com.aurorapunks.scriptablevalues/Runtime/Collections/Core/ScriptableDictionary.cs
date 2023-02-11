using System;
using System.Collections;
using System.Collections.Generic;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	/// <summary>
	///     Base class for a scriptable object holds a dictionary.
	/// </summary>
	public abstract class ScriptableDictionary : RuntimeScriptableObject
	{
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
	public abstract class ScriptableDictionary<TKey, TValue> : ScriptableDictionary, ISerializationCallbackReceiver, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary where TKey : notnull
	{
		[SerializeField]
		[Tooltip("If read only, the dictionary cannot be changed at runtime and won't be cleared on start.")]
		private bool isReadOnly = false;
		[SerializeField]
		[Tooltip("If true, an equality check will be run before setting an item through the indexer to make sure the new object is not the same as the old one.")]
		private bool setEqualityCheck = true;
		[SerializeField]
		[Tooltip("If true, the dictionary will be cleared on play mode start/game boot.")]
		private bool clearOnStart = true;

		[SerializeField]
		internal List<TKey> keys = new List<TKey>();
		[SerializeField]
		internal List<TValue> values = new List<TValue>();

		internal Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

		object IDictionary.this[object key]
		{
			get { return EqualityHelper.IsSameType(key, out TKey newKey) ? dictionary[newKey] : default(object); }
			set
			{
				if (EqualityHelper.IsSameType(key, out TKey newKey) && EqualityHelper.IsSameType(value, out TValue newValue))
				{
					SetValue(newKey, newValue);
				}
			}
		}

		public TValue this[TKey key] { get { return dictionary[key]; } set { SetValue(key, value); } }

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
					Dictionary<TKey, TValue> newDictionary = value == null ? new Dictionary<TKey, TValue>(dictionary) : new Dictionary<TKey, TValue>(dictionary, value);

					dictionary = newDictionary;
				}
			}
		}

		/// <summary>
		///     If true, an equality check will be run before setting an item through the indexer to make sure the new object is
		///     not the same as the old one.
		/// </summary>
		public bool SetEqualityCheck { get { return setEqualityCheck; } set { setEqualityCheck = value; } }
		/// <summary>
		///     If true, the dictionary will be cleared on play mode start/game boot.
		/// </summary>
		public bool ClearOnStart { get { return clearOnStart; } set { clearOnStart = value; } }

		bool IDictionary.IsFixedSize { get { return isReadOnly; } }
		bool ICollection.IsSynchronized { get { return false; } }
		object ICollection.SyncRoot { get { return this; } }
		ICollection IDictionary.Values { get { return dictionary.Values; } }
		ICollection IDictionary.Keys { get { return dictionary.Keys; } }

		/// <summary>
		///     If read only, the dictionary cannot be changed at runtime and won't be cleared on start.
		/// </summary>
		public bool IsReadOnly { get { return isReadOnly; } set { isReadOnly = value; } }

		/// <summary>
		///     Gets the number of key/value pairs contained in the dictionary.
		/// </summary>
		public int Count { get { return dictionary.Count; } }

		/// <summary>
		///     Gets a collection containing the keys in the dictionary.
		/// </summary>
		public ICollection<TKey> Keys { get { return dictionary.Keys; } }

		/// <summary>
		///     Gets a collection containing the values in the dictionary.
		/// </summary>
		public ICollection<TValue> Values { get { return dictionary.Values; } }

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { return dictionary.Keys; } }
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { return dictionary.Values; } }

		/// <summary>
		///     Called when an item was added. Gives you the key and value of the newly added item.
		/// </summary>
		public event Action<TKey, TValue> OnAdded;
		/// <summary>
		///     Called when an item was set. Gives you the key, the old value, and the new value of the item.
		/// </summary>
		public event Action<TKey, TValue, TValue> OnSet;
		/// <summary>
		///     Called when an item was removed. Gives you the key and value of the removed item.
		/// </summary>
		public event Action<TKey, TValue> OnRemoved;
		/// <summary>
		///     Called when the dictionary is cleared.
		/// </summary>
		public event Action OnCleared;
		/// <summary>
		///     Called when the dictionary is changed in any way.
		/// </summary>
		public event Action<DictionaryChangeType> OnChanged;

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
		///     Sets or adds a value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to set.</param>
		/// <param name="value">The new value to set.</param>
		private void SetValue(TKey key, TValue value)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be changed at runtime.");
				return;
			}

			// Check if the item already exists.
			// If it does, update it.
			// Otherwise, add it.
			if (dictionary.TryGetValue(key, out TValue oldValue))
			{
				// If the equality check is enabled, we don't want to set the value if it's the same as the current value.
				if (setEqualityCheck && EqualityHelper.Equals(oldValue, value))
				{
					return;
				}

				// Get the index of the value.
				int valueIndex = values.IndexOf(oldValue);
				if (valueIndex >= 0)
				{
					// Update the value in the values list.
					values[valueIndex] = value;
				}

				// Update the value in the dictionary.
				dictionary[key] = value;

				OnSet?.Invoke(key, oldValue, value);
				OnChanged?.Invoke(DictionaryChangeType.Set);

				AddStackTrace();
			}
			else
			{
				Add(key, value);
			}
		}

		/// <summary>
		///     Attempts to add the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add-</param>
		/// <param name="value">The value of the element to add.</param>
		/// <returns>True if the key/value pair was added to the dictionary; otherwise, false.</returns>
		public bool TryAdd(TKey key, TValue value)
		{
			// If the game is playing, we don't want to set the value if it's read only.
			if (Application.isPlaying && isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be added to at runtime.");
				return false;
			}

			bool result = dictionary.TryAdd(key, value);
			// If the item was added, add it to the lists and invoke the event.
			if (result)
			{
				// We must update the lists too so they are in sync with the dictionary.
				keys.Add(key);
				values.Add(value);

				OnAdded?.Invoke(key, value);
				OnChanged?.Invoke(DictionaryChangeType.Added);

				AddStackTrace();
			}

			return result;
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
		///     Tries to find a key in the dictionary.
		/// </summary>
		/// <param name="predicate">The <see cref="Predicate{T}" /> to check against.</param>
		/// <param name="key">The key if it was found. Will be the default value if it wasn't found.</param>
		/// <returns>True if the key was found; otherwise, false.</returns>
		public bool TryFindKey(Predicate<TKey> predicate, out TKey key)
		{
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
		public bool TryFindValue(Predicate<TValue> predicate, out TValue value)
		{
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
		///     Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its
		///     entries.
		/// </summary>
		public void TrimExcess()
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be trimmed at runtime.");
				return;
			}

			dictionary.TrimExcess();

			keys.TrimExcess();
			values.TrimExcess();

			OnChanged?.Invoke(DictionaryChangeType.Trimmed);

			AddStackTrace();
		}

		/// <summary>
		///     Sets the capacity of this dictionary to hold up a specified number of entries without any further expansion of its
		///     backing storage.
		/// </summary>
		/// <param name="capacity">The new capacity.</param>
		public void TrimExcess(int capacity)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be trimmed at runtime.");
				return;
			}

			dictionary.TrimExcess(capacity);

			keys.TrimExcess();
			values.TrimExcess();

			OnChanged?.Invoke(DictionaryChangeType.Trimmed);

			AddStackTrace();
		}

		/// <inheritdoc />
		public override void ResetValues()
		{
			ResetStackTraces();

			OnAdded = null;
			OnSet = null;
			OnRemoved = null;
			OnCleared = null;

			if (!isReadOnly && clearOnStart)
			{
				dictionary.Clear();
				keys.Clear();
				values.Clear();
			}
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override void OnExitPlayMode()
		{
			if (!isReadOnly && clearOnStart && dictionary.Count > 0)
			{
				Debug.LogWarning($"There are left over objects in the scriptable dictionary {name}. You should clear the dictionary before leaving play mode.");
			}

			EventHelper.WarnIfLeftOverSubscribers(OnAdded, nameof(OnAdded), this);
			EventHelper.WarnIfLeftOverSubscribers(OnSet, nameof(OnSet), this);
			EventHelper.WarnIfLeftOverSubscribers(OnRemoved, nameof(OnRemoved), this);
			EventHelper.WarnIfLeftOverSubscribers(OnCleared, nameof(OnCleared), this);

			dictionary.TrimExcess();
		}
#endif

		/// <summary>
		///     Determines whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>True if the key type is the same as the generic type and the dictionary contains the key; otherwise, false.</returns>
		bool IDictionary.Contains(object key)
		{
			return EqualityHelper.IsSameType(key, out TKey newKey) && ContainsKey(newKey);
		}

		/// <summary>
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary) dictionary).GetEnumerator();
		}

		/// <summary>
		///     Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		void IDictionary.Remove(object key)
		{
			if (EqualityHelper.IsSameType(key, out TKey newKey))
			{
				Remove(newKey);
			}
		}

		/// <summary>
		///     Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.</param>
		void IDictionary.Add(object key, object value)
		{
			if (EqualityHelper.IsSameType(key, out TKey newKey) && EqualityHelper.IsSameType(value, out TValue newValue))
			{
				Add(newKey, newValue);
			}
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
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>A enumerator for the dictionary.</returns>
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) dictionary).GetEnumerator();
		}

		/// <summary>
		///     Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>A enumerator for the dictionary.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) dictionary).GetEnumerator();
		}

		/// <summary>
		///     Adds a key/value pair to the dictionary.
		/// </summary>
		/// <param name="item">The key/value pair to add.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		/// <summary>
		///     Removes all elements from the dictionary.
		/// </summary>
		public void Clear()
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be cleared at runtime.");
				return;
			}

			dictionary.Clear();

			keys.Clear();
			values.Clear();

			OnCleared?.Invoke();
			OnChanged?.Invoke(DictionaryChangeType.Cleared);

			AddStackTrace();
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
		/// <param name="arrayIndex">The index at which copying beings.</param>
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>) dictionary).CopyTo(array, arrayIndex);
		}

		/// <summary>
		///     Removes a key/value pair from the dictionary.
		/// </summary>
		/// <param name="item">The key/value pair to remove.</param>
		/// <returns>True if the key/value pair was found and removed; otherwise, false.</returns>
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			AddStackTrace();

			if (dictionary.TryGetValue(item.Key, out TValue value) && EqualityHelper.Equals(item.Value, value))
			{
				return Remove(item.Key);
			}

			return false;
		}

		/// <summary>
		///     Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add.</param>
		public void Add(TKey key, TValue value)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be added to at runtime.");
				return;
			}

			dictionary.Add(key, value);

			keys.Add(key);
			values.Add(value);

			OnAdded?.Invoke(key, value);
			OnChanged?.Invoke(DictionaryChangeType.Added);

			AddStackTrace();
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
		///     Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		///     True if the element was found and removed; otherwise, false. This method returns false if the key is not found
		///     in the dictionary.
		/// </returns>
		public bool Remove(TKey key)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be removed from at runtime.");
				return false;
			}

			bool removed = false;
			if (dictionary.ContainsKey(key))
			{
				removed = dictionary.Remove(key, out TValue oldItem);
				if (removed)
				{
					keys.Remove(key);
					values.Remove(oldItem);

					AddStackTrace();

					OnRemoved?.Invoke(key, oldItem);
					OnChanged?.Invoke(DictionaryChangeType.Removed);
				}
			}

			return removed;
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

			for (int i = 0; i < keys.Count; i++)
			{
				dictionary.Add(keys[i], values[i]);
			}
		}
	}
}