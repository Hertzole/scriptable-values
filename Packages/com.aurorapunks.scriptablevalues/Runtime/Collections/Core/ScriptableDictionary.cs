using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;

namespace AuroraPunks.ScriptableValues
{
	public abstract class ScriptableDictionary : RuntimeScriptableObject
	{
		internal virtual bool IsValid()
		{
			return false;
		}
		
		internal virtual bool IsIndexValid(int index)
		{
			return false;
		}
	}
	
	public abstract partial class ScriptableDictionary<TKey, TValue> : ScriptableDictionary, ISerializationCallbackReceiver, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary where TKey : notnull
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
		private List<TKey> keys = new List<TKey>();
		[SerializeField] 
		private List<TValue> values = new List<TValue>();

		private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

		object IDictionary.this[object key]
		{
			get { return IsValidType(key, out TKey newKey) ? dictionary[newKey] : default(object); }
			set
			{
				if (IsValidType(key, out TKey newKey) && IsValidType(value, out TValue newValue))
				{
					SetValue(newKey, newValue);
				}
			}
		}

		TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] { get { return dictionary[key]; } }

		public TValue this[TKey key] { get { return dictionary[key]; } set { SetValue(key, value); } }

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

		bool IDictionary.IsFixedSize { get { return isReadOnly; } }

		int ICollection.Count { get { return dictionary.Count; } }
		bool ICollection.IsSynchronized { get { return false; } }
		object ICollection.SyncRoot { get { return this; } }
		ICollection IDictionary.Values { get { return dictionary.Values; } }
		ICollection IDictionary.Keys { get { return dictionary.Keys; } }

		public bool IsReadOnly { get { return isReadOnly; } set { isReadOnly = value; } }

		public int Count { get { return dictionary.Count; } }

		public ICollection<TKey> Keys { get { return dictionary.Keys; } }
		public ICollection<TValue> Values { get { return dictionary.Values; } }
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { return dictionary.Keys; } }
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { return dictionary.Values; } }

		public bool SetEqualityCheck { get { return setEqualityCheck; } set { setEqualityCheck = value; } }
		public bool ClearOnStart { get { return clearOnStart; } set { clearOnStart = value; } }

		public event Action<TKey, TValue> OnAdded;
		public event Action<TKey, TValue, TValue> OnSet;
		public event Action<TKey, TValue> OnRemoved;
		public event Action OnCleared;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsValidType<TType>(object value, out TType newValue)
		{
			if (value is TType newValueT)
			{
				newValue = newValueT;
				return true;
			}
			
#if DEBUG
			Debug.LogError($"{typeof(TType)} is not assignable from {value.GetType()}.");
#endif
			newValue = default;
			return false;
		}

		internal override bool IsValid()
		{
			for (int i = 0; i < keys.Count; i++)
			{
				for (int j = i; j < keys.Count; j++)
				{
					if (i == j)
					{
						continue;
					}
					
					if (Comparer.Equals(keys[i], keys[j]))
					{
						return false;
					}
				}
			}

			return true;
		}

		internal override bool IsIndexValid(int index)
		{
			for (int i = 0; i < keys.Count; i++)
			{
				if (i == index)
				{
					continue;
				}
				
				if (Comparer.Equals(keys[i], keys[index]))
				{
					return false;
				}
			}
			
			return true;
		}

		private void SetValue(TKey key, TValue value)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be changed at runtime.");
				return;
			}
			
			if (dictionary.TryGetValue(key, out TValue oldValue))
			{
				if (setEqualityCheck && !EqualityHelper.Equals(oldValue, value))
				{
					return;
				}

				AddStackTrace();

				int valueIndex = values.IndexOf(oldValue);
				if(valueIndex >= 0)
				{
					values[valueIndex] = value;
				}
				
				dictionary[key] = value;
				
				OnSet?.Invoke(key, oldValue, value);
			}
			else
			{
				Add(key, value);
			}
		}

		public bool TryAdd(TKey key, TValue value)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be added to at runtime.");
				return false;
			}
			
			bool result = dictionary.TryAdd(key, value);
			if (result)
			{
				AddStackTrace();

				keys.Add(key);
				values.Add(value);
				
				OnAdded?.Invoke(key, value);
			}

			return result;
		}

		public bool ContainsValue(TValue value)
		{
			return dictionary.ContainsValue(value);
		}

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

		public void TrimExcess()
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be trimmed at runtime.");
				return;
			}
			
			AddStackTrace();

			dictionary.TrimExcess();
			
			keys.TrimExcess();
			values.TrimExcess();
		}

		public void TrimExcess(int capacity)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be trimmed at runtime.");
				return;
			}
			

			AddStackTrace();

			dictionary.TrimExcess(capacity);
			
			keys.TrimExcess();
			values.TrimExcess();
		}

		bool IDictionary.Contains(object key)
		{
			return IsValidType(key, out TKey newKey) && ContainsKey(newKey);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary)dictionary).GetEnumerator();
		}

		void IDictionary.Remove(object key)
		{
			if (IsValidType(key, out TKey newKey))
			{
				Remove(newKey);
			}
		}

		void IDictionary.Add(object key, object value)
		{
			if (IsValidType(key, out TKey newKey) && IsValidType(value, out TValue newValue))
			{
				Add(newKey, newValue);
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection) dictionary).CopyTo(array, index);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) dictionary).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) dictionary).GetEnumerator();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be cleared at runtime.");
				return;
			}
			

			AddStackTrace();

			dictionary.Clear();
			
			keys.Clear();
			values.Clear();
			
			OnCleared?.Invoke();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return dictionary.ContainsKey(item.Key) && dictionary.ContainsValue(item.Value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>) dictionary).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			AddStackTrace();

			if(dictionary.TryGetValue(item.Key, out TValue value) && EqualityHelper.Equals(item.Value, value))
			{
				return Remove(item.Key);
			}

			return false;
		}

		public void Add(TKey key, TValue value)
		{
			if (isReadOnly)
			{
				Debug.LogError($"{this} is marked as read only and cannot be added to at runtime.");
				return;
			}
			
			AddStackTrace();
			
			dictionary.Add(key, value);

			keys.Add(key);
			values.Add(value);
			
			OnAdded?.Invoke(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

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
				}
			}

			return removed;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			return dictionary.TryGetValue(key, out value);
		}

		bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

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
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			// Does nothing.
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
#if DEBUG
			if (!IsValid())
			{
				return;
			}
#endif
			
			dictionary.Clear();

			if (keys.Count != values.Count)
			{
				return;
			}
			
			for (int i = 0; i < keys.Count; i++)
			{
				dictionary.Add(keys[i], values[i]);
			}
		}
	}
}