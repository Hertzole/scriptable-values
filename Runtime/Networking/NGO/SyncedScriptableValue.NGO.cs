#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING && SCRIPTABLE_VALUES_NGO
using System;
using Unity.Netcode;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableValue<T> : NetworkVariableBase
	{
		private bool hasInitializedNetworkVariable;
		private bool didSet;

		private NetworkVariable<T> networkVariable;

		private bool IsSpawned
		{
			get { return GetBehaviour() != null && GetBehaviour().IsSpawned; }
		}
		
		private bool CanWrite
		{
			get { return CanClientWrite(GetBehaviour().NetworkManager.LocalClientId); }
		}

		public SyncedScriptableValue(NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm)
			: base(readPerm, writePerm)
		{
			hasInitializedNetworkVariable = false;
		}

		partial void OnInitialized()
		{
			networkVariable = new NetworkVariable<T>(targetValue.Value, ReadPerm, WritePerm);

			if (IsSpawned && GetBehaviour().IsServer)
			{
				SetDirty(true);
			}
		}

		partial void OnValueChanging(T previousValue, T newValue)
		{
			if (!isInitialized || !IsSpawned)
			{
				return;
			}

			InitializeIfNeeded();

			if (didSet)
			{
				didSet = false;
				return;
			}

			if (!CanWrite)
			{
				throw new InvalidOperationException("Cannot change the value. Client does not have write permission.");
			}

			didSet = true;
			networkVariable.Value = newValue;
			SetDirty(true);
		}

		public override void SetDirty(bool isDirty)
		{
			InitializeIfNeeded();

			base.SetDirty(isDirty);
		}

		public override void WriteDelta(FastBufferWriter writer)
		{
			if (!isInitialized)
			{
				return;
			}
			
			InitializeIfNeeded();

			networkVariable.WriteDelta(writer);
			didSet = false;
		}

		public override void WriteField(FastBufferWriter writer)
		{
			if (!isInitialized)
			{
				return;
			}
            
			InitializeIfNeeded();

			networkVariable.WriteField(writer);
			didSet = false;
		}

		public override void ReadField(FastBufferReader reader)
		{
			if (!isInitialized)
			{
				return;
			}
            
			InitializeIfNeeded();

			networkVariable.ReadField(reader);

			didSet = true;
			targetValue.Value = networkVariable.Value;
		}

		public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
		{
			if (!isInitialized)
			{
				return;
			}
            
			InitializeIfNeeded();

			networkVariable.ReadDelta(reader, keepDirtyDelta);

			didSet = true;
			targetValue.Value = networkVariable.Value;
			Debug.Log($"Read delta {targetValue.Value} {networkVariable.Value}");
		}

		public override void Dispose()
		{
			base.Dispose();

			DisposeScriptableValue();

			networkVariable?.Dispose();
		}

		private void InitializeIfNeeded()
		{
			if (hasInitializedNetworkVariable)
			{
				return;
			}

			networkVariable.Initialize(GetBehaviour());
			hasInitializedNetworkVariable = true;
		}
	}
}
#endif