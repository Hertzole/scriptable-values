#if (FISHNET || SCRIPTABLE_VALUES_NGO)
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
		private DidOperationFlags didOperation;

		private readonly SyncList<T> syncList = new SyncList<T>();

		[Flags]
		private enum DidOperationFlags
		{
			None = 0,
			Add = 1,
			Insert = 2,
			Set = 4,
			Remove = 8,
			Clear = 16
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
			if (!isInitialized || asServer)
			{
				return;
			}

			switch (op)
			{
				case SyncListOperation.Add:
					if ((didOperation & DidOperationFlags.Add) != 0)
					{
						didOperation &= ~DidOperationFlags.Add;
						return;
					}

					didOperation |= DidOperationFlags.Add;
					targetList.Add(newItem);
					break;
				case SyncListOperation.Insert:
					if ((didOperation & DidOperationFlags.Insert) != 0)
					{
						didOperation &= ~DidOperationFlags.Insert;
						return;
					}

					didOperation |= DidOperationFlags.Insert;
					targetList.Insert(index, newItem);
					break;
				case SyncListOperation.Set:
					if ((didOperation & DidOperationFlags.Set) != 0)
					{
						didOperation &= ~DidOperationFlags.Set;
						return;
					}

					didOperation |= DidOperationFlags.Set;
					targetList[index] = newItem;
					break;
				case SyncListOperation.RemoveAt:
					if ((didOperation & DidOperationFlags.Remove) != 0)
					{
						didOperation &= ~DidOperationFlags.Remove;
						return;
					}

					didOperation |= DidOperationFlags.Remove;
					targetList.RemoveAt(index);
					break;
				case SyncListOperation.Clear:
					if ((didOperation & DidOperationFlags.Clear) != 0)
					{
						didOperation &= ~DidOperationFlags.Clear;
						return;
					}

					didOperation |= DidOperationFlags.Clear;
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
			syncList.Add(item);
		}

		partial void OnRemoved(int index, T item)
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
			syncList.RemoveAt(index);
		}

		partial void OnInserted(int index, T item)
		{
			if ((didOperation & DidOperationFlags.Insert) != 0)
			{
				didOperation &= ~DidOperationFlags.Insert;
				return;
			}

			bool asServerInvoke = !IsNetworkInitialized || NetworkBehaviour.IsServer;

			if (!asServerInvoke)
			{
				return;
			}

			didOperation |= DidOperationFlags.Insert;
			syncList.Insert(index, item);
		}

		partial void OnSet(int index, T oldItem, T newItem)
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
			syncList[index] = newItem;
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
			syncList.Clear();
		}

		public void Dispose()
		{
			DisposeTargetList();
			syncList.OnChange -= OnSyncListChanged;
		}

		public object GetSerializedType()
		{
			return null;
		}
	}
}
#endif