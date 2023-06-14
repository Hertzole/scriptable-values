#if (FISHNET)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING
using System;

namespace Hertzole.ScriptableValues
{
	public sealed partial class SyncedScriptableDictionary<TKey, TValue> : IDisposable
	{
		private ScriptableDictionary<TKey, TValue> targetDictionary;

		public void Initialize(ScriptableDictionary<TKey, TValue> dictionary)
		{
			targetDictionary = dictionary;

			dictionary.OnAdded += OnAdded;
			dictionary.OnRemoved += OnRemoved;
			dictionary.OnSet += OnSet;
			dictionary.OnCleared += OnCleared;

			OnInitialized();
		}

		partial void OnAdded(TKey key, TValue value);

		partial void OnRemoved(TKey key, TValue value);

		partial void OnSet(TKey key, TValue oldValue, TValue newValue);

		partial void OnCleared();

		partial void OnInitialized();

		public void Dispose()
		{
			targetDictionary.OnAdded -= OnAdded;
			targetDictionary.OnRemoved -= OnRemoved;
			targetDictionary.OnSet -= OnSet;
			targetDictionary.OnCleared -= OnCleared;

			OnDisposed();
		}

		partial void OnDisposed();
	}
}
#endif