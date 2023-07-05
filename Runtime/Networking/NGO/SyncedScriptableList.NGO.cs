#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING && SCRIPTABLE_VALUES_NGO
using System;
using Unity.Netcode;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableList<T> : NetworkVariableBase where T : unmanaged, IEquatable<T>
	{
		private bool hasInitializedNetworkList;
		private bool forceIsSpawned;

		private DidOperationFlags didOperation;

		private NetworkList<T> networkList;

		private bool IsSpawned
		{
			get { return GetBehaviour() != null && (forceIsSpawned || GetBehaviour().IsSpawned); }
		}

		private bool CanWrite
		{
			get { return CanClientWrite(GetBehaviour().NetworkManager.LocalClientId); }
		}

		public SyncedScriptableList(NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) :
			base(readPerm, writePerm)
		{
			hasInitializedNetworkList = false;
		}

		partial void OnInitialized()
		{
			networkList = new NetworkList<T>(targetList.list, ReadPerm, WritePerm);
			networkList.OnListChanged += OnNetworkListChanged;

			if (IsSpawned && GetBehaviour().IsServer)
			{
				SetDirty(true);
			}

			InitializeIfNeeded();
		}

		partial void OnAdded(T item)
		{
			if (!isInitialized || !IsSpawned)
			{
				return;
			}

			InitializeIfNeeded();

			if ((didOperation & DidOperationFlags.Add) != 0)
			{
				didOperation &= ~DidOperationFlags.Add;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot add to the list. Client does not have write permission.");
			}

			didOperation |= DidOperationFlags.Add;
			networkList.Add(item);
			SetDirty(true);
		}

		partial void OnRemoved(int index, T item)
		{
			if (!isInitialized || !IsSpawned)
			{
				return;
			}

			InitializeIfNeeded();

			if ((didOperation & DidOperationFlags.Remove) != 0)
			{
				didOperation &= ~DidOperationFlags.Remove;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot remove from the list. Client does not have write permission.");
			}

			didOperation |= DidOperationFlags.Remove;
			networkList.RemoveAt(index);
			SetDirty(true);
		}

		partial void OnInserted(int index, T item)
		{
			if (!isInitialized || !IsSpawned)
			{
				return;
			}

			InitializeIfNeeded();

			if ((didOperation & DidOperationFlags.Insert) != 0)
			{
				didOperation &= ~DidOperationFlags.Insert;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot insert into the list. Client does not have write permission.");
			}

			didOperation |= DidOperationFlags.Insert;
			networkList.Insert(index, item);
			SetDirty(true);
		}

		partial void OnSet(int index, T oldItem, T newItem)
		{
			if (!isInitialized || !IsSpawned)
			{
				return;
			}

			InitializeIfNeeded();

			if ((didOperation & DidOperationFlags.Set) != 0)
			{
				didOperation &= ~DidOperationFlags.Set;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot set the list item. Client does not have write permission.");
			}

			didOperation |= DidOperationFlags.Set;
			networkList[index] = newItem;
			SetDirty(true);
		}

		partial void OnCleared()
		{
			if (!isInitialized || !IsSpawned)
			{
				return;
			}

			InitializeIfNeeded();

			if ((didOperation & DidOperationFlags.Clear) != 0)
			{
				didOperation &= ~DidOperationFlags.Clear;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot clear the list. Client does not have write permission.");
			}

			didOperation |= DidOperationFlags.Clear;
			networkList.Clear();
			SetDirty(true);
		}

		private void OnNetworkListChanged(NetworkListEvent<T> changeEvent)
		{
			switch (changeEvent.Type)
			{
				case NetworkListEvent<T>.EventType.Add:
					if ((didOperation & DidOperationFlags.Add) != 0)
					{
						didOperation &= ~DidOperationFlags.Add;
						return;
					}

					didOperation |= DidOperationFlags.Add;
					targetList.Add(changeEvent.Value);
					break;
				case NetworkListEvent<T>.EventType.Insert:
					if ((didOperation & DidOperationFlags.Insert) != 0)
					{
						didOperation &= ~DidOperationFlags.Insert;
						return;
					}

					didOperation |= DidOperationFlags.Insert;
					targetList.Insert(changeEvent.Index, changeEvent.Value);
					break;
				case NetworkListEvent<T>.EventType.Remove:
					if ((didOperation & DidOperationFlags.Remove) != 0)
					{
						didOperation &= ~DidOperationFlags.Remove;
						return;
					}

					didOperation |= DidOperationFlags.Remove;
					targetList.Remove(changeEvent.Value);
					break;
				case NetworkListEvent<T>.EventType.RemoveAt:
					if ((didOperation & DidOperationFlags.RemoveAt) != 0)
					{
						didOperation &= ~DidOperationFlags.RemoveAt;
						return;
					}

					didOperation |= DidOperationFlags.RemoveAt;
					targetList.RemoveAt(changeEvent.Index);
					break;
				case NetworkListEvent<T>.EventType.Value:
					if ((didOperation & DidOperationFlags.Set) != 0)
					{
						didOperation &= ~DidOperationFlags.Set;
						return;
					}

					didOperation |= DidOperationFlags.Set;
					targetList[changeEvent.Index] = changeEvent.Value;
					break;
				case NetworkListEvent<T>.EventType.Clear:
					if ((didOperation & DidOperationFlags.Clear) != 0)
					{
						didOperation &= ~DidOperationFlags.Clear;
						return;
					}

					didOperation |= DidOperationFlags.Clear;
					targetList.Clear();
					break;
				case NetworkListEvent<T>.EventType.Full:
					if ((didOperation & DidOperationFlags.Clear) != 0)
					{
						didOperation &= ~DidOperationFlags.Clear;
						return;
					}

					didOperation |= DidOperationFlags.Clear;
					targetList.Clear();
					for (int i = 0; i < networkList.Count; i++)
					{
						didOperation |= DidOperationFlags.Add;
						targetList.Add(networkList[i]);
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public override void SetDirty(bool isDirty)
		{
			InitializeIfNeeded();

			base.SetDirty(isDirty);
		}

		public override void ResetDirty()
		{
			base.ResetDirty();
			networkList.ResetDirty();
		}

		public override void WriteDelta(FastBufferWriter writer)
		{
			if (!isInitialized)
			{
				return;
			}

			InitializeIfNeeded();

			networkList.WriteDelta(writer);
		}

		public override void WriteField(FastBufferWriter writer)
		{
			if (!isInitialized)
			{
				return;
			}

			InitializeIfNeeded();

			networkList.WriteField(writer);
		}

		public override void ReadField(FastBufferReader reader)
		{
			if (!isInitialized)
			{
				return;
			}

			InitializeIfNeeded();

			networkList.ReadField(reader);
			// We're forcing the spawned state here because it will say it isn't spawned, but if we can read, it is spawned.
			forceIsSpawned = true;
			OnNetworkListChanged(new NetworkListEvent<T>
			{
				Type = NetworkListEvent<T>.EventType.Full,
				Index = 0,
				PreviousValue = default,
				Value = default
			});

			forceIsSpawned = false;
		}

		public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
		{
			if (!isInitialized)
			{
				return;
			}

			InitializeIfNeeded();

			networkList.ReadDelta(reader, keepDirtyDelta);
		}

		public override void Dispose()
		{
			base.Dispose();

			if (networkList != null)
			{
				networkList.OnListChanged -= OnNetworkListChanged;
				networkList.Dispose();
			}

			DisposeTargetList();
		}

		private void InitializeIfNeeded()
		{
			if (hasInitializedNetworkList)
			{
				return;
			}

			NetworkBehaviour behaviour = GetBehaviour();

			if (behaviour == null)
			{
				return;
			}

			networkList.Initialize(behaviour);
			hasInitializedNetworkList = true;
		}
	}
}
#endif