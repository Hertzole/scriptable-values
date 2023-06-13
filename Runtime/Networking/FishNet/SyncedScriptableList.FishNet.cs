#if (FISHNET)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING && FISHNET
using System;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableList<T> : SyncBase, ICustomSync
	{
		private readonly SyncList<T> syncList = new SyncList<T>();

		public object GetSerializedType()
		{
			return null;
		}

		protected override void Registered()
		{
			syncList.InitializeInstance(NetworkBehaviour, byte.MaxValue - SyncIndex, Settings.WritePermission, Settings.ReadPermission, Settings.SendRate,
				Settings.Channel, IsSyncObject);

			base.Registered();
			syncList.SetRegistered();
		}

		partial void OnInitialized()
		{
			syncList.OnChange += OnSyncListChanged;
		}

		private void OnSyncListChanged(SyncListOperation op, int index, T oldItem, T newItem, bool asServer)
		{
			if (asServer)
			{
				return;
			}

			switch (op)
			{
				case SyncListOperation.Add:
					targetList.Add(newItem);
					break;
				case SyncListOperation.Insert:
					targetList.Insert(index, newItem);
					break;
				case SyncListOperation.Set:
					targetList[index] = newItem;
					break;
				case SyncListOperation.RemoveAt:
					targetList.RemoveAt(index);
					break;
				case SyncListOperation.Clear:
					targetList.Clear();
					break;
				case SyncListOperation.Complete:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(op), op, null);
			}
		}

		partial void OnAdded(T item)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncList.Add(item);
		}

		partial void OnRemoved(int index, T item)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncList.RemoveAt(index);
		}

		partial void OnInserted(int index, T item)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncList.Insert(index, item);
		}

		partial void OnSet(int index, T oldItem, T newItem)
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncList[index] = newItem;
		}

		partial void OnCleared()
		{
			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			syncList.Clear();
		}

		partial void OnDisposed()
		{
			syncList.OnChange -= OnSyncListChanged;
		}
	}
}
#endif