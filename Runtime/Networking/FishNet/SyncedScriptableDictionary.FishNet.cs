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
		private DidOperationFlags didOperation;

		private readonly SyncDictionary<TKey, TValue> syncDictionary = new SyncDictionary<TKey, TValue>();

		[Flags]
		private enum DidOperationFlags
		{
			None = 0,
			Add = 1,
			Set = 2,
			Remove = 4,
			Clear = 8
		}

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
					if ((didOperation & DidOperationFlags.Add) != 0)
					{
						didOperation &= ~DidOperationFlags.Add;
						return;
					}

					didOperation |= DidOperationFlags.Add;
					targetDictionary.Add(key, value);
					break;
				case SyncDictionaryOperation.Clear:
					if ((didOperation & DidOperationFlags.Clear) != 0)
					{
						didOperation &= ~DidOperationFlags.Clear;
						return;
					}

					didOperation |= DidOperationFlags.Clear;
					targetDictionary.Clear();
					break;
				case SyncDictionaryOperation.Remove:
					if ((didOperation & DidOperationFlags.Remove) != 0)
					{
						didOperation &= ~DidOperationFlags.Remove;
						return;
					}

					didOperation |= DidOperationFlags.Remove;
					targetDictionary.Remove(key);
					break;
				case SyncDictionaryOperation.Set:
					if ((didOperation & DidOperationFlags.Set) != 0)
					{
						didOperation &= ~DidOperationFlags.Set;
						return;
					}

					didOperation |= DidOperationFlags.Set;
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
			if ((didOperation & DidOperationFlags.Add) != 0)
			{
				didOperation &= ~DidOperationFlags.Add;
				return;
			}

			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			didOperation |= DidOperationFlags.Add;
			syncDictionary.Add(key, value);
		}

		partial void OnRemoved(TKey key, TValue value)
		{
			if ((didOperation & DidOperationFlags.Remove) != 0)
			{
				didOperation &= ~DidOperationFlags.Remove;
				return;
			}

			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			didOperation |= DidOperationFlags.Remove;
			syncDictionary.Remove(key);
		}

		partial void OnSet(TKey key, TValue oldValue, TValue newValue)
		{
			if ((didOperation & DidOperationFlags.Set) != 0)
			{
				didOperation &= ~DidOperationFlags.Set;
				return;
			}

			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			didOperation |= DidOperationFlags.Set;
			syncDictionary[key] = newValue;
		}

		partial void OnCleared()
		{
			if ((didOperation & DidOperationFlags.Clear) != 0)
			{
				didOperation &= ~DidOperationFlags.Clear;
				return;
			}

			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			didOperation |= DidOperationFlags.Clear;
			syncDictionary.Clear();
		}

		partial void OnDisposed()
		{
			syncDictionary.OnChange -= OnSyncDictionaryChanged;
		}

		public override void Reset()
		{
			base.Reset();

			syncDictionary.Clear();
		}

		public object GetSerializedType()
		{
			return null;
		}
	}
}
#endif