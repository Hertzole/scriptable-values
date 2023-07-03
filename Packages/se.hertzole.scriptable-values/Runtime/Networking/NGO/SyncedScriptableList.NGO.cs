using System;
using Unity.Netcode;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableList<T> : NetworkVariableBase where T : unmanaged, IEquatable<T>
	{
		private bool hasInitialized;
		private bool didAdd;
		private bool didRemove;
		private bool didInsert;
		private bool didSet;
		private bool didClear;

		private NetworkList<T> networkList;

		private bool CanWrite
		{
			get { return GetBehaviour() != null && GetBehaviour().IsSpawned && CanClientWrite(GetBehaviour().NetworkManager.LocalClientId); }
		}

		public SyncedScriptableList(NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm) :
			base(readPerm, writePerm)
		{
			hasInitialized = false;
		}

		partial void OnInitialized()
		{
			networkList = new NetworkList<T>(null, ReadPerm, WritePerm);
			networkList.OnListChanged += OnNetworkListChanged;
		}

		partial void OnAdded(T item)
		{
			InitializeIfNeeded();

			if (didAdd)
			{
				didAdd = false;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot add to the list. Client does not have write permission.");
			}

			didAdd = true;

			networkList.Add(item);
			SetDirty(true);
		}

		partial void OnRemoved(int index, T item)
		{
			InitializeIfNeeded();

			if (didRemove)
			{
				didRemove = false;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot remove from the list. Client does not have write permission.");
			}

			didRemove = true;

			networkList.RemoveAt(index);
			SetDirty(true);
		}

		partial void OnInserted(int index, T item)
		{
			InitializeIfNeeded();

			if (didInsert)
			{
				didInsert = false;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot insert into the list. Client does not have write permission.");
			}

			didInsert = true;
			networkList.Insert(index, item);
			SetDirty(true);
		}

		partial void OnSet(int index, T oldItem, T newItem)
		{
			InitializeIfNeeded();

			if (didSet)
			{
				didSet = false;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot set the list item. Client does not have write permission.");
			}

			didSet = true;
			networkList[index] = newItem;
			SetDirty(true);
		}

		partial void OnCleared()
		{
			InitializeIfNeeded();

			if (didClear)
			{
				didClear = false;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot clear the list. Client does not have write permission.");
			}

			didClear = true;
			networkList.Clear();
			SetDirty(true);
		}

		private void OnNetworkListChanged(NetworkListEvent<T> changeEvent)
		{
			switch (changeEvent.Type)
			{
				case NetworkListEvent<T>.EventType.Add:
					if (didAdd)
					{
						didAdd = false;
						return;
					}

					didAdd = true;
					targetList.Add(changeEvent.Value);
					break;
				case NetworkListEvent<T>.EventType.Insert:
					if (didInsert)
					{
						didInsert = false;
						return;
					}

					didInsert = true;
					targetList.Insert(changeEvent.Index, changeEvent.Value);
					break;
				case NetworkListEvent<T>.EventType.Remove:
					if (didRemove)
					{
						didRemove = false;
						return;
					}

					didRemove = true;
					targetList.Remove(changeEvent.Value);
					break;
				case NetworkListEvent<T>.EventType.RemoveAt:
					if (didRemove)
					{
						didRemove = false;
						return;
					}

					didRemove = true;

					targetList.RemoveAt(changeEvent.Index);
					break;
				case NetworkListEvent<T>.EventType.Value:
					if (didSet)
					{
						didSet = false;
						return;
					}

					didSet = true;
					targetList[changeEvent.Index] = changeEvent.Value;
					break;
				case NetworkListEvent<T>.EventType.Clear:
					if (didClear)
					{
						didClear = false;
						return;
					}

					didClear = true;
					targetList.Clear();
					break;
				case NetworkListEvent<T>.EventType.Full:
					if (didClear)
					{
						didClear = false;
						return;
					}

					didClear = true;
					targetList.Clear();
					for (int i = 0; i < networkList.Count; i++)
					{
						didAdd = true;
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
			InitializeIfNeeded();

			networkList.WriteDelta(writer);
		}

		public override void WriteField(FastBufferWriter writer)
		{
			InitializeIfNeeded();

			networkList.WriteField(writer);
		}

		public override void ReadField(FastBufferReader reader)
		{
			InitializeIfNeeded();

			networkList.ReadField(reader);
			OnNetworkListChanged(new NetworkListEvent<T>
			{
				Type = NetworkListEvent<T>.EventType.Full,
				Index = 0,
				PreviousValue = default,
				Value = default
			});
		}

		public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
		{
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
			if (hasInitialized)
			{
				return;
			}

			networkList.Initialize(GetBehaviour());
			hasInitialized = true;
		}
	}
}