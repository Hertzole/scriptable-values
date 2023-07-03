#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING && FISHNET
using System;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableDictionary<TKey, TValue> : SyncBase, ICustomSync
	{
		private readonly SyncDictionary<TKey, TValue> syncDictionary = new SyncDictionary<TKey, TValue>();

		protected override void Registered()
		{
			syncDictionary.InitializeInstance(NetworkBehaviour, byte.MaxValue - SyncIndex, Settings.WritePermission, Settings.ReadPermission, Settings.SendRate,
				Settings.Channel, IsSyncObject);

			base.Registered();
			syncDictionary.SetRegistered();
		}

		partial void OnInitialized()
		{
			syncDictionary.OnChange += OnSyncDictionaryChanged;
		}

		private void OnSyncDictionaryChanged(SyncDictionaryOperation op, TKey key, TValue value, bool asServer)
		{
			if (asServer)
			{
				return;
			}

			switch (op)
			{
				case SyncDictionaryOperation.Add:
					targetDictionary.Add(key, value);
					break;
				case SyncDictionaryOperation.Clear:
					targetDictionary.Clear();
					break;
				case SyncDictionaryOperation.Remove:
					targetDictionary.Remove(key);
					break;
				case SyncDictionaryOperation.Set:
					targetDictionary[key] = value;
					break;
				case SyncDictionaryOperation.Complete:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(op), op, null);
			}
		}

		partial void OnAdded(TKey key, TValue value)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncDictionary.Add(key, value);
		}

		partial void OnRemoved(TKey key, TValue value)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncDictionary.Remove(key);
		}

		partial void OnSet(TKey key, TValue oldValue, TValue newValue)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncDictionary[key] = newValue;
		}

		partial void OnCleared()
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncDictionary.Clear();
		}

		partial void OnDisposed()
		{
			syncDictionary.OnChange -= OnSyncDictionaryChanged;
		}

		public object GetSerializedType()
		{
			return null;
		}
	}
}
#endif