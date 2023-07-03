#if (FISHNET || SCRIPTABLE_VALUES_NGO)
#define SCRIPTABLE_VALUES_NETWORKING
#endif

#if SCRIPTABLE_VALUES_NETWORKING && SCRIPTABLE_VALUES_NGO
using Unity.Netcode;
using UnityEngine;

namespace Hertzole.ScriptableValues
{
	partial class SyncedScriptableValue<T> : NetworkVariableBase
	{
		private bool hasInitialized;

		private readonly NetworkVariable<T> networkVariable;

		public SyncedScriptableValue(NetworkVariableReadPermission readPerm = DefaultReadPerm, NetworkVariableWritePermission writePerm = DefaultWritePerm)
			: base(readPerm, writePerm)
		{
			hasInitialized = false;

			networkVariable = new NetworkVariable<T>(readPerm: readPerm, writePerm: writePerm);
		}

		partial void OnValueChanging(T previousValue, T newValue)
		{
			InitializeIfNeeded();
			
			if (GetBehaviour() == null || !GetBehaviour().IsSpawned)
			{
				return;
			}

			networkVariable.Value = newValue;
			SetDirty(true);
		}

		private void OnNetworkVariableChanged(T previousValue, T newValue)
		{
			InitializeIfNeeded();

			if (GetBehaviour().IsServer)
			{
				return;
			}

			targetValue.Value = newValue;
		}

		public override void SetDirty(bool isDirty)
		{
			InitializeIfNeeded();

			base.SetDirty(isDirty);
		}

		public override void WriteDelta(FastBufferWriter writer)
		{
			InitializeIfNeeded();

			networkVariable.WriteDelta(writer);
		}

		public override void WriteField(FastBufferWriter writer)
		{
			InitializeIfNeeded();

			networkVariable.WriteField(writer);
		}

		public override void ReadField(FastBufferReader reader)
		{
			InitializeIfNeeded();

			networkVariable.ReadField(reader);

			if (GetBehaviour() != null && !GetBehaviour().IsServer)
			{
				targetValue.Value = networkVariable.Value;
			}
		}

		public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
		{
			InitializeIfNeeded();

			networkVariable.ReadDelta(reader, keepDirtyDelta);
			
			if (GetBehaviour() != null && !GetBehaviour().IsServer)
			{
				targetValue.Value = networkVariable.Value;
			}
		}

		public override void Dispose()
		{
			base.Dispose();

			DisposeScriptableValue();
			networkVariable.Dispose();
		}

		private void InitializeIfNeeded()
		{
			if (hasInitialized)
			{
				return;
			}

			networkVariable.Initialize(GetBehaviour());
			hasInitialized = true;
		}
	}
}
#endif