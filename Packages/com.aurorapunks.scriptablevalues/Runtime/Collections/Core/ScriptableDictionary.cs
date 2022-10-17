using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AuroraPunks.ScriptableValues.Helpers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AuroraPunks.ScriptableValues
{
	public abstract partial class ScriptableDictionary<TKey, TValue> : RuntimeScriptableObject, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary where TKey : notnull
	{
		[SerializeField]
		[Tooltip("If true, an equality check will be run before setting an item through the indexer to make sure the new object is not the same as the old one.")]
		private bool setEqualityCheck = true;
		
#if UNITY_EDITOR
#pragma warning disable 0414 // Disable "private field assigned but not used" warning
		[SerializeField] 
		private TKey editorKey = default;
		[SerializeField] 
		private TValue editorValue = default;
#pragma warning restore 0414
#endif

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

		TValue IDictionary<TKey, TValue>.this[TKey key] { get { return dictionary[key]; } set { SetValue(key, value); } }

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

		bool IDictionary.IsFixedSize { get { return false; } }
		bool IDictionary.IsReadOnly { get { return false; } }

		int ICollection.Count { get { return dictionary.Count; } }
		bool ICollection.IsSynchronized { get { return false; } }
		object ICollection.SyncRoot { get { return this; } }
		ICollection IDictionary.Values { get { return dictionary.Values; } }
		ICollection IDictionary.Keys { get { return dictionary.Keys; } }

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly { get { return false; } }

		public int Count { get { return dictionary.Count; } }

		public ICollection<TKey> Keys { get { return dictionary.Keys; } }
		public ICollection<TValue> Values { get { return dictionary.Values; } }
		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys { get { return dictionary.Keys; } }
		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values { get { return dictionary.Values; } }

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
			throw new InvalidCastException($"Cannot cast {value.GetType()} to {typeof(TType)}");
#else
			newValue = default;
			return false;
#endif
		}

		private void SetValue(TKey key, TValue value)
		{
			if (dictionary.TryGetValue(key, out TValue oldValue))
			{
				if (setEqualityCheck && !EqualityHelper.Equals(oldValue, value))
				{
					return;
				}

#if UNITY_EDITOR
				AddStackTrace(new StackTrace(true));
#endif
				
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
			bool result = dictionary.TryAdd(key, value);
			if (result)
			{
#if UNITY_EDITOR
				AddStackTrace(new StackTrace(true));
#endif
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
#if UNITY_EDITOR
			AddStackTrace(new StackTrace(true));
#endif
			dictionary.TrimExcess();
		}

		public void TrimExcess(int capacity)
		{
#if UNITY_EDITOR
			AddStackTrace(new StackTrace(true));
#endif
			dictionary.TrimExcess(capacity);
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
#if UNITY_EDITOR
			AddStackTrace(new StackTrace(true));
#endif
			dictionary.Clear();
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
#if UNITY_EDITOR
			AddStackTrace(new StackTrace(true));
#endif
			return ContainsKey(item.Key) && dictionary.Remove(item.Key);
		}

		public void Add(TKey key, TValue value)
		{
#if UNITY_EDITOR
			AddStackTrace(new StackTrace(true));
#endif
			dictionary.Add(key, value);
			OnAdded?.Invoke(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return dictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			bool removed = false;
			if (dictionary.ContainsKey(key))
			{
				removed = dictionary.Remove(key, out TValue oldItem);
				if (removed)
				{
#if UNITY_EDITOR
					AddStackTrace(new StackTrace(true));
#endif
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
#if UNITY_EDITOR
			ResetStackTraces();
#endif

			OnAdded = null;
			OnSet = null;
			OnRemoved = null;
			OnCleared = null;

			dictionary.Clear();
			dictionary.TrimExcess();
		}
		
#if UNITY_EDITOR
		protected override void OnExitPlayMode()
		{
			if (dictionary.Count > 0)
			{
				Debug.LogWarning($"There are left over objects in the scriptable dictionary {name}. You should clear the dictionary before leaving play mode.");
			}
			
			EventHelper.WarnIfLeftOverSubscribers(OnAdded, nameof(OnAdded), this);
			EventHelper.WarnIfLeftOverSubscribers(OnSet, nameof(OnSet), this);
			EventHelper.WarnIfLeftOverSubscribers(OnRemoved, nameof(OnRemoved), this);
			EventHelper.WarnIfLeftOverSubscribers(OnCleared, nameof(OnCleared), this);
		}
#endif
	}
}